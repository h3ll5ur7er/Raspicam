using System;
using System.Windows.Input;

namespace MVVMBase.VM
{
    public class CommandRelay<T1,T2> : ICommand<T1,T2>
    {
        public event EventHandler CanExecuteChanged;
        private bool isEnabled = true;
        private readonly Action<T1, T2> action;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            Execute((T1)parameter, (T2)parameter);
        }

        public CommandRelay(Action<T1,T2> a)
        {
            action = a;
        }

        public void Execute(T1 p1, T2 p2)
        {
            action(p1, p2);
        }
        public static implicit operator CommandRelay<T1, T2>(Action<T1, T2> a) => new CommandRelay<T1, T2>(a) { IsEnabled = true };
        public static implicit operator Action<T1, T2>(CommandRelay<T1, T2> a) => a.action;
    }

    public class CommandRelay<T> : ICommand<T>
    {
        public event EventHandler CanExecuteChanged;
        private bool isEnabled = true;
        private readonly Action<T> action;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        public CommandRelay(Action<T> a)
        {
            action = a;
        }

        public void Execute(T parameter)
        {
            action(parameter);
        }
        public static implicit operator CommandRelay<T>(Action<T> a) => new CommandRelay<T>(a) { IsEnabled = true };
        public static implicit operator Action<T>(CommandRelay<T> a) => a.action;
    }

    public class CommandRelay : ICommand
    {

        public event EventHandler CanExecuteChanged;
        private bool isEnabled = true;
        private readonly Action action;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            action();
        }

        public CommandRelay(Action a)
        {
            action = a;
        }

        public static implicit operator CommandRelay(Action a) => new CommandRelay(a) {IsEnabled = true};
        public static implicit operator Action(CommandRelay a) => a.action;
    }
}