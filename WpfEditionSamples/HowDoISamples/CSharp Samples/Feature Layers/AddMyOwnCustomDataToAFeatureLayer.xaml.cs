using System;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class AddMyOwnCustomDataToAFeatureLayer : UserControl
    {
        public AddMyOwnCustomDataToAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldShapeLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldShapeLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer worldLabelLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.CreateSimpleTextStyle("DynamicPopulation", "Arial", 10, DrawingFontStyles.Regular, GeoColor.StandardColors.Red, 0, -12);
            worldLabelLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLabelLayer.FeatureSource.CustomColumnFetch += new EventHandler<CustomColumnFetchEventArgs>(FeatureSource_CustomColumnFetch);

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.TileType = TileType.SingleTile;
            staticOverlay.Layers.Add("WorldShapeLayer", worldShapeLayer);
            staticOverlay.Layers.Add("WorldLabelLayer", worldLabelLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }

        private void FeatureSource_CustomColumnFetch(object sender, CustomColumnFetchEventArgs e)
        {
            if (e.ColumnName == "DynamicPopulation")
            {
                //China
                if (e.Id == "47")
                {
                    e.ColumnValue = "Pop: 1.3b";
                }

                //USA
                if (e.Id == "135")
                {
                    e.ColumnValue = "Pop: 320m";
                }
            }
        }
    }
}
