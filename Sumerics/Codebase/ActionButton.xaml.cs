using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sumerics
{
    /// <summary>
    /// Interaction logic for ActionButton.xaml
    /// </summary>
    public partial class ActionButton : UserControl
    {
        #region Fields

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ActionButton), new PropertyMetadata(null, OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ActionButton), new PropertyMetadata(null, OnCommandParameterChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ActionButton), new PropertyMetadata(null, OnTextChanged));

        #endregion

        #region ctor

        public ActionButton()
        {
            InitializeComponent();
            TBlock.MouseDown += TextBlockMouseDown;
        }

        #endregion

        #region Properties

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
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
            that.TBlock.Text = (string)e.NewValue;
        }

        void TextBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Command != null)
                Command.Execute(CommandParameter);
        }

        #endregion
    }
}
