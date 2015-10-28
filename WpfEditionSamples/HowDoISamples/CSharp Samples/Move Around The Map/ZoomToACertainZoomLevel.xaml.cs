using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ZoomToACertainZoomLevel : UserControl
    {
        public ZoomToACertainZoomLevel()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-5.4882521385258514, 79.90134747181304, 43.288252138525863, 45.0986525281869);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            wpfMap1.Refresh();
        }

        private void cmbZoomLevel_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                double scale = GetScaleFromZoomLevelSet(cmbZoomLevel.SelectedIndex);
                wpfMap1.ZoomToScale(scale);
            }
        }

        private static double GetScaleFromZoomLevelSet(int comboboxIndex)
        {
            double scale = 0;
            ZoomLevelSet zoomLevelSet = new ZoomLevelSet();

            switch (comboboxIndex)
            {
                case 0:
                    scale = zoomLevelSet.ZoomLevel10.Scale;
                    break;
                case 1:
                    scale = zoomLevelSet.ZoomLevel09.Scale;
                    break;
                case 2:
                    scale = zoomLevelSet.ZoomLevel08.Scale;
                    break;
                case 3:
                    scale = zoomLevelSet.ZoomLevel07.Scale;
                    break;
                case 4:
                    scale = zoomLevelSet.ZoomLevel06.Scale;
                    break;
                case 5:
                    scale = zoomLevelSet.ZoomLevel05.Scale;
                    break;
                case 6:
                    scale = zoomLevelSet.ZoomLevel04.Scale;
                    break;
                case 7:
                    scale = zoomLevelSet.ZoomLevel03.Scale;
                    break;
                case 8:
                    scale = zoomLevelSet.ZoomLevel02.Scale;
                    break;
                default:
                    break;
            }

            return scale;
        }
    }
}
