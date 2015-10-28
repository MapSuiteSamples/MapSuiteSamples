using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class CreateABufferAroundAFeature : UserControl
    {
        public CreateABufferAroundAFeature()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-135.224054810107, 62.2893787272533, -58.8379651537011, 7.78687151295263);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.Open();
            Feature feature = worldLayer.QueryTools.GetFeatureById("137", new string[0]);
            worldLayer.Close();

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.GeographicColors.DeepOcean;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            inMemoryLayer.InternalFeatures.Add("POLYGON", feature);

            InMemoryFeatureLayer bufferLayer = new InMemoryFeatureLayer();
            bufferLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);
            bufferLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay inmemoryOverlay = new LayerOverlay();
            inmemoryOverlay.TransitionEffect = TransitionEffect.None;
            inmemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            wpfMap1.Overlays.Add("InmemoryOverlay", inmemoryOverlay);

            LayerOverlay bufferOverlay = new LayerOverlay();
            bufferOverlay.TransitionEffect = TransitionEffect.None;
            bufferOverlay.Layers.Add("BufferLayer", bufferLayer);
            wpfMap1.Overlays.Add("BufferOverlay", bufferOverlay);

            wpfMap1.Refresh();
        }

        private void btnBuffer_Click(object sender, RoutedEventArgs e)
        {
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("InMemoryFeatureLayer");
            InMemoryFeatureLayer bufferLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("BufferLayer");

            AreaBaseShape baseShape = (AreaBaseShape)inMemoryLayer.InternalFeatures["POLYGON"].GetShape();
            MultipolygonShape bufferedShape = baseShape.Buffer(100, 8, BufferCapType.Butt, GeographyUnit.DecimalDegree, DistanceUnit.Kilometer);
            Feature bufferFeature = new Feature(bufferedShape.GetWellKnownBinary(), "Buffer1");

            bufferLayer.InternalFeatures.Clear();
            bufferLayer.InternalFeatures.Add("BufferFeature", bufferFeature);

            wpfMap1.Overlays["BufferOverlay"].Refresh();
        }
    }
}
