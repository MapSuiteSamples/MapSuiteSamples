using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class GetAllFeaturesFromAFeatureLayer : UserControl
    {
        public GetAllFeaturesFromAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-116.18203125000002, 77.671875, 143.97421874999998, -60.4921875);

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.TileType = TileType.SingleTile;
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }

        private void btnGetAllFeatures_Click(object sender, RoutedEventArgs e)
        {
            string[] returningColumns = new string[] { "CNTRY_NAME", "CURR_TYPE", "RECID" };
            FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("WorldLayer");
            worldLayer.Open();
            Collection<Feature> allFeaturs = worldLayer.FeatureSource.GetAllFeatures(returningColumns);
            worldLayer.Close();

            //TODO: Portable Change.
            //dgridFeatures.DataContext =  MapEngine.LoadDataTable(allFeaturs, returningColumns);
            dgridFeatures.DataContext = (DataTable)MapEngine.LoadDataTable(allFeaturs, returningColumns);
            dgridFeatures.SetBinding(ListView.ItemsSourceProperty, new Binding());
        }
    }
}