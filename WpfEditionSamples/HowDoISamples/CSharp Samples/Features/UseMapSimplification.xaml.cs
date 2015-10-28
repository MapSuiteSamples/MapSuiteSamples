using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System;
using System.Globalization;

namespace CSHowDoISamples
{
    public partial class UseMapSimplification : UserControl
    {
        private AreaBaseShape areaBaseShape;

        public UseMapSimplification()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-177.39584350585937, 83.113876342773437, -52.617362976074219, 14.550546646118164);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");

            worldLayer.Open();
            Feature feature = worldLayer.QueryTools.GetFeatureById("135", new string[0]);
            areaBaseShape = (AreaBaseShape)feature.GetShape();
            worldLayer.Close();

            InMemoryFeatureLayer simplificationLayer = new InMemoryFeatureLayer();
            simplificationLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            simplificationLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            simplificationLayer.InternalFeatures.Add(feature);

            LayerOverlay simplificationOverlay = new LayerOverlay();
            simplificationOverlay.TileType = TileType.SingleTile;
            simplificationOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            simplificationOverlay.Layers.Add("SimplificationLayer", simplificationLayer);
            wpfMap1.Overlays.Add("SimplificationOverlay", simplificationOverlay);

            cmbSimplificationType.SelectedIndex = 0;
            cmbTolerance.SelectedIndex = 0;

            wpfMap1.Refresh();
        }

        private void btnSimplify_Click(object sender, RoutedEventArgs e)
        {
            InMemoryFeatureLayer simplificationLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("SimplificationLayer");

            double tolerance = Convert.ToDouble(cmbTolerance.SelectedItem.ToString().Split(':')[1], CultureInfo.InvariantCulture);
            SimplificationType simplificationType = (SimplificationType)cmbSimplificationType.SelectedIndex;

            MultipolygonShape multipolygonShape = areaBaseShape.Simplify(tolerance, simplificationType);
            simplificationLayer.InternalFeatures.Clear();
            simplificationLayer.InternalFeatures.Add(new Feature(multipolygonShape));

            wpfMap1.Overlays["SimplificationOverlay"].Refresh();
        }
    }
}
