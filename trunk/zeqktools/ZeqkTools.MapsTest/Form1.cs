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

namespace ZeqkTools.MapsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (ZeqkTools.WindowsForms.Maps.frmGeoPolygon myForm = new ZeqkTools.WindowsForms.Maps.frmGeoPolygon())
            {
                List<PointLatLng> points = new List<PointLatLng>();
                myForm.Address = "Claypole, Buenos Aires, Argentina";
                //myForm.AlowDrawPolygon = false;
                myForm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (ZeqkTools.WindowsForms.Maps.frmGeoPolygon myForm = new ZeqkTools.WindowsForms.Maps.frmGeoPolygon())
            {
                List<PointLatLng> polygon = new List<PointLatLng>();
                PointLatLng point1 = new PointLatLng(-34.802387, -58.372250);
                polygon.Add(point1);
                PointLatLng point2 = new PointLatLng(-34.819160, -58.344440);
                polygon.Add(point2);
                PointLatLng point3 = new PointLatLng(-34.785187, -58.315773);
                polygon.Add(point3);
                PointLatLng point4 = new PointLatLng(-34.773769, -58.343925);
                polygon.Add(point4);
                PointLatLng point5 = new PointLatLng(-34.802387, -58.372250);
                polygon.Add(point5);
                //myForm.Polygon = polygon;

                List<GMapMarker> myMarks = new List<GMapMarker>();
                GMapMarkerGoogleRed myMark1 = new GMapMarkerGoogleRed(new PointLatLng(-34.788151, -58.345299));
                GMapMarkerGoogleRed myMark2 = new GMapMarkerGoogleRed(new PointLatLng(-34.799286, -58.334827));
                PointLatLng middlePoint = Functions.CalculateMiddlePoint(polygon);
                Pen pen = new Pen(Brushes.Aqua, 3);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                GMapMarkerPolygon myPolygonMark = new GMapMarkerPolygon(middlePoint, polygon, pen);

                myMarks.Add(myMark1);
                myMarks.Add(myMark2);
                myMarks.Add(myPolygonMark);

                myForm.SecondaryMarkers = myMarks;
                myForm.AllowDrawPolygon = true;
                myForm.Address = "Claypole, Buenos Aires, Argentina";
                myForm.MapZoom = 12;
                myForm.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (ZeqkTools.WindowsForms.Maps.frmGeoPoint myForm = new ZeqkTools.WindowsForms.Maps.frmGeoPoint())
            {
                myForm.Address = "Claypole, Buenos Aires, Argentina";
                if (myForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Result is ok");
                }
            }
        }
    }
}
