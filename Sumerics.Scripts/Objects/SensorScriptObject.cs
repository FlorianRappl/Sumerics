using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using YAMP;

namespace Sumerics.Controls
{
	public class SensorScriptObject : AbstractScriptObject
	{
		string variable;

		public SensorScriptObject()
		{
			Title = "Sensors";
		}

		protected override AbstractScriptObject CreateCopy()
		{
			var sensors = new SensorScriptObject();
			sensors.Title = "Pick a sensor function";
			sensors.variable = variable;
			return sensors;
		}

		public string SensorFunction
		{
			get { return variable; }
			set
			{
				variable = value;
				RaisePropertyChanged("SensorFunction");
			}
		}

		protected override UIElement CreateContent()
		{
			var cb = new ComboBox();

			cb.Height = 20;
			cb.DataContext = this;
			cb.Margin = new Thickness(5);
			cb.IsEditable = false;
			cb.ItemsSource = ScriptControl.Instance.SensorFunctions;

			if (cb.Items.Count > 0)
				cb.SelectedIndex = 0;

			var binding = new Binding("SensorFunction");
			binding.Mode = BindingMode.TwoWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			cb.SetBinding(ComboBox.SelectedItemProperty, binding);

			return cb;
		}
	}
}
