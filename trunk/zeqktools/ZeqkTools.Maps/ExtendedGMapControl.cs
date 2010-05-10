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

namespace ZeqkTools.Maps
{
    public partial class ExtendedGMapControl : GMapControl
    {
        public ExtendedGMapControl()
        {
            InitializeComponent();
        }

        public ExtendedGMapControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void ExtendedGMapControl_OnCurrentPositionChanged(GMap.NET.PointLatLng point)
        {

        }

        private void ExtendedGMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;

                if (this.PolygonsEnabled)
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

                if (this.PolygonsEnabled)
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
            if (this.PolygonsEnabled)
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
    }
}
