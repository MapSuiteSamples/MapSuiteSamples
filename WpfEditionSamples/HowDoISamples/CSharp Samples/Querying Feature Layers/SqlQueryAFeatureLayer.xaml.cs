using System.Data;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class SqlQueryAFeatureLayer : UserControl
    {
        public SqlQueryAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-165.7421875, 80.23046875, -35.6640625, 13.08203125);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.LineJoin = DrawingLineJoin.Round;
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.TransitionEffect = TransitionEffect.None;
            staticOverlay.Layers.Add("StatesLayer", statesLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            ShapeFileFeatureLayer statesLayer = (ShapeFileFeatureLayer)wpfMap1.FindFeatureLayer("StatesLayer");

            statesLayer.Open();
            DataTable dataTable = statesLayer.QueryTools.ExecuteQuery(tbxSQL.Text);
            statesLayer.Close();

            dgridResult.DataContext = dataTable;
        }
    }
}