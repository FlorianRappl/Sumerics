namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;
    using System;
    using System.Linq;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : MetroWindow
    {
        public EditorWindow()
        {
            InitializeComponent();
            Closing += (s, ev) =>
            {
                var vm = DataContext as EditorViewModel;

                if (vm != null)
                {
                    ev.Cancel = vm.CloseAll();
                }
            };
        }

        void SelectedTabChanged(Object sender, SelectionChangedEventArgs e)
        {
            var tabs = sender as TabControl;
            var selected = tabs.SelectedItem;

            foreach (var tab in tabs.Items.OfType<EditorFileViewModel>())
            {
                tab.IsSelected = Object.ReferenceEquals(selected, tab);
            }
        }
    }
}
