/*
 * Creado por SharpDevelop.
 * Usuario: Supernato
 * Fecha: 01/05/2010
 * Hora: 08:38 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Localizer;

namespace Test
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
		
		void MainFormLoad(object sender, EventArgs e)
		{
			foreach(String language in Globalization.LanguagesList)
			{
				grdLanguages.Rows.Add(language);
			}
		}
	
		
		void GrdLanguagesSelectionChanged(object sender, EventArgs e)
		{
			if(grdLanguages.SelectedRows.Count==1)
			{
				Globalization.SetCurrentLanguage(grdLanguages.SelectedRows[0].Cells[0].Value.ToString());
				Globalization.RefreshUI(this);
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			MessageBox.Show(Globalization.GetString("File not found."));
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			MessageBox.Show(Globalization.GetString("Do you want to delete this record?"), Globalization.GetString("Error"), MessageBoxButtons.YesNo);
		}
		
		
		void Button4Click(object sender, EventArgs e)
		{
			MessageBox.Show(Globalization.GetString("Please enter your password."));
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			MessageBox.Show(Globalization.GetString("This is a message without translation"));
		}
		
	}
}
