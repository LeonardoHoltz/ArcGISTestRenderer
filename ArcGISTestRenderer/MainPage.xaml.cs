using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace ArcGISTestRenderer
{
    public sealed partial class MainPage : Page
    {
        public MapViewModel MapViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            MapViewModel = new MapViewModel();
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK1c1fc5824d93475092062f6ca098d7e7yoLb20wEM1QQUqVkgCc8K8AE8HA1YRVj25IQZZdhGfkxj0wgk50Bn9TVR4agbplF";
            MapPoint mapCenterPoint = new MapPoint(0, 0, SpatialReferences.Wgs84);
            MapViewTest.SetViewpointAsync(new Viewpoint(mapCenterPoint, 10000000));
            MapViewTest.GraphicsOverlays.Add(MapViewModel.pointsGraphicsOverlay);
            MapViewTest.GraphicsOverlays.Add(MapViewModel.militaryGraphicsOverlay);
        }

        private void CreatePointButton_Click(object sender, RoutedEventArgs e)
        {
            MapViewModel.CreatePoint();
        }

        private void MovePointButton_Click(object sender, RoutedEventArgs e)
        {
            MapViewModel.MovePoint();
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
                    onlyReturnPopups,
                    maximumResults);
                
                if (identifyResults.Graphics.Count > 0)
                {
                    //Graphic g = identifyResults.Graphics.First();
                    await new MessageDialog("Tapped on Graphic", "").ShowAsync();
                    MapViewModel.pointsGraphicsOverlay.SelectGraphics(identifyResults.Graphics);
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.ToString(), "Error").ShowAsync();
            }
        }
    }
}
