using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class ChangeDefaultLogo : UserControl
    {
        public ChangeDefaultLogo()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);
            wpfMap1.MapTools.Logo.Source = new BitmapImage(new Uri("/Resources/ThinkGeoLogo.png", UriKind.RelativeOrAbsolute));

            WorldMapKitWmsWpfOverlay worldOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldOverlay);

            wpfMap1.Refresh();
        }
    }
}
