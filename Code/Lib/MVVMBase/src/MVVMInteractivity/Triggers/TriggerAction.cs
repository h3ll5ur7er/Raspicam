using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace MVVMBase.Interactivity
{
    [DefaultTrigger(typeof(UIElement), typeof(BindableEventTrigger), "MouseLeftButtonDown")]
    [DefaultTrigger(typeof(ButtonBase), typeof(BindableEventTrigger), "Click")]
    public abstract class TriggerAction : Animatable, IAttachedObject
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(TriggerAction), new FrameworkPropertyMetadata(true));
        private bool isHosted;
        private DependencyObject associatedObject;
        private readonly Type associatedObjectTypeConstraint;
        
        public bool IsEnabled
        {
            get
            {
                return (bool)GetValue(IsEnabledProperty);
            }
            set
            {
                SetValue(IsEnabledProperty, value);
            }
        }
        
        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return associatedObject;
            }
        }
        
        protected virtual Type AssociatedObjectTypeConstraint
        {
            get
            {
                ReadPreamble();
                return associatedObjectTypeConstraint;
            }
        }
        
        internal bool IsHosted
        {
            get
            {
                ReadPreamble();
                return isHosted;
            }
            set
            {
                WritePreamble();
                isHosted = value;
                WritePostscript();
            }
        }

        DependencyObject IAttachedObject.AssociatedObject => AssociatedObject;

        internal TriggerAction(Type associatedObjectTypeConstraint)
        {
            this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
        }

        internal void CallInvoke(object parameter)
        {
            if (!IsEnabled)
                return;
            Invoke(parameter);
        }

        protected abstract void Invoke(object parameter);

        protected virtual void OnAttached()
        {
        }

        protected virtual void OnDetaching()
        {
        }

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(GetType());
        }

        public void Attach(DependencyObject dependencyObject)
        {
            if (Equals(dependencyObject, AssociatedObject))
                return;
            if (AssociatedObject != null)
                throw new InvalidOperationException("CannotHostTriggerActionMultipleTimesExceptionMessage");
            if (dependencyObject != null && !AssociatedObjectTypeConstraint.IsInstanceOfType(dependencyObject))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "TypeConstraintViolatedExceptionMessage"+" "+ (object)GetType().Name+" "+(object)dependencyObject.GetType().Name+" "+(object)AssociatedObjectTypeConstraint.Name));
            WritePreamble();
            associatedObject = dependencyObject;
            WritePostscript();
            OnAttached();
        }

        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            associatedObject = null;
            WritePostscript();
        }
    }

    public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
    {
        protected new T AssociatedObject => (T)base.AssociatedObject;

        protected sealed override Type AssociatedObjectTypeConstraint => base.AssociatedObjectTypeConstraint;

        protected TriggerAction()
            : base(typeof(T))
        {
        }
    }
}