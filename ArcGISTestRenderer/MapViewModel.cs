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

        public Geometry SketchGeometry { get; set; }

        private double _unitsPerPixel;

        public double UnitsPerPixel
        {
            get { return _unitsPerPixel; }
            set
            {
                _unitsPerPixel = value;
                UpdateLabelDistances();
                OnPropertyChanged();
            }
        }

        private List<Contact> ContactsAdded = new List<Contact>();

        public MapViewModel()
        {
            UnitsPerPixel = 2645.8333333304759;
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
            DictionarySymbolStyle mil2525DStyle = await DictionarySymbolStyle.CreateFromFileAsync(path + "\\Restaurant.stylx");
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
                Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.LineCenterEnd,
                TextLayout = LabelTextLayout.Horizontal,
            };
            labelDefLarge.OverrunStrategy = LabelOverrunStrategy.Allow;
            labelDefLarge.RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None;
            labelDefLarge.RepeatStrategy = LabelRepeatStrategy.Repeat;
            labelDefLarge.StackAlignment = LabelStackAlignment.Dynamic;
            labelDefLarge.LineConnection = LabelLineConnection.None;
            labelDefLarge.LabelOverlapStrategy = LabelOverlapStrategy.Allow;

            pointsGraphicsOverlay.LabelDefinitions.Add(labelDefLarge);
            pointsGraphicsOverlay.LabelsEnabled = true;

            //LoadMilitaryMessages();
            Latitude = "30";
            Longitude = "30";

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

        private void UpdateLabelDistances()
        {
            foreach (Contact contact in ContactsAdded)
            {
                contact.UpdateAnchorDistance(UnitsPerPixel);
            }
        }



        #region Buttons Events

        public void CreatePoint()
        {
            // Create a point geometry.
            if (!String.IsNullOrEmpty(Latitude) && !String.IsNullOrEmpty(Longitude))
            {
                Contact contact = new Contact(Double.Parse(Latitude), Double.Parse(Longitude), UnitsPerPixel);
                pointsGraphicsOverlay.Graphics.Add(contact.MainGraphic);
                pointsGraphicsOverlay.Graphics.Add(contact.LineGraphic);
                ContactsAdded.Add(contact);
            }
        }

        public void MovePoint(MapPoint newAnchor, Graphic editedAnchor)
        {
            Contact contactAssociated = ContactsAdded.FirstOrDefault(contact => contact.LineGraphic == editedAnchor);
            
            MapPoint newAnchorPoint = (MapPoint)GeometryEngine.Project(newAnchor, SpatialReferences.Wgs84);

            // move line endpoint to the new anchor
            PolylineBuilder lineBuilder = new PolylineBuilder(SpatialReferences.Wgs84);
            lineBuilder.AddPoint(contactAssociated.MainPoint);
            lineBuilder.AddPoint(newAnchorPoint);
            contactAssociated.LineGeometry = lineBuilder.ToGeometry();
            contactAssociated.LineGraphic.Geometry = contactAssociated.LineGeometry;
            contactAssociated.SetNewPixelDistance(UnitsPerPixel);
            contactAssociated.AnchorPoint = newAnchorPoint;
        }

        #endregion


    }
}
