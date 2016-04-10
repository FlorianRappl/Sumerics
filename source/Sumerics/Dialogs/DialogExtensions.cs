namespace Sumerics.Dialogs
{
    using System.Linq;
    using System.Windows;

    static class DialogExtensions
    {
        public static T Show<T>(this IDialogHandler handler)
            where T : Window, new()
        {
            var current = Get<T>();

            if (current == null)
            {
                current = new T();
                current.Show();
            }
            else
            {
                current.Activate();
            }

            return current;
        }

        public static T Get<T>()
            where T : Window
        {
            return  App.Current.Windows.OfType<T>().FirstOrDefault();
        }

        public static void Close<T>(this IDialogHandler handler)
            where T : Window
        {
            var current = Get<T>();

            if (current != null)
            {
                current.Close();
            }
        }
    }
}
