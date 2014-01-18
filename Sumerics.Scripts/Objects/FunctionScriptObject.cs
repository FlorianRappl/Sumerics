using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sumerics.Controls
{
	public class FunctionScriptObject : AbstractScriptObject
	{
		string variable;

		public FunctionScriptObject()
		{
			Title = "Functions";
		}

		protected override AbstractScriptObject CreateCopy()
		{
			var functions = new FunctionScriptObject();
			functions.Title = "Pick a numeric function";
			functions.variable = variable;
			return functions;
		}

		public string NumericFunction
		{
			get { return variable; }
			set
			{
				variable = value;
				RaisePropertyChanged("NumericFunction");
			}
		}

		protected override UIElement CreateContent()
		{
			var cb = new ComboBox();

			cb.Height = 20;
			cb.DataContext = this;
			cb.Margin = new Thickness(5);
			cb.IsEditable = false;
			cb.ItemsSource = ScriptControl.Instance.NumericFunctions;

			if (cb.Items.Count > 0)
				cb.SelectedIndex = 0;

			var binding = new Binding("NumericFunction");
			binding.Mode = BindingMode.TwoWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			cb.SetBinding(ComboBox.SelectedItemProperty, binding);

			return cb;
		}
	}
}
