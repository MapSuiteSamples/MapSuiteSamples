using System;
using System.Linq;
using System.Windows;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace ThinkGeo.MapSuite.Overlays
{
    public partial class MainWindow : Window
    {
        private static readonly RectangleShape globeExtent = new RectangleShape(-10790688.9993993, 3923186.86300616, -10766229.2115264, 3906401.33357841);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs args)
        {
            //  Set the map unit as Meter. The default value is DecimalDegree.
            map.MapUnit = GeographyUnit.Meter;
            
            //  Set the extent of the mapcontrol
            map.CurrentExtent = globeExtent;

            //  Set the Logo disabled, or it will be displayed at the bottom-right corner of map control by default
            //  The default value is true.
            map.MapTools.Logo.IsEnabled = false;
            
            //  Set the MouseCoordinate enabled. It indicators that whether can get the coordiate text when mouse moves in the map control.
            //  The default value is false.
            map.MapTools.MouseCoordinate.IsEnabled = true;

            //  Set the MouseCoordinate type as Custom.
            //  So it can raise the CustomFormatted event when mouse moves in the map control, if the MouseCoordinate.IsEnabled above is "true"
            map.MapTools.MouseCoordinate.MouseCoordinateType = MouseCoordinateType.Custom;
            map.MapTools.MouseCoordinate.CustomFormatted += (s, e) =>
            {
                coordinateX.Text = String.Format("X:{0:N2}", e.WorldCoordinate.X);
                coordinateY.Text = String.Format("Y:{0:N2}", e.WorldCoordinate.Y);
            };

            //  Reset the extent of the map control to default.
            map.MapTools.PanZoomBar.GlobeButtonClick += (s, e) => e.NewExtent = globeExtent;

            OverlaySwitcher overlaySwitcher = new OverlaySwitcher();
            overlaySwitcher.OverlayChanged += (s, e) =>
            {
                BingMapsOverlay bingMapsOverlay = e.Overlay as BingMapsOverlay;
                if (bingMapsOverlay != null)
                {
                    e.Cancel = ApplyBingMapsKey();
                }
            };
            map.MapTools.Add(overlaySwitcher);

            InitializeOverlays();

            map.Refresh();
        }

        private void InitializeOverlays()
        {
            WorldMapKitWmsWpfOverlay worldMapKitOverlay = OverlayBuilder.CreateWorldMapKitWmsWpfOverlayer();
            map.Overlays.Add(worldMapKitOverlay);

            OpenStreetMapOverlay openStreetMapOverlay = OverlayBuilder.CreateOpenStreetMapOverlay();
            openStreetMapOverlay.IsVisible = false;
            map.Overlays.Add(openStreetMapOverlay);

            BingMapsOverlay bingMapsOverlayer = OverlayBuilder.CreateBingMapsOverlay();
            bingMapsOverlayer.IsVisible = false;
            map.Overlays.Add(bingMapsOverlayer);

            GoogleMapsOverlay googleMapsOverlayer = OverlayBuilder.CreateGoogleMapsOverlay();
            googleMapsOverlayer.IsVisible = false;
            map.Overlays.Add(googleMapsOverlayer);

            LayerOverlay layerOverlay = OverlayBuilder.CreateCustomLayerOverlay();
            map.Overlays.Add(layerOverlay);

            AdornmentOverlay adornmentOverlay = OverlayBuilder.CreateAdornmentOverlay();
            map.Overlays.Add(adornmentOverlay);

            PopupOverlay popupOverlay = OverlayBuilder.CreatePopupOverlay();
            map.Overlays.Add(popupOverlay);
            popupOverlay.Open();

            SimpleMarkerOverlay simpleMarkerOverlay = OverlayBuilder.CreateSimpleMarkerOverlay();
            map.Overlays.Add(simpleMarkerOverlay);

            InMemoryMarkerOverlay inMemoryMarkerOverlay = OverlayBuilder.CreateInMemoryMarkerOverlay();
            map.Overlays.Add(inMemoryMarkerOverlay);

        }

        private bool ApplyBingMapsKey()
        {
            bool cancel = false;
            string bingMapsKey = MapSuiteSampleHelper.GetBingMapsKey();
            if (!string.IsNullOrEmpty(bingMapsKey))
            {
                foreach (BingMapsOverlay bingOverlay in map.Overlays.OfType<BingMapsOverlay>())
                {
                    bingOverlay.ApplicationId = bingMapsKey;
                }
            }
            else
            {
                cancel = true;
            }

            return cancel;
        }
    }
}
