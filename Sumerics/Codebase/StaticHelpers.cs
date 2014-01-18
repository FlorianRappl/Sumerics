using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sumerics
{
	public static class StaticHelpers
	{
		public static T GetWindow<T>() where T : Window, new()
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
