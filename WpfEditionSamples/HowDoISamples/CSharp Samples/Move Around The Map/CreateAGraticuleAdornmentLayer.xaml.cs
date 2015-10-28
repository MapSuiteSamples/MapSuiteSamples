using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class CreateAGraticuleAdornmentLayer : UserControl
    {
        public CreateAGraticuleAdornmentLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add("WorldOverlay", worldMapKitOverlay);

            GraticuleAdornmentLayer graticuleAdornmentLayer = new GraticuleAdornmentLayer();
            graticuleAdornmentLayer.GraticuleLineStyle.OuterPen.Color = GeoColor.FromArgb(125, GeoColor.StandardColors.Navy);
            wpfMap1.AdornmentOverlay.Layers.Add("graticule", graticuleAdornmentLayer);

            wpfMap1.CurrentExtent = new RectangleShape(-139.2, 92.4, 120.9, -93.2);
            wpfMap1.Refresh();
        }
    }
}
