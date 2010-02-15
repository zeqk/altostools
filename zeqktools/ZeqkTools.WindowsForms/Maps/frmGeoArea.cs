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
    public partial class frmGeoArea : Form
    {
        public MapType MapType = MapType.GoogleMap;
        

        public List<PointLatLng> _polygon;
        public List<GMapMarker> Points;
        private List<GMapMarker> _auxPoints;

        public List<PointLatLng> Polygon
        {
            get 
            {
                return this.top.Markers.OfType<GMapMarkerPolygon>().First().GeoPoints;            
            }
            set
            {
                _polygon = value;
            }
        }

        public string Address
        {
            get { return txtAddress.Text; }
            set { txtAddress.Text = value; }
        }
	
	

        // marker
        GMapMarker center;

        // layers
        GMapOverlay top;
        GMapOverlay vertices;
        GMapOverlay auxiliar;
        GMapOverlay auxiliarData;

        bool isMouseDown;
        bool polygonIsComplete = false;
        bool isDraggingVertice = false;
        bool isDraggingAuxiliar = false;
        
        GMapMarker selectedVertice;
        GMapMarker selectedAuxiliar;


        public frmGeoArea()
        {
            _polygon = new List<PointLatLng>();
            Points = new List<GMapMarker>();
            _auxPoints = new List<GMapMarker>();
            InitializeComponent();
        }

        private void frmGeoArea_Load(object sender, EventArgs e)
        {

            cboMapType.DataSource = Enum.GetValues(MapType.GetType());

            ConfigMap();
            //Seteo el centro del mapa
            PointLatLng middle = new PointLatLng(0, 0);
            if (_polygon.Count > 0)
            {
                List<GeoPoint> points = _polygon.Select(p => new GeoPoint(p.Lat, p.Lng)).ToList();
                GeoPoint point = Functions.CalculateMiddlePoint(points);
                middle.Lat = point.Lat;
                middle.Lng = point.Lng;
            }
            else
                if (Points.Count > 0)
                    middle = CalculateMiddlePoint(Points);

            MainMap.CurrentPosition = middle;

            center = new GMapMarkerCross(MainMap.CurrentPosition);
            top.Markers.Add(center);

            if (_polygon.Count > 2)
            {
                //recorro los vertices del polígono
                for (int i = 0; i < _polygon.Count; i++)
                {
                    GMapMarkerWitheSquare vertice = null;

                    //ya que el primer y el último vertice tienen las misma cordenadas, hago también que sean el mismo objeto
                    if (i == _polygon.Count - 1)
                        vertice = (GMapMarkerWitheSquare)vertices.Markers[0];
                    else
                        vertice = new GMapMarkerWitheSquare(_polygon[i]);

                    vertices.Markers.Add(vertice);
                    DrawPloygon(vertices.Markers.ToList());
                    polygonIsComplete = true;

                    //empiezo a agregar los marcadores auxiliares a partir del segundo vertice
                    if (i > 0)
                    {
                        PointLatLng auxMiddle = CalculateMiddlePoint(vertices.Markers[i - 1], vertices.Markers[i]);
                        GMapMarkerGraySquare auxMark = new GMapMarkerGraySquare(auxMiddle);
                        auxiliar.Markers.Add(auxMark);
                    }
                }
            }
            foreach (var mark in Points)
                auxiliarData.Markers.Add(mark);

            if (middle.IsEmpty && !string.IsNullOrEmpty(txtAddress.Text))
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
            MainMap.MapType = MapType;
            MainMap.MaxZoom = 20;
            MainMap.MinZoom = 5;
            MainMap.Zoom = 16;

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
                auxiliarData = new GMapOverlay(MainMap, "auxiliarData");
                MainMap.Overlays.Add(auxiliarData);

            }

            MainMap.ZoomAndCenterMarkers(null);
        }

        // current point changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            center.Position = point;
        }       

        

        #region Auxiliar functions

        private PointLatLng CalculateMiddlePoint(params GMapMarker[] marks)
        {
            return CalculateMiddlePoint(marks.ToList());
        }

        private PointLatLng CalculateMiddlePoint(List<GMapMarker> marks)
        {
            var points = marks.Select(m => new GeoPoint(m.Position.Lat,m.Position.Lng));

            var aux = Functions.CalculateMiddlePoint(points.ToList());
            PointLatLng point = new PointLatLng(aux.Lat, aux.Lng);
            
            return point;

        }

        private PointLatLng CalculateMiddlePoint(List<PointLatLng> marks)
        {
            var points = marks.Select(m => new GeoPoint(m.Lat, m.Lng));

            var aux = Functions.CalculateMiddlePoint(points.ToList());
            PointLatLng point = new PointLatLng(aux.Lat, aux.Lng);

            return point;
        }

        private RectLatLng CalculateRectangle(IList<GMapMarker> marks)
        {
            RectLatLng rect = new RectLatLng();

            if (marks.Count > 1)
            {
                double maxLat = marks.Max(m => m.Position.Lat);
                double minLat = marks.Min(m => m.Position.Lat);

                double maxLng = marks.Max(m => m.Position.Lng);
                double minLng = marks.Min(m => m.Position.Lng);

                double widthLat = maxLat - minLat;
                double heightLng = maxLng - minLng;

                rect = new RectLatLng(maxLat, minLng, heightLng, widthLat);
            }
            else
            {
                if (marks.Count > 0)
                {
                    SizeLatLng size = new SizeLatLng(0.005, 0.009);
                    PointLatLng point = new PointLatLng(marks[0].Position.Lat + 0.0025, marks[0].Position.Lng - 0.0045);
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
        private void DrawPloygon(List<GMapMarker> vertices)
        {
            var center = CalculateMiddlePoint(vertices);
            GMapMarkerPolygon polygon = null;
            //si ya hay un poligono, lo remuevo
            if (this.top.Markers.OfType<GMapMarkerPolygon>().Count() > 0)
            {
                polygon = (GMapMarkerPolygon)this.top.Markers.Where(m => m.GetType() == typeof(GMapMarkerPolygon)).First();
                top.Markers.Remove(polygon);
            }
            Pen pen = new Pen(Brushes.Blue, 3);

            polygon = new GMapMarkerPolygon(center, vertices.Select(m => m.Position).ToList(),pen);
            top.Markers.Add(polygon);
        }


        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (polygonIsComplete)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show("Polygon is incomplete");
        }

        private void btnGenImage_Click(object sender, EventArgs e)
        {
            if (top.Markers[1].GetType() == typeof(GMapMarkerPolygon))
            {
                GMapMarkerPolygon polygon = (GMapMarkerPolygon)top.Markers[1];
                double maxLat = polygon.GeoPoints.Max(p => p.Lat);
                double minLat = polygon.GeoPoints.Min(p => p.Lat);

                double maxLng = polygon.GeoPoints.Max(p => p.Lng);
                double minLng = polygon.GeoPoints.Min(p => p.Lng);
                RectLatLng area = new RectLatLng(maxLat, minLng, maxLng - minLng, maxLat - minLat);
                MainMap.SelectedArea = area;
            }
            else
                MainMap.SelectedArea = CalculateRectangle(Points);

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



        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            MainMap.Zoom = trackBar1.Value;
        }

        private void MainMap_OnMapZoomChanged()
        {
            trackBar1.Value = Convert.ToInt32(MainMap.Zoom);
        }

        private void cboMapType_SelectedValueChanged(object sender, EventArgs e)
        {
            MainMap.MapType = (MapType)cboMapType.SelectedValue;
        }

        void MainMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false; //se suelta el botón izquierda

                //OnDrop
                //si hay algún marcador seleccionado, se deselecciona
                if (selectedVertice != null)
                {

                    //hasta que el polígono esté completo, creo lineas auxiliares que unan los vertices
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
                            DrawPloygon(vertices.Markers.ToList());

                            //reacomodo los marcadores auxiliares intermedios
                            int selectedIndex = vertices.Markers.IndexOf(selectedVertice);
                            if (selectedIndex == 0)
                            {
                                GMapMarker marker = auxiliar.Markers.Last();

                                marker.Position = CalculateMiddlePoint(vertices.Markers[vertices.Markers.Count - 2], vertices.Markers[selectedIndex]);
                            }
                            else
                                auxiliar.Markers[selectedIndex - 1].Position = CalculateMiddlePoint(vertices.Markers[selectedIndex - 1], vertices.Markers[selectedIndex]);

                            auxiliar.Markers[selectedIndex].Position = CalculateMiddlePoint(vertices.Markers[selectedIndex], vertices.Markers[selectedIndex + 1]);
                            


                        }
                    }

                    selectedVertice = null;
                }
                //si termine de arrastrar un marcador auxiliar
                if (selectedAuxiliar != null)
                {
                    if (isDraggingAuxiliar)
                    {
                        isDraggingAuxiliar = false;
                        //creo un nuevo vertice
                        GMapMarkerWitheSquare newVertice = new GMapMarkerWitheSquare(selectedAuxiliar.Position);                        

                        int selectedIndex = auxiliar.Markers.IndexOf(selectedAuxiliar);
                        auxiliar.Markers.Remove(selectedAuxiliar);


                        //agrego el vertice pero en el orden correcto
                        int newVerticeIndex = selectedIndex + 1;

                        vertices.Markers.Insert(newVerticeIndex, newVertice);
                        DrawPloygon(vertices.Markers.ToList());

                        //dibujo los nuevos marcadores auxiliares
                        PointLatLng auxMarker1Position = CalculateMiddlePoint(vertices.Markers[newVerticeIndex - 1], vertices.Markers[newVerticeIndex]);
                        GMapMarkerGraySquare auxMarker1 = new GMapMarkerGraySquare(auxMarker1Position);                        

                        PointLatLng auxMarker2Position = CalculateMiddlePoint(vertices.Markers[newVerticeIndex], vertices.Markers[newVerticeIndex + 1]);
                        GMapMarkerGraySquare auxMarker2 = new GMapMarkerGraySquare(auxMarker2Position);                        

                        auxiliar.Markers.Insert(selectedIndex, auxMarker1);
                        auxiliar.Markers.Insert(selectedIndex + 1, auxMarker2);

                    }

                    selectedAuxiliar = null;
                }
            }
        }

        void MainMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                //sólo se crear nuevos marcadores con el click, si el polígono está incompleto
                if (!polygonIsComplete)
                    if (selectedVertice == null)
                    {
                        GMapMarkerWitheSquare marker = new GMapMarkerWitheSquare(MainMap.FromLocalToLatLng(e.X, e.Y));                        
                        vertices.Markers.Add(marker);
                        this.selectedVertice = marker;
                        
                    }
            }
        }

        private void MainMap_OnMarkerClick(GMapMarker item)
        {
            //solo puedo crear el polígono si el polígon está incompleto
            if (!polygonIsComplete)
            {

                //sólo puedo cerrar el polígono si selecciono el primero vertice, y no sea la primera vez
                if (vertices.Markers.First() == this.selectedVertice && vertices.Markers.Count > 1)
                {
                    
                    this.vertices.Markers.Add(this.selectedVertice);
                    //creo el polígono
                    DrawPloygon(vertices.Markers.ToList());                    
                    polygonIsComplete = true;                    
                    auxiliar.Markers.Clear();
                    //genero  marcadores intermedios entre los vertices
                    for (int i = 0; i < vertices.Markers.Count; i++)
                    {
                        if (i != vertices.Markers.Count - 1)
                        {
                            PointLatLng middle = CalculateMiddlePoint(vertices.Markers[i], vertices.Markers[i + 1]);
                            GMapMarkerGraySquare auxMark = new GMapMarkerGraySquare(middle);
                            auxiliar.Markers.Add(auxMark);
                        }
                    }

                }
            }
                 
        }

        private void MainMap_OnMarkerEnter(GMapMarker item)
        {            
            //se selecciona el marcador en el que se hizo click
            if (vertices.Markers.Contains(item))
                this.selectedVertice = item;
            
            if(auxiliar.Markers.Contains(item))
                this.selectedAuxiliar = item;
            
        }

        private void MainMap_OnMarkerLeave(GMapMarker item)
        {            
            //si el marcador está siendo arrastrado no se deselecciona
            if (!isDraggingVertice)
                selectedVertice = null;

            if (!isDraggingAuxiliar)
                selectedAuxiliar = null;
        }

        private void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                //si hay un vertice seleccionado
                if (this.selectedVertice != null)
                {
                    //se arrastra el marcador seleccionado
                    this.selectedVertice.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                    isDraggingVertice = true;
                }
                else
                {
                    if (this.selectedAuxiliar != null)
                    {
                        this.selectedAuxiliar.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                        isDraggingAuxiliar = true;
                    }
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            GoToAddress(txtAddress.Text);
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
    }
}
