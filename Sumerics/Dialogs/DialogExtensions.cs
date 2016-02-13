namespace Sumerics.Dialogs
{
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
            foreach (var window in App.Current.Windows)
            {
                var current = window as T;

                if (current != null)
                {
                    return current;
                }
            }

            return default(T);
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
