
namespace GMap.NET.WindowsForms.Markers
{
   using System.Linq;
   using System.Drawing;
   using System.Collections.Generic;

   public class GMapMarkerPolygon : GMapMarker
   {
      public Pen Pen;

      public List<PointLatLng> GeoPoints;      

      public RectLatLng Rectangle
      {
          get 
          {
              double maxLat = GeoPoints.Max(p => p.Lat);
              double minLat = GeoPoints.Min(p => p.Lat);

              double maxLng = GeoPoints.Max(p => p.Lng);
              double minLng = GeoPoints.Min(p => p.Lng);

              RectLatLng rect = new RectLatLng(maxLat, minLng, maxLng - minLng, maxLat - minLat);

              return rect; 
          }
          
      }

      private List<Point> _localPoints;

      public GMapMarkerPolygon(PointLatLng p, List<PointLatLng> points, Pen pen)
          : base(p)
      {
          Pen = pen;

          GeoPoints = points;

      }

      public GMapMarkerPolygon(PointLatLng p,List<PointLatLng> points)
          : base(p)
      {
         Pen = new Pen(Brushes.Blue, 3);
         
          GeoPoints = points;
          
      }

      public override void OnRender(Graphics g)
      {
          _localPoints = new List<Point>();
          foreach (var item in GeoPoints)
          {
              Position = item;
              _localPoints.Add(LocalPosition);
          }

          g.DrawPolygon(Pen, _localPoints.ToArray());
      }
   }
}
