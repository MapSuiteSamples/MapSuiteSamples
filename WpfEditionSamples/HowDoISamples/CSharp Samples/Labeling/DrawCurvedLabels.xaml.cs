using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DrawCurvedLabels : UserControl
    {
        public DrawCurvedLabels()
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
            austinStreetsLabelLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            TextStyle textStyle = TextStyles.LocalRoad1("FENAME");
            textStyle.TextLineSegmentRatio = double.MaxValue;
            textStyle.SplineType = SplineType.StandardSplining;
            austinStreetsLabelLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(textStyle);

            LayerOverlay austinStreetsOverlay = new LayerOverlay();
            austinStreetsOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.FromArgb(255, 233, 232, 214))));
            austinStreetsOverlay.Layers.Add("AustinStreetsShapeLayer", austinStreetsShapeLayer);
            austinStreetsOverlay.Layers.Add("AustinStreetsLabelLayer", austinStreetsLabelLayer);
            wpfMap1.Overlays.Add("AustinStreetsOverlay", austinStreetsOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-97.6881803712033, 30.3177912428115, -97.6723016938352, 30.3064615919325);

            wpfMap1.Refresh();
        }
    }
}
