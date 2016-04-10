namespace Sumerics
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<Object> _execute;
        readonly Predicate<Object> _canExecute;

        #endregion

        #region ctors

        public RelayCommand(Action<Object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<Object> execute, Predicate<Object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Methods

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(Object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
