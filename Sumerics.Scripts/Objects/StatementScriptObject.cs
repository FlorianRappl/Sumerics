using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sumerics.Controls
{
	public class StatementScriptObject : AbstractScriptObject
    {
        string variable;

		public StatementScriptObject()
		{
            Title = "Statements";
			Height = 90;
		}

        public string Statement
        {
            get { return variable; }
            set
            {
                variable = value;
                RaisePropertyChanged("Statement");
            }
        }

		protected override AbstractScriptObject CreateCopy()
        {
            var o = new StatementScriptObject();
            o.Statement = variable;
			o.Title = "Enter statements";
            return o;
		}

		protected override UIElement CreateContent()
		{
			/*
			Buttons = new CircleButton[] {
				new CircleButton
				{
					Clicked = () => { },
					Image = new BitmapImage(new Uri(@"Images\addstatement.png", UriKind.Relative)),
					ToolTip = "Add statement line"
				}
			};*/

			var tb = new TextBox();
			tb.Margin = new Thickness(5);
			tb.Height = 20;
			tb.Foreground = Brushes.SteelBlue;
			tb.DataContext = this;

			var binding = new Binding("Statement");
			binding.Mode = BindingMode.TwoWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			tb.SetBinding(TextBox.TextProperty, binding);

			return tb;
		}
	}
}
