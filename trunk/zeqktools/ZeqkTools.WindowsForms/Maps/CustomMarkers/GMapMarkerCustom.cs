
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
        //g.DrawImageUnscaled(Resources.shadow50, LocalPosition.X-10, LocalPosition.Y-40);
        g.DrawImageUnscaled(_icon, LocalPosition.X, LocalPosition.Y);
        //g.DrawImageUnscaled(Resources.drag_cross_67_16, LocalPosition.X-8, LocalPosition.Y-8);
      }
   }
}
