using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class FindDifferenceBetweenTwoFeatures : UserControl
    {
        public FindDifferenceBetweenTwoFeatures()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            // Setup the inMemoryLayer.   
            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = new AreaStyle(new GeoSolidBrush(new GeoColor(50, 100, 100, 200)));
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.RoyalBlue;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            inMemoryLayer.InternalFeatures.Add("AreaShape1", new Feature(new RectangleShape(10, 50, 50, 10).GetWellKnownBinary(), "AreaShape1"));
            inMemoryLayer.InternalFeatures.Add("AreaShape2", new Feature(new RectangleShape(30, 80, 80, 30).GetWellKnownBinary(), "AreaShape2"));

            LayerOverlay inmemoryOverlay = new LayerOverlay();
            inmemoryOverlay.TileType = TileType.SingleTile;
            inmemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            wpfMap1.Overlays.Add("InmemoryOverlay", inmemoryOverlay);

            wpfMap1.Refresh();
        }

        private void btnDifference_Click(object sender, RoutedEventArgs e)
        {
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("InMemoryFeatureLayer");

            if (inMemoryLayer.InternalFeatures.Count > 1)
            {
                AreaBaseShape sourceShape = (AreaBaseShape)inMemoryLayer.InternalFeatures["AreaShape2"].GetShape();
                AreaBaseShape targetShape = (AreaBaseShape)inMemoryLayer.InternalFeatures["AreaShape1"].GetShape();
                AreaBaseShape resultShape = sourceShape.GetDifference(targetShape);

                inMemoryLayer.InternalFeatures.Clear();
                inMemoryLayer.InternalFeatures.Add("ResultFeature", new Feature(resultShape.GetWellKnownBinary(), "ResultFeature"));
                inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.StandardColors.Blue);

                wpfMap1.Overlays["InmemoryOverlay"].Refresh();
            }
        }
    }
}
