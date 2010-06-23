using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using ZeqkTools.WindowsForms.Maps;

namespace ZeqkTools.MapsTest
{
    public partial class frmGeoPolygonTest : Form
    {
        #region Fields
        private MapType _mapType;
        private int _mapZoom;
        private bool _allowDrawPolygon;     
               
        private GMapPolygon currentPolygon;
        private List<GMapMarker> _secondaryMarkers;
        private List<GMapPolygon> _secondaryPolygons;
        #endregion

        #region Internal variables  

        // markers
        GMapMarker center;
        GMapMarker currentMarker;

        // layers
        GMapOverlay top;

        bool isMouseDown;
        #endregion

        #region Public properties

        /// <summary>
        /// Get set the main polygon
        /// </summary>
        public GMapPolygon Polygon
        {
            get 
            {                
                return currentPolygon;            
            }
            set
            {
                currentPolygon = value;
            }
        }

        /// <summary>
        /// Get/set the secondary markers, this markers will be show, but does not affect the drawing of the main polygon
        /// </summary>
        public List<GMapMarker> SecondaryMarkers
        {
            get { return _secondaryMarkers; }
            set { _secondaryMarkers = value; }
        }

        /// <summary>
        /// Get/set the secondary markers, this polygons will be show, but does not affect the drawing of the main polygon
        /// </summary>
        public List<GMapPolygon> SecondaryPolygons
        {
            get { return _secondaryPolygons; }
            set { _secondaryPolygons = value; }
        }

        /// <summary>
        /// Get set the address to center the map
        /// </summary>
        public string Address
        {
            get { return txtAddress.Text; }
            set { txtAddress.Text = value; }
        }

        public MapType MapType
        {
            get { return _mapType; }
            set { _mapType = value; }
        }

        public int MapZoom
        {
            get { return _mapZoom; }
            set { _mapZoom = value; }
        }

        public bool AllowDrawPolygon
        {
            get { return _allowDrawPolygon; }
            set { _allowDrawPolygon = value; }
        }
	
        #endregion        

        #region Constructors
        public frmGeoPolygonTest()
        {
            //contruct fields
            _secondaryMarkers = new List<GMapMarker>();
            _secondaryPolygons = new List<GMapPolygon>();
            _mapType = MapType.GoogleMap;
            _mapZoom = 15;

            currentPolygon = new GMapPolygon(new List<PointLatLng>(), "MyPolygon");
            
            InitializeComponent();
        }
        #endregion
        private void frmGeoArea_Load(object sender, EventArgs e)
        {          
            
            //Load comboboxes
            cboMapType.DataSource = Enum.GetValues(_mapType.GetType());

            //Config map
            ConfigMap();

            //Set the data to show on map

            //get the center of the markers
            PointLatLng? middle = null;
            if (currentPolygon.Points.Count > 0)
            {
                middle = Functions.CalculateMiddlePoint(currentPolygon);
            }
            else
                if (_secondaryMarkers.Count > 0)
                    middle = CalculateMiddlePoint(_secondaryMarkers);

            //set the center of the map
            if (middle.HasValue)
            {
                MainMap.CurrentPosition = middle.Value;
                center = new GMapMarkerCross(MainMap.CurrentPosition);
                top.Markers.Add(center);

                // set current marker
                if (!_allowDrawPolygon)
                {
                    currentMarker = new GMapMarkerGoogleRed(MainMap.CurrentPosition);
                    top.Markers.Add(currentMarker);
                }
            }

            //add the secondary markers in the top layer
            foreach (var mark in _secondaryMarkers)
            {
                top.Markers.Add(mark);
            }

            foreach (GMapPolygon polygon in _secondaryPolygons)
            {
                top.Polygons.Add(polygon);
            }

            //config the polygon             
            top.Polygons.Add(currentPolygon);

            if (_allowDrawPolygon)
                MainMap.SetDrawingPolygon(currentPolygon);

            if (center == null)
                GoToAddress(Address);            

        }

        private void ConfigMap()
        {
            // config gmaps
            GMaps.Instance.UseRouteCache = true;
            GMaps.Instance.UseGeocoderCache = true;
            GMaps.Instance.UsePlacemarkCache = true;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // config map 
            MainMap.MapType = _mapType;
            MainMap.MaxZoom = 20;
            MainMap.MinZoom = 5;
            MainMap.Zoom = _mapZoom;

            MainMap.CurrentPosition = new PointLatLng();
            MainMap.PolygonsEnabled = true;
            MainMap.AllowDrawPolygon = _allowDrawPolygon;

            // map events
            MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            MainMap.OnMapZoomChanged += new MapZoomChanged(this.MainMap_OnMapZoomChanged);

            if (!_allowDrawPolygon)
            {
                MainMap.MouseMove += new MouseEventHandler(MainMap_MouseMove);
                MainMap.MouseDown += new MouseEventHandler(MainMap_MouseDown);
                MainMap.MouseUp += new MouseEventHandler(MainMap_MouseUp);
            }
            // get zoom  
            trackBarZoom.Minimum = MainMap.MinZoom;
            trackBarZoom.Maximum = MainMap.MaxZoom;
            trackBarZoom.Value = Convert.ToInt32(MainMap.Zoom);

            // add custom layers  
            {
                top = new GMapOverlay(MainMap, "top");
                MainMap.Overlays.Add(top);
            }

            MainMap.ZoomAndCenterMarkers("top");
        }

