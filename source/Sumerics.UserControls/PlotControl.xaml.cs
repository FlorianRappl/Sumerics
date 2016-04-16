namespace Sumerics.Controls
{
    using Sumerics.Controls.Plots;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl
    {
        #region Fields

        static readonly ControlFactory Factory = new ControlFactory();

        #endregion

        #region ctor

        public PlotControl()
        {
            InitializeComponent();
			PlotArea.Content = Factory.CreateDefault();
            DataContextChanged += (s, ev) => PlotArea.Content = Factory.Create(ev.NewValue);
		}

		#endregion

        #region Dependency Properties

        public ICommand GridCommand
        {
            get { return (ICommand)GetValue(GridCommandProperty); }
            set { SetValue(GridCommandProperty, value); }
        }

        public static readonly DependencyProperty GridCommandProperty =
            DependencyProperty.Register("GridCommand", typeof(ICommand), typeof(PlotControl), new PropertyMetadata(null));

        public Object GridCommandParameter
        {
            get { return (Object)GetValue(GridCommandParameterProperty); }
            set { SetValue(GridCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty GridCommandParameterProperty =
            DependencyProperty.Register("GridCommandParameter", typeof(Object), typeof(PlotControl), new PropertyMetadata(null));

        public ICommand CenterCommand
        {
            get { return (ICommand)GetValue(CenterCommandProperty); }
            set { SetValue(CenterCommandProperty, value); }
        }

        public static readonly DependencyProperty CenterCommandProperty =
            DependencyProperty.Register("CenterCommand", typeof(ICommand), typeof(PlotControl), new PropertyMetadata(null));

        public Object CenterCommandParameter
        {
            get { return (Object)GetValue(CenterCommandParameterProperty); }
            set { SetValue(CenterCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CenterCommandParameterProperty =
            DependencyProperty.Register("CenterCommandParameter", typeof(Object), typeof(PlotControl), new PropertyMetadata(null));

        public ICommand SettingsCommand
        {
            get { return (ICommand)GetValue(SettingsCommandProperty); }
            set { SetValue(SettingsCommandProperty, value); }
        }

        public static readonly DependencyProperty SettingsCommandProperty =
            DependencyProperty.Register("SettingsCommand", typeof(ICommand), typeof(PlotControl), new PropertyMetadata(null));

        public Object SettingsCommandParameter
        {
            get { return (Object)GetValue(SettingsCommandParameterProperty); }
            set { SetValue(SettingsCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty SettingsCommandParameterProperty =
            DependencyProperty.Register("SettingsCommandParameter", typeof(Object), typeof(PlotControl), new PropertyMetadata(null));

        public ICommand SeriesCommand
        {
            get { return (ICommand)GetValue(SeriesCommandProperty); }
            set { SetValue(SeriesCommandProperty, value); }
        }

        public static readonly DependencyProperty SeriesCommandProperty =
            DependencyProperty.Register("SeriesCommand", typeof(ICommand), typeof(PlotControl), new PropertyMetadata(null));

        public Object SeriesCommandParameter
        {
            get { return (Object)GetValue(SeriesCommandParameterProperty); }
            set { SetValue(SeriesCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty SeriesCommandParameterProperty =
            DependencyProperty.Register("SeriesCommandParameter", typeof(Object), typeof(PlotControl), new PropertyMetadata(null));

        public ICommand ConsoleCommand
        {
            get { return (ICommand)GetValue(ConsoleCommandProperty); }
            set { SetValue(ConsoleCommandProperty, value); }
        }

        public static readonly DependencyProperty ConsoleCommandProperty =
            DependencyProperty.Register("ConsoleCommand", typeof(ICommand), typeof(PlotControl), new PropertyMetadata(null));

        public Object ConsoleCommandParameter
        {
            get { return (Object)GetValue(ConsoleCommandParameterProperty); }
            set { SetValue(ConsoleCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ConsoleCommandParameterProperty =
            DependencyProperty.Register("ConsoleCommandParameter", typeof(Object), typeof(PlotControl), new PropertyMetadata(null));

        public ICommand DuplicateCommand
        {
            get { return (ICommand)GetValue(DuplicateCommandProperty); }
            set { SetValue(DuplicateCommandProperty, value); }
        }

        public static readonly DependencyProperty DuplicateCommandProperty =
            DependencyProperty.Register("DuplicateCommand", typeof(ICommand), typeof(PlotControl), new PropertyMetadata(null));

        public Object DuplicateCommandParameter
        {
            get { return (Object)GetValue(DuplicateCommandParameterProperty); }
            set { SetValue(DuplicateCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty DuplicateCommandParameterProperty =
            DependencyProperty.Register("DuplicateCommandParameter", typeof(Object), typeof(PlotControl), new PropertyMetadata(null));

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(PlotControl), new PropertyMetadata(null));

        public Object SaveCommandParameter
        {
            get { return (Object)GetValue(SaveCommandParameterProperty); }
            set { SetValue(SaveCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandParameterProperty =
            DependencyProperty.Register("SaveCommandParameter", typeof(Object), typeof(PlotControl), new PropertyMetadata(null));

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
                control.DockImg.Source = new BitmapImage(new Uri(@"Images\undock.png", UriKind.Relative));
                control.ConsoleButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                control.DockImg.Source = new BitmapImage(new Uri(@"Images\dock.png", UriKind.Relative));
                control.ConsoleButton.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #endregion

        #region Printing Methods

        void PrintButtonClick(Object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();
            var result = printDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var canvas = new Canvas
                {
                    Width = printDialog.PrintableAreaWidth,
                    Height = printDialog.PrintableAreaHeight
                };

                canvas.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                canvas.Arrange(new Rect(0, 0, printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                //_plot.RenderToCanvas(canvas);
                canvas.UpdateLayout();
                //printDialog.PrintVisual(canvas, _plot.Plot.Title ?? "Sumerics Plot");
            }
        }

        #endregion

        #region Methods

        void GridButtonClick(Object sender, RoutedEventArgs e)
		{
            var command = GridCommand;
            var parameter = GridCommandParameter;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
		}

        void CenterButtonClick(Object sender, RoutedEventArgs e)
        {
            var command = CenterCommand;
            var parameter = CenterCommandParameter;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void SettingsButtonClick(Object sender, RoutedEventArgs e)
        {
            var command = SettingsCommand;
            var parameter = SettingsCommandParameter;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void SeriesButtonClick(Object sender, RoutedEventArgs e)
        {
            var command = SeriesCommand;
            var parameter = SeriesCommandParameter;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void ConsoleButtonClick(Object sender, RoutedEventArgs e)
        {
            var command = ConsoleCommand;
            var parameter = ConsoleCommandParameter;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void DuplicateButtonClick(Object sender, RoutedEventArgs e)
        {
            var command = DuplicateCommand;
            var parameter = DuplicateCommandParameter;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void SaveButtonClick(Object sender, RoutedEventArgs e)
        {
            var command = SaveCommand;
            var parameter = SaveCommandParameter;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

		#endregion
	}
}
