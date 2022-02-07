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
using Windows.UI.Input;

namespace ArcGISTestRenderer
{
    public sealed partial class MainPage : Page
    {
        public MapViewModel MapViewModel { get; set; }

        public bool IsLabelBeingMoved { get; set; } = false;

        public Graphic editedLineGraphic { get; set; } = null;

        public double CurrentMapScale { get; set; }

        //public static doubleTapTime

        public MainPage()
        {
            this.InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK1c1fc5824d93475092062f6ca098d7e7yoLb20wEM1QQUqVkgCc8K8AE8HA1YRVj25IQZZdhGfkxj0wgk50Bn9TVR4agbplF";
            MapPoint mapCenterPoint = new MapPoint(0, 0, SpatialReferences.Wgs84);
            MapViewTest.SetViewpointAsync(new Viewpoint(mapCenterPoint, 10000000));
            MapViewModel = new MapViewModel();
            CurrentMapScale = MapViewTest.MapScale;
            MapViewTest.SelectionProperties.Color = Color.Transparent;
            MapViewTest.GraphicsOverlays.Add(MapViewModel.pointsGraphicsOverlay);
            MapViewTest.GraphicsOverlays.Add(MapViewModel.militaryGraphicsOverlay);
            MapViewTest.PointerReleased += MapViewTest_PointerReleased;
            MapViewTest.PointerPressed += MapViewTest_PointerPressed;
            MapViewTest.PointerMoved += MapViewTest_PointerMoved;
            MapViewTest.ViewpointChanged += MapViewTest_ViewpointChanged;
            MapViewTest.GeoViewDoubleTapped += MapViewTest_GeoViewDoubleTapped;
            MapViewTest.InteractionOptions = new MapViewInteractionOptions();
        }

        private async void MapViewTest_GeoViewDoubleTapped(object sender, Esri.ArcGISRuntime.UI.Controls.GeoViewInputEventArgs e)
        {
            double tolerance = 30d;
            bool onlyReturnPopups = false;
            try
            {
                IdentifyGraphicsOverlayResult identifyResults = await MapViewTest.IdentifyGraphicsOverlayAsync(
                    MapViewModel.pointsGraphicsOverlay,
                    e.Position,
                    tolerance,
                    onlyReturnPopups,
                    maximumResults: 100);

                if (identifyResults.Graphics.Count > 0)
                {
                    Graphic g = identifyResults.Graphics.FirstOrDefault(graphic => graphic.Attributes["graphicType"].Equals("labelLine"));
                    if (g != null)
                    {
                        string colorAttribute = (string)g.Attributes["labelColor"];
                        int index = colorAttribute.Length - 1;
                        int number = colorAttribute[index] - '0';
                        int newNumber = number % 4 + 1;
                        string newNumberString = Convert.ToString(newNumber);
                        Console.WriteLine(newNumberString);
                        string oldNumberString = Convert.ToString(number);
                        string newColor = colorAttribute.Replace(oldNumberString, newNumberString);
                        g.Attributes["labelColor"] = newColor;
                        MapViewTest.InteractionOptions.IsZoomEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.ToString(), "Error").ShowAsync();
            }
        }

        private void MapViewTest_ViewpointChanged(object sender, EventArgs e)
        {
            if (MapViewTest.MapScale != CurrentMapScale)
            {
                CurrentMapScale = MapViewTest.MapScale;
                MapViewModel.UnitsPerPixel = MapViewTest.UnitsPerPixel;
            }
        }

        private void MapViewTest_PointerMoved(object sender, PointerRoutedEventArgs e)
        { 
            if(IsLabelBeingMoved && editedLineGraphic != null)
            {
                PointerPoint pointerPoint = e.GetCurrentPoint(sender as UIElement);
                Windows.Foundation.Point p = pointerPoint.Position;
                MapPoint newAnchor = MapViewTest.ScreenToLocation(p);
                MapViewModel.MovePoint(newAnchor, editedLineGraphic);
            }
        }

        private void MapViewTest_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsLabelBeingMoved = false;
            MapViewTest.InteractionOptions.IsPanEnabled = true;
            editedLineGraphic = null;
        }

        private async void MapViewTest_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(sender as UIElement);
            Windows.Foundation.Point point = pointerPoint.Position;
            double tolerance = 30d;
            bool onlyReturnPopups = false;
            try
            {
                
                IdentifyGraphicsOverlayResult identifyResults = await MapViewTest.IdentifyGraphicsOverlayAsync(
                    MapViewModel.pointsGraphicsOverlay,
                    point,
                    tolerance,
                    onlyReturnPopups,
                    maximumResults: 100);
                

                if (identifyResults.Graphics.Count > 0)
                {
                    Graphic g = identifyResults.Graphics.FirstOrDefault(graphic => graphic.Attributes["graphicType"].Equals("labelLine"));
                    if (g != null)
                    {
                        editedLineGraphic = g;
                        IsLabelBeingMoved = true;

                        // block map panning
                        MapViewTest.InteractionOptions.IsPanEnabled = false;
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
            int maximumResults = 100;
            bool onlyReturnPopups = false;

            try
            {
                IdentifyGraphicsOverlayResult identifyResults = await MapViewTest.IdentifyGraphicsOverlayAsync(
                    MapViewModel.pointsGraphicsOverlay,
                    e.Position,
                    tolerance,
                    onlyReturnPopups,
                    maximumResults);

                if (identifyResults.Graphics.Count > 0)
                {
                    Graphic g = identifyResults.Graphics.FirstOrDefault(graphic => graphic.Attributes["graphicType"].Equals("labelLine"));
                    if (g != null)
                    {
                        MapViewTest.InteractionOptions.IsZoomEnabled = !MapViewTest.InteractionOptions.IsZoomEnabled;
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.ToString(), "Error").ShowAsync();
            }
            
        }

        private void CreatePointButton_Click(object sender, RoutedEventArgs e)
        {
            MapViewModel.CreatePoint();
        }

    }
}
