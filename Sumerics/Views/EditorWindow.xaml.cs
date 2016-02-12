using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sumerics
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : MetroWindow
    {
        public EditorWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        public void OpenFile(String file)
        {
            var context = DataContext as EditorViewModel;
            context.OpenFile(file);
        }

        void OnWindowClosing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            var evm = DataContext as EditorViewModel;

            if (evm != null)
            {
                e.Cancel = evm.CloseAll();
            }
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
