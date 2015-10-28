using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    /// <summary>
    /// Interaction logic for PlotALatitudeAndLongitudePointOnTheMap.xaml
    /// </summary>
    public partial class PlotALatitudeAndLongitudePointOnTheMap : UserControl
    {
        public PlotALatitudeAndLongitudePointOnTheMap()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            InMemoryFeatureLayer pointLayer = new InMemoryFeatureLayer();
            pointLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.PointType = PointType.Bitmap;
            pointLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.Image = new GeoImage(@"..\..\SampleData\Data\United States.png");
            pointLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay pointOverlay = new LayerOverlay();
            pointOverlay.TileType = TileType.SingleTile;
            pointOverlay.Layers.Add("PointLayer", pointLayer);
            wpfMap1.Overlays.Add("PointOverlay", pointOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            wpfMap1.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double latitude = double.Parse(tbLat.Text);
            double longitude = double.Parse(tbLon.Text);
            Feature feature = new Feature(longitude, latitude, "Point1");

            LayerOverlay pointOverlay = (LayerOverlay)wpfMap1.Overlays["PointOverlay"];
            InMemoryFeatureLayer pointLayer = (InMemoryFeatureLayer)pointOverlay.Layers["PointLayer"];
            pointLayer.InternalFeatures.Clear();
            if (!pointLayer.InternalFeatures.Contains("Point1"))
            {
                pointLayer.InternalFeatures.Add("Point1", feature);
            }

            pointOverlay.Refresh();
        }
    }
}
