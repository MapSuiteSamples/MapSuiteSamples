using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DisplayASimpleMap : UserControl
    {
        public DisplayASimpleMap()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeographyUnit.DecimalDegree;
            Map1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);
            WorldMapKitWmsWpfOverlay worldOverlay = new WorldMapKitWmsWpfOverlay();
            Map1.Overlays.Add("WMK", worldOverlay);
            Map1.Refresh(); 
        }
    }
}
