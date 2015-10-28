using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class SpatialQueryAFeatureLayer : UserControl
    {
        public SpatialQueryAFeatureLayer()
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

            InMemoryFeatureLayer rectangleLayer = new InMemoryFeatureLayer();
            rectangleLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = new AreaStyle(new GeoSolidBrush(new GeoColor(50, 100, 100, 200)));
            rectangleLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.DarkBlue;
            rectangleLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            rectangleLayer.InternalFeatures.Add("Rectangle", new Feature("POLYGON((-50 -20,-50 20,50 20,50 -20,-50 -20))", "Rectangle"));

            InMemoryFeatureLayer spatialQueryResultLayer = new InMemoryFeatureLayer();
            spatialQueryResultLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = new AreaStyle(new GeoSolidBrush(GeoColor.FromArgb(200, GeoColor.SimpleColors.PastelRed)));
            spatialQueryResultLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Red;
            spatialQueryResultLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            LayerOverlay spatialQueryResultOverlay = new LayerOverlay();
            spatialQueryResultOverlay.TileType = TileType.SingleTile;
            spatialQueryResultOverlay.Layers.Add("RectangleLayer", rectangleLayer);
            spatialQueryResultOverlay.Layers.Add("SpatialQueryResultLayer", spatialQueryResultLayer);
            wpfMap1.Overlays.Add("SpatialQueryResultOverlay", spatialQueryResultOverlay);

            wpfMap1.Refresh();
        }

        private void cboSpatialQueryType_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                ShapeFileFeatureLayer worldLayer = (ShapeFileFeatureLayer)wpfMap1.FindFeatureLayer("WorldLayer");
                InMemoryFeatureLayer rectangleLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("RectangleLayer");
                InMemoryFeatureLayer spatialQueryResultLayer = (InMemoryFeatureLayer)wpfMap1.FindFeatureLayer("SpatialQueryResultLayer");

                Feature rectangleFeature = rectangleLayer.InternalFeatures["Rectangle"];
                Collection<Feature> spatialQueryResults;
                worldLayer.Open();
                switch (cboSpatialQueryType.SelectedItem.ToString().Split(':')[1].Trim())
                {
                    case "Within":
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesWithin(rectangleFeature, new string[0]);
                        break;
                    case "Containing":
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesContaining(rectangleFeature, new string[0]);
                        break;
                    case "Disjointed":
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesDisjointed(rectangleFeature, new string[0]);
                        break;
                    case "Intersecting":
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesIntersecting(rectangleFeature, new string[0]);
                        break;
                    case "Overlapping":
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesOverlapping(rectangleFeature, new string[0]);
                        break;
                    case "TopologicalEqual":
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesTopologicalEqual(rectangleFeature, new string[0]);
                        break;
                    case "Touching":
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesTouching(rectangleFeature, new string[0]);
                        break;
                    default:
                        spatialQueryResults = worldLayer.QueryTools.GetFeaturesWithin(rectangleFeature, new string[0]);
                        break;
                }
                worldLayer.Close();

                spatialQueryResultLayer.InternalFeatures.Clear();
                foreach (Feature feature in spatialQueryResults)
                {
                    spatialQueryResultLayer.InternalFeatures.Add(feature.Id, feature);
                }

                wpfMap1.Overlays["SpatialQueryResultOverlay"].Refresh();
            }
        }
    }
}
