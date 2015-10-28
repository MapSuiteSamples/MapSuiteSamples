using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    /// <summary>
    /// Interaction logic for UsingTileCache.xaml
    /// </summary>
    public partial class UsingTileCache : UserControl
    {
        public UsingTileCache()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-155.733, 95.60, 104.42, -81.9);

            LayerOverlay worldOverlay = new LayerOverlay();
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            BackgroundLayer backgroundLayer = new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean));
            worldOverlay.Layers.Add(backgroundLayer);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            worldOverlay.Layers.Add("WorldLayer", worldLayer);

            // If you want to use file cache which saves images to the disk;
            // When loading back the same tile, we'll find from the disk first.
            // Turn the cache on enhance the performance a lot; 
            // if your map image is static, we recommend to turn the cache on. 
            // It's off by default.
            FileBitmapTileCache bitmapTileCache = new FileBitmapTileCache();
            bitmapTileCache.CacheDirectory = @"..\..\SampleData\Data\SampleCacheTiles";
            bitmapTileCache.CacheId = "World02CachedTiles";
            bitmapTileCache.TileAccessMode = TileAccessMode.ReadOnly;
            bitmapTileCache.ImageFormat = TileImageFormat.Png;
            worldOverlay.TileCache = bitmapTileCache;
            worldOverlay.TransitionEffect = TransitionEffect.None;

            wpfMap1.Refresh();
        }
    }
}
