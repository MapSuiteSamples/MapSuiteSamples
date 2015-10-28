using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class FindOutColumnDataInAFeatureLayer : UserControl
    {
        public FindOutColumnDataInAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-128.31093750000002, 95.25, 131.84531249999998, -63.65625);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.TileType = TileType.SingleTile;
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            PopupOverlay popupOverlay = new PopupOverlay();
            wpfMap1.Overlays.Add("PopupOverlay", popupOverlay);

            wpfMap1.Refresh();
        }

        private void wpfMap1_MapClick(object sender, MapClickWpfMapEventArgs e)
        {
            FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("WorldLayer");

            worldLayer.Open();
            Collection<Feature> selectedFeatures = worldLayer.QueryTools.GetFeaturesContaining(e.WorldLocation, new string[2] { "CNTRY_NAME", "POP_CNTRY" });
            worldLayer.Close();

            if (selectedFeatures.Count > 0)
            {
                StringBuilder info = new StringBuilder();
                info.AppendLine(String.Format(CultureInfo.InvariantCulture, "CNTRY_NAME:\t{0}", selectedFeatures[0].ColumnValues["CNTRY_NAME"]));
                info.AppendLine(String.Format(CultureInfo.InvariantCulture, "POP_CNTRY:\t{0}", double.Parse(selectedFeatures[0].ColumnValues["POP_CNTRY"]).ToString("n0")));
                TBInfo.Text = info.ToString();

                PopupOverlay popupOverlay = (PopupOverlay)wpfMap1.Overlays["PopupOverlay"];
                Popup popup = new Popup(e.WorldLocation);
                popup.Content = info.ToString();
                popup.FontSize = 10d;
                popup.FontFamily = new System.Windows.Media.FontFamily("Verdana");

                popupOverlay.Popups.Clear();
                popupOverlay.Popups.Add(popup);
                popupOverlay.Refresh();
            }
        }
    }
}
