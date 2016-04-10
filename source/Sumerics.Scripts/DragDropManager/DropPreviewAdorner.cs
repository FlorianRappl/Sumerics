using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sumerics.Controls
{
	public class DropPreviewAdorner : Adorner
	{
		ContentPresenter _presenter;
		double _left = 0;
		double _top = 0;

		public double Left
		{
			get { return _left; }
			set 
			{ 
				_left = value;
				UpdatePosition();
			}
		}

		public double Top
		{
			get { return _top; }
			set 
			{
				_top = value;
				UpdatePosition();
			}
		}

		public DropPreviewAdorner(UIElement feedbackUI, UIElement adornedElt) : base(adornedElt)
		{
			_presenter = new ContentPresenter();
			_presenter.Content = feedbackUI;
			_presenter.IsHitTestVisible = false;
			Visibility = Visibility.Collapsed;
		}

		private void UpdatePosition()
		{
			var layer = Parent as AdornerLayer;

			if (layer != null)
				layer.Update(AdornedElement);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			_presenter.Measure(constraint);
			return _presenter.DesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			_presenter.Arrange(new Rect(finalSize));
			return finalSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			return _presenter;
		}

		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			var result = new GeneralTransformGroup();
			result.Children.Add(new TranslateTransform(Left, Top));

			if (Left > 0)
				Visibility = Visibility.Visible;

			result.Children.Add(base.GetDesiredTransform(transform));
			return result;
		}
	}
}
