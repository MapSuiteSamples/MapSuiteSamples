using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class UseRotationProjectionForAFeatureLayer : UserControl
    {
        private RotationProjection rotateProjection;

        public UseRotationProjectionForAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            rotateProjection = new RotationProjection();
            wpfMap1.CurrentExtent = rotateProjection.GetUpdatedExtent(new RectangleShape(-180.0, 83.0, 180.0, -90.0));

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLayer.FeatureSource.Projection = rotateProjection;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.TileType = TileType.SingleTile;
            worldOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }

        private void btnRotateCounterclockwise_Click(object sender, RoutedEventArgs e)
        {
            rotateProjection.Angle += 20;
            wpfMap1.CurrentExtent = rotateProjection.GetUpdatedExtent(wpfMap1.CurrentExtent);

            wpfMap1.Overlays["WorldOverlay"].Refresh();
        }

        private void btnRotateClockwise_Click(object sender, RoutedEventArgs e)
        {
            rotateProjection.Angle -= 20;
            wpfMap1.CurrentExtent = rotateProjection.GetUpdatedExtent(wpfMap1.CurrentExtent);

            wpfMap1.Overlays["WorldOverlay"].Refresh();
        }
    }
}
