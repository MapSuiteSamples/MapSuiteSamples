using System.IO;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class LoadSdfFeatureLayer : UserControl
    {
        public LoadSdfFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
                wpfMap1.CurrentExtent = new RectangleShape(-87.7649869909628, 43.7975200004804, -87.6955215108997, 43.6913981287878);

                WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
                wpfMap1.Overlays.Add(worldMapKitOverlay);

                SdfFeatureLayer sdfLayer = new SdfFeatureLayer(@"..\..\SampleData\Data\Sheboygan_CityLimits.sdf", null);
                sdfLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.FromArgb(100, GeoColor.SimpleColors.Green), GeoColor.SimpleColors.Green);
                sdfLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

                LayerOverlay dynamicOverlay = new LayerOverlay();
                dynamicOverlay.DrawingExceptionMode = DrawingExceptionMode.DrawException;
                dynamicOverlay.Layers.Add("WorldLayer", sdfLayer);
                dynamicOverlay.TileType = TileType.SingleTile;
                wpfMap1.Overlays.Add(dynamicOverlay);

                wpfMap1.Refresh();
            }
            catch (FileNotFoundException ex)
            {
                string message = "You should get Fdo dependencies from [Install-Path]\\Developer Reference\\System32, and put MapSuiteFdoExtensionx86 folder to System32 folder.\r\n\r\n" + ex.Message;
                MessageBox.Show(message, "FileNotFound", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, (MessageBoxOptions)0);
            }
        }
    }
}
