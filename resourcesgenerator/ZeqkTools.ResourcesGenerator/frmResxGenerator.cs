﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Collections;
using ZeqkTools.WindowsForms;

namespace ZeqkTools.ResourcesGenerator
{
    public partial class frmResxGenerator : Form
    {   

        public frmResxGenerator()
        {
            InitializeComponent();
        }

        #region GenerateResx

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (chklstFiles.CheckedItems.Count == 0 ||
                txtCultureTag.Text == "" ||
                txtFrom.Text == "" || txtTo.Text == "")
            {
                MessageBox.Show("Must be define all settings");
            }
            else
            {
                foreach (var item in chklstFiles.CheckedItems)
                {
                    string file = item.ToString();
                    string resxFile = Path.GetFileNameWithoutExtension(item.ToString());
                    resxFile += "."+ txtCultureTag.Text + ".resx";

                    file = Path.Combine(txtSource.Text,file);
                    resxFile = Path.Combine(txtDestiny.Text,resxFile);

                    try
                    {
                        bool successfullyGenerated = GenerateResx(file, resxFile, txtFrom.Text, txtTo.Text);
                        if (successfullyGenerated)
                            MessageBox.Show("Resource file were generated successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
            }

                 
        }

        private bool GenerateResx(string file, string resxFile, string fromStr, string toStr)
        {

            bool rv = false;

            Dictionary<object, object> existingEntries = new Dictionary<object, object>();

            try
            {
                if (File.Exists(resxFile))
                    existingEntries = GetEntries(resxFile);


                using (StreamReader sr = new StreamReader(file))
                {
                    using (ResXResourceWriter rxrw = new ResXResourceWriter(resxFile))
                    {

                        foreach (var item in existingEntries)
                        {
                            rxrw.AddResource(item.Key.ToString(), item.Value);
                        }

                        string line = "";
                        int count = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains(fromStr) && line.Contains(toStr))
                            {
                                string msg = ExtractString(line, fromStr, toStr);
                                if (msg != "")
                                {
                                    if (!existingEntries.ContainsKey(msg))
                                    {                                    
                                        rxrw.AddResource(msg, msg);
                                        existingEntries.Add(msg, msg);
                                    }
                                    count++;                                    
                                }
                            }
                        }

                        rxrw.Close();
                        rv = true;
                    }
                }     
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.IO.FileNotFoundException))
                {
                    throw new Exception("Incorrect file.", ex);
                }
                else
                    throw ex;
            }
            return rv;
              
        }

        private Dictionary<object,object> GetEntries(string resxFile)
        {
            Dictionary<object, object> values = new Dictionary<object, object>();

            using (ResXResourceReader rxrr = new ResXResourceReader(resxFile))
            {                
                foreach (DictionaryEntry entry in rxrr)
                    values.Add(entry.Key, entry.Value);
            }

            return values;
        }

        private string ExtractString(string text, string from, string to)
        {
            string rv = "";

            int firstIndex = text.IndexOf(from);
            firstIndex = firstIndex + from.Length;
            int lastIndex = text.IndexOf(to);
            int lenght = lastIndex - firstIndex;
            if (lenght > 0)
                rv = text.Substring(firstIndex, lenght);
            return rv;
        }

        private void btnSelectSource_Click(object sender, EventArgs e)
        {
            if (fbdDestiny.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = fbdDestiny.SelectedPath;
                txtDestiny.Text = fbdDestiny.SelectedPath;

                DirectoryInfo dir = new DirectoryInfo(txtSource.Text);
                FileInfo[] fileList = dir.GetFiles();
                foreach (FileInfo item in fileList)
                {
                    chklstFiles.Items.Add(item.Name);
                }
            }
        }

        private void btnSelectDestiny_Click(object sender, EventArgs e)
        {
            if(fbdDestiny.ShowDialog() == DialogResult.OK)
                txtDestiny.Text = fbdDestiny.SelectedPath;
        }
        #endregion
        #region Merge

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (chkLstResxFiles.CheckedItems.Count == 0 ||
                !txtMergedResxFile.Text.Contains(".resx"))
            {
                MessageBox.Show("Must be define all settings");
            }
            else
            {
                foreach (var item in chkLstResxFiles.CheckedItems)
                {
                    string file = item.ToString();
                    string resxFile = txtMergedResxFile.Text;

                    file = Path.Combine(txtSource.Text, file);
                    

                    try
                    {
                        bool successfullyMerged = Merge(file, resxFile);
                        if (successfullyMerged)
                            MessageBox.Show("Files were merged successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }


        private bool Merge(string file, string resxFile)
        {
            bool rv = false;

            Dictionary<object, object> existingEntries = new Dictionary<object, object>();

            try
            {
                if (File.Exists(resxFile))
                    existingEntries = GetEntries(resxFile);


                using (ResXResourceReader rxrr = new ResXResourceReader(file))
                {
                    using (ResXResourceWriter rxrw = new ResXResourceWriter(resxFile))
                    {

                        foreach (var item in existingEntries)
                        {
                            rxrw.AddResource(item.Key.ToString(), item.Value);
                        }

                        foreach (DictionaryEntry item in rxrr)
                        {
                            if (!existingEntries.ContainsKey(item.Key))
                            {
                                rxrw.AddResource(item.Key.ToString(), item.Value);
                                existingEntries.Add(item.Key, item.Value);
                            }
                        }    

                        rxrw.Close();
                    }
                }
                rv = true;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.IO.FileNotFoundException))
                {
                    throw new Exception("Incorrect file.", ex);
                }
                else
                    throw ex;
            }


            return rv;
        }

        private void btnSelectMergedSource_Click(object sender, EventArgs e)
        {
            if (fbdDestiny.ShowDialog() == DialogResult.OK)
            {
                txtMergeSource.Text = fbdDestiny.SelectedPath;
                txtMergedResxFile.Text = fbdDestiny.SelectedPath;
                sfdMergedResx.InitialDirectory = fbdDestiny.SelectedPath;


                DirectoryInfo dir = new DirectoryInfo(txtMergeSource.Text);
                FileInfo[] fileList = dir.GetFiles("*.resx");
                foreach (FileInfo item in fileList)
                {
                    chkLstResxFiles.Items.Add(item.Name);
                }


            }
        }

        private void btnSelectMergedResx_Click(object sender, EventArgs e)
        {
            if (sfdMergedResx.ShowDialog() == DialogResult.OK)
            {
                txtMergedResxFile.Text = Path.GetFullPath(sfdMergedResx.FileName);
            }
        }

        #endregion

        private void menuAbout_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.Text = "About " + Application.ProductName + " " + Application.ProductVersion;
            ab.AppTitle = Application.ProductName;
            ab.AppDescription = "A program for extract string from text file to resource file";
            ab.AppVersion = Application.ProductVersion;
            ab.AppCopyright = "GNU GPL 2010  Zeqk";
            ab.AppMoreInfo = "Web site: http://zeqk.wordpress.com \n\n";
            ab.AppMoreInfo += "This program is free software: you can redistribute it and/or modify ";
            ab.AppMoreInfo += "it under the terms of the GNU General Public License as published by ";
            ab.AppMoreInfo += "the Free Software Foundation, either version 3 of the License, or ";
            ab.AppMoreInfo += "(at your option) any later version.\n\n";

            ab.AppMoreInfo += "This program is distributed in the hope that it will be useful, ";
            ab.AppMoreInfo += "but WITHOUT ANY WARRANTY; without even the implied warranty of ";
            ab.AppMoreInfo += "MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the ";
            ab.AppMoreInfo += "GNU General Public License for more details.\n\n";

            ab.AppMoreInfo += "You should have received a copy of the GNU General Public License ";
            ab.AppMoreInfo += "along with this program.  If not, see <http://www.gnu.org/licenses/>.";

            ab.AppDetailsButton = false;
            ab.ShowDialog(this);
        }

        
    }
}
