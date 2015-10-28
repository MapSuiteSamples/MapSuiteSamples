using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System.Windows.Shapes;
using System.Windows.Media;

namespace CSHowDoISamples
{
    public partial class PanZoomBar : UserControl
    {
        public PanZoomBar()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);
            wpfMap1.MapTools.PanZoomBar.IsEnabled = true;
            wpfMap1.MapTools.PanZoomBar.DisplayZoomBarText = DisplayZoomBarText.Display;

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            worldOverlay.Layers.Add(worldLayer);
            wpfMap1.Overlays.Add(worldOverlay);

            wpfMap1.Refresh();
        }

        private void chbVisibility_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (wpfMap1 != null)
            {
                wpfMap1.MapTools.PanZoomBar.IsEnabled = (bool)checkBox.IsChecked;
            }
        }

        private void chbTrackLevel_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (wpfMap1 != null)
            {
                wpfMap1.MapTools.PanZoomBar.DisplayZoomBarText =
                    (bool)checkBox.IsChecked ? DisplayZoomBarText.Display : DisplayZoomBarText.None;
            }
        }
    }
}
