using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class CreateASpatialIndexForAShapeFileFeatureLayer : UserControl
    {
        public CreateASpatialIndexForAShapeFileFeatureLayer()
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
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }

        private void buildSpatialIndex_Click(object sender, RoutedEventArgs e)
        {
            ShapeFileFeatureLayer.BuildIndexFile(@"..\..\SampleData\Data\Countries02.shp", BuildIndexMode.DoNotRebuild);
            MessageBox.Show("Finish building spatial index.", "BuildIndexFile", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
        }
    }
}
