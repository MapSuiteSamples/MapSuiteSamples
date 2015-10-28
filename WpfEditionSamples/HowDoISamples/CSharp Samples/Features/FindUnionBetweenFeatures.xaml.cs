using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class FindUnionBetweenFeatures : UserControl
    {
        public FindUnionBetweenFeatures()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-165.946875, 86.4359375, -35.86875, -6.3765625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            worldLayer.Open();
            Collection<Feature> allFeatures = worldLayer.QueryTools.GetAllFeatures(ReturningColumnsType.AllColumns);
            worldLayer.Close();

            // Setup the inMemoryLayer.   
            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = new AreaStyle(new GeoSolidBrush(GeoColor.FromArgb(100, GeoColor.SimpleColors.Green)));
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.SimpleColors.Green;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            foreach (Feature feature in allFeatures)
            {
                inMemoryLayer.InternalFeatures.Add(feature);
            }

            LayerOverlay inMemoryOverlay = new LayerOverlay();
            inMemoryOverlay.TileType = TileType.SingleTile;
            inMemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            wpfMap1.Overlays.Add("InMemoryOverlay", inMemoryOverlay);

            wpfMap1.Refresh();
        }

        private void btnUnion_Click(object sender, RoutedEventArgs e)
        {
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("InMemoryFeatureLayer");

            if (inMemoryLayer.InternalFeatures.Count > 1)
            {
                GeoCollection<AreaBaseShape> areaBaseShapes = new GeoCollection<AreaBaseShape>();

                GeoCollection<Feature> features = inMemoryLayer.InternalFeatures;
                foreach (Feature feature in features)
                {
                    AreaBaseShape targetAreaBaseShape = feature.GetShape() as AreaBaseShape;
                    if (targetAreaBaseShape != null)
                    {
                        areaBaseShapes.Add(targetAreaBaseShape);
                    }
                }

                AreaBaseShape unionedAreaBaseShape = AreaBaseShape.Union(areaBaseShapes);
                unionedAreaBaseShape.Id = "UnionedFeature";
                inMemoryLayer.InternalFeatures.Clear();
                inMemoryLayer.InternalFeatures.Add("UnionedFeature", new Feature(unionedAreaBaseShape));
                inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);

                wpfMap1.Overlays["InMemoryOverlay"].Refresh();
            }
        }
    }
}
