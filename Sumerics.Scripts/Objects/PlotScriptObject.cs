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
	public class PlotScriptObject : AbstractScriptObject
	{
		string variable;

		public PlotScriptObject()
		{
			Title = "Plots";
			InputConnectors = 2;
		}

		protected override AbstractScriptObject CreateCopy()
		{
			var plots = new PlotScriptObject();
			plots.Title = "Choose a plot method";
			plots.variable = variable;
			return plots;
		}

		public string PlotFunction
		{
			get { return variable; }
			set
			{
				variable = value;
				RaisePropertyChanged("PlotFunction");
			}
		}

		protected override UIElement CreateContent()
		{
			var cb = new ComboBox();

			cb.Height = 20;
			cb.DataContext = this;
			cb.Margin = new Thickness(5);
			cb.IsEditable = false;
			cb.ItemsSource = ScriptControl.Instance.PlotFunctions;

			if (cb.Items.Count > 0)
				cb.SelectedIndex = 0;

			var binding = new Binding("PlotFunction");
			binding.Mode = BindingMode.TwoWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			cb.SetBinding(ComboBox.SelectedItemProperty, binding);

			return cb;
		}
	}
}
