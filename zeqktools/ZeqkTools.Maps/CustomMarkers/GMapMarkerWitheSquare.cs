
namespace GMap.NET.WindowsForms.Markers
{
   using System.Drawing;

   public class GMapMarkerWitheSquare : GMapMarkerCustom
   {
	
       public GMapMarkerWitheSquare(PointLatLng p)
           : base(p)
      {           
          Icon = ZeqkTools.Maps.Properties.Resources.square;
          this.Size = new Size(8, 8);
      }
   }
}
