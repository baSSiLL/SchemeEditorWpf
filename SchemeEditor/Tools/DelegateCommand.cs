using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics.Contracts;
using System.Windows;

namespace SchemeEditor.Tools
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            Contract.Requires(execute != null);

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
            : this(execute != null ? new Action<object>(p => execute.Invoke()) : null,
                   canExecute != null ? new Func<object, bool>(p => canExecute.Invoke()) : null)
        {
            Contract.Requires(execute != null);
        }


        public void InvalidateCanExecute()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(CommandManager.InvalidateRequerySuggested));
            }
        }


        #region ICommand members

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute.Invoke(parameter);
            }
        }

        #endregion
    }
}
