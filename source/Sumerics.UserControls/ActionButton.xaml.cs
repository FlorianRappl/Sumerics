namespace Sumerics.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ActionButton.xaml
    /// </summary>
    public partial class ActionButton : UserControl
    {
        #region Fields

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ActionButton), new PropertyMetadata(null, OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(Object), typeof(ActionButton), new PropertyMetadata(null, OnCommandParameterChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(String), typeof(ActionButton), new PropertyMetadata(null, OnTextChanged));

        #endregion

        #region ctor

        public ActionButton()
        {
            InitializeComponent();
            TBlock.MouseDown += TextBlockMouseDown;
        }

        #endregion

        #region Properties

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region Methods

        static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var that = (ActionButton)d;
            that.CButton.Command = (ICommand)e.NewValue;
        }

        static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var that = (ActionButton)d;
            that.CButton.CommandParameter = e.NewValue;
        }

        static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var that = (ActionButton)d;
            that.TBlock.Text = (String)e.NewValue;
        }

        void TextBlockMouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }

        #endregion
    }
}
