using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class LoadAHeatLayer : UserControl
    {
        public LoadAHeatLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.MapTools.MouseCoordinate.IsEnabled = true;
            wpfMap1.CurrentExtent = new RectangleShape(-126.4, 48.8, -67.0, 19.0);
            wpfMap1.MaximumScale = 36911986.875;

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureSource featureSource = new ShapeFileFeatureSource(@"..\..\SampleData\Data\cities_e.shp");
            HeatLayer heatLayer = new HeatLayer(featureSource);
            heatLayer.HeatStyle = new HeatStyle(10, 75, DistanceUnit.Kilometer);

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.TransitionEffect = TransitionEffect.None;
            layerOverlay.Layers.Add(heatLayer);
            layerOverlay.OverlayCanvas.Opacity = .8;
            wpfMap1.Overlays.Add(layerOverlay);

            wpfMap1.Refresh();
        }
    }
}
