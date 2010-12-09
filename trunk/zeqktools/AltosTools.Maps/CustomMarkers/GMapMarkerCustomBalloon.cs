
namespace GMap.NET.WindowsForms.Markers
{
   using System.Drawing;

   public class GMapMarkerCustomBalloon : GMapMarker
   {
       private Bitmap _icon;

       public Bitmap Icon
       {
           get { return _icon; }
           set { _icon = value; }
       }
	
       public GMapMarkerCustomBalloon(PointLatLng p)
           : base(p)
      {
          _icon = AltosTools.Maps.Properties.Resources.red;
      }

       public GMapMarkerCustomBalloon(PointLatLng p, Bitmap icon)
           : base(p)
       {
           _icon = icon;
       }

      public override void OnRender(Graphics g)
      {  
          //g.DrawImageUnscaled(_icon, LocalPosition.X - (_icon.Size.Width / 2), LocalPosition.Y - (_icon.Size.Height / 2));
          g.DrawImageUnscaled(_icon, LocalPosition.X - (_icon.Size.Width / 2), LocalPosition.Y - _icon.Size.Height);        
      }
   }
}
