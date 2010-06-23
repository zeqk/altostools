
namespace GMap.NET.WindowsForms.Markers
{
   using System.Drawing;

   public class GMapMarkerCustom : GMapMarker
   {
       private Bitmap _icon;

       public Bitmap Icon
       {
           get { return _icon; }
           set { _icon = value; }
       }
	
       public GMapMarkerCustom(PointLatLng p)
           : base(p)
      {
      }

       public GMapMarkerCustom(PointLatLng p, Bitmap icon)
           : base(p)
       {
           _icon = icon;
       }

      public override void OnRender(Graphics g)
      {  
          g.DrawImageUnscaled(_icon, LocalPosition.X - (_icon.Size.Width / 2), LocalPosition.Y - (_icon.Size.Height / 2));        
      }
   }
}
