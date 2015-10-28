using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    /// <summary>
    /// Interaction logic for SampleTemplate.xaml
    /// </summary>
    public partial class ConvertScreenCoordinatesToWorldCoordinates : UserControl
    {
        public ConvertScreenCoordinatesToWorldCoordinates()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.TileType = TileType.SingleTile;
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add(worldOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            wpfMap1.Refresh();
        }

        private void wpfMap1_MapClick(object sender, MapClickWpfMapEventArgs e)
        {
            screenPosition.Text = "(" + e.ScreenX + ", " + e.ScreenY + ")";

            worldPosition.Text = string.Format(CultureInfo.InvariantCulture, "({0}, {1})", e.WorldX.ToString("N4", CultureInfo.InvariantCulture), e.WorldY.ToString("N4", CultureInfo.InvariantCulture));
        }
    }
}
