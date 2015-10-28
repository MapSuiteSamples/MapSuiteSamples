using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class LoadMsSql2008FeatureLayer : UserControl
    {
        public LoadMsSql2008FeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-126.4, 48.8, -67.0, 19.0);

            string connectString = "Data Source=192.168.0.58,1041;Initial Catalog=DatabaseName;Persist Security Info=True;User ID=username;Password=password";
            MsSql2008FeatureLayer sql2008Layer = new MsSql2008FeatureLayer(connectString, "states", "recid");
            sql2008Layer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            sql2008Layer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay sql2008Overlay = new LayerOverlay();
            sql2008Overlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            sql2008Overlay.Layers.Add("Sql2008Layer", sql2008Layer);
            wpfMap1.Overlays.Add("Sql2008Overlay", sql2008Overlay);

            wpfMap1.Refresh();
        }
    }
}
