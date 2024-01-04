using System;
using System.Windows.Input;

namespace MMIM.Common
{
    internal class Command : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        private bool _lastValue;

        internal Command(Func<bool> canExecute, Action execute )
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            var canExecute = _canExecute();
            if (canExecute != _lastValue)
            {
                _lastValue = canExecute;
                if (CanExecuteChanged != null) CanExecuteChanged.Invoke(null, null);
            }
            return canExecute;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter)) throw new InvalidOperationException("CanExecute check failed");
            _execute();
        }

        public event EventHandler CanExecuteChanged;
    }
}
