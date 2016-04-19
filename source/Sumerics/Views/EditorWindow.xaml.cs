namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;

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
    }
}
