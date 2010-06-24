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
using System.Collections;
using AltosTools.WindowsForms.Maps;
using AltosTools.MapsTest;

namespace AltosTools.MapsTest
{
    public partial class frmMapTest : Form
    {
        #region Fields
        public object Object;

        private MapType _mapType;
        private int _mapZoom;
        private MapModeEnum _mapMode;

        private List<GMapPolygon> _otherPolygons;
        private List<GMapMarker> _otherMarkers;
        
        #endregion

        #region Internal variables  

        GMapPolygon currentPolygon;
        // markers
        GMapMarker centerMarker;
        GMapMarker currentMarker;

        // layers
        GMapOverlay top;

        bool isMouseDown;
        #endregion

        #region Public properties

        /// <summary>
        /// Get set the main polygon
        /// </summary>
        public GMapPolygon MainPolygon
        {
            get 
            {                
                return currentPolygon;            
            }
        }

        /// <summary>
        /// Get set the main marker
        /// </summary>
        public GMapMarker MainMarker
        {
            get
            {
                return currentMarker;
            }
        }

        /// <summary>
        /// Get set the address to center the map
        /// </summary>
        public string Address
        {
            get { return txtAddress.Text; }
            set { txtAddress.Text = value; }
        }

        public List<GMapPolygon> OtherPolygons
        {
            get 
            {
                List<GMapPolygon> rv = top.Polygons.ToList();
                rv.Remove(currentPolygon);
                return rv;
            }
            set
            {
                _otherPolygons = value;
            }
        }

        public List<GMapMarker> OtherMarkers
        {
            get
            {
                List<GMapMarker> rv = top.Markers.ToList();
                rv.Remove(currentMarker);
                return rv;
            }
            set
            {
                _otherMarkers = value;
            }
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

        public MapModeEnum MapMode
        {
            get { return _mapMode; }
            set { _mapMode = value; }
        }
	
        #endregion        

        #region Constructors
        public frmMapTest()
        {
            //contruct fields
            _mapType = MapType.GoogleMap;
            _mapZoom = 15;
            _mapMode = MapModeEnum.ReadOnly;

            //initializing
            currentPolygon = new GMapPolygon(new List<PointLatLng>(), "MyPolygon");
            currentMarker = new GMapMarkerGoogleRed(new PointLatLng());

            InitializeComponent();
        }
        #endregion
        private void frmGeoArea_Load(object sender, EventArgs e)
        {
            ConfigureAdditionalData();            

            //load comboboxes
            cboMapType.DataSource = Enum.GetValues(_mapType.GetType());

            //config map
            ConfigMap();

            //extract data from the Object property
            ExtractObjectData();

            if (_otherMarkers != null)
            {
                foreach (GMapMarker item in _otherMarkers)
                    top.Markers.Add(item);
            }

            if (_otherPolygons != null)
            {
                foreach (GMapPolygon item in _otherPolygons)
                    top.Polygons.Add(item);
            }

            PointLatLng? center = null;
            switch (_mapMode)
            {
                case MapModeEnum.EditPoint:
                    center = GetMainMarkerCenter();
                    if (center == null)
                        center = GetOtherPolygonsAndMarkersCenter();
                    break;
                case MapModeEnum.EditArea:
                    center = GetMainPolygonCenter();
                    if (center == null)
                        center = GetOtherPolygonsAndMarkersCenter();
                    break;
                case MapModeEnum.ReadOnly:
                    center = GetOtherPolygonsAndMarkersCenter();
                    break;
                default:
                    break;
            }

            SetCenter(center);
            //MainMap.ZoomAndCenterMarkers("top");
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
            if (_mapMode == MapModeEnum.EditArea)
                MainMap.AllowDrawPolygon = true;
            else
                MainMap.AllowDrawPolygon = false;

            // map events
            MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            MainMap.OnMapZoomChanged += new MapZoomChanged(this.MainMap_OnMapZoomChanged);

            if (_mapMode == MapModeEnum.EditArea)
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

        private void SetCenter(PointLatLng? center)
        {
            if (center != null)
            {
                MainMap.CurrentPosition = center.Value;

                if (centerMarker == null)
                    centerMarker = new GMapMarkerCross(new PointLatLng());

                centerMarker.Position = MainMap.CurrentPosition;
                if (!top.Markers.Contains(centerMarker))
                    top.Markers.Add(centerMarker);
            }
            else
            {
                GoToAddress(this.Address);
                centerMarker.Position = MainMap.CurrentPosition;
                if (!top.Markers.Contains(centerMarker))
                    top.Markers.Add(centerMarker);
            }

            txtLat.Text = MainMap.CurrentPosition.Lat.ToString(CultureInfo.CurrentCulture);
            txtLng.Text = MainMap.CurrentPosition.Lng.ToString(CultureInfo.CurrentCulture);
        }

        private void GoToAddress(string keywordsToSearch)
        {

            GeoCoderStatusCode status = MainMap.SetCurrentPositionByKeywords(keywordsToSearch);
            if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                MessageBox.Show("Google Maps Geocoder can't find: '" + txtAddress.Text + "', reason: " + status.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // set current marker
            if (_mapMode == MapModeEnum.EditPoint)
            {
                if (!top.Markers.Contains(currentMarker))
                    top.Markers.Add(currentMarker);

                currentMarker.Position = MainMap.CurrentPosition;
            }            
            
        }

        void ExtractObjectData()
        {
            //to implement
        }

        #region Map event methods

        private void MainMap_OnMapZoomChanged()
        {
            trackBarZoom.Value = Convert.ToInt32(MainMap.Zoom);
        }

        // current point changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            centerMarker.Position = point;
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
                MainMap.SelectedArea = CalculateRectangle(currentPolygon.Points);
            else
                //MainMap.SelectedArea = CalculateRectangle(_secondaryMarkers.Select(m => m.Position).ToList()); TODO: generar el rectangulo segun los poligonos secundarios

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
            SetCenter(null); //Set center by MainMap.CurrentPosition
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

            PointLatLng point = AltosTools.Functions.CalculateMiddlePoint(points.ToList());

            return point;

        }

        private PointLatLng CalculateMiddlePoint(List<PointLatLng> marks)
        {

            PointLatLng point = AltosTools.Functions.CalculateMiddlePoint(marks);

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

        private void chklstTerritory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void ConfigureAdditionalData()
        {

            //to implement

        }

        #region GetCenter methods

        PointLatLng? GetMainPolygonCenter()
        {
            PointLatLng? center = null;

            if(currentPolygon.Points.Count > 0)                
                center = AltosTools.Functions.CalculateMiddlePoint(currentPolygon);

            return center;            
        }

        PointLatLng? GetMainMarkerCenter()
        {
            PointLatLng? center = null;
            if(currentMarker != null)
                center = currentMarker.Position;
            return center;
        }

        PointLatLng? GetOtherPolygonsAndMarkersCenter()
        {
            PointLatLng? center = null;

            List<PointLatLng> centers = new List<PointLatLng>();

            if (_otherPolygons != null && _otherPolygons.Count > 0)
                centers.Add(AltosTools.Functions.CalculateMiddlePoint(_otherPolygons[0]));

            if (_otherMarkers != null && _otherMarkers.Count > 0)
                centers.Add(CalculateMiddlePoint(_otherMarkers));

            if (centers.Count > 0)
                center = AltosTools.Functions.CalculateMiddlePoint(centers);

            return center;
        }     

        #endregion






    }
}
