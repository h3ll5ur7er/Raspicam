using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace MVVMBase.Interactivity
{
    public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), null);
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
        private string commandName;
        
        public string CommandName
        {
            get
            {
                ReadPreamble();
                return commandName;
            }
            set
            {
                if (CommandName == value)
                    return;
                WritePreamble();
                commandName = value;
                WritePostscript();
            }
        }
        
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }
        
        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        protected override void Invoke(object parameter)
        {
            if (AssociatedObject == null)
                return;
            var command = ResolveCommand();
            if (command == null || !command.CanExecute(CommandParameter))
                return;
            command.Execute(CommandParameter);
        }

        private ICommand ResolveCommand()
        {
            ICommand command = null;
            if (Command != null)
                command = Command;
            else if (AssociatedObject != null)
            {
                foreach (var propertyInfo in AssociatedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, CommandName, StringComparison.Ordinal))
                        command = (ICommand)propertyInfo.GetValue(AssociatedObject, null);
                }
            }
            return command;
        }
    }
}