        private void GoToAddress(string keywordsToSearch)
        {

            GeoCoderStatusCode status = MainMap.SetCurrentPositionByKeywords(keywordsToSearch);
            if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                MessageBox.Show("Google Maps Geocoder can't find: '" + txtAddress.Text + "', reason: " + status.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // set current marker
            if (!_allowDrawPolygon)
            {
                if (currentMarker == null)
                {
                    currentMarker = new GMapMarkerGoogleRed(new PointLatLng());
                    top.Markers.Add(currentMarker);
                }

                currentMarker.Position = MainMap.CurrentPosition;
            }

            if (center == null)
                center = new GMapMarkerCross(new PointLatLng());
            
            center.Position = MainMap.CurrentPosition;
            if(!top.Markers.Contains(center))
                top.Markers.Add(center);

            txtLat.Text = MainMap.CurrentPosition.Lat.ToString(CultureInfo.CurrentCulture);
            txtLng.Text = MainMap.CurrentPosition.Lng.ToString(CultureInfo.CurrentCulture);
        }

        #region Map event methods

        private void MainMap_OnMapZoomChanged()
        {
            trackBarZoom.Value = Convert.ToInt32(MainMap.Zoom);
        }

        // current point changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            center.Position = point;
        }

        void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                UpdateCurrentMarkerPositionText();
            }
        }

        void MainMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }

        void UpdateCurrentMarkerPositionText()
        {
            txtLat.Text = currentMarker.Position.Lat.ToString(CultureInfo.InvariantCulture);
            txtLng.Text = currentMarker.Position.Lng.ToString(CultureInfo.InvariantCulture);
        }

        void MainMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                UpdateCurrentMarkerPositionText();
            }
        }
        
        #endregion        

        #region Controls events methods

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnGenImage_Click(object sender, EventArgs e)
        {
            //calculate the area to print
            if (currentPolygon.Points.Count > 0)
            {
                MainMap.SelectedArea = CalculateRectangle(currentPolygon.Points);
            }
            else
                MainMap.SelectedArea = CalculateRectangle(_secondaryMarkers.Select(m => m.Position).ToList());

            MainMap.SelectedArea = AddMargin(MainMap.SelectedArea);


            if (!MainMap.SelectedArea.IsEmpty)
            {
                StaticImage st = new StaticImage(MainMap);
                st.Owner = this;
                st.Show();
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cboMapType_SelectedValueChanged(object sender, EventArgs e)
        {
            MainMap.MapType = (MapType)cboMapType.SelectedValue;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            GoToAddress(txtAddress.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            MainMap.ClearDrawingPolygon();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            MainMap.Zoom = trackBarZoom.Value;
        }

        #endregion

        #region Auxiliar functions

        private PointLatLng CalculateMiddlePoint(params GMapMarker[] marks)
        {
            return CalculateMiddlePoint(marks.ToList());
        }

        private PointLatLng CalculateMiddlePoint(List<GMapMarker> marks)
        {
            List<PointLatLng> points = marks.Select(m => m.Position).ToList();

            PointLatLng point = Functions.CalculateMiddlePoint(points.ToList());

            return point;

        }

        private PointLatLng CalculateMiddlePoint(List<PointLatLng> marks)
        {

            PointLatLng point = Functions.CalculateMiddlePoint(marks);

            return point;
        }        

        private RectLatLng CalculateRectangle(IList<PointLatLng> points)
        {
            RectLatLng rect = new RectLatLng();

            if (points.Count > 1)
            {
                double maxLat = points.Max(p => p.Lat);
                double minLat = points.Min(p => p.Lat);

                double maxLng = points.Max(p => p.Lng);
                double minLng = points.Min(p => p.Lng);

                double widthLat = maxLat - minLat;
                double heightLng = maxLng - minLng;

                rect = new RectLatLng(maxLat, minLng, heightLng, widthLat);

            }
            else
            {
                if (points.Count > 0)
                {
                    SizeLatLng size = new SizeLatLng(0.005, 0.009);
                    PointLatLng point = new PointLatLng(points[0].Lat + 0.0025, points[0].Lng - 0.0045);
                    rect = new RectLatLng(point, size);

                }
            }
            return rect;
        }

        private RectLatLng AddMargin(RectLatLng rect)
        {
            rect.LocationTopLeft = new PointLatLng(rect.LocationTopLeft.Lat + 0.0009, rect.LocationTopLeft.Lng - 0.002);
            rect.HeightLat = rect.HeightLat + 0.0018;
            rect.WidthLng = rect.WidthLng + 0.004;

            return rect;
        }

        #endregion

        

        

        


    }
}
