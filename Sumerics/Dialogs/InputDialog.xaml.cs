using MahApps.Metro.Controls;
using Sumerics.Controls;
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
using System.Windows.Shapes;

namespace Sumerics
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : MetroWindow
    {
        #region Members

        string input;
        MathInputPanelWrapper mipw;

        #endregion

        #region ctor

        public InputDialog()
        {
            input = string.Empty;
            InitializeComponent();
            SetupMathInput();
            Input.Focus();
            Closing += IsClosing;
        }

        #endregion

        #region Properties

        public string UserInput
        {
            get { return input; }
            set { input = value; }
        }

        public string UserMessage
        {
            get { return Message.Text; }
            set { Message.Text = value; }
        }

        #endregion

        #region Methods

        void SetupMathInput()
        {
            mipw = new MathInputPanelWrapper("Draw input");

            if (mipw.IsAvailable)
                mipw.OnInsertPressed += QueryInsertPressed;
            else
                DrawInput.Visibility = System.Windows.Visibility.Collapsed;
        }

        void QueryInsertPressed(object sender, string query)
        {
            Input.Text = MathMLParser.Parse(query);
        }

        public static string Show(string message)
        {
            var inp = new InputDialog();
            inp.Message.Text = string.IsNullOrEmpty(message) ? "Your input has been requested:" : message;
            inp.ShowDialog();
            return inp.UserInput;
        }

        void CloseClick(object sender, RoutedEventArgs e)
        {
            input = Input.Text;
            Close();
        }

        void DrawClick(object sender, RoutedEventArgs e)
        {
            mipw.Open();
        }

        void IsClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mipw.Close();
            mipw = null;
        }

        void TextBoxEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                CloseClick(sender, e);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
                Close();
        }

        #endregion
    }
}
