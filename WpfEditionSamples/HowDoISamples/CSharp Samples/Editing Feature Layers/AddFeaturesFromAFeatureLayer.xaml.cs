﻿using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class AddFeaturesFromAFeatureLayer : UserControl
    {
        public AddFeaturesFromAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(0, 100, 100, 0);
            wpfMap1.BackgroundOverlay.BackgroundBrush = new GeoSolidBrush(GeoColor.StandardColors.White);

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.InternalFeatures.Add("Polygon", new Feature("POLYGON((10 60,40 70,30 85, 10 60))"));
            inMemoryLayer.InternalFeatures.Add("Multipoint", new Feature("MULTIPOINT(10 20, 30 20,40 20, 10 30, 30 30, 40 30)"));
            inMemoryLayer.InternalFeatures.Add("Line", new Feature("LINESTRING(60 60, 70 70,75 60, 80 70, 85 60,95 80)"));
            inMemoryLayer.InternalFeatures.Add("Rectangle", new Feature(new RectangleShape(65, 30, 95, 15)));

            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.StandardColors.RoyalBlue);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Blue;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle.OuterPen = new GeoPen(GeoColor.FromArgb(200, GeoColor.StandardColors.Red), 5);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.SymbolPen = new GeoPen(GeoColor.FromArgb(255, GeoColor.StandardColors.Green), 8);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay inMemoryFeatureOverlay = new LayerOverlay();
            inMemoryFeatureOverlay.TileType = TileType.SingleTile;
            inMemoryFeatureOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            wpfMap1.Overlays.Add("InMemoryOverlay", inMemoryFeatureOverlay);

            wpfMap1.Refresh();
        }

        private void btnAddAFeature_Click(object sender, RoutedEventArgs e)
        {
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("InMemoryFeatureLayer");

            BaseShape shape = new EllipseShape(new PointShape(50, 50), 10, 10);
            shape.Id = "Ellipse";

            inMemoryLayer.Open();
            inMemoryLayer.EditTools.BeginTransaction();
            inMemoryLayer.EditTools.Add(new Feature(shape));
            inMemoryLayer.EditTools.CommitTransaction();
            inMemoryLayer.Close();

            wpfMap1.Refresh(wpfMap1.Overlays["InMemoryOverlay"]);
        }
    }
}
