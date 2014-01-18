using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sumerics.Controls
{
	public class ConditionScriptObject : AbstractScriptObject
	{
		OutConnectorNode ifBlock;
		OutConnectorNode elseBlock;

		public ConditionScriptObject()
		{
			Title = "Conditions";
			Height = 120;
		}

		protected override AbstractScriptObject CreateCopy()
		{
			var n = new ConditionScriptObject();
			n.Title = "Set if and else blocks";
			return n;
		}

		protected override UIElement CreateContent()
		{
			var sp = new StackPanel();
			sp.VerticalAlignment = VerticalAlignment.Center;
			var tbTrue = new TextBlock();
			tbTrue.HorizontalAlignment = HorizontalAlignment.Center;
			tbTrue.Text = "If true";
			tbTrue.Margin = new Thickness(5);
			sp.Children.Add(tbTrue);
			ifBlock = new OutConnectorNode();
			sp.Children.Add(ifBlock.Control);
			var tbFalse = new TextBlock();
			tbFalse.HorizontalAlignment = HorizontalAlignment.Center;
			tbFalse.Text = "If false";
			tbFalse.Margin = new Thickness(5);
			sp.Children.Add(tbFalse);
			elseBlock = new OutConnectorNode();
			sp.Children.Add(elseBlock.Control);
			return sp;
		}
	}
}
