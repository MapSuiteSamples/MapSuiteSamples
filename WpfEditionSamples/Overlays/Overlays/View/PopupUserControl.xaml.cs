using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using ThinkGeo.MapSuite.Core;

namespace ThinkGeo.MapSuite.Overlays
{
    public partial class PopupUserControl : UserControl
    {
        public PopupUserControl(Feature feature)
        {
            InitializeComponent();
            LoadInformation(feature);
        }

        private void LoadInformation(Feature feature)
        {
            if (feature == null) return;
            Collection<string> informations = new Collection<string>();
            foreach (var item in feature.ColumnValues)
            {
                string key = item.Key.ToUpperInvariant();
                if (key == "NAME")
                {
                    txtHeader.Text = item.Value;
                }
                else if (key == "ADDRESS")
                {
                    txtAddress.Text = item.Value;
                }
                else
                {
                    informations.Add(string.Format(CultureInfo.InvariantCulture, "{0} : {1}", item.Key, item.Value));
                }
            }
            lbInformations.ItemsSource = informations;

        }
    }
}