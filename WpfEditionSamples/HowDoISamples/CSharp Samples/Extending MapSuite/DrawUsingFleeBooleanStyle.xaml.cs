using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class DrawUsingFleeBooleanStyle : UserControl
    {
        public DrawUsingFleeBooleanStyle()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);

            WorldMapKitWmsWpfOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsWpfOverlay();
            wpfMap1.Overlays.Add(worldMapKitDesktopOverlay);

            // Highlight the countries that are land locked and have a population greater than 10 million   
            string expression = "(ToInt32(POP_CNTRY)>10000000) AND (ToChar(LANDLOCKED)='Y')";
            FleeBooleanStyle landLockedCountryStyle = new FleeBooleanStyle(expression);

            // You can access the static methods on these types.  We use this   
            // to access the Convert.Toxxx methods to convert variable types   
            landLockedCountryStyle.StaticTypes.Add(typeof(System.Convert));
            // The math class might be handy to include but in this sample we do not use it   
            //landLockedCountryStyle.StaticTypes.Add(typeof(System.Math));   

            landLockedCountryStyle.ColumnVariables.Add("POP_CNTRY");
            landLockedCountryStyle.ColumnVariables.Add("LANDLOCKED");

            landLockedCountryStyle.CustomTrueStyles.Add(new AreaStyle(new GeoPen(GeoColor.SimpleColors.Green, 2), new GeoSolidBrush(GeoColor.FromArgb(100, GeoColor.SimpleColors.Green))));
            landLockedCountryStyle.CustomFalseStyles.Add(AreaStyles.CreateSimpleAreaStyle(GeoColor.SimpleColors.Transparent, GeoColor.FromArgb(100, GeoColor.SimpleColors.Green)));

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp", ShapeFileReadWriteMode.ReadOnly);
            worldLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(landLockedCountryStyle);
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.TileType = TileType.SingleTile;
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }
    }
}
