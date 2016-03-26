using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Animation;

namespace MVVMBase.Interactivity
{
    public abstract class Behavior : Animatable, IAttachedObject
    {
        private readonly Type associatedType;
        private DependencyObject associatedObject;
            
        protected Type AssociatedType
        {
            get
            {
                ReadPreamble();
                return associatedType;
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

        DependencyObject IAttachedObject.AssociatedObject => AssociatedObject;

        internal event EventHandler AssociatedObjectChanged;

        internal Behavior(Type associatedType)
        {
            this.associatedType = associatedType;
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

        private void OnAssociatedObjectChanged()
        {
            AssociatedObjectChanged?.Invoke(this, new EventArgs());
        }
            
        public void Attach(DependencyObject dependencyObject)
        {
            if (Equals(dependencyObject, AssociatedObject))
                return;
            if (AssociatedObject != null)
                throw new InvalidOperationException("CannotHostBehaviorMultipleTimesExceptionMessage");
            if (dependencyObject != null && !AssociatedType.IsInstanceOfType(dependencyObject))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "TypeConstraintViolatedExceptionMessage"+" "+(object)GetType().Name + " " + (object)dependencyObject.GetType().Name + " " + (object)AssociatedType.Name));
            WritePreamble();
            associatedObject = dependencyObject;
            WritePostscript();
            OnAssociatedObjectChanged();
            OnAttached();
        }
            
        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            associatedObject = null;
            WritePostscript();
            OnAssociatedObjectChanged();
        }
    }
}