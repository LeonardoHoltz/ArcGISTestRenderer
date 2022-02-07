using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.GeoAnalysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISTestRenderer
{
    public class Contact
    {
        #region Constants

        const int DEGREE_TO_METERS = 111320;

        #endregion Constants

        private Graphic _lineGraphic;
        public Graphic LineGraphic
        {
            get { return _lineGraphic; }
            set { _lineGraphic = value; }
        }

        private Graphic _mainGraphic;
        public Graphic MainGraphic
        {
            get { return _mainGraphic; }
            set { _mainGraphic = value; }
        }

        private Polyline _lineGeometry;
        public Polyline LineGeometry
        {
            get { return _lineGeometry; }
            set { _lineGeometry = value; }
        }

        private MapPoint _anchorPoint;
        public MapPoint AnchorPoint
        {
            get { return _anchorPoint; }
            set 
            {
                _anchorPoint = value;
            }
        }

        private MapPoint _mainPoint;
        public MapPoint MainPoint
        {
            get { return _mainPoint; }
            set { _mainPoint = value; }
        }

        #region Symbols

        private SimpleMarkerSymbol _anchorSymbol = new SimpleMarkerSymbol
        {
            Style = SimpleMarkerSymbolStyle.Circle,
            Color = System.Drawing.Color.Transparent,
            Size = 0.0,
            Outline = new SimpleLineSymbol
            {
                Style = SimpleLineSymbolStyle.Solid,
                Color = System.Drawing.Color.Transparent,
                Width = 0.0
            }
        };

        #endregion Symbols

        public int pixelDistance { get; set; } = 100;

        private SimpleLineSymbol _lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.DashDot, Color.Blue, 1);

        public Contact(double latitude, double longitude, double unitsPerPixel)
        {
            // Main Point:
            MainPoint = new MapPoint(longitude, latitude, SpatialReferences.Wgs84);
            MainGraphic = new Graphic(MainPoint);
            MainGraphic.Attributes["sidc"] = 10065400001103000000;
            MainGraphic.Attributes["graphicType"] = "mainPoint";

            // Calculate distance of each coordinate to the anchor by making the inverse of the euclidean distance:
            double distanceInUnits = unitsPerPixel * pixelDistance;
            double latLonDistance = Math.Sqrt(Math.Pow(distanceInUnits, 2) / 2.0);
            latLonDistance = latLonDistance / DEGREE_TO_METERS;

            // Anchor Point:
            AnchorPoint = new MapPoint(longitude - latLonDistance, latitude + latLonDistance, SpatialReferences.Wgs84);

            // Line Between Main Point and Anchor Point:
            PolylineBuilder lineBuilder = new PolylineBuilder(SpatialReferences.Wgs84);
            
            lineBuilder.AddPoint(AnchorPoint);
            lineBuilder.AddPoint(MainPoint);

            LineGeometry = lineBuilder.ToGeometry();
            LineGraphic = new Graphic(LineGeometry, _lineSymbol);
            LineGraphic.Attributes["graphicType"] = "labelLine";
            LineGraphic.Attributes["label"] = "<BOL>Bom dia</BOL>\nTeste2\n250.053.123\n---";
            LineGraphic.Attributes["labelColor"] = "neutral1";
            LineGraphic.ZIndex = MainGraphic.ZIndex - 5;
        }

        public void SetNewPixelDistance(double unitsPerPixel)
        {
            MapPoint mapMainPoint = (MapPoint)GeometryEngine.Project(LineGeometry.Parts[0].EndPoint, SpatialReferences.WebMercator);
            MapPoint mapAnchorPoint = (MapPoint)GeometryEngine.Project(LineGeometry.Parts[0].StartPoint, SpatialReferences.WebMercator);

            double dx = -(mapMainPoint.X - mapAnchorPoint.X);
            double dy = -(mapMainPoint.Y - mapAnchorPoint.Y);
            double lenghtMeters = Math.Sqrt(dx * dx + dy * dy);
            pixelDistance = (int)Math.Floor(lenghtMeters / unitsPerPixel);
        }

        public void UpdateAnchorDistance(double unitsPerPixel)
        {
            double intendedDistanceInUnits = unitsPerPixel * pixelDistance;

            MapPoint mapMainPoint = (MapPoint)GeometryEngine.Project(LineGeometry.Parts[0].EndPoint, SpatialReferences.WebMercator);
            MapPoint mapAnchorPoint = (MapPoint)GeometryEngine.Project(LineGeometry.Parts[0].StartPoint, SpatialReferences.WebMercator);

            double dx = -(mapMainPoint.X - mapAnchorPoint.X);
            double dy = -(mapMainPoint.Y - mapAnchorPoint.Y);

            double lenghtMeters = Math.Sqrt(dx * dx + dy * dy);
            double scalePercentual = intendedDistanceInUnits / lenghtMeters;

            dx = dx * scalePercentual;
            dy = dy * scalePercentual;

            mapAnchorPoint = new MapPoint(mapMainPoint.X + dx, mapMainPoint.Y + dy);
            PolylineBuilder lineBuilder = new PolylineBuilder(SpatialReferences.WebMercator);
            MapPoint startPoint = new MapPoint(mapMainPoint.X, mapMainPoint.Y);
            MapPoint endPoint = new MapPoint(mapAnchorPoint.X, mapAnchorPoint.Y);
            
            lineBuilder.AddPoint(endPoint);
            lineBuilder.AddPoint(startPoint);

            LineGeometry = (Polyline)GeometryEngine.Project(lineBuilder.ToGeometry(), SpatialReferences.Wgs84);
            LineGraphic.Geometry = LineGeometry;
        }
    }
}
