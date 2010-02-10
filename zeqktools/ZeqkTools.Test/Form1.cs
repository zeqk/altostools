using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;

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
            checkedListComboBox1.DisplayMember = "Name";
            


        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             int total = (int) numericUpDown1.Value;

             List<Item> list = new List<Item>();
             for (int i = 0; i < total; i++)
             {
                 list.Add(new Item(i, i.ToString()));
             }
             checkedListComboBox1.DataSource = list;
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
            checkedListComboBox1.CheckAllItems();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkedListComboBox1.UncheckAllItems();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (ZeqkTools.WindowsForms.Maps.frmGeoPoint myForm = new ZeqkTools.WindowsForms.Maps.frmGeoPoint())
            {
                myForm.Address = "Claypole";
                myForm.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (ZeqkTools.WindowsForms.Maps.frmGeoArea myForm = new ZeqkTools.WindowsForms.Maps.frmGeoArea())
            {
                //List<PointLatLng> points = new List<PointLatLng>();
                //PointLatLng point1 = new PointLatLng(-34.71317,-58.59649);
                //points.Add(point1);
                //PointLatLng point2 = new PointLatLng(-34.81696, -58.36578);
                //points.Add(point2);
                //PointLatLng point3 = new PointLatLng(-34.57759,-58.34381);
                //points.Add(point3);
                //myForm.Area = points;
                myForm.ShowDialog();
            }
        }




        
    }
}
