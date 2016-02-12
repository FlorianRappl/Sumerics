namespace Sumerics
{
    using MahApps.Metro.Controls;
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
                var evm = DataContext as EditorViewModel;

                if (evm != null)
                {
                    ev.Cancel = evm.CloseAll();
                }
            };
        }

        public void OpenFile(String file)
        {
            var context = DataContext as EditorViewModel;
            context.OpenFile(file);
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
