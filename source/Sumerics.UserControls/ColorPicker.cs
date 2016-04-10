namespace Sumerics.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Simple yet customizable templated control to pick a color/brush.
    /// </summary>
    public class ColorPicker : Control
    {
        #region Commands

        public static readonly RoutedCommand ColorDropCommand = new RoutedCommand("ColorDrop", typeof(ColorPicker));
        public static readonly RoutedCommand ColorSelectedCommand = new RoutedCommand("ColorSelected", typeof(ColorPicker));

        #endregion

        #region Events

        /// <summary>
        /// ColorChanged Routed Event
        /// </summary>
        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged",
            RoutingStrategy.Bubble, typeof(RoutedHandler<ColorRoutedEventArgs>), typeof(ColorPicker));

        /// <summary>
        /// Bubbled event that occurs when a diagram element is added.
        /// </summary>
        public event RoutedHandler<ColorRoutedEventArgs> ColorChanged
        {
            add { AddHandler(ColorChangedEvent, value); }
            remove { RemoveHandler(ColorChangedEvent, value); }
        }

        /// <summary>
        /// A helper method to raise the ColorChanged event.
        /// </summary>
        /// <param name="e">the argument</param>
        private ColorRoutedEventArgs RaiseColorChangedEvent(ColorEventArgs e)
        {
            return RaiseColorChangedEvent(this, e);
        }

        /// <summary>
        /// A static helper method to raise the ColorChanged event on a target element.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        /// <param name="e">the argument</param>
        internal static ColorRoutedEventArgs RaiseColorChangedEvent(UIElement target, ColorEventArgs e)
        {
            if (target != null)
            {
                var args = new ColorRoutedEventArgs(e) { RoutedEvent = ColorChangedEvent };
                target.RaiseEvent(args);
                return args;
            }

			return null;
        }

        /// <summary>
        /// PreviewColorChanged Routed Event
        /// </summary>
        public static readonly RoutedEvent PreviewColorChangedEvent = EventManager.RegisterRoutedEvent("PreviewColorChanged",
            RoutingStrategy.Bubble, typeof(RoutedHandler<ColorRoutedEventArgs>), typeof(ColorPicker));

        /// <summary>
        /// Tunneled event that occurs when a diagram element is added.
        /// </summary>
        public event RoutedHandler<ColorRoutedEventArgs> PreviewColorChanged
        {
            add { AddHandler(PreviewColorChangedEvent, value); }
            remove { RemoveHandler(PreviewColorChangedEvent, value); }
        }

        /// <summary>
        /// A helper method to raise the PreviewColorChanged event.
        /// </summary>
        /// <param name="e">the argument</param>
        private ColorRoutedEventArgs RaisePreviewColorChangedEvent(ColorEventArgs e)
        {
            return RaisePreviewColorChangedEvent(this, e);
        }

        /// <summary>
        /// A static helper method to raise the PreviewColorChanged event on a target element.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        /// <param name="e">the argument</param>
        internal static ColorRoutedEventArgs RaisePreviewColorChangedEvent(UIElement target, ColorEventArgs e)
        {
            if (target != null)
            {
                var args = new ColorRoutedEventArgs(e) { RoutedEvent = PreviewColorChangedEvent };
                target.RaiseEvent(args);
                return args;
            }

			return null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color popup.
        /// </summary>
        /// <value>The color popup.</value>
        Popup ColorPopup { get; set; }

        /// <summary>
        /// Gets or sets the main button.
        /// </summary>
        /// <value>The main button.</value>
        Button MainButton { get; set; }

		#region CurrentColor

        /// <summary>
        /// CurrentColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty CurrentColorProperty =
            DependencyProperty.Register("CurrentColor", typeof(Brush), typeof(ColorPicker),
                new FrameworkPropertyMetadata((Brush)Brushes.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));		

        /// <summary>
        /// Gets or sets the CurrentColor property. This dependency property indicates ....
        /// </summary>
        public Brush CurrentColor
        {
            get { return (Brush)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        #endregion

		#region ItemsSource

		void ReadStandardColors()
		{
			var list = new List<VisualTag>();
			var toc = typeof(Colors);
			var props = toc.GetProperties();

			foreach (var prop in props)
			{
				list.Add(new VisualTag
				{
					Title = prop.Name,
					Brush = new SolidColorBrush((Color)prop.GetValue(null))
				});
			}

			ItemsSource = list;
		}

        /// <summary>
        /// ItemsSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ColorPicker),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnItemsSourceChanged)));

        /// <summary>
        /// Gets or sets the ItemsSource property.  This dependency property 
        /// indicates ....
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemsSource property.
        /// </summary>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #endregion

        #region Constructor

        public ColorPicker()
        {
            CommandBindings.Add(new CommandBinding(ColorSelectedCommand, Executed, CanExecute));
            CommandBindings.Add(new CommandBinding(ColorDropCommand, Executed, CanExecute));
			ReadStandardColors();
        }

        #endregion

		#region Methods

        /// <summary>
        /// Determines whether this instance can execute the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.CanExecuteRoutedEventArgs"/> instance
		/// containing the event data.</param>
        void CanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
			e.CanExecute = true;
        }

        /// <summary>
        /// Executeds the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance
		/// containing the event data.</param>
        void Executed(Object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command.Equals(ColorDropCommand))
            {
                if (ColorPopup != null)
                {
                    ColorPopup.Placement = PlacementMode.Mouse;
                    ColorPopup.StaysOpen = false;
                    ColorPopup.IsOpen = true;
                }

				e.Handled = true;
            }
            else if (e.Command.Equals(ColorSelectedCommand))
            {
                var tag = e.Parameter as VisualTag;

                if (tag != null)
                {
                    SelectColor(tag.Brush);
                }

				e.Handled = true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call
		/// <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ColorPopup = Template.FindName("ColorPopup", this) as Popup;
            MainButton = Template.FindName("ColorApplyButton", this) as Button;
        }

        /// <summary>
        /// Raises the <see cref="ColorChanged"/> event and closes the popup.
        /// </summary>
        /// <param name="brush">The brush.</param>
        void SelectColor(Brush brush)
        {
            RaiseColorChangedEvent(new ColorEventArgs(brush));

			CurrentColor = brush;

            if (ColorPopup != null)
            {
                ColorPopup.IsOpen = false;
            }
        }

        #endregion
    }
}
