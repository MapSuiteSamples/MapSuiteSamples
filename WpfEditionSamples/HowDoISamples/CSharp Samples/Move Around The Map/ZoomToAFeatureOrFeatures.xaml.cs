using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System.Collections.ObjectModel;

namespace CSHowDoISamples
{
    public partial class ZoomToAFeatureOrFeatures : UserControl
    {
        public ZoomToAFeatureOrFeatures()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.TransitionEffect = TransitionEffect.None;
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", staticOverlay);

            wpfMap1.Refresh();
        }

        private void btnMultipleFeatures_Click(object sender, RoutedEventArgs e)
        {
            Collection<string> featureIDs = new Collection<string>();
            featureIDs.Add("63");  // For US
            featureIDs.Add("6");   // For Canada
            featureIDs.Add("137"); // For Mexico

            FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("WorldLayer");
            lock (worldLayer)
            {
                if (!worldLayer.IsOpen) worldLayer.Open();
                Collection<Feature> features = worldLayer.FeatureSource.GetFeaturesByIds(featureIDs, new string[0]);
                wpfMap1.CurrentExtent = ExtentHelper.GetBoundingBoxOfItems(features);
            }

            wpfMap1.Refresh();
        }

        private void btnOneFeature_Click(object sender, RoutedEventArgs e)
        {
            FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("WorldLayer");
            lock (worldLayer)
            {
                if (!worldLayer.IsOpen) worldLayer.Open();
                wpfMap1.CurrentExtent = worldLayer.FeatureSource.GetBoundingBoxById("137");
            }

            wpfMap1.Refresh();
        }
    }
}
