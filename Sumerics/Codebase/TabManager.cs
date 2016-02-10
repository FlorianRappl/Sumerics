namespace Sumerics
{
    using System;
    using System.Windows.Controls;

    sealed class TabManager : ITabManager
    {
        readonly TabControl _control;

        public TabManager(TabControl control)
        {
            _control = control;
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
