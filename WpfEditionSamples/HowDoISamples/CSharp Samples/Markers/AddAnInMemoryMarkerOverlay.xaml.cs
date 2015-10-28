using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class AddAnInMemoryMarkerOverlay : UserControl
    {
        public AddAnInMemoryMarkerOverlay()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            InMemoryMarkerOverlay markerOverlay = new InMemoryMarkerOverlay();
            markerOverlay.Columns.Add(new FeatureSourceColumn("Name"));
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.ImageSource = new BitmapImage(new Uri("/Resources/AQUA.png", UriKind.RelativeOrAbsolute));
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.Width = 20;
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.Height = 34;
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.YOffset = -17;
            markerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.ToolTip = "This is [#Name#].";
            markerOverlay.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            wpfMap1.Overlays.Add("MarkerOverlay", markerOverlay);

            Feature newFeature = new Feature(-95.2806, 38.9554);
            newFeature.ColumnValues.Add("Name", "Lawrence");

            markerOverlay.FeatureSource.BeginTransaction();
            markerOverlay.FeatureSource.AddFeature(newFeature);
            markerOverlay.FeatureSource.CommitTransaction();

            wpfMap1.Refresh();
        }
    }
}
