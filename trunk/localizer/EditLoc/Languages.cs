/*
 * Creado por SharpDevelop.
 * Usuario: Supernato
 * Fecha: 06/05/2010
 * Hora: 09:04 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace EditLoc
{
	/// <summary>
	/// Description of Languages.
	/// </summary>
	public static class Languages
	{
		private static String _languagesPath;
		private static Dictionary<String, String> _languagesFiles;
		private static List<String> _listLanguages;
		
		static Languages()
		{
			_languagesPath = Directory.GetCurrentDirectory();
			_languagesFiles = new Dictionary<string, string>();
			_listLanguages = new List<string>();
			_localizedStrings = new
		}
		
		public static String Path
		{
			get { return _languagesPath; }
			set {
				_languagesPath = value;
				LoadLanguagesList();
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
		
		public static List<String> LanguagesList
		{
			get { return _listLanguages; }
		}
		
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
						if(!_localizedStrings.ContainsKey(original.InnerText))
						{
							_localizedStrings.Add(original.InnerText, translated.InnerText);
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
}
