using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace ThinkGeo.MapSuite.DrawingAndEditing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            wpfMap1.Refresh();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                switch (button.Name)
                {
                    case "btnTrackNormal":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.None;
                        break;
                    case "btnTrackPoint":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.Point;
                        break;
                    case "btnTrackLine":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.Line;
                        break;
                    case "btnTrackRectangle":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.Rectangle;
                        break;
                    case "btnTrackSquare":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.Square;
                        break;
                    case "btnTrackPolygon":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.Polygon;
                        break;
                    case "btnTrackCircle":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.Circle;
                        break;
                    case "btnTrackEllipse":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.Ellipse;
                        break;
                    case "btnTrackEdit":
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.None;
                        foreach (Feature feature in wpfMap1.TrackOverlay.TrackShapeLayer.InternalFeatures)
                        {
                            wpfMap1.EditOverlay.EditShapesLayer.InternalFeatures.Add(feature);
                        }

                        wpfMap1.TrackOverlay.TrackShapeLayer.InternalFeatures.Clear();
                        wpfMap1.EditOverlay.CalculateAllControlPoints();
                        wpfMap1.Refresh(new Overlay[] { wpfMap1.EditOverlay, wpfMap1.TrackOverlay });
                        break;
                    case "btnTrackDelete":
                        // should use TrackMode.EditShape select a shape, and then call DeleteTrackShape();
                        int lastIndex = wpfMap1.EditOverlay.EditShapesLayer.InternalFeatures.Count - 1;

                        if (lastIndex >= 0)
                        {
                            wpfMap1.EditOverlay.EditShapesLayer.InternalFeatures.RemoveAt(lastIndex);
                            wpfMap1.EditOverlay.CalculateAllControlPoints();
                        }

                        wpfMap1.Refresh(wpfMap1.EditOverlay);
                        break;
                    default:
                        wpfMap1.TrackOverlay.TrackMode = TrackMode.None;
                        break;
                }
            }
        }
    }
}
