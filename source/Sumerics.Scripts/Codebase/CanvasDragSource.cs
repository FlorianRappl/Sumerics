using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sumerics.Controls
{
    public class CanvasDragSource : DragSourceBase
    {
        public CanvasDragSource()
        {
            SupportedFormat = "CanvasExample";
        }

        public override void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects)
        {
            if ((finalEffects & DragDropEffects.Move) == DragDropEffects.Move)
            {
				//Do NOT remove the item
                //(SourceUI as Canvas).Children.Remove(draggedElt);
            }
        }

        public override bool IsDraggable(UIElement dragElt)
        {
			return (dragElt is ScriptElement);
        }
    }
}
