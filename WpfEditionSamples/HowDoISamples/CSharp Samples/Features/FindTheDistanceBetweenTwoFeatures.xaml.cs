using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System.Globalization;

namespace CSHowDoISamples
{
    public partial class FindTheDistanceBetweenTwoFeatures : UserControl
    {
        public FindTheDistanceBetweenTwoFeatures()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-128.31093750000002, 95.25, 131.84531249999998, -63.65625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            InMemoryFeatureLayer usInMemoryLayer = new InMemoryFeatureLayer();
            usInMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.PointType = PointType.Bitmap;
            usInMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.Image = new GeoImage(@"..\..\SampleData\Data\United States.png");
            usInMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            usInMemoryLayer.InternalFeatures.Add("US", new Feature(-98.58, 39.57, "1"));

            InMemoryFeatureLayer chinaInMemoryLayer = new InMemoryFeatureLayer();
            chinaInMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.PointType = PointType.Bitmap;
            chinaInMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.Image = new GeoImage(@"..\..\SampleData\Data\China.png");
            chinaInMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            chinaInMemoryLayer.InternalFeatures.Add("CHINA", new Feature(104.72, 34.45, "2"));

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            staticOverlay.Layers.Add("USInMemoryFeatureLayer", usInMemoryLayer);
            staticOverlay.Layers.Add("ChinaInMemoryFeatureLayer", chinaInMemoryLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }

        private void btnGetDistance_Click(object sender, RoutedEventArgs e)
        {
            InMemoryFeatureLayer usInMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("USInMemoryFeatureLayer");
            InMemoryFeatureLayer chinaInMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("ChinaInMemoryFeatureLayer");

            BaseShape usShape = usInMemoryLayer.InternalFeatures["US"].GetShape();
            BaseShape chinaShape = chinaInMemoryLayer.InternalFeatures["CHINA"].GetShape();

            double distance = usShape.GetDistanceTo(chinaShape, GeographyUnit.DecimalDegree, DistanceUnit.Kilometer);
            txtDistance.Text = string.Format(CultureInfo.InvariantCulture, "{0 :N4} Km", distance);
        }
    }
}
