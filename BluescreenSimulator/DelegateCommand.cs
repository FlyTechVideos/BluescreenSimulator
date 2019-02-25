using System;
using System.Windows.Input;

namespace BluescreenSimulator
{
    public class DelegateCommand : ICommand
    {
        private Func<object, bool> _canExecute = _ => true;
        private Action<object> _execute = _ => { };

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? _execute;
            _canExecute = canExecute ?? _canExecute;
        }
        public DelegateCommand(Action execute, Func<bool> canExecute = null) : this(_ => execute(), _ => canExecute?.Invoke() ?? true) { }

        public virtual bool CanExecute(object parameter) => _canExecute(parameter);

        public virtual void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}