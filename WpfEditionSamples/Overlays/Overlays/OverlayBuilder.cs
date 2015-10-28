using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace ThinkGeo.MapSuite.Overlays
{
    internal static class OverlayBuilder
    {
        private const string uriTemplate = "pack://application:,,,/Images/{0}";

        public static OpenStreetMapOverlay CreateOpenStreetMapOverlay()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "TileCache");
            const string openStreetMapOverlayName = "Open Street Map";

            OpenStreetMapOverlay openStreetMapOverlay = new OpenStreetMapOverlay();

            //  Set the name of overlay and it will be displayed in the swicher overlay. 
            openStreetMapOverlay.Name = openStreetMapOverlayName;

            //  Set the image format as Jpeg. The default value is Png.
            openStreetMapOverlay.ImageFormat = TileImageFormat.Jpeg;

            //  Set the height and width of the image. The default value is 256.
            openStreetMapOverlay.TileHeight = 512;
            openStreetMapOverlay.TileWidth = 512;
            //  Set cache of the tiles, it indicates where the tiles will be saved to. 
            //  As it can improve the performance of application.
            openStreetMapOverlay.TileCache = new FileBitmapTileCache(cacheDirectory, openStreetMapOverlayName);
            
            return openStreetMapOverlay;
        }

        public static GoogleMapsOverlay CreateGoogleMapsOverlay()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "TileCache");
            const string googleMapsOverlayName = "Google Maps";

            GoogleMapsOverlay googleMapsOverlay = new GoogleMapsOverlay();

            //  Set the name of overlay and it will be displayed in the swicher overlay. 
            googleMapsOverlay.Name = googleMapsOverlayName;

            //  Set the image format as Jpeg. The default value is Png.
            googleMapsOverlay.ImageFormat = TileImageFormat.Jpeg;

            //  Set the height and width of the image. The default value is 256.
            googleMapsOverlay.TileHeight = 512;
            googleMapsOverlay.TileWidth = 512;
            googleMapsOverlay.TileCache = new FileBitmapTileCache(cacheDirectory, googleMapsOverlayName);
           
            return googleMapsOverlay;
        }

        public static BingMapsOverlay CreateBingMapsOverlay()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "TileCache");
            const string bingMapsOverlayName = "Bing Maps";

            //  Create BingMaps Overlay.
            BingMapsOverlay bingMapsOverlay = new BingMapsOverlay();
            bingMapsOverlay.Name = bingMapsOverlayName;
            bingMapsOverlay.ImageFormat = TileImageFormat.Jpeg;
            bingMapsOverlay.TileHeight = 512;
            bingMapsOverlay.TileWidth = 512;
            bingMapsOverlay.TileCache = new FileBitmapTileCache(cacheDirectory, bingMapsOverlayName);
           
            return bingMapsOverlay;
        }

        public static WorldMapKitWmsWpfOverlay CreateWorldMapKitWmsWpfOverlayer()
        {
            string cacheDirectory = Path.Combine(Path.GetTempPath(), "TileCache");
            const string worldMapKitOverlayName = "World Map Kit";

            WorldMapKitWmsWpfOverlay worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
            worldMapKitOverlay.Name = worldMapKitOverlayName;

            //  Set the Projection for WorldMapKitOverlay.
            //  The default value is DecimalDegrees
            worldMapKitOverlay.Projection = WorldMapKitProjection.SphericalMercator;
            worldMapKitOverlay.TileHeight = 512;
            worldMapKitOverlay.TileWidth = 512;
            worldMapKitOverlay.TileCache = new FileBitmapTileCache(cacheDirectory, worldMapKitOverlayName);
            
            return worldMapKitOverlay;
        }

        public static AdornmentOverlay CreateAdornmentOverlay()
        {
            const string adornmentOverlayName = "Adornment Overlay";

            AdornmentOverlay adornmentOverlay = new AdornmentOverlay();
            adornmentOverlay.Name = adornmentOverlayName;

            //  Create a ScaleBarAdornmentLayer and add it to AdornmentOverlay
            //  The default location is lower-left corner of the map control.
            ScaleBarAdornmentLayer scaleBarAdornmentLayer = new ScaleBarAdornmentLayer();
            adornmentOverlay.Layers.Add(scaleBarAdornmentLayer);
           
            return adornmentOverlay;
        }

        public static InMemoryMarkerOverlay CreateInMemoryMarkerOverlay()
        {
            const string inMemoryMarkerOverlayName = "InMemory Marker Overlay";

            InMemoryMarkerOverlay inMemoryMarkerOverlay = new InMemoryMarkerOverlay();
            inMemoryMarkerOverlay.Name = inMemoryMarkerOverlayName;
            inMemoryMarkerOverlay.Columns.Add(new FeatureSourceColumn("Name"));

            //  Set the marker's name as its Tooltip.
            inMemoryMarkerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.ToolTip = "[#Name#]";

            //  Set the marker's style.
            inMemoryMarkerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.ImageSource = new BitmapImage(new Uri("/Images/marker.png", UriKind.RelativeOrAbsolute));
            inMemoryMarkerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.Width = 20;
            inMemoryMarkerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.Height = 34;
            inMemoryMarkerOverlay.ZoomLevelSet.ZoomLevel01.DefaultPointMarkerStyle.YOffset = -17;
            inMemoryMarkerOverlay.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            //  Create a feature and add it to FeatureSource.
            Feature newFeature = new Feature("POINT(-10780998.2372393 3912039.4655777)");
            newFeature.ColumnValues.Add("Name", "William and Abbie Allen Elementary School");
            inMemoryMarkerOverlay.FeatureSource.BeginTransaction();
            inMemoryMarkerOverlay.FeatureSource.AddFeature(newFeature);
            inMemoryMarkerOverlay.FeatureSource.CommitTransaction();

            return inMemoryMarkerOverlay;
        }

        public static SimpleMarkerOverlay CreateSimpleMarkerOverlay()
        {
            const string simpleMarkerOverlayName = "Simple Marker Overlay";

            SimpleMarkerOverlay simpleMarkerOverlay = new SimpleMarkerOverlay();
            simpleMarkerOverlay.Name = simpleMarkerOverlayName;

            //  Create Marker and set the image for marker.
            Marker marker = new Marker(new PointShape(-10778193.6066375, 3912148.55));
            Uri resourceUri = new Uri(string.Format(uriTemplate, "drawpoint.png"), UriKind.RelativeOrAbsolute);
            marker.ImageSource = new BitmapImage(resourceUri);

            //  Set centerY of the marker
            marker.YOffset = -marker.ImageSource.Height * 0.5;
            simpleMarkerOverlay.Markers.Add(marker);

            return simpleMarkerOverlay;
        }

        public static PopupOverlay CreatePopupOverlay()
        {
            const string popupOverlayName = "Popup Overlay";

            PopupOverlay popupOverlay = new PopupOverlay();
            popupOverlay.Name = popupOverlayName;
            Popup popup = CreatePopup();
            popupOverlay.Popups.Add(popup);

            return popupOverlay;
        }

        public static LayerOverlay CreateCustomLayerOverlay()
        {
            const string customOverlayName = "Custom Overlay";

            //  Create a layeroverlay and set TileType as SingleTile. It means that the overlay is formed by one single tile.
            //  The TileType default value is MultipleTile
            LayerOverlay layerOverlay = new LayerOverlay() { TileType = TileType.SingleTile };
            layerOverlay.Name = customOverlayName;

            //  Create and initialize a projection. It will be apply to the FeatureSource create below.
            ManagedProj4Projection wgs84ToMercatorProjection = new ManagedProj4Projection();
            wgs84ToMercatorProjection.InternalProjectionParametersString = Proj4Projection.GetDecimalDegreesParametersString();
            wgs84ToMercatorProjection.ExternalProjectionParametersString = Proj4Projection.GetSphericalMercatorParametersString();
        
            //  Create a ShapeFileFeatureLayer by a shapefile.
            ShapeFileFeatureLayer restrictedLayer = new ShapeFileFeatureLayer(@"..\..\App_Data\CityLimitPolygon.shp");
            
            //  Set comtom styles, they will be uesed when draw the area. The priority of custom style is higher than default's.
            restrictedLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(new AreaStyle(new GeoPen(GeoColor.SimpleColors.White, 5.5f), new GeoSolidBrush(GeoColor.SimpleColors.Transparent)));
            restrictedLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(new AreaStyle(new GeoPen(GeoColor.SimpleColors.Red, 1.5f) { DashStyle = LineDashStyle.Dash }, new GeoSolidBrush(GeoColor.SimpleColors.Transparent)));
            restrictedLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
           
            //  Set the projection of the features.
            restrictedLayer.FeatureSource.Projection = wgs84ToMercatorProjection;
            
            //  Add the layer to the layeroverlay.
            layerOverlay.Layers.Add(restrictedLayer);

            //  Create schools feature layer by Schools.shp.
            ShapeFileFeatureLayer schoolsFeatureLayer = new ShapeFileFeatureLayer(@"..\..\App_Data\POIs\Schools.shp");
            
            //  Set the default point style which determines how to display the point.
            schoolsFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = new PointStyle(new GeoImage(GetImageStream("school.png")));
            schoolsFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            schoolsFeatureLayer.FeatureSource.Projection = wgs84ToMercatorProjection;
            layerOverlay.Layers.Add(schoolsFeatureLayer);

            ShapeFileFeatureLayer hospitalsFeatureLayer = new ShapeFileFeatureLayer(@"..\..\App_Data\POIs\Medical_Facilities.shp");
            hospitalsFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = new PointStyle(new GeoImage(GetImageStream("medical_facility.png")));
            hospitalsFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            hospitalsFeatureLayer.FeatureSource.Projection = wgs84ToMercatorProjection;
            layerOverlay.Layers.Add(hospitalsFeatureLayer);

            ShapeFileFeatureLayer hotelsFeatureLayer = new ShapeFileFeatureLayer(@"..\..\App_Data\POIs\Hotels.shp");
            hotelsFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = new PointStyle(new GeoImage(GetImageStream("hotel.png")));
            hotelsFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            hotelsFeatureLayer.FeatureSource.Projection = wgs84ToMercatorProjection;
            layerOverlay.Layers.Add(hotelsFeatureLayer);

            return layerOverlay;
        }

        private static Stream GetImageStream(string resourceName)
        {
            Uri resourceUri = new Uri(string.Format(uriTemplate, resourceName), UriKind.RelativeOrAbsolute);
            return Application.GetResourceStream(resourceUri).Stream;
        }

        private static Popup CreatePopup()
        {
            ShapeFileFeatureLayer schoolsFeatureLayer = new ShapeFileFeatureLayer(@"..\..\App_Data\POIs\Schools.shp");
            ManagedProj4Projection wgs84ToMercatorProjection = new ManagedProj4Projection();
            wgs84ToMercatorProjection.InternalProjectionParametersString = Proj4Projection.GetDecimalDegreesParametersString();
            wgs84ToMercatorProjection.ExternalProjectionParametersString = Proj4Projection.GetSphericalMercatorParametersString();
            schoolsFeatureLayer.FeatureSource.Projection = wgs84ToMercatorProjection;
            wgs84ToMercatorProjection.Open();
            schoolsFeatureLayer.Open();
            Feature feature = schoolsFeatureLayer.FeatureSource.GetFeatureById("10", ReturningColumnsType.AllColumns);

            Popup popup = new Popup(new PointShape(feature.GetWellKnownText()));

            popup.Content = new PopupUserControl(feature);
            return popup;
        }
    }
}
