using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET;

namespace ZeqkTools.WindowsForms.Maps
{
    public partial class ExtendedGMapControl : GMapControl
    {
        #region Fields

        private bool _allowDrawPolygon;

        #endregion

        #region Internal variables

        // markers
        GMapMarker selectedVertice;
        GMapMarker selectedIntermediatePoint;

        // layers
        GMapOverlay firstOverlay;
        GMapOverlay vertices;
        GMapOverlay auxiliar;

        bool isMouseDown;
        bool polygonIsComplete = false;
        bool isDraggingVertice = false;
        bool isDraggingIntermediatePoint = false;

        GMapPolygon _polygon;

        #endregion

        #region Constructors

        public ExtendedGMapControl()
        {
            InitializeComponent();
        }

        public ExtendedGMapControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Properties
        
        public bool AllowDrawPolygon
        {
            get { return _allowDrawPolygon; }
            set { _allowDrawPolygon = value; }
        }

        #endregion

        #region Public methods

        public void SetDrawingPolygon(GMapPolygon polygon)
        {
            firstOverlay = this.Overlays[0];
            _polygon = polygon;

            // add custom layers  
            {
                auxiliar = new GMapOverlay(this, "auxiliar");
                this.Overlays.Add(auxiliar);
                vertices = new GMapOverlay(this, "vertices");
                this.Overlays.Add(vertices);
            }


            //draw the polygon
            if (polygon.Points.Count > 2)
            {
                //go through the polygon vertices
                for (int i = 0; i < polygon.Points.Count; i++)
                {
                    GMapMarkerWitheSquare vertice = null;

                    //the first and last vertice have the same coordinate, so they are the same object
                    if (i == polygon.Points.Count - 1) //only the last vertice
                        vertice = (GMapMarkerWitheSquare)vertices.Markers[0];
                    else
                        vertice = new GMapMarkerWitheSquare(polygon.Points[i]);

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

            if (!firstOverlay.Polygons.Contains(_polygon))
                firstOverlay.Polygons.Add(_polygon);
            

        }

        public void ClearDrawingPolygon()
        {
            if(this.PolygonsEnabled && _allowDrawPolygon)
            {
                auxiliar.Markers.Clear();
                vertices.Markers.Clear();

                //clear polygon
                _polygon.Points.Clear();
                this.UpdatePolygonLocalPosition(_polygon);

                polygonIsComplete = false;
            }
        }

        #endregion

        #region Map event methods
        private void ExtendedGMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;

                if (this.PolygonsEnabled && _allowDrawPolygon)
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
                                this.UpdatePolygonLocalPosition(_polygon);

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
                            this.UpdatePolygonLocalPosition(_polygon);

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

        private void ExtendedGMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                if (this.PolygonsEnabled && _allowDrawPolygon)
                {
                    //if the polygon is incomplete, click will be create new vertices
                    if (!polygonIsComplete)
                        if (selectedVertice == null)
                        {
                            GMapMarkerWitheSquare marker = new GMapMarkerWitheSquare(this.FromLocalToLatLng(e.X, e.Y));
                            vertices.Markers.Add(marker);
                            this.selectedVertice = marker;

                        }
                }
            }
        }

        private void ExtendedGMapControl_OnMarkerClick(GMapMarker item)
        {
            if (this.PolygonsEnabled && _allowDrawPolygon)
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
                        this.UpdatePolygonLocalPosition(_polygon);

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

        private void ExtendedGMapControl_OnMarkerEnter(GMapMarker item)
        {
            if (this.PolygonsEnabled && _allowDrawPolygon)
            {
                //select the marker that was clicked
                if (vertices.Markers.Contains(item))
                    this.selectedVertice = item;

                if (auxiliar.Markers.Contains(item))
                    this.selectedIntermediatePoint = item;
            }
        }

        private void ExtendedGMapControl_OnMarkerLeave(GMapMarker item)
        {
            //if the marker is being dragged, then is not deselected
            if (!isDraggingVertice)
                selectedVertice = null;

            if (!isDraggingIntermediatePoint)
                selectedIntermediatePoint = null;
        }

        private void ExtendedGMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                if (this.PolygonsEnabled && _allowDrawPolygon)
                {
                    //if there is a selected vertice
                    if (this.selectedVertice != null)
                    {
                        //drag the selected vertice
                        this.selectedVertice.Position = this.FromLocalToLatLng(e.X, e.Y);
                        isDraggingVertice = true;
                    }
                    else
                    {
                        //else, if there is a selected intermediate point, be dragg
                        if (this.selectedIntermediatePoint != null)
                        {
                            this.selectedIntermediatePoint.Position = this.FromLocalToLatLng(e.X, e.Y);
                            isDraggingIntermediatePoint = true;
                        }
                    }
                }
            }
        }

        private void ExtendedGMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.PolygonsEnabled)
            {
                PointLatLng point = this.FromLocalToLatLng(e.X, e.Y);
                foreach (GMapPolygon polygon in this.Overlays[0].Polygons)
                {
                    if (Functions.PointInPolygon(point, polygon.Points.ToArray()))
                    {
                        if (polygon.Name != null)
                        {
                            ToolTip tip = new ToolTip();
                            tip.SetToolTip(this, polygon.Name.ToString());
                            tip.Show("",this.FindForm(),5000);
                        }
                    }
                }
            }
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
        #endregion
    }
}
