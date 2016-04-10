namespace Sumerics.Proxies
{
    using Sumerics.Views;
    using System;
    using System.Windows.Controls;

    sealed class TabProxy : ITabs
    {
        TabControl _control;

        public TabControl Tabs
        {
            get { return _control ?? (_control = GetTabs()); }
        }

        static TabControl GetTabs()
        {
            var window = App.Current.MainWindow as MainWindow;

            if (window != null)
            {
                return window.MainTabs;
            }

            return null;
        }

        public Int32 SelectedIndex
        {
            get { return _control.SelectedIndex; }
        }

        public void Change(Int32 selectedIndex)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (selectedIndex >= 0 && selectedIndex < _control.Items.Count)
                {
                    _control.SelectedIndex = selectedIndex;
                }
            });
        }
    }
}
