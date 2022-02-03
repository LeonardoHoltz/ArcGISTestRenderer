﻿using System;
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
            MapViewInteractionOptions interactionOptions = new MapViewInteractionOptions()
            {
                IsPanEnabled = true
            };
            MapViewTest.InteractionOptions = interactionOptions;
            editedLineGraphic = null;
        }

        private async void MapViewTest_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(sender as UIElement);
            Windows.Foundation.Point point = pointerPoint.Position;
            double tolerance = 10d;
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
                    Graphic g = identifyResults.Graphics.FirstOrDefault(graphic => graphic.Attributes["graphicType"].Equals("line"));
                    if (g != null)
                    {
                        editedLineGraphic = g;
                        IsLabelBeingMoved = true;
                        // block map panning
                        MapViewInteractionOptions interactionOptions = new MapViewInteractionOptions()
                        {
                            IsPanEnabled = false
                        };
                        MapViewTest.InteractionOptions = interactionOptions;
                        
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
            /*
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
                    Graphic g = identifyResults.Graphics.FirstOrDefault(graphic => graphic.Attributes["graphicType"].Equals("mainPoint"));
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
            */
        }

        private void CreatePointButton_Click(object sender, RoutedEventArgs e)
        {
            MapViewModel.CreatePoint();
        }

    }
}
