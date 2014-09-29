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
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.ObjectModel;

namespace AltosTools.WindowsForms.Maps
{
   public partial class StaticImage : Form
   {
      GMapControl MainMap;
      BackgroundWorker bg = new BackgroundWorker();
      string path = "";

      readonly List<GPoint> tileArea = new List<GPoint>();

      public StaticImage(GMapControl main)
      {
          InitializeComponent();

          MainMap = main;

          numericUpDown1.Maximum = main.MaxZoom;
          numericUpDown1.Minimum = main.MinZoom;
          numericUpDown1.Value = new decimal(main.Zoom);

          bg.WorkerReportsProgress = true;
          bg.WorkerSupportsCancellation = true;
          bg.DoWork += new DoWorkEventHandler(bg_DoWork);
          bg.ProgressChanged += new ProgressChangedEventHandler(bg_ProgressChanged);
          bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
      }

      void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
          if (!e.Cancelled)
          {
              if (e.Error != null)
              {
                  MessageBox.Show("Error:" + e.Error.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              else if (e.Result != null)
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

          GPoint p = (GPoint)e.UserState;
          this.Text = "Static Map maker: Downloading[" + p + "]: " + tileArea.IndexOf(p) + " of " + tileArea.Count;
      }

      void bg_DoWork(object sender, DoWorkEventArgs e)
      {
          MapInfo info = (MapInfo)e.Argument;
          if (!info.Area.IsEmpty)
          {
              //var types = GMaps.Instance.GetAllLayersOfType(info.Type);

              string bigImage = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + "GMap at zoom " + info.Zoom + " - " + info.Type + "-" + DateTime.Now.Ticks + ".png";
              e.Result = bigImage;

              // current area
              GPoint topLeftPx = info.Type.Projection.FromLatLngToPixel(info.Area.LocationTopLeft, info.Zoom);
              GPoint rightButtomPx = info.Type.Projection.FromLatLngToPixel(info.Area.Bottom, info.Area.Right, info.Zoom);
              GPoint pxDelta = new GPoint(rightButtomPx.X - topLeftPx.X, rightButtomPx.Y - topLeftPx.Y);
              GMap.NET.GSize maxOfTiles = info.Type.Projection.GetTileMatrixMaxXY(info.Zoom);

              int padding = info.MakeWorldFile ? 0 : 22;
              {
                  using (Bitmap bmpDestination = new Bitmap((int)(pxDelta.X + padding * 2), (int)(pxDelta.Y + padding * 2)))
                  {
                      using (Graphics gfx = Graphics.FromImage(bmpDestination))
                      {
                          gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                          gfx.SmoothingMode = SmoothingMode.HighQuality;

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

                                  foreach (var tp in info.Type.Overlays)
                                  {
                                      Exception ex;
                                      GMapImage tile;

                                      // tile number inversion(BottomLeft -> TopLeft) for pergo maps
                                      if (tp.InvertedAxisY)
                                      {
                                          tile = GMaps.Instance.GetImageFrom(tp, new GPoint(p.X, maxOfTiles.Height - p.Y), info.Zoom, out ex) as GMapImage;
                                      }
                                      else // ok
                                      {
                                          tile = GMaps.Instance.GetImageFrom(tp, p, info.Zoom, out ex) as GMapImage;
                                      }

                                      if (tile != null)
                                      {
                                          using (tile)
                                          {
                                              long x = p.X * info.Type.Projection.TileSize.Width - topLeftPx.X + padding;
                                              long y = p.Y * info.Type.Projection.TileSize.Width - topLeftPx.Y + padding;
                                              {
                                                  gfx.DrawImage(tile.Img, x, y, info.Type.Projection.TileSize.Width, info.Type.Projection.TileSize.Height);
                                              }
                                          }
                                      }
                                  }
                              }
                          }

                          // draw polygons
                          {
                              foreach (GMapPolygon r in info.Polygons)
                              {
                                  if (r.IsVisible)
                                  {
                                      using (GraphicsPath rp = new GraphicsPath())
                                      {
                                          for (int j = 0; j < r.Points.Count; j++)
                                          {
                                              var pr = r.Points[j];
                                              GPoint px = info.Type.Projection.FromLatLngToPixel(pr.Lat, pr.Lng, info.Zoom);

                                              px.Offset(padding, padding);
                                              px.Offset(-topLeftPx.X, -topLeftPx.Y);

                                              GPoint p2 = px;

                                              if (j == 0)
                                              {
                                                  rp.AddLine(p2.X, p2.Y, p2.X, p2.Y);
                                              }
                                              else
                                              {
                                                  System.Drawing.PointF p = rp.GetLastPoint();
                                                  rp.AddLine(p.X, p.Y, p2.X, p2.Y);
                                              }
                                          }

                                          if (rp.PointCount > 0)
                                          {
                                              rp.CloseFigure();

                                              gfx.FillPath(r.Fill, rp);

                                              gfx.DrawPath(r.Stroke, rp);
                                          }
                                      }
                                  }
                              }
                          }
                          
                          // draw markers
                          {
                              foreach (GMapMarker r in info.Markers)
                              {
                                  if (r.IsVisible)
                                  {
                                      var pr = r.Position;
                                      GPoint px = info.Type.Projection.FromLatLngToPixel(pr.Lat, pr.Lng, info.Zoom);

                                      px.Offset(padding, padding);
                                      px.Offset(-topLeftPx.X, -topLeftPx.Y);
                                      px.Offset(r.Offset.X, r.Offset.Y);

                                      var auxLocalPosition = r.LocalPosition;

                                      r.LocalPosition = new System.Drawing.Point((int)px.X, (int)px.Y);

                                      r.OnRender(gfx);

                                      r.LocalPosition = auxLocalPosition;
                                  }
                              }

                          }
                      }

                      bmpDestination.Save(bigImage, ImageFormat.Png);
                  }
              }              
          }
      }

