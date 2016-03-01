namespace Sumerics.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    sealed class ConsoleEnterViewModel : BaseViewModel
    {
        readonly ICommand _evaluate;
        String _input;

        public ConsoleEnterViewModel(IConsole console)
        {
            _input = String.Empty;
            _evaluate = new RelayCommand(obj =>
            {
                var window = obj as Window;

                if (!String.IsNullOrEmpty(_input))
                {
                    var query = _input;
                    Input = String.Empty;
                    console.Execute(query);
                }

                if (window != null)
                {
                    window.Close();
                }
            });
        }

        public ICommand Evaluate
        {
            get { return _evaluate; }
        }

        public String Input
        {
            get { return _input; }
            set { _input = value; RaisePropertyChanged(); }
        }
    }
}
