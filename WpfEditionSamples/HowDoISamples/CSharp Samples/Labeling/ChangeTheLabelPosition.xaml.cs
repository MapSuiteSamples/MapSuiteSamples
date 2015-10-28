using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ChangeTheLabelPosition : UserControl
    {
        public ChangeTheLabelPosition()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;

            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer worldLabelLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.CreateSimpleTextStyle("STATE_NAME", "Arial", 7, DrawingFontStyles.Bold, GeoColor.FromArgb(255, 91, 91, 91));
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.PointPlacement = PointPlacement.Center;
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.OverlappingRule = LabelOverlappingRule.NoOverlapping;
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.DuplicateRule = LabelDuplicateRule.NoDuplicateLabels;
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.LabelPositions.Add("33", new WorldLabelingCandidate("Kansas State", new PointShape(-91.3969, 28.1016)));
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.LabelPositions.Add("4", new WorldLabelingCandidate("North Dakota State", new PointShape(-101.09, 51.11)));
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.LabelPositions.Add("24", new WorldLabelingCandidate("California State", new PointShape(-126.2, 36.27)));

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            staticOverlay.Layers.Add("StatesLayer", statesLayer);
            staticOverlay.Layers.Add("worldLabelLayer", worldLabelLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-126.4, 48.8, -67.0, 19.0);

            wpfMap1.Refresh();
        }
    }
}
