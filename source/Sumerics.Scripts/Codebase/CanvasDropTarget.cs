using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Sumerics.Controls
{
    public class CanvasDropTarget : DropTargetBase
    {
        public CanvasDropTarget()
        {
            SupportedFormat = "CanvasExample";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {
            var canvas = TargetUI as Canvas;
            var elt = ExtractElement(obj) as ScriptElement;
			var copy = new ScriptElement(elt);
            canvas.Children.Add(copy);
            Canvas.SetLeft(copy, dropPoint.X);
            Canvas.SetTop(copy, dropPoint.Y);
        }

		public override UIElement GetVisualFeedback(IDataObject obj)
		{
			var elt = base.GetVisualFeedback(obj);

			var anim = new DoubleAnimation(0.9, new Duration(TimeSpan.FromMilliseconds(500)));
			anim.From = 0.4;
			anim.AutoReverse = true;
			anim.RepeatBehavior = RepeatBehavior.Forever;
			elt.BeginAnimation(UIElement.OpacityProperty, anim);

			return elt;
		}
    }
}
