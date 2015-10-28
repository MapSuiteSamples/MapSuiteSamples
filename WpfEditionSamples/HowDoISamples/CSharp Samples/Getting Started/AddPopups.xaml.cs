using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class AddPopups : UserControl
    {
        public AddPopups()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeographyUnit.DecimalDegree;

            WorldMapKitWmsWpfOverlay worldOverlay = new WorldMapKitWmsWpfOverlay();
            Map1.Overlays.Add(worldOverlay);

            PopupOverlay popupOverlay = new PopupOverlay();
            Map1.Overlays.Add(popupOverlay);

            ShapeFileFeatureLayer majorCitiesShapeLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\MajorCities.shp");
            majorCitiesShapeLayer.Open();
            Collection<Feature> features = majorCitiesShapeLayer.QueryTools.GetAllFeatures(ReturningColumnsType.AllColumns);
            foreach (Feature feature in features)
            {
                Popup popup = new Popup(feature.GetShape().GetCenterPoint());
                popup.Content = feature.ColumnValues["AREANAME"];
                popupOverlay.Popups.Add(popup);
            }
            majorCitiesShapeLayer.Close();

            Map1.CurrentExtent = popupOverlay.GetBoundingBox();
            Map1.Refresh();
        }


    }
}
