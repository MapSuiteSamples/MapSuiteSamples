using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class MouseCoordinate : UserControl
    {
        public MouseCoordinate()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);
            wpfMap1.MapTools.MouseCoordinate.IsEnabled = true;

            WorldMapKitWmsWpfOverlay worldOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldOverlay);

            wpfMap1.Refresh();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapTools.MouseCoordinate.CustomFormatted -= new System.EventHandler<CustomFormattedMouseCoordinateMapToolEventArgs>(MouseCoordinate_CustomMouseCoordinateFormat);
            RadioButton button = (RadioButton)sender;
            switch (button.Tag.ToString())
            {
                case "0":
                    wpfMap1.MapTools.MouseCoordinate.MouseCoordinateType = MouseCoordinateType.LongitudeLatitude;
                    break;
                case "1":
                    wpfMap1.MapTools.MouseCoordinate.MouseCoordinateType = MouseCoordinateType.LatitudeLongitude;
                    break;
                case "2":
                    wpfMap1.MapTools.MouseCoordinate.MouseCoordinateType = MouseCoordinateType.DegreesMinutesSeconds;
                    break;
                case "3":
                    wpfMap1.MapTools.MouseCoordinate.MouseCoordinateType = MouseCoordinateType.Custom;
                    wpfMap1.MapTools.MouseCoordinate.CustomFormatted += new System.EventHandler<CustomFormattedMouseCoordinateMapToolEventArgs>(MouseCoordinate_CustomMouseCoordinateFormat);
                    break;
            }
        }

        private void MouseCoordinate_CustomMouseCoordinateFormat(object sender, CustomFormattedMouseCoordinateMapToolEventArgs e)
        {
            ((MouseCoordinateMapTool)sender).Foreground = new SolidColorBrush(Colors.Red);
            e.Result = string.Format(CultureInfo.InvariantCulture, "{0},{1}", e.WorldCoordinate.X.ToString("N3"), e.WorldCoordinate.Y.ToString("N3"));
        }
    }
}
