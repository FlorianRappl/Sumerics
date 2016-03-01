namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Models;
    using Sumerics.ViewModels;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SaveFileWindow.xaml
    /// </summary>
    public partial class SaveImageWindow : MetroWindow
    {
        #region ctor

        public SaveImageWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        async void ClearSelected(Object sender, RoutedEventArgs e)
        {
            var view = sender as ListView;

            if (view != null)
            {
                view.SelectedIndex = -1;

                if (sender != Current)
                {
                    //This hack seems strange but unfortunately it is the way to go
                    await Task.Delay(10);
                    Current.Focus();
                }
            }
            
        }

        void TextBoxKeyPressed(Object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                var vm = DataContext as SaveImageViewModel;

                if (vm != null)
                {
                    vm.FileName = tb.Text;
                    var path = Path.Combine(vm.CurrentDirectory.FullName, vm.FileName);

                    if (Directory.Exists(path))
                    {
                        vm.CurrentDirectory = new FolderModel(path);
                    }
                    else if (vm.CanAccept)
                    {
                        vm.Accept.Execute(this);
                    }
                }
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        void TextBoxChanged(Object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            var vm = DataContext as SaveImageViewModel;

            if (tb != null && vm != null)
            {
                vm.CanAccept = !tb.Text.Equals(String.Empty) && vm.IsValid(tb.Text);
            }
        }

        #endregion
    }
}