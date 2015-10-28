using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class FindFeaturesWithinDistance : UserControl
    {
        private PointShape pointShape;

        public FindFeaturesWithinDistance()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-143.4, 109.3, 116.7, -76.3);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            InMemoryFeatureLayer highlightLayer = new InMemoryFeatureLayer();
            highlightLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.Capital2;
            highlightLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Evergreen1;
            highlightLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.TransitionEffect = TransitionEffect.None;
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            LayerOverlay dynamicOverlay = new LayerOverlay();
            dynamicOverlay.TileType = TileType.SingleTile;
            dynamicOverlay.Layers.Add("HighlightLayer", highlightLayer);
            wpfMap1.Overlays.Add("HighlightOverlay", dynamicOverlay);

            wpfMap1.MapClick += new EventHandler<MapClickWpfMapEventArgs>(wpfMap1_MapClick);
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            wpfMap1.Refresh();
        }

        private void wpfMap1_MapClick(object sender, MapClickWpfMapEventArgs e)
        {
            pointShape = e.WorldLocation;
            FindWithinDistanceFeatures();
        }

        private void cmbDistance_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pointShape != null)
            {
                FindWithinDistanceFeatures();
            }
        }

        private void FindWithinDistanceFeatures()
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("WorldLayer");
                InMemoryFeatureLayer highlightLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("HighlightLayer");

                // Find the countries within special distance.
                double distance = Convert.ToDouble(cmbDistance.SelectedItem.ToString().Split(':')[1], CultureInfo.InvariantCulture);
                worldLayer.Open();
                Collection<Feature> selectedFeatures = worldLayer.QueryTools.GetFeaturesWithinDistanceOf(pointShape, GeographyUnit.DecimalDegree, DistanceUnit.Kilometer, distance, new string[0]);
                worldLayer.Close();

                if (highlightLayer.InternalFeatures.Count > 0)
                {
                    highlightLayer.InternalFeatures.Clear();
                }

                highlightLayer.InternalFeatures.Add("Point", new Feature(pointShape));
                foreach (Feature feature in selectedFeatures)
                {
                    highlightLayer.InternalFeatures.Add(feature.Id, feature);
                }

                wpfMap1.Overlays["HighlightOverlay"].Refresh();
            }
        }
    }
}
