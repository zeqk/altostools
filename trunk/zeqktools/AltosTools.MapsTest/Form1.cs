using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;

namespace AltosTools.MapsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (AltosTools.MapsTest.frmMapTest myForm = new AltosTools.MapsTest.frmMapTest())
            {
                List<PointLatLng> vertices = new List<PointLatLng>();
                PointLatLng point1 = new PointLatLng(-34.802387, -58.372250);
                vertices.Add(point1);
                PointLatLng point2 = new PointLatLng(-34.819160, -58.344440);
                vertices.Add(point2);
                PointLatLng point3 = new PointLatLng(-34.785187, -58.315773);
                vertices.Add(point3);
                PointLatLng point4 = new PointLatLng(-34.773769, -58.343925);
                vertices.Add(point4);
                PointLatLng point5 = new PointLatLng(-34.802387, -58.372250);
                vertices.Add(point5);
                GMapPolygon polygon = new GMapPolygon(vertices, "MyPolygon");
                polygon.Tag = "Mi poligono";
                myForm.MapMode = MapModeEnum.EditArea;
                myForm.MainPolygon = polygon;
                
                List<GMapMarker> myMarks = new List<GMapMarker>();
                var myMark1 = new GMarkerGoogle(new PointLatLng(-34.788151, -58.345299),GMarkerGoogleType.green);
                var myMark2 = new GMarkerGoogle(new PointLatLng(-34.799286, -58.334827), GMarkerGoogleType.green); //green
                
                myMarks.Add(myMark1);
                myMarks.Add(myMark2);
                myForm.OtherMarkers = myMarks;
                myForm.Address = "Claypole, Buenos Aires, Argentina";
                myForm.MapZoom = 12;
                myForm.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (AltosTools.MapsTest.frmMapTest myForm = new AltosTools.MapsTest.frmMapTest())
            {
                myForm.Address = "Claypole, Buenos Aires, Argentina";
                myForm.MapMode = MapModeEnum.EditPoint;
                if (myForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Result is ok");
                }
            }
        }
    }
}
