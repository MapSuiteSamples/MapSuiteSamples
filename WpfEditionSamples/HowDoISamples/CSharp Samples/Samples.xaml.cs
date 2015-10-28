using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace CSHowDoISamples
{
    public partial class Samples : Window
    {
        private const string startHtml = "<body oncontextmenu='return false;'><div class='divbody'><pre name='code' class='c-sharp:nocontrols'>";
        private const string endHtml = "</pre></div><link type='text/css' rel='stylesheet' href='{0}\\SyntaxHighlighter.css'></link><script language='javascript' src='{0}\\shCore.js'></script><script language='javascript' src='{0}\\shBrushCSharp.js'></script><script language='javascript' src='{0}\\shBrushXml.js'></script><script language='javascript'>dp.SyntaxHighlighter.HighlightAll('code');</script></body>";
        private readonly static string mainFolder = ((new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)).Parent.Parent).FullName + "\\";
        private readonly static string highlighterFolder = Directory.GetDirectories(mainFolder, "SyntaxHighlighter", SearchOption.AllDirectories)[0];
        private readonly static string sourceFolder = highlighterFolder + "\\SourceCode\\";

        public Samples()
        {
            InitializeComponent();

            if (IsNetworkAlive())
            {
                AdRotator.Visibility = Visibility.Collapsed;
                AdRotatorHost.Visibility = Visibility.Visible;
                AdRotatorBrowser.Url = new Uri("http://gis.thinkgeo.com/Default.aspx?tabid=640");

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(20);
                timer.Start();
                timer.Tick += new EventHandler((sender, args) =>
                {
                    BitmapImage imageSource = new BitmapImage(new Uri("http://gis.thinkgeo.com/Default.aspx?tabid=640&random=" + Guid.NewGuid().ToString()));
                    AdRotator.Source = imageSource;
                });
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Add(new DisplayASimpleMap());

            LoadSamples();
        }

        private void LoadSamples()
        {
            XDocument xDoc = XDocument.Load("../../SampleList.xml");

            XElement rootElement = xDoc.Root;
            TreeViewItem rootItem = GetTreeNode(rootElement);
            rootItem.IsExpanded = true;
            SampleTree.Items.Add(rootItem);

            foreach (XElement element in rootElement.Elements())
            {
                AddItem(rootItem, element);
            }
        }

        private void AddItem(TreeViewItem rootItem, XElement element)
        {
            TreeViewItem childItem = GetTreeNode(element);
            rootItem.Items.Add(childItem);

            if (element.Elements("Sample").Count() > 0)
            {
                foreach (XElement childElement in element.Elements("Sample"))
                {
                    AddItem(childItem, childElement);
                }
            }
        }

        private static TreeViewItem GetTreeNode(XElement element)
        {
            string name = element.Attribute("Name").Value;
            TreeViewItem treeItem = new TreeViewItem();
            treeItem.Margin = new Thickness(0, 4, 0, 0);

            TextBlock header = new TextBlock();
            header.Text = name;
            //if (header.Text.Length > 27)
            //{
            //    header.Text = header.Text.Substring(0, 27) + "...";
            //}

            if (element.Attributes("Class").Count() > 0)
            {
                SampleNode sampleNode = new SampleNode();
                sampleNode.Name = name;
                sampleNode.ClassName = element.Attribute("Class").Value;

                if (element.Elements("Description").Count() != 0)
                {
                    sampleNode.Description = element.Element("Description").Value;
                }

                header.Tag = sampleNode;
                header.Cursor = Cursors.Hand;

                header.ToolTip = name;
                ToolTipService.SetInitialShowDelay(header, 100);
            }

            treeItem.Header = header;
            return treeItem;
        }

        private void SampleTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem currentItem = (TreeViewItem)e.NewValue;
            TextBlock currentHeader = (TextBlock)currentItem.Header;
            CSharpHost.Visibility = Visibility.Collapsed;
            XamlHost.Visibility = Visibility.Collapsed;

            if (currentHeader.Tag != null)
            {
                MainContent.Children.Clear();

                SampleNode sampleNode = currentHeader.Tag as SampleNode;
                if (sampleNode != null)
                {
                    string className = string.Format(CultureInfo.InvariantCulture, "CSHowDoISamples.{0}", sampleNode.ClassName);
                    Type type = GetType().Assembly.GetType(className);
                    Control currentSample = Activator.CreateInstance(type) as Control;
                    MainContent.Children.Add(currentSample);
                    currentSample.Unloaded += new RoutedEventHandler(currentSample_Unloaded);

                    TextBlock instructionLabel = currentSample.FindName("InstructionLabel") as TextBlock;

                    if (instructionLabel != null)
                    {
                        instructionLabel.Text = sampleNode.Description;
                    }
                }
            }
        }

        private void currentSample_Unloaded(object sender, RoutedEventArgs e)
        {
            var map = ((Control)sender).FindName("wpfMap1") as WpfMap;
            if (map != null)
            {
                map.Dispose();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            CSharpHost.Visibility = Visibility.Collapsed;
            XamlHost.Visibility = Visibility.Collapsed;

            if (MainContent.Children.Count != 0)
            {
                string sampleName = MainContent.Children[0].GetType().Name;

                switch (button.Content.ToString())
                {
                    case "Sample":
                        MainContent.Visibility = Visibility.Visible;
                        break;
                    case "C# Source":
                        string cSharpSourceName = String.Format(CultureInfo.InvariantCulture, "{0}.xaml.cs", sampleName);
                        string cSharpSourceFullName = GetSourceFullName(cSharpSourceName);

                        CSharpBrowser.Url = new Uri(cSharpSourceFullName);
                        CSharpHost.Visibility = Visibility.Visible;
                        break;
                    case "XAML":
                        string xamlSourceName = String.Format(CultureInfo.InvariantCulture, "{0}.xaml", sampleName);
                        string xamlSourceFullName = GetSourceFullName(xamlSourceName);

                        XamlBrowser.Url = new Uri(xamlSourceFullName);
                        XamlHost.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private string GetSourceFullName(string sourceName)
        {
            string sourceFullName = String.Format(CultureInfo.InvariantCulture, "{0}{1}.html", sourceFolder, sourceName);
            string[] fileNames = Directory.GetFiles(mainFolder, sourceName, SearchOption.AllDirectories);

            if (!File.Exists(sourceFullName))
            {
                DirectoryInfo sourceCodeFolder = new FileInfo(sourceFullName).Directory;
                if (!sourceCodeFolder.Exists)
                {
                    sourceCodeFolder.Create();
                }

                StreamReader reader = null;
                StreamWriter writer = null;
                try
                {
                    reader = new StreamReader(fileNames[0]);
                    writer = new StreamWriter(File.Create(sourceFullName));

                    writer.Write(startHtml);
                    writer.Write(reader.ReadToEnd());
                    writer.Write((String.Format(CultureInfo.InvariantCulture, endHtml, highlighterFolder)));
                }
                finally
                {
                    if (reader != null) { reader.Close(); reader.Dispose(); }
                    if (writer != null) { writer.Close(); writer.Dispose(); }
                }
            }

            return sourceFullName;
        }

        private static bool IsNetworkAlive()
        {
            ObjectQuery objectQuery = new ObjectQuery("select * from Win32_NetworkAdapter where NetConnectionStatus=2");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(objectQuery);
            return (searcher.Get().Count > 0) ? true : false;
        }
    }

    public class SampleNode
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
    }
}
