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

namespace ZeqkTools.WindowsForms.Maps
{
    public partial class frmGeoPolygon : Form
    {
        #region Fields
        private MapType _mapType;
        private int _mapZoom;
        private bool _allowDrawPolygon;        
               
        private GMapPolygon _polygon;
        private List<GMapMarker> _secondaryMarkers;
        private List<GMapPolygon> _secondaryPolygons;
        #endregion

        #region Internal variables  

        // markers
        GMapMarker center;
        GMapMarker selectedVertice;
        GMapMarker selectedIntermediatePoint;

        // layers
        GMapOverlay top;
        GMapOverlay vertices;
        GMapOverlay auxiliar;
        GMapOverlay polygonsRects;

        bool isMouseDown;
        bool polygonIsComplete = false;
        bool isDraggingVertice = false;
        bool isDraggingIntermediatePoint = false;
        #endregion

        #region Public properties

        /// <summary>
        /// Get set the main polygon
        /// </summary>
        public GMapPolygon Polygon
        {
            get 
            {                
                return _polygon;            
            }
            set
            {
                _polygon = value;
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
        public frmGeoPolygon()
        {
            //contruct fields
            _secondaryMarkers = new List<GMapMarker>();            
            _mapType = MapType.GoogleMap;
            _mapZoom = 15;
            _allowDrawPolygon = true;

            _polygon = new GMapPolygon(new List<PointLatLng>(), "MyPolygon");
            
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
            if (_polygon.Points.Count > 0)
            {
                middle = Functions.CalculateMiddlePoint(_polygon);
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

            //draw the polygon
            if (_polygon.Points.Count > 2)
            {
                //go through the polygon vertices
                for (int i = 0; i < _polygon.Points.Count; i++)
                {
                    GMapMarkerWitheSquare vertice = null;

                    //the first and last vertice have the same coordinate, so they are the same object
                    if (i == _polygon.Points.Count - 1) //only the last vertice
                        vertice = (GMapMarkerWitheSquare)vertices.Markers[0];
                    else
                        vertice = new GMapMarkerWitheSquare(_polygon.Points[i]);

                    vertices.Markers.Add(vertice);

                    //start to add intermediate points from the second vertice
                    if (i > 0)
                    {
                        PointLatLng intermedium = CalculateMiddlePoint(vertices.Markers[i - 1], vertices.Markers[i]);
                        GMapMarkerGraySquare intermediatePoint = new GMapMarkerGraySquare(intermedium);
                        auxiliar.Markers.Add(intermediatePoint);
                    }
                }
                polygonIsComplete = true;
            }
            else
                _polygon = new GMapPolygon(new List<PointLatLng>(), "MyPolygon");

            
            top.Polygons.Add(_polygon);
            

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

            // map events
            MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            //MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
            //MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);
            MainMap.OnMarkerClick += new MarkerClick(MainMap_OnMarkerClick);
            //MainMap.OnEmptyTileError += new EmptyTileError(MainMap_OnEmptyTileError);
            //MainMap.OnMapTypeChanged += new MapTypeChanged(MainMap_OnMapTypeChanged);
            MainMap.MouseMove += new MouseEventHandler(MainMap_MouseMove);
            MainMap.MouseDown += new MouseEventHandler(MainMap_MouseDown);
            MainMap.MouseUp += new MouseEventHandler(MainMap_MouseUp);
            MainMap.OnMarkerEnter += new MarkerEnter(this.MainMap_OnMarkerEnter);
            MainMap.OnMarkerLeave += new MarkerLeave(this.MainMap_OnMarkerLeave);
            MainMap.OnMapZoomChanged += new MapZoomChanged(this.MainMap_OnMapZoomChanged);

            // get zoom  
            trackBar1.Minimum = MainMap.MinZoom;
            trackBar1.Maximum = MainMap.MaxZoom;
            trackBar1.Value = Convert.ToInt32(MainMap.Zoom);

            // add custom layers  
            {

                top = new GMapOverlay(MainMap, "top");
                MainMap.Overlays.Add(top);
                auxiliar = new GMapOverlay(MainMap, "auxiliar");
                MainMap.Overlays.Add(auxiliar);
                vertices = new GMapOverlay(MainMap, "vertices");
                MainMap.Overlays.Add(vertices);
                polygonsRects = new GMapOverlay(MainMap, "polygonsRects");
                MainMap.Overlays.Add(polygonsRects);
            }

            MainMap.ZoomAndCenterMarkers(null);
        }

        private void GoToAddress(string keywordToSearch)
        {

            GeoCoderStatusCode status = MainMap.SetCurrentPositionByKeywords(keywordToSearch);
            if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                MessageBox.Show("Google Maps Geocoder can't find: '" + txtAddress.Text + "', reason: " + status.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            center = new GMapMarkerCross(MainMap.CurrentPosition);
            top.Markers.Add(center);

            txtLat.Text = MainMap.CurrentPosition.Lat.ToString(CultureInfo.CurrentCulture);
            txtLng.Text = MainMap.CurrentPosition.Lng.ToString(CultureInfo.CurrentCulture);
        }

        #region Map event methods

        private void MainMap_OnMapZoomChanged()
        {
            trackBar1.Value = Convert.ToInt32(MainMap.Zoom);
        }

        // current point changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            center.Position = point;
        }

        void MainMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;

                if (_allowDrawPolygon)
                {
                    //OnDrop
                    if (selectedVertice != null)
                    {

                        //until the poligon is complete, make auxiliary lines joining the vertices
                        if (!polygonIsComplete && vertices.Markers.Count > 1)
                        {
                            int verticeIndex = vertices.Markers.IndexOf(selectedVertice);
                            Pen pen = new Pen(Brushes.DarkGray, 3);
                            GMapMarkerLine auxLine = new GMapMarkerLine(selectedVertice.Position, vertices.Markers[verticeIndex - 1].Position, pen);
                            auxiliar.Markers.Add(auxLine);
                        }


                        if (isDraggingVertice)
                        {
                            isDraggingVertice = false;

                            if (polygonIsComplete)
                            {
                                _polygon.Points.Clear();
                                _polygon.Points.AddRange(vertices.Markers.Select(m => m.Position));
                                MainMap.UpdatePolygonLocalPosition(_polygon);

                                //rearrangement intermediate points
                                int selectedIndex = vertices.Markers.IndexOf(selectedVertice);
                                //previous point
                                if (selectedIndex == 0)
                                {
                                    GMapMarker intermediatePoint = auxiliar.Markers.Last();
                                    intermediatePoint.Position = CalculateMiddlePoint(vertices.Markers[vertices.Markers.Count - 2], vertices.Markers[selectedIndex]);
                                }
                                else
                                    auxiliar.Markers[selectedIndex - 1].Position = CalculateMiddlePoint(vertices.Markers[selectedIndex - 1], vertices.Markers[selectedIndex]);
                                //next point
                                auxiliar.Markers[selectedIndex].Position = CalculateMiddlePoint(vertices.Markers[selectedIndex], vertices.Markers[selectedIndex + 1]);
                            }
                        }

                        selectedVertice = null;
                    }

                    //if dragging intermediate point dragging is finish
                    if (selectedIntermediatePoint != null)
                    {
                        if (isDraggingIntermediatePoint)
                        {
                            isDraggingIntermediatePoint = false;
                            //make a new vertice
                            GMapMarkerWitheSquare newVertice = new GMapMarkerWitheSquare(selectedIntermediatePoint.Position);
                            //remove the old intermediate point
                            int selectedIndex = auxiliar.Markers.IndexOf(selectedIntermediatePoint);
                            auxiliar.Markers.Remove(selectedIntermediatePoint);

                            //add the new vertice, in the correct position of the vertices collection
                            int newVerticeIndex = selectedIndex + 1;

                            vertices.Markers.Insert(newVerticeIndex, newVertice);
                            
                            //update polygon
                            _polygon.Points.Clear();
                            _polygon.Points.AddRange(vertices.Markers.Select(m => m.Position));
                            MainMap.UpdatePolygonLocalPosition(_polygon);

                            //make and add new intermediate points
                            PointLatLng intermediatePosition1 = CalculateMiddlePoint(vertices.Markers[newVerticeIndex - 1], vertices.Markers[newVerticeIndex]);
                            GMapMarkerGraySquare intermediatePoint1 = new GMapMarkerGraySquare(intermediatePosition1);

                            PointLatLng intermediatePosition2 = CalculateMiddlePoint(vertices.Markers[newVerticeIndex], vertices.Markers[newVerticeIndex + 1]);
                            GMapMarkerGraySquare intermediatePoint2 = new GMapMarkerGraySquare(intermediatePosition2);

                            auxiliar.Markers.Insert(selectedIndex, intermediatePoint1);
                            auxiliar.Markers.Insert(selectedIndex + 1, intermediatePoint2);

                        }

                        selectedIntermediatePoint = null;
                    }
                }
            }
        }

        void MainMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                if (_allowDrawPolygon)
                {
                    //if the polygon is incomplete, click will be create new vertices
                    if (!polygonIsComplete)
                        if (selectedVertice == null)
                        {
                            GMapMarkerWitheSquare marker = new GMapMarkerWitheSquare(MainMap.FromLocalToLatLng(e.X, e.Y));
                            vertices.Markers.Add(marker);
                            this.selectedVertice = marker;

                        }
                }
            }
        }

