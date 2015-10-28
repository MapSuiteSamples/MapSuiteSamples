using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ZoomToACertainScale : UserControl
    {
        public ZoomToACertainScale()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitWpfOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitWpfOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            wpfMap1.Refresh();
        }

        private void cmbScale_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                // Zoom to a certain scale.
                string zoomLevelScale = cmbScale.SelectedItem.ToString();
                double scale = Convert.ToDouble(zoomLevelScale.Split(':')[2], CultureInfo.InvariantCulture);
                wpfMap1.ZoomToScale(scale);
            }
        }
    }
}
