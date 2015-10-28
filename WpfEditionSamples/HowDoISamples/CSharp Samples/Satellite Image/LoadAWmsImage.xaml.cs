using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class LoadAWmsImage : UserControl
    {
        public LoadAWmsImage()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-143.4, 109.3, 116.7, -76.3);

            UseOverlay();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                RadioButton rbt = (RadioButton)sender;
                switch (rbt.Content.ToString())
                { 
                    case "Use WmsRasterLayer":
                        UseLayer();
                        break;
                    case "Use WmsOverlay":
                        UseOverlay();
                        break;
                }
            }
        }

        private void UseOverlay()
        {
            wpfMap1.Overlays.Clear();

            WmsOverlay wmsOverlay = new WmsOverlay();
            wmsOverlay.ServerUris.Add(new Uri("http://howdoiwms.thinkgeo.com/WmsServer.aspx"));
            wmsOverlay.Parameters.Add("LAYERS", "Countries02");
            wmsOverlay.Parameters.Add("STYLES", "SIMPLE");
            wpfMap1.Overlays.Add(wmsOverlay);

            wpfMap1.Refresh();
        }

        private void UseLayer()
        {
            wpfMap1.Overlays.Clear();

            WmsRasterLayer wmsImageLayer = new WmsRasterLayer(new Uri("http://howdoiwms.thinkgeo.com/WmsServer.aspx"));
            wmsImageLayer.UpperThreshold = double.MaxValue;
            wmsImageLayer.LowerThreshold = 0;

            wmsImageLayer.Open();
            foreach (string layerName in wmsImageLayer.GetServerLayerNames())
            {
                wmsImageLayer.ActiveLayerNames.Add(layerName);
                wmsImageLayer.ActiveStyleNames.Add("Simple");
            }
            // this parameter is just optional.
            wmsImageLayer.Exceptions = "application/vnd.ogc.se_xml";

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.Layers.Add("wmsImageLayer", wmsImageLayer);
            wpfMap1.Overlays.Add(staticOverlay);
            wmsImageLayer.Close();

            wpfMap1.Refresh();
        }
    }
}
