using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class AddAFeatureSourceMarkerOverlay : UserControl
    {
        public AddAFeatureSourceMarkerOverlay()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-131.22, 55.05, -54.03, 16.91);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            FeatureSourceMarkerOverlay markerOverlay = new FeatureSourceMarkerOverlay();
            markerOverlay.FeatureSource = new ShapeFileFeatureSource(@"..\..\SampleData\Data\MajorCities.shp");
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.ImageSource = new BitmapImage(new Uri("/Resources/AQUA.png", UriKind.RelativeOrAbsolute));
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.Width = 20;
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.Height = 34;
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.YOffset = -17;
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.ToolTip = "This is [#AREANAME#].";
            markerOverlay.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            wpfMap1.Overlays.Add("MarkerOverlay", markerOverlay);

            wpfMap1.Refresh();
        }
    }
}
