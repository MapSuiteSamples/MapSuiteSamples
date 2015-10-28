using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ScaleUpAndDownAFeature : UserControl
    {
        public ScaleUpAndDownAFeature()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-165.946875, 86.4359375, -35.86875, -6.3765625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.Open();
            Feature feature = worldLayer.QueryTools.GetFeatureById("135", new string[0]);
            worldLayer.Close();

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.SimpleColors.Green;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            inMemoryLayer.InternalFeatures.Add("135", feature);

            LayerOverlay inMemoryOverlay = new LayerOverlay();
            inMemoryOverlay.TileType = TileType.SingleTile;
            inMemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            wpfMap1.Overlays.Add("InMemoryOverlay", inMemoryOverlay);

            wpfMap1.Refresh();
        }

        private void btnScaleUp_Click(object sender, RoutedEventArgs e)
        {
            UpdateFeatureByScale(25, true);
        }

        private void btnScaleDown_Click(object sender, RoutedEventArgs e)
        {
            UpdateFeatureByScale(20, false);
        }

        private void UpdateFeatureByScale(double percentage, bool isScaleUp)
        {
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("InMemoryFeatureLayer");
            inMemoryLayer.Open();
            inMemoryLayer.EditTools.BeginTransaction();
            if (isScaleUp)
            {
                inMemoryLayer.EditTools.ScaleUp("135", percentage);
            }
            else
            {
                inMemoryLayer.EditTools.ScaleDown("135", percentage);
            }
            inMemoryLayer.EditTools.CommitTransaction();
            inMemoryLayer.Close();

            wpfMap1.Overlays["InMemoryOverlay"].Refresh();
        }
    }
}
