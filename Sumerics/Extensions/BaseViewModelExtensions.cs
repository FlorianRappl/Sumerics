namespace Sumerics
{
    using Sumerics.ViewModels;

    static class BaseViewModelExtensions
    {
        public static void ShowWindow(this BaseViewModel vm)
        {
            var window = WindowFactory.Instance.Create(vm);

            if (window != null)
            {
                window.Show();
            }
        }

        public static void ShowDialog(this BaseViewModel vm)
        {
            var window = WindowFactory.Instance.Create(vm);

            if (window != null)
            {
                window.ShowDialog();
            }
        }
    }
}
