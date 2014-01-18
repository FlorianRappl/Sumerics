using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sumerics.Controls
{
	public class CircleButton
	{
		public BitmapImage Image { get; set; }

		public Action Clicked { get; set; }

		public string ToolTip { get; set; }
	}
}
