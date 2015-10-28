using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class HighlightAFeatureOnTheMap : UserControl
    {
        public HighlightAFeatureOnTheMap()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));

            InMemoryFeatureLayer highlightLayer = new InMemoryFeatureLayer();
            highlightLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            highlightLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.FromArgb(100, GeoColor.StandardColors.DarkGreen));

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            LayerOverlay highlightOverlay = new LayerOverlay();
            highlightOverlay.TileType = TileType.SingleTile;
            highlightOverlay.Layers.Add("HighlightLayer", highlightLayer);
            wpfMap1.Overlays.Add("HighlightOverlay", highlightOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-177.333203125, 92.96484375, 82.823046875, -89.84765625);
            wpfMap1.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LayerOverlay worldOverlay = (LayerOverlay)wpfMap1.Overlays["WorldOverlay"];
            FeatureLayer worldLayer = (FeatureLayer)worldOverlay.Layers["WorldLayer"];

            LayerOverlay highlightOverlay = (LayerOverlay)wpfMap1.Overlays["HighlightOverlay"];
            InMemoryFeatureLayer highlightLayer = (InMemoryFeatureLayer)highlightOverlay.Layers["HighlightLayer"];

            worldLayer.Open();
            Feature feature = worldLayer.QueryTools.GetFeatureById("135", new string[0]);
            worldLayer.Close();

            highlightLayer.InternalFeatures.Clear();
            highlightLayer.InternalFeatures.Add("135", feature);

            highlightOverlay.Refresh();
        }
    }
}
