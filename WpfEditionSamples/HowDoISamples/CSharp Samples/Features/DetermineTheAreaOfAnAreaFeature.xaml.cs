using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CSHowDoISamples
{
    public partial class DetermineTheAreaOfAnAreaFeature : UserControl
    {
        public DetermineTheAreaOfAnAreaFeature()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            PopupOverlay popupOverlay = new PopupOverlay();
            wpfMap1.Overlays.Add("PopupOverlay", popupOverlay);

            wpfMap1.Refresh();
        }

        private void wpfMap1_MapClick(object sender, MapClickWpfMapEventArgs e)
        {
            FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("WorldLayer");

            // Find the country the user clicked on.
            worldLayer.Open();
            Collection<Feature> selectedFeatures = worldLayer.QueryTools.GetFeaturesContaining(e.WorldLocation, new string[1] { "CNTRY_NAME" });
            worldLayer.Close();

            // Determine the area of the country.
            if (selectedFeatures.Count > 0)
            {
                AreaBaseShape areaShape = (AreaBaseShape)selectedFeatures[0].GetShape();
                double area = areaShape.GetArea(GeographyUnit.DecimalDegree, AreaUnit.SquareKilometers);
                string areaMessage = string.Format(CultureInfo.InvariantCulture, "{0} has an area of \r{1:N0} square kilometers.", selectedFeatures[0].ColumnValues["CNTRY_NAME"].Trim(), area);

                Popup popup = new Popup(e.WorldLocation);
                popup.Content = areaMessage;
                PopupOverlay popupOverlay = (PopupOverlay)wpfMap1.Overlays["PopupOverlay"];
                popupOverlay.Popups.Clear();
                popupOverlay.Popups.Add(popup);
                popupOverlay.Refresh();
            }
        }
    }
}
