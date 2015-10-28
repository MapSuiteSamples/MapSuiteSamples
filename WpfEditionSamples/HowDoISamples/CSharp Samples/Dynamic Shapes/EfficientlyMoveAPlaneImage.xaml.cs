using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class EfficientlyMoveAPlaneImage : UserControl
    {
        private DispatcherTimer timer;
        private int index;

        public EfficientlyMoveAPlaneImage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
            Unloaded += new RoutedEventHandler(EfficientlyMoveAPlaneImage_Unloaded);
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-139.2, 92.4, 120.9, -93.2);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            // Setup the shapefile layer.   
            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            // Setup the mapshape layer.
            InMemoryFeatureLayer bitmapLayer = new InMemoryFeatureLayer();
            bitmapLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.PointType = PointType.Bitmap;
            bitmapLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.Image = new GeoImage(@"..\..\SampleData\Images\Prop Plane.png");
            bitmapLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.ContestedBorder2;
            bitmapLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            PointShape planeShape = new PointShape(-95.2806, 38.9554);
            PointShape destinationPoint = new PointShape(36.04, 48.49);
            MultilineShape airLineShape = planeShape.GreatCircle(destinationPoint);

            bitmapLayer.Open();
            bitmapLayer.EditTools.BeginTransaction();
            bitmapLayer.EditTools.Add(new Feature(planeShape.GetWellKnownBinary(), "Plane"));
            bitmapLayer.EditTools.Add(new Feature(airLineShape.GetWellKnownBinary(), "AirLine"));
            bitmapLayer.EditTools.CommitTransaction();
            bitmapLayer.Close();

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            LayerOverlay planeOverlay = new LayerOverlay();
            planeOverlay.TileType = TileType.SingleTile;
            planeOverlay.Layers.Add("BitmapLayer", bitmapLayer);
            wpfMap1.Overlays.Add("PlaneOverlay", planeOverlay);

            wpfMap1.Refresh();

            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            LayerOverlay planeOverlay = (LayerOverlay)wpfMap1.Overlays["PlaneOverlay"];
            InMemoryFeatureLayer bitmapLayer = (InMemoryFeatureLayer)planeOverlay.Layers["BitmapLayer"];
            PointShape pointShape = bitmapLayer.InternalFeatures[0].GetShape() as PointShape;
            LineShape airLine = ((MultilineShape)bitmapLayer.InternalFeatures[1].GetShape()).Lines[0];
            index += 30;
            if (index < airLine.Vertices.Count)
            {
                Vertex newPosition = airLine.Vertices[index];
                pointShape.X = newPosition.X;
                pointShape.Y = newPosition.Y;
                pointShape.Id = "Plane";

                bitmapLayer.Open();
                bitmapLayer.EditTools.BeginTransaction();
                bitmapLayer.EditTools.Update(pointShape);
                bitmapLayer.EditTools.CommitTransaction();
                bitmapLayer.Close();
            }
            else
            {
                index = 0;
            }

            planeOverlay.Refresh();
        }

        private void EfficientlyMoveAPlaneImage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
            }
        }
    }
}
