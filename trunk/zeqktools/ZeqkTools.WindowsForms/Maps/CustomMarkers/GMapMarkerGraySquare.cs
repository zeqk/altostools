
namespace GMap.NET.WindowsForms.Markers
{
   using System.Drawing;

   public class GMapMarkerGraySquare : GMapMarkerCustom
   {

       public GMapMarkerGraySquare(PointLatLng p)
           : base(p)
      {
          Icon = ZeqkTools.WindowsForms.Properties.Resources.transparentsquare;
          this.Size = new Size(8, 8);
      }
   }
}
