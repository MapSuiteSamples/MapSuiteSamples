using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class GetAColumnDataInAFeatureLayer : UserControl
    {
        public GetAColumnDataInAFeatureLayer()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-116.18203125000002, 77.671875, 143.97421874999998, -60.4921875);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay staticOverlay = new LayerOverlay();
            staticOverlay.TileType = TileType.SingleTile;
            staticOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add(staticOverlay);

            wpfMap1.Refresh();
        }

        private void btnColumns_Click(object sender, RoutedEventArgs e)
        {
            FeatureLayer worldLayer = wpfMap1.FindFeatureLayer("WorldLayer");
            worldLayer.Open();
            Collection<FeatureSourceColumn> allColumns = worldLayer.QueryTools.GetColumns();
            worldLayer.Close();

            dgridFeatures.DataContext = GetDataTableFromFeatureSourceColumns(allColumns);
            dgridFeatures.SetBinding(ListView.ItemsSourceProperty, new Binding());
        }

        private static DataTable GetDataTableFromFeatureSourceColumns(Collection<FeatureSourceColumn> featureSourceColumns)
        {
            DataTable dataTable = new DataTable();
            dataTable.Locale = CultureInfo.InvariantCulture;

            // Setup the column.
            dataTable.Columns.Add("ColumnName");
            dataTable.Columns.Add("MaxLength");
            dataTable.Columns.Add("TypeName");

            foreach (FeatureSourceColumn featureSourceColumn in featureSourceColumns)
            {
                // Add a record.
                DataRow dataRow = dataTable.NewRow();

                dataRow[0] = featureSourceColumn.ColumnName;
                dataRow[1] = featureSourceColumn.MaxLength;
                dataRow[2] = featureSourceColumn.TypeName;

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
