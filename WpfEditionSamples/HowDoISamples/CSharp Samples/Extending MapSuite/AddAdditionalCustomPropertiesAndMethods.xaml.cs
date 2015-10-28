using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class AddAdditionalCustomPropertiesAndMethods : UserControl
    {
        public AddAdditionalCustomPropertiesAndMethods()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-177.39584350585937, 83.113876342773437, -52.617362976074219, 14.550546646118164);
            wpfMap1.BackgroundOverlay.BackgroundBrush = new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean);

            AdministrationShapeFileLayer worldLayer = new AdministrationShapeFileLayer(@"..\..\SampleData\Data\Countries02.shp", SecurityLevel.AverageUsageLevel1);
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;

            AdministrationShapeFileLayer statesLayer = new AdministrationShapeFileLayer(@"..\..\SampleData\Data\USStates.shp", SecurityLevel.AverageUsageLevel2);
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.State1;
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.LineJoin = DrawingLineJoin.Round;
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.TransitionEffect = TransitionEffect.None;
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            worldOverlay.Layers.Add("StatesLayer", statesLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }

        public enum SecurityLevel
        {
            AdministrativeLevel = 0,
            AverageUsageLevel1 = 1,
            AverageUsageLevel2 = 2
        }

        public class AdministrationShapeFileLayer : ShapeFileFeatureLayer
        {
            private SecurityLevel securityLevel;

            public AdministrationShapeFileLayer(string pathFileName)
                : base(pathFileName)
            {
                securityLevel = SecurityLevel.AdministrativeLevel;
            }

            public AdministrationShapeFileLayer(string pathFileName, SecurityLevel securityLevel)
                : base(pathFileName)
            {
                this.securityLevel = securityLevel;
            }

            public SecurityLevel SecurityLevel
            {
                get
                {
                    return securityLevel;
                }
                set
                {
                    securityLevel = value;
                }
            }
        }

        private void cboSecturityLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wpfMap1.Overlays.Count > 0)
            {
                foreach (Layer layer in ((LayerOverlay)wpfMap1.Overlays[0]).Layers)
                {
                    layer.IsVisible = true;
                    SecurityLevel securityLevel = ((AdministrationShapeFileLayer)layer).SecurityLevel;

                    if (cboSecturityLevel.SelectedItem.ToString().Split(':')[1].Trim() == "AverageUsageLevel1" && securityLevel == SecurityLevel.AverageUsageLevel2)
                    {
                        layer.IsVisible = false;
                    }
                    else if (cboSecturityLevel.SelectedItem.ToString().Split(':')[1].Trim() == "AverageUsageLevel2" && securityLevel == SecurityLevel.AverageUsageLevel1)
                    {
                        layer.IsVisible = false;
                    }
                }

                wpfMap1.Overlays["WorldOverlay"].Refresh();
            }
        }
    }
}
