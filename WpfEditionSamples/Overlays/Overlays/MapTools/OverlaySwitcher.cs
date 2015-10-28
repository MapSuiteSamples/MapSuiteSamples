using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ThinkGeo.MapSuite.WpfDesktopEdition;

namespace ThinkGeo.MapSuite.Overlays
{
    [ToolboxItem(false)]
    [DesignTimeVisible(false)]
    [Serializable]
    public class OverlaySwitcher : MapTool
    {
        private Border borderPanel;
        private ItemsControl baseOverlayItemsControl;
        private ItemsControl customOverlayItemsControl;
        public event EventHandler<OverlayChangedOverlaySwitcherEventArgs> OverlayChanged;

        public OverlaySwitcher()
            : base(true)
        {
            DefaultStyleKey = typeof(OverlaySwitcher);

            Width = 230;
            Margin = new Thickness(0, 10, 10, 0);
            VerticalAlignment = VerticalAlignment.Top;
            HorizontalAlignment = HorizontalAlignment.Right;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            baseOverlayItemsControl = (ItemsControl)GetTemplateChild("baseOverlayItemsControl");
            customOverlayItemsControl = (ItemsControl)GetTemplateChild("customOverlayItemsControl");
            borderPanel = (Border)GetTemplateChild("borderPanel");

            Button collapseExpandButton = (Button)GetTemplateChild("collapseExpandButton");
            if (collapseExpandButton != null)
            {
                collapseExpandButton.Click -= CollapseExpandButton_Click;
                collapseExpandButton.Click += CollapseExpandButton_Click;
            }

            BindingOverlays();
        }

        protected virtual void OnOverlayChanged(OverlayChangedOverlaySwitcherEventArgs e)
        {
            EventHandler<OverlayChangedOverlaySwitcherEventArgs> handler = OverlayChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void BindingOverlays()
        {
            if (CurrentMap != null)
            {
                OverlayItem[] baseOverlayItems = CurrentMap.Overlays.Where(o => o.IsBase).Select(o => new OverlayItem(o, o.IsVisible, ChangeBaseOverlay)).ToArray();
                baseOverlayItemsControl.ItemsSource = baseOverlayItems;

                OverlayItem[] customOverlayItems = CurrentMap.Overlays.Where(o => !o.IsBase).Select(o => new OverlayItem(o, true, ChangeCustomOverlay)).ToArray();
                customOverlayItemsControl.ItemsSource = customOverlayItems;
            }
        }

        private void ChangeBaseOverlay(OverlayItem item)
        {
            if (IsLoaded)
            {
                OverlayChangedOverlaySwitcherEventArgs overlaySwitchedEventArgs = new OverlayChangedOverlaySwitcherEventArgs(item.Overlay);
                OnOverlayChanged(overlaySwitchedEventArgs);
                if (overlaySwitchedEventArgs.Cancel)
                {
                    item.IsVisible = false;
                }
                else
                {
                    foreach (Overlay overlay in CurrentMap.Overlays.Where(o => o.IsBase))
                    {
                        overlay.IsVisible = item.Overlay == overlay;
                    }
                    CurrentMap.Refresh();
                }
            }
        }

        private void ChangeCustomOverlay(OverlayItem item)
        {
            item.Overlay.IsVisible = item.IsVisible;
            CurrentMap.Refresh();
        }

        private void CollapseExpandButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (borderPanel.Visibility == Visibility.Visible)
            {
                borderPanel.Visibility = Visibility.Collapsed;
                button.Content = new Uri("/Images/switcher_maxmize.png", UriKind.RelativeOrAbsolute);
            }
            else
            {
                borderPanel.Visibility = Visibility.Visible;
                button.Content = new Uri("/Images/switcher_minimize.png", UriKind.RelativeOrAbsolute);
            }
        }

        private class OverlayItem
        {
            private bool isVisible;
            private Overlay overlay;
            private Action<OverlayItem> command;

            public OverlayItem(Overlay overlay, bool isVisible, Action<OverlayItem> command)
            {
                this.overlay = overlay;
                this.isVisible = isVisible;
                this.command = command;
            }

            public bool IsVisible
            {
                get { return isVisible; }
                set
                {
                    isVisible = value;
                    if (((value && overlay.IsBase) || !overlay.IsBase) && command != null)
                    {
                        command(this);
                    }
                }
            }

            public Overlay Overlay
            {
                get { return overlay; }
            }
        }
    }
}