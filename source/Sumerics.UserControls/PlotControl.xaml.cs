namespace Sumerics.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl
    {
        #region ctor

        public PlotControl()
        {
            InitializeComponent();
		}

		#endregion

        #region Dependency Properties

        public Boolean IsDocked
        {
            get { return (Boolean)GetValue(IsDockedProperty); }
            set { SetValue(IsDockedProperty, value); }
        }

        public static readonly DependencyProperty IsDockedProperty =
            DependencyProperty.Register("IsDocked", typeof(Boolean), typeof(PlotControl), new PropertyMetadata(Changed));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var docked = (Boolean)e.NewValue;
            var control = d as PlotControl;

            if (docked)
            {
                control.DockButton.Visibility = System.Windows.Visibility.Collapsed;
                control.UndockButton.Visibility = System.Windows.Visibility.Visible;
                control.ConsoleButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                control.DockButton.Visibility = System.Windows.Visibility.Visible;
                control.UndockButton.Visibility = System.Windows.Visibility.Collapsed;
                control.ConsoleButton.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #endregion
	}
}
