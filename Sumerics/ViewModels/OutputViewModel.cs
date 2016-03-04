namespace Sumerics.ViewModels
{
    using System;

    sealed class OutputViewModel :  BaseViewModel
    {
        private String _title;
        private String _message;

        public String Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        public String Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }
    }
}
