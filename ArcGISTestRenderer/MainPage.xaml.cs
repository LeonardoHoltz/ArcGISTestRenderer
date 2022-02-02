using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System.Drawing;
using Esri.ArcGISRuntime.Data;
using Windows.UI.Popups;
using Esri.ArcGISRuntime.UI;
using System.ComponentModel;
using Esri.ArcGISRuntime.Symbology;

namespace ArcGISTestRenderer
{
    public sealed partial class MainPage : Page
    {
        public MapViewModel MapViewModel { get; set; }

        private SketchEditor _sketchEditor;

        public SketchEditor SketchEditor
        {
            get
            {
                return _sketchEditor;
            }
            set
            {
                _sketchEditor = value;
            }
        }

        public bool IsLabelBeingMoved { get; set; } = false;

        public MainPage()
        {
            this.InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            SetupSketchEditor();
            MapViewModel = new MapViewModel();
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK1c1fc5824d93475092062f6ca098d7e7yoLb20wEM1QQUqVkgCc8K8AE8HA1YRVj25IQZZdhGfkxj0wgk50Bn9TVR4agbplF";
            MapPoint mapCenterPoint = new MapPoint(0, 0, SpatialReferences.Wgs84);
            MapViewTest.SetViewpointAsync(new Viewpoint(mapCenterPoint, 10000000));
            MapViewTest.GraphicsOverlays.Add(MapViewModel.pointsGraphicsOverlay);
            MapViewTest.GraphicsOverlays.Add(MapViewModel.militaryGraphicsOverlay);
            MapViewTest.PointerReleased += MapViewTest_PointerReleased;
            MapViewTest.PointerPressed += MapViewTest_PointerPressed;
            MapViewTest.PointerWheelChanged += MapViewTest_PointerWheelChanged;
            MapViewTest.SelectionProperties.Color = Color.Transparent;
        }

        private void MapViewTest_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {

        }

        private void MapViewTest_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            if (!IsLabelBeingMoved)
            {
                SketchEditor.Stop();
            }
        }

        private async void MapViewTest_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Windows.Foundation.Point point = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
            point.X = point.X - 200;
            point.Y = point.Y - 40;
            double tolerance = 100d;
            bool onlyReturnPopups = false;
            try
            {
                IdentifyGraphicsOverlayResult identifyResults = await MapViewTest.IdentifyGraphicsOverlayAsync(
                    MapViewModel.pointsGraphicsOverlay,
                    point,
                    tolerance,
                    onlyReturnPopups);

                if (identifyResults.Graphics.Count > 0)
                {
                    Graphic g = identifyResults.Graphics.FirstOrDefault(graphic => graphic.Attributes["graphicType"].Equals("anchor"));
                    if (g != null)
                    {
                        MapViewModel.MovePoint(SketchEditor, g);
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.ToString(), "Error").ShowAsync();
            }
        }

        private async void MapViewTest_GeoViewTapped(object sender, Esri.ArcGISRuntime.UI.Controls.GeoViewInputEventArgs e)
        {
            double tolerance = 15d;
            int maximumResults = 1;
            bool onlyReturnPopups = false;

            try
            {
                IdentifyGraphicsOverlayResult identifyResults = await MapViewTest.IdentifyGraphicsOverlayAsync(
                    MapViewModel.pointsGraphicsOverlay,
                    e.Position,
                    tolerance,
                    onlyReturnPopups);

                if (identifyResults.Graphics.Count > 0)
                {
                    Graphic g = identifyResults.Graphics.FirstOrDefault(graphic => graphic.Attributes["graphicType"].Equals("mainSymbol"));
                    if (g != null)
                    {
                        g.IsSelected = !g.IsSelected;
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.ToString(), "Error").ShowAsync();
            }
        }

        private void SetupSketchEditor()
        {
            DataContext = MapViewTest.SketchEditor;
            SketchEditor = MapViewTest.SketchEditor;
            SketchEditor.Style.ShowNumbersForVertices = false;

            SimpleMarkerSymbol anchorPreviewSymbol = new SimpleMarkerSymbol
            {
                Style = SimpleMarkerSymbolStyle.Square,
                Color = System.Drawing.Color.Transparent,
                Size = 70.0,
                Outline = new SimpleLineSymbol
                {
                    Style = SimpleLineSymbolStyle.Dash,
                    Color = System.Drawing.Color.Blue,
                    Width = 1.0,
                }
            };

            SimpleMarkerSymbol anchorCurrentPlaceSymbol = new SimpleMarkerSymbol
            {
                Style = SimpleMarkerSymbolStyle.Square,
                Color = System.Drawing.Color.Transparent,
                Size = 70.0,
                Outline = new SimpleLineSymbol
                {
                    Style = SimpleLineSymbolStyle.Null,
                    Color = System.Drawing.Color.Transparent,
                    Width = 0.0,
                }
            };
            

            //SketchEditor.Style.VertexSymbol = anchorSymbol;
            SketchEditor.Style.FeedbackVertexSymbol = anchorPreviewSymbol;
            SketchEditor.Style.SelectedVertexSymbol = anchorCurrentPlaceSymbol;
            SketchEditor.PropertyChanged += SketchEditor_PropertyChanged;
            SketchEditor.GeometryChanged += SketchEditor_GeometryChanged;
            SketchEditor.SelectedVertexChanged += SketchEditor_SelectedVertexChanged;
        }

        private void SketchEditor_SelectedVertexChanged(object sender, VertexChangedEventArgs e)
        {
            IsLabelBeingMoved = true;
        }

        private void SketchEditor_GeometryChanged(object sender, GeometryChangedEventArgs e)
        {
            SketchEditor.Stop();
            IsLabelBeingMoved = false;
        }

        private void SketchEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SketchEditor.EditConfiguration != null)
            {
                SketchEditor.EditConfiguration.RequireSelectionBeforeDrag = false;
                SketchEditor.EditConfiguration.AllowRotate = false;
                SketchEditor.ClearVertexSelection();
            }
        }

        private void CreatePointButton_Click(object sender, RoutedEventArgs e)
        {
            MapViewModel.CreatePoint();
        }

    }
}
