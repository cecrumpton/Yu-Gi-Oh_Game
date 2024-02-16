using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Yu_Gi_Oh_Game.Other
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        //private readonly bool? _boolCanExecute;

        public RelayCommand(Action onExecuteMethod, Func<bool>? onCanExecuteMethod = null)
        {
            _execute = onExecuteMethod;
            _canExecute = onCanExecuteMethod;
        }

        //public RelayCommand(Action onExecuteMethod, bool onCanExecuteMethod)
        //{
        //    _execute = onExecuteMethod;
        //    _boolCanExecute = onCanExecuteMethod;
        //}

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            //if (_boolCanExecute != null)
            //    return (bool)_boolCanExecute;
            
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }

        #endregion
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<bool>? _canExecute;
        //private readonly bool? _boolCanExecute;

        public RelayCommand(Action<T> onExecuteMethod, Func<bool>? onCanExecuteMethod = null)
        {
            _execute = onExecuteMethod;
            _canExecute = onCanExecuteMethod;
        }

        //public RelayCommand(Action<T> onExecuteMethod, bool onCanExecuteMethod)
        //{
        //    _execute = onExecuteMethod;
        //    _boolCanExecute = onCanExecuteMethod;
        //}

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            //if (_boolCanExecute != null)
            //    return (bool)_boolCanExecute;

            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute((T) parameter);
        }

        #endregion
    }

    //public class RelayCommand : ICommand
    //{
    //    public delegate void ICommandOnExecute(object parameter);
    //    public delegate bool ICommandOnCanExecute(object parameter);

    //    private ICommandOnExecute _execute;
    //    private ICommandOnCanExecute _canExecute;

    //    public RelayCommand(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod)
    //    {
    //        _execute = onExecuteMethod;
    //        _canExecute = onCanExecuteMethod;
    //    }

    //    #region ICommand Members

    //    public event EventHandler CanExecuteChanged
    //    {
    //        add { CommandManager.RequerySuggested += value; }
    //        remove { CommandManager.RequerySuggested -= value; }
    //    }

    //    public bool CanExecute(object parameter)
    //    {
    //        return _canExecute.Invoke(parameter);
    //    }

    //    public void Execute(object parameter)
    //    {
    //        _execute.Invoke(parameter);
    //    }

    //    #endregion
    //}
}
