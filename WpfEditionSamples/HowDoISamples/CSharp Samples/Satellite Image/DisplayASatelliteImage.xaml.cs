using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DisplayASatelliteImage : UserControl
    {
        public DisplayASatelliteImage()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.Meter;
            wpfMap1.CurrentExtent = new RectangleShape(-10000000, 10000000, 10000000, -10000000);

            EcwRasterLayer ecwImageLayer = new EcwRasterLayer(@"..\..\SampleData\Data\World.ecw");
            ecwImageLayer.UpperThreshold = double.MaxValue;
            ecwImageLayer.LowerThreshold = 0;

            ManagedProj4Projection proj4 = new ManagedProj4Projection();
            proj4.InternalProjectionParametersString = ManagedProj4Projection.GetDecimalDegreesParametersString();
            proj4.ExternalProjectionParametersString = ManagedProj4Projection.GetEpsgParametersString(3857);

            ecwImageLayer.ImageSource.Projection = proj4;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.Layers.Add("EcwImageLayer", ecwImageLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }
    }
}
