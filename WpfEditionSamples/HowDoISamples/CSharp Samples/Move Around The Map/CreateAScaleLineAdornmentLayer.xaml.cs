using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class CreateAScaleLineAdornmentLayer : UserControl
    {
        public CreateAScaleLineAdornmentLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ScaleLineAdornmentLayer scaleLineAdornmentLayer = new ScaleLineAdornmentLayer();
            wpfMap1.AdornmentOverlay.Layers.Add("ScaleLineAdornmentLayer", scaleLineAdornmentLayer);

            wpfMap1.Refresh();
        }

        private void cbxScaleLineLocation_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                ScaleLineAdornmentLayer currentScaleLineAdornmentLayer = (ScaleLineAdornmentLayer)wpfMap1.AdornmentOverlay.Layers["ScaleLineAdornmentLayer"];
                currentScaleLineAdornmentLayer.Location = (AdornmentLocation)cbxScaleLineLocation.SelectedIndex;

                wpfMap1.Refresh(wpfMap1.AdornmentOverlay);
            }
        }
    }
}
