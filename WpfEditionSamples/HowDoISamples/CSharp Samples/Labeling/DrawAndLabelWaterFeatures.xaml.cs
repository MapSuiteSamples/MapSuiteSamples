using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DrawAndLabelWaterFeatures : UserControl
    {
        public DrawAndLabelWaterFeatures()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            ShapeFileFeatureLayer utahWaterShapeLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\UtahWater.shp");
            utahWaterShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Water1;
            utahWaterShapeLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer utahWaterLabelLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\UtahWater.shp");
            utahWaterLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.Water1("Landname");
            utahWaterLabelLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay utahWaterOverlay = new LayerOverlay();
            utahWaterOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.FromArgb(255, 233, 232, 214))));
            utahWaterOverlay.Layers.Add("UtahWaterShapes", utahWaterShapeLayer);
            utahWaterOverlay.Layers.Add("UtahWaterLabels", utahWaterLabelLayer);
            wpfMap1.Overlays.Add("UtahWaterOverlay", utahWaterOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-113.11473388671875, 41.949816894531253, -111.08226318359375, 40.499621582031253);

            wpfMap1.Refresh();
        }
    }
}
