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
            MapPoint mapCenterPoint = new MapPoint(-118.805, 34.027, SpatialReferences.Wgs84);
            MapViewTest.SetViewpointAsync(new Viewpoint(mapCenterPoint, 100000));
            MapViewTest.GraphicsOverlays = MapViewModel.GraphicsOverlays;
        }
    }
}
