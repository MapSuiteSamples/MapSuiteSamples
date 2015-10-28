using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class AddLabelOnMarkers : UserControl
    {
        private int index = 1;
        public AddLabelOnMarkers()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            SimpleMarkerOverlay markerOverlay = new SimpleMarkerOverlay();
            wpfMap1.Overlays.Add("MarkerOverlay", markerOverlay);

            wpfMap1.Refresh();
        }

        private void wpfMap1_MapClick(object sender, MapClickWpfMapEventArgs e)
        {
            SimpleMarkerOverlay markerOverlay = (SimpleMarkerOverlay)wpfMap1.Overlays["MarkerOverlay"];
            Marker marker = new Marker(e.WorldLocation);
            marker.ImageSource = new BitmapImage(new Uri("/Resources/AQUABLANK.png", UriKind.RelativeOrAbsolute));
            marker.Width = 20;
            marker.Height = 34;
            marker.YOffset = -17;

            TextBlock content = new TextBlock();
            content.Text = (index++).ToString();
            content.FontSize = 14;
            content.FontWeight = FontWeights.Bold;
            content.Foreground = new SolidColorBrush(Colors.White);
            content.Margin = new Thickness(0, -10, 0, 0);
            marker.Content = content;

            markerOverlay.Markers.Add(marker);
            markerOverlay.Refresh();
        }
    }
}
