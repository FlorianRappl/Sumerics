using System.Windows;
using System;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Xml;
using System.IO;
using System.Windows.Markup;

namespace Sumerics.Controls
{
	public abstract class DropTargetBase
	{
		string supportedFormat = "SampleFormat";
		UIElement _targetUI;

		public virtual UIElement TargetUI
		{
			get
			{
				return _targetUI;
			}
			set
			{
				_targetUI = value;
			}
		}

		public virtual string SupportedFormat
		{
			get { return supportedFormat; }
			set { supportedFormat = value; }
		}

		public virtual bool IsValidDataObject(IDataObject obj)
		{
			return (obj.GetDataPresent(supportedFormat));
		}

		public abstract void OnDropCompleted(IDataObject obj, Point dropPoint);

		public virtual UIElement GetVisualFeedback(IDataObject obj)
		{
			var elt = ExtractElement(obj);
			var t = elt.GetType();

			var rect = new Rectangle();
			rect.Width = (double)t.GetProperty("Width").GetValue(elt, null);
			rect.Height = (double)t.GetProperty("Height").GetValue(elt, null);
			rect.Fill = new VisualBrush(elt);
			rect.Opacity = 0.5;
			rect.IsHitTestVisible = false;

			return rect;
		}

		public virtual UIElement ExtractElement(IDataObject obj)
		{
			var xamlString = obj.GetData(supportedFormat) as string;
			var reader = XmlReader.Create(new StringReader(xamlString));
			var elt = XamlReader.Load(reader) as UIElement;
			return elt;
		}
	}
}
