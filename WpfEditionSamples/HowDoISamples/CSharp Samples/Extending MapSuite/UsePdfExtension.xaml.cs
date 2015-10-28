using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PdfSharp;
using PdfSharp.Pdf;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class UsePdfExtension : UserControl
    {
        public UsePdfExtension()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            wpfMap1.MapUnit = GeographyUnit.DecimalDegree;
            wpfMap1.CurrentExtent = new RectangleShape(-111.7875, 92.0859375, 148.36875, -93.5390625);

            EcwRasterLayer worldImageLayer = new EcwRasterLayer(@"..\..\SampleData\Data\World.ecw");

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\Countries02.shp");
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.StartCap = DrawingLineCap.Round;
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.EndCap = DrawingLineCap.Round;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer usStatesLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\USStates.shp");
            usStatesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.State2;
            usStatesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.StartCap = DrawingLineCap.Round;
            usStatesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer worldCapitalsLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\WorldCapitals.shp");
            worldCapitalsLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.Capital3;
            worldCapitalsLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer worldCapitalsLabelsLayer = new ShapeFileFeatureLayer(@"..\..\SampleData\Data\WorldCapitals.shp");
            worldCapitalsLabelsLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.Capital3("city_name");
            worldCapitalsLabelsLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.HaloPen = new GeoPen(GeoColor.StandardColors.White, 2);
            worldCapitalsLabelsLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.HaloPen.StartCap = DrawingLineCap.Round;
            worldCapitalsLabelsLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.HaloPen.EndCap = DrawingLineCap.Round;
            worldCapitalsLabelsLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.SuppressPartialLabels = true;
            worldCapitalsLabelsLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay worldOverlay = new LayerOverlay();
            worldOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean)));
            worldOverlay.Layers.Add("WorldImageLayer", worldImageLayer);
            worldOverlay.Layers.Add("WorldLayer", worldLayer);
            worldOverlay.Layers.Add("USStatesLayer", usStatesLayer);
            worldOverlay.Layers.Add("WorldCapitals", worldCapitalsLayer);
            worldOverlay.Layers.Add("WorldCapitalsLabels", worldCapitalsLabelsLayer);
            wpfMap1.Overlays.Add("WorldOverlay", worldOverlay);

            wpfMap1.Refresh();
        }

        // Here we setup the PDF page and then create our PDFGeoCanvas.
        // We loop through all the layers to draw and then save & pop the 
        // PDF
        private void btnToPdf_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            if (pageOrientationLandscape.IsChecked == true)
            {
                page.Orientation = PageOrientation.Landscape;
            }

            PdfGeoCanvas pdfGeoCanvas = new PdfGeoCanvas();

            // This allows you to control the area in which you want the
            // map to draw in.  Leaving this commented out uses the whole page
            //pdfGeoCanvas.DrawingArea = new Rectangle(200, 50, 400, 400);
            Collection<SimpleCandidate> labelsInLayers = new Collection<SimpleCandidate>();
            pdfGeoCanvas.BeginDrawing(page, wpfMap1.CurrentExtent, GeographyUnit.DecimalDegree);
            foreach (Layer layer in ((LayerOverlay)wpfMap1.Overlays[0]).Layers)
            {
                layer.Open();
                layer.Draw(pdfGeoCanvas, labelsInLayers);
                layer.Close();
            }
            pdfGeoCanvas.EndDrawing();
            string filename = GetTemporaryFolder() + "\\MapSuite PDF Map.pdf";
            try
            {
                document.Save(filename);
                OpenPdfFile(filename);
            }
            catch (Exception ex)
            {
                string message = "You have no permission to write file to disk." + ex.Message;
                MessageBox.Show(message, "No permission", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, (MessageBoxOptions)0);
            }
        }

        private static string GetTemporaryFolder()
        {
            string returnValue = string.Empty;
            if (string.IsNullOrEmpty(returnValue))
            {
                returnValue = Environment.GetEnvironmentVariable("Temp");
            }

            if (string.IsNullOrEmpty(returnValue))
            {
                returnValue = Environment.GetEnvironmentVariable("Tmp");
            }

            if (string.IsNullOrEmpty(returnValue))
            {
                returnValue = @"c:\MapSuiteTemp";
            }
            else
            {
                returnValue = returnValue + "\\" + "MapSuite";
            }

            return returnValue;
        }

        private static void OpenPdfFile(string filename)
        {
            try
            {
                Process.Start(filename);
            }
            catch (Win32Exception ex)
            {
                if (ex.Message == "No application is associated with the specified file for this operation")
                {
                    //string message = "You can't open Pdf file, the reason maybe you don't install Adobe Reader\r\n\r\n" + ex.ToString();
                    //MessageBox.Show(message, "Open Pdf file failed", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                    ProcessStartInfo psi = new ProcessStartInfo(@"C:\WINDOWS\system32\rundll32.exe");
                    psi.Arguments = @" C:\WINDOWS\system32\shell32.dll, OpenAs_RunDLL " + filename;
                    Process.Start(psi);
                }
            }
        }
    }
}
