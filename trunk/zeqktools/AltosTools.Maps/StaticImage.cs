using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.ObjectModel;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace AltosTools.WindowsForms.Maps
{
   public partial class StaticImage : Form
   {
      GMapControl MainMap;
      BackgroundWorker bg = new BackgroundWorker();
      readonly List<GMap.NET.Point> tileArea = new List<GMap.NET.Point>();
      string path = "";

      public StaticImage(GMapControl main)
      {
         InitializeComponent();
          
         this.MainMap = main;          
         numericUpDown1.Maximum = MainMap.MaxZoom;
         numericUpDown1.Minimum = MainMap.MinZoom;
         numericUpDown1.Value = Convert.ToInt32(MainMap.Zoom);

         bg.WorkerReportsProgress = true;
         bg.WorkerSupportsCancellation = true;
         bg.DoWork += new DoWorkEventHandler(bg_DoWork);
         bg.ProgressChanged += new ProgressChangedEventHandler(bg_ProgressChanged);
         bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
      }

      void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         if(!e.Cancelled)
         {
            if(e.Error != null)
            {
               MessageBox.Show("Error:" + e.Error.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(e.Result != null)
            {
               try
               {
                  Process.Start(e.Result as string);
               }
               catch
               {
               }
            }
         }

         this.Text = "Static Map maker";
         progressBar1.Value = 0;
         button1.Enabled = true;
         numericUpDown1.Enabled = true;
      }

      void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         progressBar1.Value = e.ProgressPercentage;

         GMap.NET.Point p = (GMap.NET.Point) e.UserState;
         this.Text = "Static Map maker: Downloading[" + p + "]: " + tileArea.IndexOf(p) + " of " + tileArea.Count;
      }

      void bg_DoWork(object sender, DoWorkEventArgs e)
      {
         MapInfo info = e.Argument as MapInfo;
         if(!info.Area.IsEmpty)
         {
            string bigImage = path;
            e.Result = bigImage;

            MapType[] types = GMaps.Instance.GetAllLayersOfType(info.Type);

            // current area
            GMap.NET.Point topLeftPx = info.Projection.FromLatLngToPixel(info.Area.LocationTopLeft, info.Zoom);
            GMap.NET.Point rightButtomPx = info.Projection.FromLatLngToPixel(info.Area.Bottom, info.Area.Right, info.Zoom);
            GMap.NET.Point pxDelta = new GMap.NET.Point(rightButtomPx.X - topLeftPx.X, rightButtomPx.Y - topLeftPx.Y);

            int padding = 0;
            {
               using(Bitmap bmpDestination = new Bitmap(pxDelta.X + padding*2, pxDelta.Y + padding*2))
               {
                   using (Graphics gfx = Graphics.FromImage(bmpDestination))
                   {
                       gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

                       int i = 0;

                       // get tiles & combine into one
                       lock (tileArea)
                       {
                           foreach (var p in tileArea)
                           {
                               if (bg.CancellationPending)
                               {
                                   e.Cancel = true;
                                   return;
                               }

                               int pc = (int)(((double)++i / tileArea.Count) * 100);
                               bg.ReportProgress(pc, p);

                               foreach (MapType tp in types)
                               {
                                   Exception ex = new Exception();
                                   WindowsFormsImage tile = GMaps.Instance.GetImageFrom(tp, p, info.Zoom, out ex) as WindowsFormsImage;
                                   if (tile != null)
                                   {
                                       using (tile)
                                       {
                                           int x = p.X * info.Projection.TileSize.Width - topLeftPx.X + padding;
                                           int y = p.Y * info.Projection.TileSize.Width - topLeftPx.Y + padding;
                                           {
                                               gfx.DrawImage(tile.Img, x, y, info.Projection.TileSize.Width, info.Projection.TileSize.Height);
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }

                   // draw info
                   {
                       System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
                       {
                           rect.Location = new System.Drawing.Point(padding, padding);
                           rect.Size = new System.Drawing.Size(pxDelta.X, pxDelta.Y);
                           
                       }

                       using (Graphics gfx = Graphics.FromImage(bmpDestination))
                       {
                           //draw polygons
                           foreach (GMapPolygon polygon in info.Polygons)
                           {
                               if (polygon.Points.Count > 2)
                               {
                                   List<System.Drawing.Point> points = new List<System.Drawing.Point>();
                                   foreach (var gPoint in polygon.Points)
                                   {
                                       int x, y = 0;
                                       FromLatLngToLocal(info, rect.Height, rect.Width, gPoint.Lat, gPoint.Lng, out x, out y);
                                       points.Add(new System.Drawing.Point(x, y));
                                   }
                                   Color color = Color.FromArgb(95, polygon.Stroke.Color);
                                   Pen pen = new Pen(color, 4);
                                   pen.DashStyle = DashStyle.Custom;

                                   gfx.DrawPolygon(pen, points.ToArray());                                   
                               }
                           }

                           //draw marks
                           foreach (var marker in info.Markers)
                           {
                               if (marker.GetType() != typeof(GMapMarkerLine) &&
                                   marker.GetType() != typeof(GMapMarkerCross))
                               {   
                                   int x, y = 0;
                                   FromLatLngToLocal(info, rect.Height, rect.Width, marker.Position.Lat, marker.Position.Lng, out x, out y);

                                   IntPtr iconHandle1 = AltosTools.Maps.Properties.Resources.legendIcon.GetHicon();
                                   if (marker.GetType().GetProperty("Icon") != null)
                                   {
                                       Bitmap bitmap = (Bitmap)marker.GetType().GetProperty("Icon", typeof(Bitmap)).GetValue(marker, null);
                                       iconHandle1 = bitmap.GetHicon();
                                   }

                                   Icon icon1 = Icon.FromHandle(iconHandle1);

                                   gfx.DrawIcon(icon1, x - (icon1.Size.Width / 2) , y - (icon1.Size.Height / 2));
                                   Font font = new Font(FontFamily.GenericSansSerif, 12);

                                   string infoTag = "";
                                   if (marker.Tag != null)
                                       infoTag = marker.Tag.ToString();

                                   gfx.DrawString(infoTag, font, Brushes.Red, x + 10, y - 10);
                               }
                           }
                       }
                   }

                   bmpDestination.Save(bigImage, ImageFormat.Png);
               }
            }
         }
      }        

      private void button1_Click(object sender, EventArgs e)
      {
         RectLatLng area = MainMap.SelectedArea;
         if(!area.IsEmpty)
         {
            if(!bg.IsBusy)
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = Path.GetFullPath(saveFileDialog.FileName);
                    lock (tileArea)
                    {
                        tileArea.Clear();
                        tileArea.AddRange(MainMap.Projection.GetAreaTileList(area, (int)numericUpDown1.Value, 1));
                        tileArea.TrimExcess();
                    }
                    numericUpDown1.Enabled = false;
                    progressBar1.Value = 0;
                    button1.Enabled = false;
                    bg.RunWorkerAsync(new MapInfo(MainMap.Projection, area, (int)numericUpDown1.Value, MainMap.MapType, MainMap.Overlays[0].Markers, MainMap.Overlays[0].Polygons));
                }
            }
         }
         else
         {
            MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
      }

      private void button2_Click(object sender, EventArgs e)
      {
         if(bg.IsBusy)
         {
            bg.CancelAsync();
         }
      }

      private void FromLatLngToLocal(MapInfo info, int imageHeigth, int imageWidth, double lat, double lng, out int x, out int y)
      {
          //
          double heigthLat = lat - info.Area.LocationRightBottom.Lat;
          double widthLng = lng - info.Area.LocationTopLeft.Lng;

          y = Convert.ToInt32((heigthLat * imageHeigth) / info.Area.HeightLat);
          x = Convert.ToInt32((widthLng * imageWidth) / info.Area.WidthLng);

          y = imageHeigth - y;
          
      }
   }

   public class MapInfo
   {
      public PureProjection Projection;
      public RectLatLng Area;
      public int Zoom;
      public MapType Type;
      public ObservableCollectionThreadSafe<GMapMarker> Markers;
      public ObservableCollection<GMapPolygon> Polygons;


      public MapInfo(PureProjection Projection, RectLatLng Area, int Zoom, MapType Type, ObservableCollectionThreadSafe<GMapMarker> markers, ObservableCollection<GMapPolygon> polygons)
      {
         this.Projection = Projection;
         this.Area = Area;
         this.Zoom = Zoom;
         this.Type = Type;
         this.Markers = markers;
         this.Polygons = polygons;
      }
   }
}
