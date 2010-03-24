using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace ZeqkTools
{
    public class Functions
    {
        static public PointLatLng CalculateMiddlePoint(List<PointLatLng> points)
        {

            double lat = 0;
            double lng = 0;

            //la distancia de la latitud. la mayor latitud - la menor latitud
            double latDistance = points.Max(p => p.Lat) - points.Min(p => p.Lat);
            //la mitad de la distancia
            double auxLat = latDistance / 2;
            //calculo el punto medio entre la latitud mayor y la latitud menor
            lat = points.Min(p => p.Lat) + auxLat;

            double lngDistance = points.Max(p => p.Lng) - points.Min(p => p.Lng);
            double auxLng = lngDistance / 2;
            lng = points.Min(p => p.Lng) + auxLng;

            PointLatLng point = new PointLatLng(lat, lng);

            return point;

        }
    }
}
