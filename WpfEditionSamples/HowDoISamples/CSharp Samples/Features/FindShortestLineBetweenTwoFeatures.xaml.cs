using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class FindShortestLineBetweenTwoFeatures : UserControl
    {
        public FindShortestLineBetweenTwoFeatures()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(0, 100, 100, 0);

            // Setup the inMemoryLayer.
            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(125, GeoColor.StandardColors.Gray);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Black;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            inMemoryLayer.InternalFeatures.Add("AreaShape1", new Feature("POLYGON((10 20,30 60,40 10,10 20))", "AreaShape1"));
            BaseShape shape = new EllipseShape(new PointShape(70, 70), 10, 20);
            shape.Id = "AreaShape2";
            inMemoryLayer.InternalFeatures.Add("AreaShape2", new Feature(shape));

            InMemoryFeatureLayer shortestLineLayer = new InMemoryFeatureLayer();
            shortestLineLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle.OuterPen.Color = GeoColor.StandardColors.Red;
            shortestLineLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay inMemoryOverlay = new LayerOverlay();
            inMemoryOverlay.TransitionEffect = TransitionEffect.None;
            inMemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            wpfMap1.Overlays.Add("InMemoryOverlay", inMemoryOverlay);

            LayerOverlay shortestLineOverlay = new LayerOverlay();
            shortestLineOverlay.TileType = TileType.SingleTile;
            shortestLineOverlay.Layers.Add("ShortestLineLayer", shortestLineLayer);
            wpfMap1.Overlays.Add("ShortestLineOverlay", shortestLineOverlay);

            wpfMap1.Refresh();
        }

        private void btnGetShortestLine_Click(object sender, RoutedEventArgs e)
        {
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("InMemoryFeatureLayer");
            InMemoryFeatureLayer shortestLineLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("ShortestLineLayer");

            BaseShape areaShape1 = inMemoryLayer.InternalFeatures["AreaShape1"].GetShape();
            BaseShape areaShape2 = inMemoryLayer.InternalFeatures["AreaShape2"].GetShape();
            MultilineShape multiLineShape = areaShape1.GetShortestLineTo(areaShape2, GeographyUnit.Meter);

            shortestLineLayer.InternalFeatures.Clear();
            shortestLineLayer.InternalFeatures.Add("ShortestLine", new Feature(multiLineShape.GetWellKnownBinary(), "ShortestLine"));

            wpfMap1.Overlays["ShortestLineOverlay"].Refresh();
        }
    }
}
