using System;
using System.Windows.Input;

namespace WpfTestStyling
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecuteFunc;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action executeAction)
        {
            _executeAction = executeAction;
        }
        public DelegateCommand(Action executeAction, Func<bool> canExecuteFunc)
        {
            _executeAction = executeAction;
            _canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _executeAction?.Invoke();
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeAction;
        private readonly Func<T, bool> _canExecuteFunc;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<T> executeAction)
        {
            _executeAction = executeAction;
        }
        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecuteFunc)
        {
            _executeAction = executeAction;
            _canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            return TryCast(parameter, out T tParam) && (_canExecuteFunc?.Invoke(tParam) ?? true);
        }

        public void Execute(object parameter)
        {
            if (TryCast(parameter, out T tParam))
                _executeAction?.Invoke(tParam);
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        private bool TryCast(object obj, out T value)
        {
            if (obj is T tparam)
            {
                value = tparam;
                return true;
            }
            value = default(T);
            return obj is null && (typeof(T).IsClass || typeof(T).IsInterface);
        }
    }
}
