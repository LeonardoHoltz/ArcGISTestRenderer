using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Geometry;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping.Labeling;
using Esri.ArcGISRuntime.UI.Controls;
using System.Drawing;

namespace ArcGISTestRenderer
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Map _map;
        public Map Map
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
                OnPropertyChanged();
            }
        }

        public GraphicsOverlay pointsGraphicsOverlay;
        public GraphicsOverlay militaryGraphicsOverlay;

        private string _latitude;
        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }

        private string _longitude;
        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                OnPropertyChanged();
            }
        }

        private int _symbolsScale;
        public int SymbolsScale
        {
            get
            {
                return _symbolsScale;
            }
            set
            {
                _symbolsScale = value;
                (pointsGraphicsOverlay.Renderer as DictionaryRenderer).ScaleExpression = new Esri.ArcGISRuntime.ArcadeExpression((_symbolsScale/10.0).ToString(CultureInfo.InvariantCulture));
            }
        }

        private bool lineBeingMoved { get; set; } = false;

        private SimpleLineSymbol lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.DashDot, Color.Blue, 1);

        SimpleMarkerSymbol anchorSymbol = new SimpleMarkerSymbol
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

        public MapViewModel()
        {
            SetupMap();
            CreateGraphics();
        }


        private void SetupMap()
        {
            // Create a new map with a 'topographic vector' basemap.
            Map = new Map(BasemapStyle.ArcGISTopographic);
        }

        private async void CreateGraphics()
        {
            // Create a new graphics overlay to contain a variety of graphics.
            pointsGraphicsOverlay = new GraphicsOverlay();
            militaryGraphicsOverlay = new GraphicsOverlay();

            string path = Directory.GetCurrentDirectory();
            DictionarySymbolStyle mil2525DStyle = await DictionarySymbolStyle.CreateFromFileAsync(path + "\\mil2525d.stylx");
            militaryGraphicsOverlay.Renderer = new DictionaryRenderer(mil2525DStyle);
            pointsGraphicsOverlay.Renderer = new DictionaryRenderer(mil2525DStyle);
            


            TextSymbol textSymbolLargeCities = new TextSymbol
            {
                Color = System.Drawing.Color.Blue,
                HaloColor = System.Drawing.Color.White,
                HaloWidth = 2,
                FontFamily = "Arial",
                Size = 20
            };

            LabelDefinition labelDefLarge = new LabelDefinition(new SimpleLabelExpression("[NAME]"), textSymbolLargeCities)
            {
                WhereClause = "[TEST] = 'city'",
                Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
                TextLayout = LabelTextLayout.Horizontal
            };

            pointsGraphicsOverlay.LabelDefinitions.Add(labelDefLarge);
            pointsGraphicsOverlay.LabelsEnabled = true;

            LoadMilitaryMessages();
            Latitude = "0";
            Longitude = "0";
            CreatePoint();
        }

        public void LoadMilitaryMessages()
        {
            // Get the path to the messages file.
            string path = Directory.GetCurrentDirectory();

            // Load the XML document.
            XElement xmlRoot = XElement.Load(path + "\\Mil2525DMessages.xml");

            // Get all of the messages.
            IEnumerable <XElement> messages = xmlRoot.Descendants("message");

            // Add a graphic for each message.
            foreach (var message in messages)
            {
                Graphic messageGraphic = GraphicFromAttributes(message.Descendants().ToList());
                militaryGraphicsOverlay.Graphics.Add(messageGraphic);
            }
        }

        private Graphic GraphicFromAttributes(List<XElement> graphicAttributes)
        {
            // Get the geometry and the spatial reference from the message elements.
            XElement geometryAttribute = graphicAttributes.First(attr => attr.Name == "_control_points");
            XElement spatialReferenceAttr = graphicAttributes.First(attr => attr.Name == "_wkid");

            // Split the geometry field into a list of points.
            Array pointStrings = geometryAttribute.Value.Split(';');

            // Create a point collection in the correct spatial reference.
            int wkid = Convert.ToInt32(spatialReferenceAttr.Value);
            SpatialReference pointSR = SpatialReference.Create(wkid);
            PointCollection graphicPoints = new PointCollection(pointSR);

            // Add a point for each point in the list.
            foreach (string pointString in pointStrings)
            {
                if (!String.IsNullOrEmpty(pointString))
                {
                    var coords = pointString.Split(',');
                    graphicPoints.Add(Convert.ToDouble(coords[0], CultureInfo.InvariantCulture), Convert.ToDouble(coords[1], CultureInfo.InvariantCulture));
                }
                
            }

            // Create a multipoint from the point collection.
            Multipoint graphicMultipoint = new Multipoint(graphicPoints);

            // Create the graphic from the multipoint.
            Graphic messageGraphic = new Graphic(graphicMultipoint);

            // Add all of the message's attributes to the graphic (some of these are used for rendering).
            foreach (XElement attr in graphicAttributes)
            {
                messageGraphic.Attributes[attr.Name.ToString()] = attr.Value;
            }

            return messageGraphic;
        }



        #region Buttons Events

        public void CreatePoint()
        {
            // Create a point geometry.
            if (!String.IsNullOrEmpty(Latitude) && !String.IsNullOrEmpty(Longitude))
            {
                MapPoint newPoint = new MapPoint(Double.Parse(Longitude), Double.Parse(Latitude), SpatialReferences.Wgs84);


                //Graphic pointGraphic = new Graphic(newPoint, pointSymbol);
                Graphic pointGraphic = new Graphic(newPoint);

                //pointGraphic.Attributes["sidc"] = "10019800001000000000"; //undefined
                //pointGraphic.Attributes["sidc"] = null; // unknown
                pointGraphic.Attributes["sidc"] = 10065400001103000000;
                pointGraphic.Attributes["graphicType"] = "mainSymbol";
                //pointGraphic.Attributes["NAME"] = "Bom dia";
                //pointGraphic.Attributes["TEST"] = "city";

                // Create a point graphic with the geometry and symbol.
                pointsGraphicsOverlay.Graphics.Add(pointGraphic);


                // Anchor Point:
                MapPoint anchorPoint = new MapPoint(Double.Parse(Longitude) - 20.0, Double.Parse(Latitude) + 10.0, SpatialReferences.Wgs84);

                Graphic anchorGraphic = new Graphic(anchorPoint, anchorSymbol);
                anchorGraphic.Attributes["NAME"] = "<BOL>Bom dia</BOL> Teste2";
                anchorGraphic.Attributes["TEST"] = "city";
                anchorGraphic.Attributes["graphicType"] = "anchor";

                pointsGraphicsOverlay.Graphics.Add(anchorGraphic);

                PolylineBuilder lineBuilder = new PolylineBuilder(SpatialReferences.Wgs84);
                lineBuilder.AddPoint(newPoint);
                lineBuilder.AddPoint(anchorPoint);
                Graphic lineGraphic = new Graphic(lineBuilder.ToGeometry(), lineSymbol);
                lineGraphic.Attributes["graphicType"] = "anchorLine";
                lineGraphic.ZIndex = pointGraphic.ZIndex - 5;
                pointsGraphicsOverlay.Graphics.Add(lineGraphic);
            }
        }

        public async void MovePoint(SketchEditor sketchEditor, Graphic graphic)
        {
            // linha está atrapalhando o sketch editor na hora de selecionar o ponto
            // fazer com que a geometria seja colocada no lugar onde o mouse soltou
            Geometry oldGeometry = graphic.Geometry;
            GraphicsOverlay associatedOverlay = graphic.GraphicsOverlay;

            Geometry newGeometry = await sketchEditor.StartAsync(graphic.Geometry);
            graphic.Geometry = newGeometry;

            Graphic lineGraphic = associatedOverlay.Graphics.FirstOrDefault(lineG => lineG.Geometry.GeometryType == GeometryType.Polyline); 
            Polyline line = (Polyline)lineGraphic.Geometry;
            MapPoint startPoint = line.Parts[0].StartPoint;
            MapPoint endGeometry = (MapPoint)newGeometry;
            PolylineBuilder newLineBuilder = new PolylineBuilder(SpatialReferences.Wgs84);

            // try to use GeometryEngine.Project next time
            string latLon = CoordinateFormatter.ToLatitudeLongitude(endGeometry, LatitudeLongitudeFormat.DegreesMinutesSeconds, 16);
            MapPoint endPoint = CoordinateFormatter.FromLatitudeLongitude(latLon, SpatialReferences.Wgs84);

            newLineBuilder.AddPoint(startPoint);
            newLineBuilder.AddPoint(endPoint);

            lineGraphic.Geometry = newLineBuilder.ToGeometry();
        }

        public void CreatePolygon()
        {
            PolygonBuilder polygonBuilder = new PolygonBuilder(SpatialReferences.WebMercator);
            polygonBuilder.AddPoint(x: -20e5, y: 20e5);
            polygonBuilder.AddPoint(x: 20e5, y: 20e5);
            polygonBuilder.AddPoint(x: -20e5, y: -20e5);
            polygonBuilder.AddPoint(x: -20e5, y: -20e5);

            Graphic polygonGraphic = new Graphic(polygonBuilder.ToGeometry());

        }

        #endregion


    }
}
