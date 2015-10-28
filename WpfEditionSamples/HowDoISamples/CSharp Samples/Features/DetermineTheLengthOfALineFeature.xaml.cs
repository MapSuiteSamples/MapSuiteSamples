using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CSHowDoISamples
{
    public partial class DetermineTheLengthOfALineFeature : UserControl
    {
        public DetermineTheLengthOfALineFeature()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-97.745827547760484, 30.297694742808115, -97.728208518132988, 30.285123327073894);

            ShapeFileFeatureLayer roadLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\austinstreets.shp");
            roadLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            roadLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.LocalRoad1;

            LayerOverlay roadOverlay = new LayerOverlay();
            roadOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.FromArgb(255, 233, 232, 214))));
            roadOverlay.Layers.Add("RoadLayer", roadLayer);
            wpfMap1.Overlays.Add("RoadOverlay", roadOverlay);

            InMemoryFeatureLayer highlightLayer = new InMemoryFeatureLayer();
            highlightLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.LocalRoad1;
            highlightLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle.InnerPen.Brush = new GeoSolidBrush(GeoColor.FromArgb(150, GeoColor.SimpleColors.Blue));
            highlightLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay highlightOverlay = new LayerOverlay();
            highlightOverlay.Layers.Add("HighlightLayer", highlightLayer);
            wpfMap1.Overlays.Add("HighlightOverlay", highlightOverlay);

            PopupOverlay popupOverlay = new PopupOverlay();
            wpfMap1.Overlays.Add("PopupOverlay", popupOverlay);

            wpfMap1.Refresh();
        }

        void wpfMap1_MapClick(object sender, MapClickWpfMapEventArgs e)
        {
            FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("RoadLayer");
            InMemoryFeatureLayer highlightLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("HighlightLayer");
            Overlay highlightOverlay = wpfMap1.Overlays["HighlightOverlay"];

            // Find the road the user clicked on.
            worldLayer.Open();
            Collection<Feature> selectedFeatures = worldLayer.QueryTools.GetFeaturesNearestTo(e.WorldLocation, GeographyUnit.DecimalDegree, 1, new string[1] { "FENAME" });
            worldLayer.Close();

            //Determine the length of the road.
            if (selectedFeatures.Count > 0)
            {
                LineBaseShape lineShape = (LineBaseShape)selectedFeatures[0].GetShape();
                highlightLayer.Open();
                highlightLayer.InternalFeatures.Clear();
                highlightLayer.InternalFeatures.Add(new Feature(lineShape));
                highlightLayer.Close();

                double length = lineShape.GetLength(GeographyUnit.DecimalDegree, DistanceUnit.Meter);
                string lengthMessage = string.Format(CultureInfo.InvariantCulture, "{0} has a length of {1:F2} meters.", selectedFeatures[0].ColumnValues["FENAME"].Trim(), length);

                Popup popup = new Popup(e.WorldLocation);
                popup.Content = lengthMessage;
                PopupOverlay popupOverlay = (PopupOverlay)wpfMap1.Overlays["PopupOverlay"];
                popupOverlay.Popups.Clear();
                popupOverlay.Popups.Add(popup);

                highlightOverlay.Refresh();
                popupOverlay.Refresh();
            }
        }
    }
}
