using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class MoveAFeature : UserControl
    {
        public MoveAFeature()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.SimpleColors.Green;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.Open();
            Feature feature = worldLayer.QueryTools.GetFeatureById("135", new string[0]);
            worldLayer.Close();

            inMemoryLayer.InternalFeatures.Add("135", feature);

            LayerOverlay inMemoryOverlay = new LayerOverlay();
            inMemoryOverlay.TileType = TileType.SingleTile;
            inMemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            wpfMap1.Overlays.Add("InMemoryOverlay", inMemoryOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-165.946875, 86.4359375, -35.86875, -6.3765625);

            wpfMap1.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Tag.ToString())
            {
                case "Move Up":
                    TranslateByOffset(0, 20);
                    break;
                case "Move Down":
                    TranslateByOffset(0, -20);
                    break;
                case "Move Left":
                    TranslateByOffset(-20, 0);
                    break;
                case "Move Right": 
                    TranslateByOffset(20, 0);
                    break;
            }
        }

        private void TranslateByOffset(double xOffset, double yOffset)
        {
            LayerOverlay inMemoryOverlay = (LayerOverlay)wpfMap1.Overlays["InMemoryOverlay"];
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)inMemoryOverlay.Layers["InMemoryFeatureLayer"];

            inMemoryLayer.Open();
            inMemoryLayer.EditTools.BeginTransaction();
            inMemoryLayer.EditTools.TranslateByOffset("135", xOffset, yOffset, GeographyUnit.Meter, DistanceUnit.Meter);
            inMemoryLayer.EditTools.CommitTransaction();
            inMemoryLayer.Close();

            inMemoryOverlay.Refresh();
        }
    }
}
