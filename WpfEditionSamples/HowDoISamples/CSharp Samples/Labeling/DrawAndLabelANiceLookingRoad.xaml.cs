using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DrawAndLabelANiceLookingRoad : UserControl
    {
        public DrawAndLabelANiceLookingRoad()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            ShapeFileFeatureLayer austinStreetsShapeLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\austinstreets.shp");
            austinStreetsShapeLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            austinStreetsShapeLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(LineStyles.LocalRoad1);

            ShapeFileFeatureLayer austinStreetsLabelLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\austinstreets.shp");
            austinStreetsLabelLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(TextStyles.LocalRoad1("FENAME"));
            austinStreetsLabelLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.FromArgb(255, 233, 232, 214))));
            staticOverlay.Layers.Add("AustinStreetsShapeLayer", austinStreetsShapeLayer);
            staticOverlay.Layers.Add("AustinStreetsLabelLayer", austinStreetsLabelLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-97.749141617693908, 30.300592918607943, -97.741202279009826, 30.29492809316849);

            wpfMap1.Refresh();
        }
    }
}
