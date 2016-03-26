using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

namespace MVVMBase.Interactivity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    //[CLSCompliant(false)]
    public sealed class DefaultTriggerAttribute : Attribute
    {
        private readonly object[] parameters;
        
        public Type TargetType { get; }

        public Type TriggerType { get; }

        public IEnumerable Parameters => parameters;

        public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter) : this(targetType, triggerType, new []{parameter})
        {
        }

        public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
        {
            if (!typeof(TriggerBase).IsAssignableFrom(triggerType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage" + " " + (object) triggerType.Name));
            TargetType = targetType;
            TriggerType = triggerType;
            this.parameters = parameters;
        }
        
        public TriggerBase Instantiate()
        {
            object obj = null;
            try
            {
                obj = Activator.CreateInstance(TriggerType, parameters);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return (TriggerBase)obj;
        }
    }
}