      readonly List<PointLatLng> GpxRoute = new List<PointLatLng>();
      RectLatLng AreaGpx = RectLatLng.Empty;

      private void button1_Click(object sender, EventArgs e)
      {
          RectLatLng? area = null;

            area = MainMap.SelectedArea;
            if (area.Value.IsEmpty)
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

          if (!bg.IsBusy)
          {
              lock (tileArea)
              {
                  tileArea.Clear();
                  tileArea.AddRange(MainMap.MapProvider.Projection.GetAreaTileList(area.Value, (int)numericUpDown1.Value, 1));
                  tileArea.TrimExcess();
              }

              numericUpDown1.Enabled = false;
              progressBar1.Value = 0;
              button1.Enabled = false;

              bg.RunWorkerAsync(new MapInfo(area.Value, (int)numericUpDown1.Value, MainMap.MapProvider, false, MainMap.Overlays[0].Markers, MainMap.Overlays[0].Polygons)); //TODO world file??
          }
      }

      private void button2_Click(object sender, EventArgs e)
      {
          if (bg.IsBusy)
          {
              bg.CancelAsync();
          }
      }

   }

   public struct MapInfo
   {
       public RectLatLng Area;
       public int Zoom;
       public GMapProvider Type;
       public bool MakeWorldFile;
       public ObservableCollectionThreadSafe<GMapMarker> Markers;
       public ObservableCollectionThreadSafe<GMapPolygon> Polygons;

       public MapInfo(RectLatLng Area, int Zoom, GMapProvider Type, bool makeWorldFile, ObservableCollectionThreadSafe<GMapMarker> markers, ObservableCollectionThreadSafe<GMapPolygon> polygons)
       {
           this.Area = Area;
           this.Zoom = Zoom;
           this.Type = Type;
           this.MakeWorldFile = makeWorldFile;
           this.Markers = markers;
           this.Polygons = polygons;
       }
   }
}
