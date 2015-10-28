using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ChangeTheLookOfAFeatureLayer : UserControl
    {
        public ChangeTheLookOfAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.TileType = TileType.SingleTile;
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }

        private void btnStyle1_Click(object sender, RoutedEventArgs e)
        {
            wpfMap1.FindFeatureLayer("WorldLayer").ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country2;
            wpfMap1.Overlays["WorldOverlay"].Refresh();
        }

        private void btnStyle2_Click(object sender, RoutedEventArgs e)
        {
            wpfMap1.FindFeatureLayer("WorldLayer").ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            wpfMap1.Overlays["WorldOverlay"].Refresh();
        }
    }
}
