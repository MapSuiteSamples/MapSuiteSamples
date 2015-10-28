using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class LoadAnOracleFeatureLayer : UserControl
    {
        public LoadAnOracleFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-126.4, 48.8, -67.0, 19.0);

            string connectString = "User ID=userid;Password=password;Data Source=192.168.0.178/orcl;";
            OracleFeatureLayer oracleLayer = new OracleFeatureLayer(connectString, "states", "recid");
            oracleLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            oracleLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay oracleOverlay = new LayerOverlay();
            oracleOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            oracleOverlay.Layers.Add("OracleLayer", oracleLayer);
            wpfMap1.Overlays.Add("OracleOverlay", oracleOverlay);

            wpfMap1.Refresh();
        }
    }
}
