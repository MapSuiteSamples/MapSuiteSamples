using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class LoadAMapFromStreams : UserControl
    {
        public LoadAMapFromStreams()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(@"C:\DoesNotExistDirectory\Countries02.shp");
            ((ShapeFileFeatureSource)shapeFileLayer.FeatureSource).StreamLoading += new EventHandler<StreamLoadingEventArgs>(LoadAMapFromStreams_StreamLoading);
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.TransitionEffect = TransitionEffect.None;
            staticOverlay.Layers.Add("WorldLayer", shapeFileLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }

        private void LoadAMapFromStreams_StreamLoading(object sender, StreamLoadingEventArgs e)
        {
            string fileName = System.IO.Path.GetFileName(e.AlternateStreamName);
            //TODO: Portable Change.
            //e.AlternateStream = new FileStream(@"..\..\SampleData\Data\" + fileName, e.FileMode, e.FileAccess);
            e.AlternateStream = new FileStream(@"..\..\SampleData\Data\" + fileName, (FileMode)e.FileMode, (FileAccess)e.FileAccess);
        }
    }
}