        private void MainMap_OnMarkerClick(GMapMarker item)
        {
            if (_allowDrawPolygon)
            {
                //only can create the polygon if the polygon is incomplete
                if (!polygonIsComplete)
                {
                    //the polygon only be closed if the click is on the first vertice, and is not the first time
                    if (vertices.Markers.First() == this.selectedVertice && vertices.Markers.Count > 1)
                    {
                        this.vertices.Markers.Add(this.selectedVertice);
                        
                        //add the vertices to polygon (make polygon)
                        _polygon.Points.AddRange(vertices.Markers.Select(m => m.Position));
                        MainMap.UpdatePolygonLocalPosition(_polygon);

                        polygonIsComplete = true;
                        auxiliar.Markers.Clear();
                        //add new intermediate points between the vertices
                        for (int i = 0; i < vertices.Markers.Count; i++)
                        {
                            if (i != vertices.Markers.Count - 1)
                            {
                                PointLatLng middle = CalculateMiddlePoint(vertices.Markers[i], vertices.Markers[i + 1]);
                                GMapMarkerGraySquare intermediatePoint = new GMapMarkerGraySquare(middle);
                                auxiliar.Markers.Add(intermediatePoint);
                            }
                        }
                    }
                }
            }
                
        }

        private void MainMap_OnMarkerEnter(GMapMarker item)
        {
            if (_allowDrawPolygon)
            {
                //select the marker that was clicked
                if (vertices.Markers.Contains(item))
                    this.selectedVertice = item;

                if (auxiliar.Markers.Contains(item))
                    this.selectedIntermediatePoint = item;
            }


            
        }

