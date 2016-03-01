namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using Sumerics.MathInput;
    using Sumerics.Resources;
    using System;
    using System.Windows;
    using System.Windows.Input;

    sealed class InputViewModel : BaseViewModel
    {
        readonly MathInputPanelWrapper _panel;
        readonly ICommand _drawInput;
        readonly ICommand _cleanup;
        readonly ICommand _confirm;
        String _userInput;
        String _message;
        Boolean _okay;

        public InputViewModel(IMathInputService service)
        {
            _userInput = String.Empty;
            _message = Messages.InputRequestedLabel;
            _panel = new MathInputPanelWrapper(Messages.DrawInput);
            _drawInput = new RelayCommand(_ => _panel.Open());
            _cleanup = new RelayCommand(_ => _panel.Close());
            _confirm = new RelayCommand(obj =>
            {
                var window = obj as Window;
                _okay = true;

                if (window != null)
                {
                    window.Close();
                }
            });

            if (_panel.IsAvailable)
            {
                _panel.OnInsertPressed += (s, e) => UserInput = service.ConvertToYamp(e);
                DrawInputVisibility = Visibility.Visible;
            }
            else
            {
                DrawInputVisibility = Visibility.Hidden;
            }
        }

        public ICommand DrawInput
        {
            get { return _drawInput; }
        }

        public ICommand Cleanup
        {
            get { return _cleanup; }
        }

        public ICommand Confirm
        {
            get { return _confirm; }
        }

        public Visibility DrawInputVisibility
        {
            get;
            private set;
        }

        public String Result
        {
            get { return _okay ? UserInput : String.Empty; }
        }

        public String UserMessage
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }

        public String UserInput
        {
            get { return _userInput; }
            private set { _userInput = value; RaisePropertyChanged(); }
        }
    }
}
