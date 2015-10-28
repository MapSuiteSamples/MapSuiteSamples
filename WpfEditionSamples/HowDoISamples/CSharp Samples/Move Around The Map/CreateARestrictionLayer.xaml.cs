using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class CreateARestrictionLayer : UserControl
    {
        public CreateARestrictionLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-44.056640625, 49.224609375, 86.021484375, -43.587890625);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add("WorldOverlay", worldMapKitDesktopOverlay);

            RestrictionLayer restrictionLayer = new RestrictionLayer();
            restrictionLayer.Zones.Add(new RectangleShape(-17.67, 37.01, 60.02, -34.68));
            restrictionLayer.RestrictionMode = RestrictionMode.ShowZones;
            restrictionLayer.UpperScale = 250000000;
            restrictionLayer.LowerScale = double.MinValue;

            LayerOverlay restrictionOverlay = new LayerOverlay();
            restrictionOverlay.Layers.Add("RestrictionLayer", restrictionLayer);
            wpfMap1.Overlays.Add("RestrictionOverlay", restrictionOverlay);

            wpfMap1.Refresh();
        }

        private void cmbRestrictionStyle_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                LayerOverlay dynamicOverlay = (LayerOverlay)wpfMap1.Overlays[1];
                RestrictionLayer currentRestrictionLayer = (RestrictionLayer)dynamicOverlay.Layers["RestrictionLayer"];
                if (currentRestrictionLayer.CustomStyles.Count > 0)
                {
                    currentRestrictionLayer.CustomStyles.Clear();
                }

                switch (cmbRestrictionStyle.SelectedItem.ToString().Split(':')[1].Trim())
                {
                    case "HatchPattern":
                        currentRestrictionLayer.RestrictionStyle = RestrictionStyle.HatchPattern;
                        break;
                    case "CircleWithSlashImage":
                        currentRestrictionLayer.RestrictionStyle = RestrictionStyle.CircleWithSlashImage;
                        break;
                    case "UseCustomStyles":
                        currentRestrictionLayer.RestrictionStyle = RestrictionStyle.UseCustomStyles;
                        AreaStyle customStyle = new AreaStyle(new GeoSolidBrush(new GeoColor(150, GeoColor.StandardColors.Gray)));
                        currentRestrictionLayer.CustomStyles.Add(customStyle);
                        break;
                    default:
                        break;
                }

                wpfMap1.Overlays["RestrictionOverlay"].Refresh();
            }
        }

        private void rbtnShowMode_Click(object sender, RoutedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                LayerOverlay dynamicOverlay = (LayerOverlay)wpfMap1.Overlays[1];
                RestrictionLayer currentRestrictionLayer = (RestrictionLayer)dynamicOverlay.Layers["RestrictionLayer"];
                currentRestrictionLayer.RestrictionMode = RestrictionMode.ShowZones;
                InstructionLabel.Text = "You can see only Africa because we have added a RestrictionLayer and its mode is ShowZones.";

                wpfMap1.Overlays["RestrictionOverlay"].Refresh();
            }
        }

        private void rbtnHideMode_Click(object sender, RoutedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                LayerOverlay dynamicOverlay = (LayerOverlay)wpfMap1.Overlays[1];
                RestrictionLayer currentRestrictionLayer = (RestrictionLayer)dynamicOverlay.Layers["RestrictionLayer"];
                currentRestrictionLayer.RestrictionMode = RestrictionMode.HideZones;
                InstructionLabel.Text = "You can not see Africa because we have added a RestrictionLayer and its mode is HideZones.";

                wpfMap1.Overlays["RestrictionOverlay"].Refresh();
            }
        }
    }
}
