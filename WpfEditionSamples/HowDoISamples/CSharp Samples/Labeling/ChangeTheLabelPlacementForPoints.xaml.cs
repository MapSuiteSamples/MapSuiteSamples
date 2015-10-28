using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ChangeTheLabelPlacementForPoints : UserControl
    {
        public ChangeTheLabelPlacementForPoints()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-126.4, 48.8, -67.0, 19.0);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;

            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.State1;
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.LineJoin = DrawingLineJoin.Round;
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer majorCitiesShapeLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\MajorCities.shp");
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.City1;
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.City1("AREANAME");
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.OverlappingRule = LabelOverlappingRule.AllowOverlapping;
            majorCitiesShapeLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay statesOverlay = new LayerOverlay();
            statesOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            statesOverlay.Layers.Add("World", worldLayer);
            statesOverlay.Layers.Add("States", statesLayer);
            wpfMap1.Overlays.Add("StatesOverlay", statesOverlay);

            LayerOverlay cityOverlay = new LayerOverlay();
            cityOverlay.TileType = TileType.SingleTile;
            cityOverlay.Layers.Add("MajorCitiesShapes", majorCitiesShapeLayer);
            wpfMap1.Overlays.Add("CityOverlay", cityOverlay);

            wpfMap1.Refresh();
        }

        private void cbxPointPlacement_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                PointPlacement placement;
                switch (cbxPointPlacement.SelectedItem.ToString().Split(':')[1].Trim())
                {
                    case "Center":
                        placement = PointPlacement.Center;
                        break;
                    case "CenterLeft":
                        placement = PointPlacement.CenterLeft;
                        break;
                    case "CenterRight":
                        placement = PointPlacement.CenterRight;
                        break;
                    case "LowerCenter":
                        placement = PointPlacement.LowerCenter;
                        break;
                    case "LowerLeft":
                        placement = PointPlacement.LowerLeft;
                        break;
                    case "LowerRight":
                        placement = PointPlacement.LowerRight;
                        break;
                    case "UpperCenter":
                        placement = PointPlacement.UpperCenter;
                        break;
                    case "UpperLeft":
                        placement = PointPlacement.UpperLeft;
                        break;
                    case "UpperRight":
                        placement = PointPlacement.UpperRight;
                        break;
                    default:
                        placement = PointPlacement.CenterRight;
                        break;
                }

                FeatureLayer labelPlacementLayer = wpfMap1.FindFeatureLayer("MajorCitiesShapes");
                labelPlacementLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.PointPlacement = placement;

                wpfMap1.Overlays["CityOverlay"].Refresh();
            }
        }
    }
}
