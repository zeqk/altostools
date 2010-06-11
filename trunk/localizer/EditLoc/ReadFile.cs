/*
 * Created by SharpDevelop.
 * User: NGRUIZ
 * Date: 5/17/2010
 * Time: 9:14 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace EditLoc
{
	/// <summary>
	/// Description of ReadFile.
	/// </summary>
	public partial class ReadFile : Form
	{
		public ReadFile()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void BtnBrowseClick(object sender, EventArgs e)
		{
			OpenFile.Filter = "All Files (*.*)|*.*";
			OpenFile.Multiselect = false;
			OpenFile.FileName = "";
			OpenFile.InitialDirectory = Directory.GetCurrentDirectory();
			OpenFile.ShowDialog();
			if(File.Exists(OpenFile.FileName))
			{
				txtFileName.Text = OpenFile.FileName;
			}
		}
		
		void BtnAddClick(object sender, EventArgs e)
		{
			if(txtBegins.Text != "" && txtEnds.Text != "")
			{
				grdCriteria.Rows.Add(txtBegins.Text, txtEnds.Text);
			}
		}

		
		void ReadFileLoad(object sender, EventArgs e)
		{
			// Add standard criteria
			grdCriteria.Rows.Add("(\"","\")");
			grdCriteria.Rows.Add("Text = \"","\"");
			grdCriteria.Rows.Add("GetString(\"","\")");
            grdCriteria.Rows.Add("GetString(\"", "\",");
			
			
		}

		
		void GrdCriteriaCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if(grdCriteria.SelectedRows.Count>0)
			{
				grdCriteria.Rows.Remove(grdCriteria.SelectedRows[0]);
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void CmdReadFileClick(object sender, EventArgs e)
		{
			if(File.Exists(txtFileName.Text))
			{
				if(grdCriteria.Rows.Count<0)
				{
					MessageBox.Show("You must add at least one criteria");
					return;
				}
				
				StreamReader reader = new StreamReader(txtFileName.Text);
				{
					String line="";
					List<String> texts = new List<string>();
					
					while(line!=null)
					{
						line=reader.ReadLine();
						if(line!=null)
						{
							foreach(DataGridViewRow criteria in grdCriteria.Rows)
							{
								String begins = (String) criteria.Cells["begins"].Value;
								String ends = (String) criteria.Cells["ends"].Value;
								
								if(line.Contains(begins) && line.Contains(ends))
								{
									int Start = line.IndexOf(begins)+begins.Length;
									int Length = line.IndexOf(ends,Start)- Start;
									if(Length>0)
									{
										if(!texts.Contains(line.Substring(Start,Length)))
										{
											texts.Add(line.Substring(Start,Length));
										}
									}
									break;
								}
							}
						}
					}
					
					// Add the strings in main form
					foreach(String text in texts)
					{
						MainForm main = (MainForm)Application.OpenForms["MainForm"];
                        if(!main.ContainsOriginalValue(text))
						    main.grdLanguages.Rows.Add(text);
                        
					}
					this.Close();
				}
			}
			else
			{
				MessageBox.Show("The file doen't exists. Please select a valid file.");
			}
		}
	}
}
