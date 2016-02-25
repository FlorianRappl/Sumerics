namespace Sumerics.Dialogs
{
    using System.Linq;
    using System.Windows;

    static class DialogExtensions
    {
        public static T Obtain<T>(this IComponents container)
            where T : Window
        {
            var current = Get<T>();

            if (current != null)
            {
                current.Activate();
                return current;
            }

            return container.Create<T>();
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
