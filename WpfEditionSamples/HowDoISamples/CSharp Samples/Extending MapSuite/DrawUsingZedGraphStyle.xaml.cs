using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using ZedGraph;

namespace CSHowDoISamples
{
    public partial class DrawUsingZedGraphStyle : UserControl
    {
        private delegate Bitmap ToUIThreadDelegate(ZedGraphDrawingEventArgs e);

        public DrawUsingZedGraphStyle()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-123.41875, 41.96396484375, -107.158984375, 30.36240234375);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add("States", statesLayer);
            worldOverlay.TransitionEffect = TransitionEffect.None;
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            ShapeFileFeatureLayer citiesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\MajorCities.shp");

            //Create our Zedgraph Sytle and wire up the event.   
            ZedGraphStyle zedGraphStyle = new ZedGraphStyle();
            zedGraphStyle.ZedGraphDrawing += new EventHandler<ZedGraphDrawingEventArgs>(zedGraphStyle_ZedGraphDrawing);

            zedGraphStyle.RequiredColumnNames.Add("WHITE");
            zedGraphStyle.RequiredColumnNames.Add("ASIAN");
            zedGraphStyle.RequiredColumnNames.Add("AREANAME");

            TextStyle textStyle = TextStyles.CreateSimpleTextStyle("AREANAME", "Arial", 12, DrawingFontStyles.Regular, GeoColor.StandardColors.Black, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green), 2, 0, -8);
            citiesLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(zedGraphStyle);
            citiesLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(textStyle);
            citiesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay citiesOverlay = new LayerOverlay();
            citiesOverlay.Layers.Add("Cities", citiesLayer);
            citiesOverlay.TileType = TileType.SingleTile;
            wpfMap1.Overlays.Add("CitiesOverlay", citiesOverlay);

            wpfMap1.Refresh();
        }

        private void zedGraphStyle_ZedGraphDrawing(object sender, ZedGraphDrawingEventArgs e)
        {
            e.Bitmap = (Bitmap)Dispatcher.Invoke(new ToUIThreadDelegate(ZedGraphDrawing), new object[] { e });
        }

        private Bitmap ZedGraphDrawing(ZedGraphDrawingEventArgs e)
        {
            double scale = wpfMap1.CurrentScale;

            // Change the size of the graph based on the scale.  It will get bigger as you zoom in.
            int graphHeight = Convert.ToInt32(1400000000 / scale);
            LayerOverlay staticOverlay = (LayerOverlay)wpfMap1.Overlays["CitiesOverlay"];
            ChangeLabelPosition(((ShapeFileFeatureLayer)staticOverlay.Layers["Cities"]), graphHeight);

            ZedGraphControl zedGraph = new ZedGraphControl();
            zedGraph.Size = new System.Drawing.Size(graphHeight, graphHeight);

            zedGraph.GraphPane.Fill.Type = FillType.None;
            zedGraph.GraphPane.Chart.Fill.Type = FillType.None;

            zedGraph.GraphPane.Border.IsVisible = false;
            zedGraph.GraphPane.Chart.Border.IsVisible = false;
            zedGraph.GraphPane.XAxis.IsVisible = false;
            zedGraph.GraphPane.YAxis.IsVisible = false;
            zedGraph.GraphPane.Legend.IsVisible = false;
            zedGraph.GraphPane.Title.IsVisible = false;

            PieItem pieItem1 = zedGraph.GraphPane.AddPieSlice(Convert.ToDouble(e.Feature.ColumnValues["WHITE"], CultureInfo.InvariantCulture), GetColorFromGeoColor(GeoColor.StandardColors.LightBlue), 0f, "White");
            pieItem1.LabelDetail.IsVisible = false;

            PieItem pieItem2 = zedGraph.GraphPane.AddPieSlice(Convert.ToDouble(e.Feature.ColumnValues["ASIAN"], CultureInfo.InvariantCulture), GetColorFromGeoColor(GeoColor.StandardColors.LightGreen), 0f, "Asian");
            pieItem2.LabelDetail.IsVisible = false;
            pieItem2.Displacement = 0.2;

            zedGraph.AxisChange();

            return zedGraph.GraphPane.GetImage();
        }

        private static void ChangeLabelPosition(ShapeFileFeatureLayer shapeFileLayer, int graphHeight)
        {
            ((TextStyle)shapeFileLayer.ZoomLevelSet.ZoomLevel01.CustomStyles[1]).XOffsetInPixel = -20;
            ((TextStyle)shapeFileLayer.ZoomLevelSet.ZoomLevel01.CustomStyles[1]).YOffsetInPixel = Convert.ToSingle(graphHeight * 0.4);
        }

        private static Color GetColorFromGeoColor(GeoColor geoColor)
        {
            return Color.FromArgb(geoColor.AlphaComponent, geoColor.RedComponent, geoColor.GreenComponent, geoColor.BlueComponent);
        }
    }
}
