/*
 * Creado por SharpDevelop.
 * Usuario: Supernato
 * Fecha: 06/05/2010
 * Hora: 07:28 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Localizer;
using System.Xml;


namespace EditLoc
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
	
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		
		
		void BtnPathClick(object sender, EventArgs e)
		{

		}
		
		
		
		void BtnLoadOriginalClick(object sender, EventArgs e)
		{
			OpenDialog.Filter = "Localization Files (*.xml)|*.xml";
			OpenDialog.Multiselect = false;
			OpenDialog.FileName = "";
			OpenDialog.InitialDirectory = Directory.GetCurrentDirectory();
			OpenDialog.ShowDialog();
			if(File.Exists(OpenDialog.FileName))
			{
				LanguageInformation li = Globalization.GetTotalStrings(OpenDialog.FileName);
				txtLanguageName.Text = li.LanguageName;
				grdLanguages.Rows.Clear();
				foreach(KeyValuePair<String, String> item in li.Strings)
				{
					grdLanguages.Rows.Add(item.Key,item.Value);
				}
			}
			

			

		}
		
		
		void BtnNewClick(object sender, EventArgs e)
		{
			DisableControls();
			grpEdit.Tag = "Add";
			txtOriginal.Text = "";
			txtTranslated.Text = "";
			txtOriginal.Focus();
		}
		
		void BtnEditClick(object sender, EventArgs e)
		{
			if(grdLanguages.SelectedRows.Count>0)
			{
				
				if(grdLanguages.SelectedRows.Count>1)
				{
					MessageBox.Show("Edition not allowed in multiple selection.");
					return;
				}
				DisableControls();
				grpEdit.Tag = "Edit";
				txtOriginal.Text = grdLanguages.SelectedRows[0].Cells[0].Value.ToString();
				txtTranslated.Text = grdLanguages.SelectedRows[0].Cells[1].Value.ToString();
				grpEdit.Visible = true;
				txtTranslated.Focus();
			}
		}
		
		void BtnRemoveClick(object sender, EventArgs e)
		{
			if(grdLanguages.SelectedRows.Count>0)
			{
				DialogResult response = MessageBox.Show("Do yo want to delete selected row(s)?","Question", MessageBoxButtons.YesNo);
				if(response == DialogResult.Yes)
				{
					foreach(DataGridViewRow dRow in grdLanguages.SelectedRows)
					{
						grdLanguages.Rows.Remove(dRow);
					}
				}
			}
		}
		
		void BtnReadFileClick(object sender, EventArgs e)
		{
			int currentRows = grdLanguages.Rows.Count;
			
			ReadFile rf = new ReadFile();
			rf.ShowDialog();
			
			MessageBox.Show("Operation Completed.\n"+ (grdLanguages.Rows.Count-currentRows).ToString()+" string(s) added.");
		}
		void BtnOKClick(object sender, EventArgs e)
		{
			if (grpEdit.Tag.ToString() == "Edit")
			{
				grdLanguages.SelectedRows[0].Cells[0].Value = txtOriginal.Text;
				grdLanguages.SelectedRows[0].Cells[1].Value = txtTranslated.Text;
			}
			else if (grpEdit.Tag.ToString() == "Add" && txtOriginal.Text != "")
			{
				grdLanguages.Rows.Add(txtOriginal.Text, txtTranslated.Text);
			}
			grpEdit.Visible = false;
			EnableControls();
			grdLanguages.Focus();
		}
		
		void BtnCancelClick(object sender, EventArgs e)
		{
			grpEdit.Visible = false;
			EnableControls();			
			grdLanguages.Focus();
		}
		
		private void DisableControls()
		{
			txtLanguageName.Enabled = false;
			btnLoadOriginal.Enabled = false;
			grdLanguages.Enabled = false;
			btnNew.Enabled = false;
			btnEdit.Enabled = false;
			btnSave.Enabled = false;
			btnExit.Enabled = false;
			
			grpEdit.Enabled = true;
			btnOK.Enabled = true;
			btnCancel.Enabled = true;
			txtOriginal.Enabled = true;
			txtTranslated.Enabled = true;
			
			grpEdit.Visible = true;
		}
		
		private void EnableControls()
		{
			txtLanguageName.Enabled = true;
			btnLoadOriginal.Enabled = true;
			grdLanguages.Enabled = true;
			btnNew.Enabled = true;
			btnEdit.Enabled = true;
			btnSave.Enabled = true;
			btnExit.Enabled = true;
			
			grpEdit.Visible = false;
		}

		void MainFormKeyUp(object sender, KeyEventArgs e)
		{
			if(grpEdit.Visible == true)
			{
				if (e.KeyCode == Keys.Enter)
				{
					btnOK.PerformClick();
				}
				if (e.KeyCode == Keys.Escape)
				{
					btnCancel.PerformClick();
				}
			}
			else
			{
				if(e.KeyCode == Keys.F2)
				{
					btnEdit.PerformClick();
				}
				if(e.KeyCode == Keys.F3)
				{
					btnNew.PerformClick();
				}
			}
		}
		
		void BtnSaveClick(object sender, EventArgs e)
		{
			if(txtLanguageName.Text == "")
			{
				MessageBox.Show("Language Name required. Please provide one.","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtLanguageName.Focus();
				return;
			}
			
			if(grdLanguages.Rows.Count<1)
			{
				MessageBox.Show("You need to add at least one translation.","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			
			SaveDialog.Filter = "Localization Files (*.xml)|*.xml";
			SaveDialog.FileName = txtLanguageName.Text;
			SaveDialog.InitialDirectory = Directory.GetCurrentDirectory();
			SaveDialog.OverwritePrompt = true;
			SaveDialog.ShowDialog();
			
			if (File.Exists(SaveDialog.FileName))
			{
				File.Delete(SaveDialog.FileName);
			}
			
			if (SaveDialog.FileName != "")
			{
				XmlDocument doc = new XmlDocument();
				
				XmlNode localizer = doc.CreateElement("Localizer");
				XmlAttribute language = doc.CreateAttribute("language");
				language.InnerText = txtLanguageName.Text;
				
				if(grdLanguages.Rows.Count>0)
				{
					for(int i=0; i<grdLanguages.Rows.Count; i++)
					{
						XmlNode nodeOriginal;
						XmlNode nodeTranslated;
						XmlNode nodeHeader;
					
						nodeHeader = doc.CreateElement("String");
						nodeOriginal = doc.CreateElement("Original");
						nodeTranslated = doc.CreateElement("Translated");
					
						nodeHeader.RemoveAll();
						nodeOriginal.InnerText = grdLanguages.Rows[i].Cells[0].Value.ToString();
						nodeTranslated.InnerText = grdLanguages.Rows[i].Cells[1].Value.ToString();
						
						nodeHeader.AppendChild(nodeOriginal);
						nodeHeader.AppendChild(nodeTranslated);
						localizer.AppendChild(nodeHeader);
					}
				}
				
				localizer.Attributes.Append(language);
				doc.AppendChild(localizer);
				doc.Save(SaveDialog.FileName);
			}
		}
		
		void GrdLanguagesCellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			btnEdit.PerformClick();
		}
		
		void BtnExitClick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
