using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using AltosTools.WindowsForms;


namespace ZeqkTools.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            extendedCheckedListBox1.DisplayMember = "Name";
            extendedCheckedListBox1.ValueMember = "Id";

            List<Item> list = new List<Item>();
            for (int i = 0; i < 6; i++)
            {
                list.Add(new Item(i, i.ToString()));
            }
            extendedCheckedListBox1.DataSource = list;
            


        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             int total = (int) numericUpDown1.Value;

             
        }

        private void checkedListComboBox1_TextUpdate(object sender, EventArgs e)
        {
            
        }

        private void checkedListComboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListComboBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AboutBox form = new AboutBox();
            form.ShowDialog();
        }




        
    }
}
