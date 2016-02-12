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
        readonly EditorViewModel _vm;

        public EditorWindow(Kernel kernel, IConsole console)
        {
            _vm = new EditorViewModel(kernel, console);
            InitializeComponent();
            DataContext = _vm;
            Closing += (s, ev) => ev.Cancel = _vm.CloseAll();
        }

        public void OpenFile(String file)
        {
            _vm.OpenFile(file);
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
