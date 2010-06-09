/*
 * Creado por SharpDevelop.
 * Usuario: Supernato
 * Fecha: 23/04/2010
 * Hora: 10:34 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 * 
 * Author: supernato
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Localizer
{
	/// <summary>
	/// Description of Globalization.
	/// </summary>
	public static class Globalization
	{
		private static List<String> _listLanguages;
		private static Dictionary<String, String> _languagesFiles;
		private static String _currentLanguage;
		private static Dictionary<String, String> _localizedStrings;
		private static Dictionary<String, String> _originalValues;
		private static String _languagesPath;
		
		
		
		static Globalization()
		{
			_listLanguages = new List<string>();
			_languagesFiles = new Dictionary<string, string>();
			_localizedStrings = new Dictionary<string, string>();
			_originalValues = new Dictionary<string, string>();
			
			// Path donde se encuentran los archivos traducidos
			_languagesPath = string.Concat(Directory.GetCurrentDirectory(), @"\languages");
			LoadLanguagesList();
		}
		
		public static List<String> LanguagesList
		{
			get { return _listLanguages; }
		}
		
		public static String LanguagesPath
		{
			get { return _languagesPath; }
			set {
				_languagesPath = value;
				LoadLanguagesList();
			}
		}
		
		public static LanguageInformation GetTotalStrings(String FileName)
		{
			try
			{
				Dictionary<String, String> ret = new Dictionary<string, string>();
				String languageName;
				
				// Crea el XmlDocument
				XmlDocument doc = new XmlDocument();
				doc.Load(FileName);
				XmlElement localizer = doc.DocumentElement;
				
				
				languageName = localizer.Attributes["language"].Value;
				
				XmlNodeList strings = localizer.SelectNodes("String");
				
				// Recorre todos los nodos "String" que contienen los textos localizados
				foreach(XmlNode nodelist in strings)
				{
					// Este valor se almacena como KEY. Es el nombre del objeto a localizar
					XmlNode original = nodelist.SelectSingleNode("Original");
					// Valor traducido del control
					XmlNode translated = nodelist.SelectSingleNode("Translated");
					
					if(original == null)
					{
						continue;
					}
					if(!ret.ContainsKey(original.InnerText))
					{
						if(translated.InnerText=="")
						{
							ret.Add(original.InnerText, "");
						}
						else
						{
							ret.Add(original.InnerText,translated.InnerText);
						}
					}
				}
				
				return new LanguageInformation(languageName, ret);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				return null;
			}
		}
		
		public static Boolean SetCurrentLanguage(String language)
		{
			try
			{
				if(_languagesFiles.ContainsKey(language))
				{
					_currentLanguage = language;
					_localizedStrings.Clear();
															
					// Crea el XmlDocument
					XmlDocument doc = new XmlDocument();
					doc.Load(_languagesFiles[language]);
					XmlElement localizer = doc.DocumentElement;
					
					XmlNodeList strings = localizer.SelectNodes("String");
					

					
					// Recorre todos los nodos "String" que contienen los textos localizados
					foreach(XmlNode nodelist in strings)
					{
						// Este valor se almacena como KEY. Es el nombre del objeto a localizar
						XmlNode original = nodelist.SelectSingleNode("Original");
						// Valor traducido del control
						XmlNode translated = nodelist.SelectSingleNode("Translated");
						
						if(original == null)
						{
							continue;
						}
						
						if(!_localizedStrings.ContainsKey(original.InnerText))
						{
							if(translated.InnerText=="")
							{
								_localizedStrings.Add(original.InnerText, original.InnerText);
							}
							else
							{
								_localizedStrings.Add(original.InnerText, translated.InnerText);
							}
						}
					}
				}
				return true;
			}
			catch(Exception ex)
			{
				return false;
			}
		}

		public static Boolean RefreshUI()
		{
			try
			{
				foreach(Form frm in Application.OpenForms)
				{
					RefreshUI(frm);
				}
				
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		public static Boolean RefreshUI(Control form)
		{
			try
			{
				foreach(Control ctl in form.Controls)
				{
					if(ctl is MenuStrip)
					{
						LocalizeMenu((MenuStrip) ctl);
						continue;
					}
					if(ctl is ComboBox)
					{
						LocalizeComboBox((ComboBox)ctl);
						continue;
					}
					if(ctl is ListBox)
					{
						LocalizeListBox((ListBox)ctl);
						continue;
					}
					if(ctl is DataGridView)
					{
						LocalizeDataGridView((DataGridView)ctl);
					}
					
					if(ctl.HasChildren)
					{
						RefreshUI(ctl);
					}
					ctl.Text = LocalizeString(ctl.Text);
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;

			}
		}
		
		public static String GetString(String text)
		{
			try
			{
				if(_localizedStrings.ContainsKey(text))
				{
					return _localizedStrings[text];
				}
				else
				{
					return text;
				}
			}
			catch
			{
				return text;
			}
		}
		
		public static String GetOriginal(String text)
		{
			try
			{
				if (_originalValues.ContainsKey(text))
				{
					return _localizedStrings[text];
				}
				else
				{
					return text;
				}
			}
			catch
			{
				return text;
			}
		}
		
		private static bool LoadLanguagesList()
		{
			try {
				DirectoryInfo dir = new DirectoryInfo(_languagesPath);
				foreach(FileInfo file in dir.GetFiles("*.*"))
				{
					if(file.Extension==".xml")
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(file.FullName);
	
						XmlElement information = doc.DocumentElement;
						
						if(!(information==null))
						{
							if(!_listLanguages.Contains(information.Attributes["language"].Value))
							{
								_listLanguages.Add(information.Attributes["language"].Value);
								_languagesFiles.Add(information.Attributes["language"].Value, file.FullName);
							}
						}
					}
				}
				return false;

			}
			catch
			{
				return false;
			}
			
		}
		
		private static bool LocalizeMenu(MenuStrip ctl)
		{
			try 
			{
				foreach( ToolStripItem itm in ctl.Items)
				{
					
					if(!(itm is ToolStripSeparator))
					{
						LocalizeMenuItem((ToolStripMenuItem)itm);
			
						if(((ToolStripMenuItem)itm).DropDownItems.Count >0)
							{
								LocalizeSubMenu(((ToolStripMenuItem)itm).DropDownItems);
							}
					}
					
					

				}
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		private static bool LocalizeSubMenu(ToolStripItemCollection col)
		{
			try
			{
				foreach( ToolStripItem itm in col)
				{
					if(!(itm is ToolStripSeparator))
					{
						itm.Text = LocalizeString(itm.Text);
						
						if(((ToolStripMenuItem)itm).DropDownItems.Count >0)
						{
							LocalizeSubMenu(((ToolStripMenuItem)itm).DropDownItems);
						}					
					}
					

				}
				return true;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}
		
		private static bool LocalizeMenuItem(ToolStripMenuItem itm)
		{
				if(_originalValues.ContainsKey(itm.Text))
						{
							String OriginalString;
							
							itm.Text = OriginalString = _originalValues[itm.Text];
							if(_localizedStrings.ContainsKey(OriginalString))
							{
								if(!_originalValues.ContainsKey(_localizedStrings[OriginalString]))
								{
									_originalValues.Remove(OriginalString);
									_originalValues.Add(_localizedStrings[OriginalString],OriginalString);
								}
								itm.Text = _localizedStrings[OriginalString];
							}
						}
					else
					{
							if(_localizedStrings.ContainsKey(itm.Text))
							{
								if(!_originalValues.ContainsKey(_localizedStrings[itm.Text]))
								{
									_originalValues.Add(_localizedStrings[itm.Text],itm.Text);
								}
								itm.Text = _localizedStrings[itm.Text];
							}
					}
					return true;
		}
		
		private static bool LocalizeComboBox(ComboBox cmb)
		{
			Object[] ObjectList = new object[cmb.Items.Count];
			
			for(int i=0;i<cmb.Items.Count;i++)
			{
				ObjectList[i]=LocalizeString(cmb.Items[i].ToString());
			}
			
			cmb.Items.Clear();
			cmb.Items.AddRange(ObjectList);

			return true;
		}
		
		private static bool LocalizeListBox(ListBox lst)
		{
			Object[] ObjectList = new object[lst.Items.Count];
			
			for(int i=0;i<lst.Items.Count;i++)
			{
				String Text;
				
				Text = lst.Items[i].ToString();
				ObjectList[i]=LocalizeString(Text);
			}
			
			lst.Items.Clear();
			lst.Items.AddRange(ObjectList);

			return true;
		}
		
		private static bool LocalizeDataGridView(DataGridView dgv)
		{
			
			for(int i=0;i<dgv.Columns.Count;i++)
			{
				dgv.Columns[i].HeaderText=LocalizeString(dgv.Columns[i].HeaderText);
			}
			return true;
		}
		
		private static String LocalizeString(String text)
		{
			try{
				
			if(_originalValues.ContainsKey(text))
			{
					String OriginalString;
					
					OriginalString = _originalValues[text];
					if(_localizedStrings.ContainsKey(OriginalString))
					{
						if(!_originalValues.ContainsKey(_localizedStrings[OriginalString]))
						{
							_originalValues.Remove(OriginalString);
							_originalValues.Add(_localizedStrings[OriginalString],OriginalString);
						}
						return _localizedStrings[OriginalString];
					}
				}
			else
			{
				if(_localizedStrings.ContainsKey(text))
				{
					if(!_originalValues.ContainsKey(_localizedStrings[text]))
					{
						_originalValues.Add(_localizedStrings[text],text);
					}
					return _localizedStrings[text];
				}
			}
		return text;
		}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				return text;
			}
		}
		
	
	}
}


