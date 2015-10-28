using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DetermineTheEnvelopeOfAFeature : UserControl
    {
        public DetermineTheEnvelopeOfAFeature()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-132.72421875, 46.709375, -67.68515625, 0.303124999999994);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            // Setup the BoundingBox InMemoryFeatureLayer.   
            InMemoryFeatureLayer boundingBoxLayer = new InMemoryFeatureLayer();
            boundingBoxLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);
            boundingBoxLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.SimpleColors.Green;
            boundingBoxLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay boundingboxOverlay = new LayerOverlay();
            boundingboxOverlay.TileType = TileType.SingleTile;
            boundingboxOverlay.Layers.Add("BoundingBoxLayer", boundingBoxLayer);
            wpfMap1.Overlays.Add("BoundingBoxOverlay", boundingboxOverlay);

            wpfMap1.Refresh();
        }

        private void btnGetEnvelope_Click(object sender, RoutedEventArgs e)
        {
            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.Open();
            RectangleShape usBoundingBox = worldLayer.QueryTools.GetFeatureById("137", new string[0]).GetBoundingBox();
            worldLayer.Close();

            InMemoryFeatureLayer boundingBoxLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("BoundingBoxLayer");
            if (!boundingBoxLayer.InternalFeatures.Contains("BoundingBox"))
            {
                boundingBoxLayer.InternalFeatures.Add("BoundingBox", new Feature(usBoundingBox.GetWellKnownBinary(), "BoundingBox"));
            }

            wpfMap1.Overlays["BoundingBoxOverlay"].Refresh();
        }
    }
}
