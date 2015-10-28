using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ConvertWorldCoordinatesToScreenCoordinates : UserControl
    {
        public ConvertWorldCoordinatesToScreenCoordinates()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.TileType = TileType.SingleTile;
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add(worldOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            wpfMap1.Refresh();
        }

        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            ScreenPointF screenPoint = ExtentHelper.ToScreenCoordinate(wpfMap1.CurrentExtent, new PointShape(Double.Parse(longitudeTextBox.Text, CultureInfo.InvariantCulture), Double.Parse(latitudeTextBox.Text, CultureInfo.InvariantCulture)), 740, 528);
            screenPositionTextBox.Text = string.Format(CultureInfo.InvariantCulture, "({0}, {1})", screenPoint.X.ToString("N1", CultureInfo.InvariantCulture), screenPoint.Y.ToString("N1", CultureInfo.InvariantCulture));
        }
    }
}
