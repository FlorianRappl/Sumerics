namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;
    using System;
    using System.Threading.Tasks;
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

        async void SelectedTabChanged(Object sender, SelectionChangedEventArgs e)
        {
            var tabs = sender as TabControl;

            if (tabs != null)
            {
                if (tabs.SelectedItem != null)
                {
                    var editor = tabs.SelectedItem as EditorFileViewModel;

                    //Again we need to hack it...
                    //For explanations see MainWindow codebehind.
                    if (editor != null)
                    {
                        await Task.Delay(100);
                        editor.Control.SetFocus();
                    }
                }
            }
        }
    }
}
