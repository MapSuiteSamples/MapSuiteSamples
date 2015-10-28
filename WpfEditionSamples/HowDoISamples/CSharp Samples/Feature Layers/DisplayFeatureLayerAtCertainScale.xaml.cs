using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DisplayFeatureLayerAtCertainScale : UserControl
    {
        public DisplayFeatureLayerAtCertainScale()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-180.0, 83.0, -20.0, -20.0);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.Name = "Countries02";
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            statesLayer.Name = "USStates";
            statesLayer.ZoomLevelSet.ZoomLevel04.DefaultAreaStyle = AreaStyles.State1;
            statesLayer.ZoomLevelSet.ZoomLevel04.DefaultAreaStyle.OutlinePen.LineJoin = DrawingLineJoin.Round;
            statesLayer.ZoomLevelSet.ZoomLevel04.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer citiesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\MajorCities.shp");
            citiesLayer.Name = "MajorCities";
            citiesLayer.ZoomLevelSet.ZoomLevel06.DefaultPointStyle = PointStyles.City1;
            citiesLayer.ZoomLevelSet.ZoomLevel06.DefaultTextStyle = TextStyles.City1("areaname");
            citiesLayer.ZoomLevelSet.ZoomLevel06.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            worldOverlay.Layers.Add("StatesLayer", statesLayer);
            worldOverlay.Layers.Add("CitiesLayer", citiesLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }

        private void btnLow_Click(object sender, RoutedEventArgs e)
        {
            ZoomLevelSet zoomLevelSet = new ZoomLevelSet();
            wpfMap1.CurrentExtent = ExtentHelper.ZoomToScale(zoomLevelSet.ZoomLevel06.Scale, wpfMap1.CurrentExtent, GeographyUnit.DecimalDegree, (float)wpfMap1.ActualWidth, (float)wpfMap1.ActualHeight);
            wpfMap1.Refresh();
        }

        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            ZoomLevelSet zoomLevelSet = new ZoomLevelSet();
            wpfMap1.CurrentExtent = ExtentHelper.ZoomToScale(zoomLevelSet.ZoomLevel05.Scale, wpfMap1.CurrentExtent, GeographyUnit.DecimalDegree, (float)wpfMap1.ActualWidth, (float)wpfMap1.ActualHeight);
            wpfMap1.Refresh();
        }

        private void btnHigh_Click(object sender, RoutedEventArgs e)
        {
            ZoomLevelSet zoomLevelSet = new ZoomLevelSet();
            wpfMap1.CurrentExtent = ExtentHelper.ZoomToScale(zoomLevelSet.ZoomLevel03.Scale, wpfMap1.CurrentExtent, GeographyUnit.DecimalDegree, (float)wpfMap1.ActualWidth, (float)wpfMap1.ActualHeight);
            wpfMap1.Refresh();
        }
    }
}
