using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    /// <summary>
    /// Interaction logic for ZoomInAndOutOfTheMap.xaml
    /// </summary>
    public partial class ZoomInAndOutOfTheMap : UserControl
    {
        public ZoomInAndOutOfTheMap()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add("WorldMapKitWmsOverlay", worldMapKitDesktopOverlay);

            wpfMap1.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Tag.ToString())
            { 
                case "Zoom In":
                    wpfMap1.ZoomIn();
                    break;
                case "Zoom Out":
                    wpfMap1.ZoomOut();
                    break;
            }
        }
    }
}
