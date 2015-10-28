using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class TileDowloadProgress : UserControl
    {
        public TileDowloadProgress()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            worldMapKitOverlay.DrawTilesProgressChanged += new System.EventHandler<DrawTilesProgressChangedTileOverlayEventArgs>(worldMapKitOverlay_DrawTilesProgressChanged);
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            wpfMap1.Refresh();
        }

        private void worldMapKitOverlay_DrawTilesProgressChanged(object sender, DrawTilesProgressChangedTileOverlayEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
        }
    }
}
