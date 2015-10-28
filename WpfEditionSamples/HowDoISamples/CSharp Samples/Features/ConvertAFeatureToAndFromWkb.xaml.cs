using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System;

namespace CSHowDoISamples
{
    public partial class ConvertAFeatureToAndFromWkb : UserControl
    {
        public ConvertAFeatureToAndFromWkb()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-116.18203125000002, 77.671875, 143.97421874999998, -60.4921875);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }

        private void convertWkbToFeature_Click(object sender, RoutedEventArgs e)
        {
            byte[] wellKnownBinary = Convert.FromBase64String(wkbTextBox.Text);
            Feature feature = new Feature(wellKnownBinary);

            wktResultTextBox.Text = feature.GetWellKnownText();
        }

        private void convertFeatureToWkb_Click(object sender, RoutedEventArgs e)
        {
            Feature feature = new Feature(wktTextBox.Text);
            byte[] wellKnownBinary = feature.GetWellKnownBinary();

            wkbResultTextBox.Text = Convert.ToBase64String(wellKnownBinary);
        }
    }
}
