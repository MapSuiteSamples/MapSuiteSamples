using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System.IO;
using System.Windows.Media.Imaging;
using System;

namespace CSHowDoISamples
{
    public partial class LoadAPostgreSqlFeatureLayer : UserControl
    {
        public LoadAPostgreSqlFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            wpfMap1.CurrentExtent = new RectangleShape(-126.4, 48.8, -67.0, 19.0);

            string connectString = "Server=192.168.0.235;User Id=userId;Password=password;DataBase=postgis;";
            PostgreSqlFeatureLayer postgreLayer = new PostgreSqlFeatureLayer(connectString, "new_states", "recid");
            postgreLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            postgreLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay postgreOverlay = new LayerOverlay();
            postgreOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            postgreOverlay.Layers.Add("PostgreLayer", postgreLayer);
            wpfMap1.Overlays.Add("PostgreOverlay", postgreOverlay);

            wpfMap1.Refresh();
        }
    }
}
