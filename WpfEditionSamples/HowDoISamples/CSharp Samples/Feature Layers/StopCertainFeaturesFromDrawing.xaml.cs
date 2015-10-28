using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class StopCertainFeaturesFromDrawing : UserControl
    {
        public StopCertainFeaturesFromDrawing()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-143.4, 109.3, 116.7, -76.3);
            wpfMap1.BackgroundOverlay.BackgroundBrush = new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.RequiredColumnNames.Add("POP_CNTRY");
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLayer.DrawingFeatures += new EventHandler<DrawingFeaturesEventArgs>(worldLayer_DrawingFeatures);

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }

        private void worldLayer_DrawingFeatures(object sender, DrawingFeaturesEventArgs e)
        {
            Collection<Feature> featuresToDrawn = new Collection<Feature>();
            foreach (Feature feature in e.FeaturesToDraw)
            {
                double population = Convert.ToDouble(feature.ColumnValues["POP_CNTRY"], CultureInfo.InvariantCulture);
                if (population > 10000000)
                {
                    featuresToDrawn.Add(feature);
                }
            }

            e.FeaturesToDraw.Clear();
            foreach (Feature feature in featuresToDrawn)
            {
                e.FeaturesToDraw.Add(feature);
            }
        }
    }
}
