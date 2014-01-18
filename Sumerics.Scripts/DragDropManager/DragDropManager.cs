using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace Sumerics.Controls
{
	public static class DragDropManager
	{
		#region Members

		static UIElement _draggedElt;
		static bool _isMouseDown = false;
		static Point _dragStartPoint;
		static Point _offsetPoint;
		static DropPreviewAdorner _overlayElt;

		#endregion

		#region Dependency Properties

		public static readonly DependencyProperty DragSourceProperty =
				DependencyProperty.RegisterAttached("DragSource", typeof(DragSourceBase), typeof(DragDropManager),
				new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDragSourceChanged)));

		public static DragSourceBase GetDragSource(DependencyObject depObj)
		{
			return depObj.GetValue(DragSourceProperty) as DragSourceBase;
		}

		public static void SetDragSource(DependencyObject depObj, bool isSet)
		{
			depObj.SetValue(DragSourceProperty, isSet);
		}

		public static readonly DependencyProperty DropTargetProperty =
			DependencyProperty.RegisterAttached("DropTarget", typeof(DropTargetBase), typeof(DragDropManager),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDropTargetChanged)));

		public static void SetDropTarget(DependencyObject depObj, bool isSet)
		{
			depObj.SetValue(DropTargetProperty, isSet);
		}

		public static DropTargetBase GetDropTarget(DependencyObject depObj)
		{
			return depObj.GetValue(DropTargetProperty) as DropTargetBase;
		}

		#endregion

		#region Property Change handlers
	  
		private static void OnDragSourceChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			var sourceElt = depObj as UIElement;

			if (args.NewValue != null && args.OldValue == null)
			{
				sourceElt.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonDown);
				sourceElt.PreviewMouseMove += new MouseEventHandler(DragSource_PreviewMouseMove);
				sourceElt.PreviewMouseUp += new MouseButtonEventHandler(DragSource_PreviewMouseUp);

				// Set the Drag source UI
				DragSourceBase advisor = args.NewValue as DragSourceBase;
				advisor.SourceUI = sourceElt;
			}
			else if (args.NewValue == null && args.OldValue != null)
			{
				sourceElt.PreviewMouseLeftButtonDown -= DragSource_PreviewMouseLeftButtonDown;
				sourceElt.PreviewMouseMove -= DragSource_PreviewMouseMove;
				sourceElt.PreviewMouseUp -= DragSource_PreviewMouseUp;
			}
		}

		static void DragSource_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			_isMouseDown = false;
			Mouse.Capture(null);
		}

		private static void OnDropTargetChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			var targetElt = depObj as UIElement;

			if (args.NewValue != null && args.OldValue == null)
			{
				targetElt.PreviewDragEnter += new DragEventHandler(DropTarget_PreviewDragEnter);
				targetElt.PreviewDragOver += new DragEventHandler(DropTarget_PreviewDragOver);
				targetElt.PreviewDragLeave += new DragEventHandler(DropTarget_PreviewDragLeave);
				targetElt.PreviewDrop += new DragEventHandler(DropTarget_PreviewDrop);

				targetElt.AllowDrop = true;

				// Set the Drag source UI
				var advisor = args.NewValue as DropTargetBase;
				advisor.TargetUI = targetElt;
			}
			else if (args.NewValue == null && args.OldValue != null)
			{
				targetElt.PreviewDragEnter -= DropTarget_PreviewDragEnter;
				targetElt.PreviewDragOver -= DropTarget_PreviewDragOver;
				targetElt.PreviewDragLeave -= DropTarget_PreviewDragLeave;
				targetElt.PreviewDrop -= DropTarget_PreviewDrop;
				targetElt.AllowDrop = false;
			}
		}


		#endregion

		/* 
		 *		Drop Target events 
		 */
		static void DropTarget_PreviewDrop(object sender, DragEventArgs e)
		{
			if (!UpdateEffects(sender, e))
				return;

			var advisor = GetDropTarget(sender as DependencyObject);
			Point dropPoint = e.GetPosition(sender as UIElement);

			// Calculate displacement for (Left, Top)
			Point offset = e.GetPosition(_overlayElt);
			dropPoint.X = dropPoint.X - offset.X;
			dropPoint.Y = dropPoint.Y - offset.Y;

			advisor.OnDropCompleted(e.Data, dropPoint);
			RemovePreviewAdorner();
			_offsetPoint = new Point(0, 0);

		}

		static void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
		{
			if (UpdateEffects(sender, e) == false)
				return;

			var advisor = GetDropTarget(sender as DependencyObject);
			var mousePoint = MouseUtilities.GetMousePosition(advisor.TargetUI);
			
			//Console.WriteLine("Inside DropTarget_PreviewDragLeave1" + mousePoint.X.ToString() + "|" + mousePoint.Y.ToString());
			//giving a tolerance of 2 so that the adorner is removed when the mouse is moved fast.
			//this might still be small...in that case increase the tolerance
			if ((mousePoint.X < 2) || (mousePoint.Y < 2)||
				(mousePoint.X > ((FrameworkElement)(advisor.TargetUI)).ActualWidth - 2) ||
				(mousePoint.Y > ((FrameworkElement)(advisor.TargetUI)).ActualHeight - 2))
			{
				RemovePreviewAdorner();
			}

			e.Handled = true;
		}

		static void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
		{
			if (!UpdateEffects(sender, e))
				return;

			// Update position of the preview Adorner
			var position = e.GetPosition(sender as UIElement);

			_overlayElt.Left = position.X - _offsetPoint.X;
			_overlayElt.Top = position.Y - _offsetPoint.Y;
			
			e.Handled = true;
		}

		static void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
		{
			if (!UpdateEffects(sender, e))
				return;

			// Setup the preview Adorner
			var feedbackUI = GetDropTarget(sender as DependencyObject).GetVisualFeedback(e.Data);
			_offsetPoint = GetOffsetPoint(e.Data);

			var advisor = GetDropTarget(sender as DependencyObject);
			var mousePoint = MouseUtilities.GetMousePosition(advisor.TargetUI);

			//giving a tolerance of 2 so that the adorner is created when the mouse is moved fast.
			//this might still be small...in that case increase the tolerance
			if ((mousePoint.X < 2) || (mousePoint.Y < 2) ||
				(mousePoint.X > ((FrameworkElement)(advisor.TargetUI)).ActualWidth - 2) ||
				(mousePoint.Y > ((FrameworkElement)(advisor.TargetUI)).ActualHeight - 2) ||
				 (_overlayElt == null))
			{
				CreatePreviewAdorner(sender as UIElement, feedbackUI);
			}

			e.Handled = true;
		}

		static Point GetOffsetPoint(IDataObject obj)
		{
			var p = (Point)obj.GetData("OffsetPoint");
			return p;
		}

		static bool UpdateEffects(object uiObject, DragEventArgs e)
		{
			var advisor = GetDropTarget(uiObject as DependencyObject);

			if (!advisor.IsValidDataObject(e.Data))
				return false;

			if ((e.AllowedEffects & DragDropEffects.Move) == 0 &&
				(e.AllowedEffects & DragDropEffects.Copy) == 0)
			{
				e.Effects = DragDropEffects.None;
				return true;
			}

			if ((e.AllowedEffects & DragDropEffects.Move) != 0 &&
				(e.AllowedEffects & DragDropEffects.Copy) != 0)
			{
				if ((e.KeyStates & DragDropKeyStates.ControlKey) != 0)
				{
				}

				e.Effects = ((e.KeyStates & DragDropKeyStates.ControlKey) != 0) ?
					DragDropEffects.Copy : DragDropEffects.Move;
			}

			return true;
		}

		/*
		 *		Drag Source events 
		 */
		static void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Make this the new drag source
			var advisor = GetDragSource(sender as DependencyObject);

			if (!advisor.IsDraggable(e.Source as UIElement))
				return;

			_draggedElt = e.Source as UIElement;
			_dragStartPoint = e.GetPosition(GetTopContainer());

			_offsetPoint = e.GetPosition(_draggedElt);
			_isMouseDown = true;
		}

		static void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (_isMouseDown && IsDragGesture(e.GetPosition(GetTopContainer())))
				DragStarted(sender as UIElement);
		}

		static void DragStarted(UIElement uiElt)
		{
			_isMouseDown = false;
			Mouse.Capture(uiElt);

			var advisor = GetDragSource(uiElt as DependencyObject);
			var data = advisor.GetDataObject(_draggedElt);
			
			data.SetData("OffsetPoint", _offsetPoint);

			var supportedEffects = advisor.SupportedEffects;

			// Perform DragDrop
			var effects = System.Windows.DragDrop.DoDragDrop(_draggedElt, data, supportedEffects);
			advisor.FinishDrag(_draggedElt, effects);

			// Clean up
			RemovePreviewAdorner();
			Mouse.Capture(null);
			_draggedElt = null;
		}

		static bool IsDragGesture(Point point)
		{
			bool hGesture = Math.Abs(point.X - _dragStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance;
			bool vGesture = Math.Abs(point.Y - _dragStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance;

			return (hGesture | vGesture);
		}

		/*
		 *		Utility functions
		 */
		static UIElement GetTopContainer()
		{
			// return  LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "canvas") as UIElement;
			return Application.Current.MainWindow.Content as UIElement;
		}

		static void CreatePreviewAdorner(UIElement adornedElt, UIElement feedbackUI)
		{
			// Clear if there is an existing preview adorner
			RemovePreviewAdorner();

			var layer = AdornerLayer.GetAdornerLayer(GetTopContainer());
			_overlayElt = new DropPreviewAdorner(feedbackUI, adornedElt);
			layer.Add(_overlayElt);
		}

		static void RemovePreviewAdorner()
		{
			if (_overlayElt != null)
			{
				AdornerLayer.GetAdornerLayer(GetTopContainer()).Remove(_overlayElt);
				_overlayElt = null;
			}
		}

	}

}
