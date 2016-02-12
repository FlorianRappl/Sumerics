namespace Sumerics
{
    using System.Threading.Tasks;
    using System.Windows;

	public static class StaticHelpers
	{
		public static T GetWindow<T>(IComponents container) 
            where T : Window
		{
			foreach (Window window in App.Current.Windows)
			{
				if (window is T)
				{
					window.Activate();
					return (T)window;
				}
			}

			var win = container.Create<T>();
			win.Show();
			return win;
		}

        public static T GetWindow<T>()
            where T : Window, new()
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is T)
                {
                    window.Activate();
                    return (T)window;
                }
            }

            var win = new T();
            win.Show();
            return win;
        }

        public static void FireAndForget(this Task task)
        {
        }

        public static void FireAndForget<T>(this Task<T> task)
        {
        }
	}
}
