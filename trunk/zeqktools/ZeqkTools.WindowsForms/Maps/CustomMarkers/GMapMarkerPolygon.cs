
namespace GMap.NET.WindowsForms.Markers
{
   using System.Drawing;
   using System.Collections.Generic;

   public class GMapMarkerPolygon : GMapMarker
   {
      public Pen Pen;

      public List<PointLatLng> GeoPoints;
      private List<Point> _localPoints;

      public GMapMarkerPolygon(PointLatLng p,List<PointLatLng> points)
          : base(p)
      {
         Pen = new Pen(Brushes.Blue, 3);
         
          GeoPoints = points;
          _localPoints = new List<Point>();
         foreach (var item in points)
         {
             Position = item;
             _localPoints.Add(LocalPosition);
         }
      }

      public override void OnRender(Graphics g)
      {
          g.DrawPolygon(Pen, _localPoints.ToArray());
      }
   }
}
