using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;
using System;

namespace CSHowDoISamples
{
    /// <summary>
    /// Interaction logic for PanAroundTheMap.xaml
    /// </summary>
    public partial class PanAroundTheMap : UserControl
    {
        public PanAroundTheMap()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            wpfMap1.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Tag.ToString();
            switch (buttonText)
            { 
                case "Up":
                case "Down":
                case "Right":
                case "Left":
                    PanDirection panDirection = (PanDirection)Enum.Parse(typeof(PanDirection), buttonText);
                    wpfMap1.Pan(panDirection, 20);
                    break;
                case "Pan":
                    float degree = float.Parse(tbDegree.Text);
                    int percentage = Int32.Parse(tbPercentage.Text);
                    wpfMap1.Pan(degree, percentage);
                    break;
            }
        }
    }
}
