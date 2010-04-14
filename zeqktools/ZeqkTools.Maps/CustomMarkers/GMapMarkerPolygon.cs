
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
          Point[] _localPoints = new Point[GeoPoints.Count];

          for (int i = 0; i < _localPoints.Length; i++)
          {
              Position = GeoPoints[i];
              _localPoints[i] = LocalPosition;
          }

          g.DrawPolygon(Pen, _localPoints);
      }
   }
}
