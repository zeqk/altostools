namespace GMap.NET.WindowsForms.Markers
{
    using System.Drawing;
    using GMap.NET.WindowsForms;

    public class GMapMarkerLine : GMapMarker
    {
        public Pen Pen;
        public PointLatLng Point1, Point2;

        public GMapMarkerLine(PointLatLng p1, PointLatLng p2)
            : base(p1)
        {
            Pen = new Pen(Brushes.Red, 5);
            this.Point1 = p1;
            this.Point2 = p2;

            // do not forget set Size of the marker
            // if so, you shall have no event on it ;}
            Size = new Size(1, 1);
        }

        public override void OnRender(Graphics g)
        {
            Position = Point1;
            Point p1 = LocalPosition;
            Position = Point2;
            Point p2 = LocalPosition;
            g.DrawLine(Pen, p1, p2);
        }
    }
}
