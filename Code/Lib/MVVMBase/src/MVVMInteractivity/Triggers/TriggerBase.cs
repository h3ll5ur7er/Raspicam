using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace MVVMBase.Interactivity
{
    [ContentProperty("Actions")]
    public abstract class TriggerBase : Animatable, IAttachedObject
    {
        private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly("Actions", typeof(TriggerActionCollection), typeof(TriggerBase), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ActionsProperty = ActionsPropertyKey.DependencyProperty;
        private DependencyObject associatedObject;
        private readonly Type associatedObjectTypeConstraint;

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

        public TriggerActionCollection Actions => (TriggerActionCollection)GetValue(ActionsProperty);

        DependencyObject IAttachedObject.AssociatedObject => AssociatedObject;

        public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

        internal TriggerBase(Type associatedObjectTypeConstraint)
        {
            this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
            var actionCollection = new TriggerActionCollection();
            SetValue(ActionsPropertyKey, actionCollection);
        }

        protected void InvokeActions(object parameter)
        {
            if (PreviewInvoke != null)
            {
                var e = new PreviewInvokeEventArgs();
                PreviewInvoke(this, e);
                if (e.Cancelling)
                    return;
            }
            foreach (var triggerAction in Actions)
                triggerAction.CallInvoke(parameter);
        }

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
                throw new InvalidOperationException("CannotHostTriggerMultipleTimesExceptionMessage");
            if (dependencyObject != null && !AssociatedObjectTypeConstraint.IsInstanceOfType(dependencyObject))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "TypeConstraintViolatedExceptionMessage"+" "+(object)GetType().Name+" "+ (object)dependencyObject.GetType().Name+" "+ (object)AssociatedObjectTypeConstraint.Name));
            WritePreamble();
            associatedObject = dependencyObject;
            WritePostscript();
            Actions.Attach(dependencyObject);
            OnAttached();
        }

        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            associatedObject = null;
            WritePostscript();
            Actions.Detach();
        }
    }
}