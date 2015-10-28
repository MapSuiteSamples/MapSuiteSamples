using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DrawAndLabelPointsOfInterests : UserControl
    {
        public DrawAndLabelPointsOfInterests()
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
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.State1;
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.LineJoin = DrawingLineJoin.Round;
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer majorCitiesShapeLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\MajorCities.shp");
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.City1;
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.City1("AREANAME");
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.BestPlacement = true;
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            staticOverlay.Layers.Add("StatesLayer", statesLayer);
            staticOverlay.Layers.Add("MajorCitiesShapeLayer", majorCitiesShapeLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-126.4, 48.8, -67.0, 19.0);

            wpfMap1.Refresh();
        }
    }
}