        private void MainMap_OnMarkerLeave(GMapMarker item)
        {
            //if the marker is being dragged, then is not deselected
            if (!isDraggingVertice)
                selectedVertice = null;

            if (!isDraggingIntermediatePoint)
                selectedIntermediatePoint = null;
        }

        private void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                if (_allowDrawPolygon)
                {
                    //if there is a selected vertice
                    if (this.selectedVertice != null)
                    {
                        //drag the selected vertice
                        this.selectedVertice.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                        isDraggingVertice = true;
                    }
                    else
                    {
                        //else, if there is a selected intermediate point, be dragg
                        if (this.selectedIntermediatePoint != null)
                        {
                            this.selectedIntermediatePoint.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                            isDraggingIntermediatePoint = true;
                        }
                    }
                }
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
            if (_polygon.Points.Count > 0)
            {
                MainMap.SelectedArea = CalculateRectangle(_polygon.Points);
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
            auxiliar.Markers.Clear();
            vertices.Markers.Clear();

            //clear polygon
            _polygon.Points.Clear();
            MainMap.UpdatePolygonLocalPosition(_polygon);

            polygonIsComplete = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            MainMap.Zoom = trackBar1.Value;
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


        RectLatLng polygonToRect(GMapPolygon polygon)
        {
            double maxLat = polygon.Points.Max(p => p.Lat);
            double minLat = polygon.Points.Min(p => p.Lat);

            double maxLng = polygon.Points.Max(p => p.Lng);
            double minLng = polygon.Points.Min(p => p.Lng);

            double widthLat = maxLat - minLat;
            double heightLng = maxLng - minLng;

            RectLatLng rect = new RectLatLng(maxLat, minLng, heightLng, widthLat);

            return rect;
        }

        void hola(GMapPolygon polygon)
        {
            RectLatLng rect = polygonToRect(polygon);
            
        }


    }
}
