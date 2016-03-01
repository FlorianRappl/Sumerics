namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Controls;
    using Sumerics.MathInput;
    using Sumerics.Resources;
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : MetroWindow
    {
        #region Fields

        readonly MathInputPanelWrapper _panel;
        readonly IMathInputService _service;

        #endregion

        #region ctor

        public InputDialog(IMathInputService service)
        {
            InitializeComponent();
            UserInput = String.Empty;
            _panel = new MathInputPanelWrapper(Messages.DrawInput);
            _service = service;

            if (_panel.IsAvailable)
            {
                _panel.OnInsertPressed += QueryInsertPressed;
            }
            else
            {
                DrawInput.Visibility = System.Windows.Visibility.Collapsed;
            }

            Input.Focus();
            Closing += IsClosing;
        }

        #endregion

        #region Properties

        public String UserInput
        {
            get;
            set;
        }

        public String UserMessage
        {
            get { return Message.Text; }
            set { Message.Text = value; }
        }

        #endregion

        #region Methods

        void QueryInsertPressed(Object sender, String query)
        {
            Input.Text = _service.ConvertToYamp(query);
        }

        public static String Show(IMathInputService service, String message)
        {
            var inp = new InputDialog(service);
            inp.UserMessage = String.IsNullOrEmpty(message) ? Messages.InputRequestedLabel : message;
            inp.ShowDialog();
            return inp.UserInput;
        }

        void CloseClick(Object sender, RoutedEventArgs e)
        {
            UserInput = Input.Text;
            Close();
        }

        void DrawClick(Object sender, RoutedEventArgs e)
        {
            _panel.Open();
        }

        void IsClosing(Object sender, CancelEventArgs e)
        {
            _panel.Close();
        }

        void TextBoxEnter(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                CloseClick(sender, e);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        #endregion
    }
}
