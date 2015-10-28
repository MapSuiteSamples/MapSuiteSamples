using System.IO;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class LoadGeoDatabaseFeatureLayer : UserControl
    {
        public LoadGeoDatabaseFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfMap1.MapUnit = GeographyUnit.Meter;
                wpfMap1.CurrentExtent = new RectangleShape(2149408.38465815, 246471.365609125, 2204046.63635703, 213231.081162168);

                PersonalGeoDatabaseFeatureLayer worldLayer = new PersonalGeoDatabaseFeatureLayer(@"..\..\SampleData\Data\JORWD6gdb.mdb", null, null, "Mains");
                worldLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.LocalRoad1;
                worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

                LayerOverlay worldOverlay = new LayerOverlay();
                worldOverlay.DrawingExceptionMode = DrawingExceptionMode.DrawException;
                worldOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.FromArgb(255, 233, 232, 214))));
                worldOverlay.Layers.Add("WorldLayer", worldLayer);
                wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

                wpfMap1.Refresh();
            }
            catch (FileNotFoundException ex)
            {
                string message = "You should get Fdo dependencies from [Install-Path]\\Developer Reference\\System32\\, and put MapSuiteFdoExtensionx86 folder to System32 folder.\r\n\r\n" + ex.Message;
                MessageBox.Show(message, "FileNotFound", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, (MessageBoxOptions)0);
            }
        }
    }
}
