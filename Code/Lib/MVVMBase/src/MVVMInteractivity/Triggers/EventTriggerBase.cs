using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace MVVMBase.Interactivity
{
    public abstract class EventTriggerBase : TriggerBase
    {
        public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(object), typeof(EventTriggerBase), new PropertyMetadata(OnSourceObjectChanged));
        public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName", typeof(string), typeof(EventTriggerBase), new PropertyMetadata(OnSourceNameChanged));
        private MethodInfo eventHandlerMethodInfo;
        
        protected sealed override Type AssociatedObjectTypeConstraint
        {
            get
            {
                var constraintAttribute = TypeDescriptor.GetAttributes(GetType())[typeof(TypeConstraintAttribute)] as TypeConstraintAttribute;
                if (constraintAttribute != null)
                    return constraintAttribute.Constraint;
                return typeof(DependencyObject);
            }
        }
        
        protected Type SourceTypeConstraint { get; }

        public object SourceObject
        {
            get
            {
                return GetValue(SourceObjectProperty);
            }
            set
            {
                SetValue(SourceObjectProperty, value);
            }
        }
        
        public string SourceName
        {
            get
            {
                return (string)GetValue(SourceNameProperty);
            }
            set
            {
                SetValue(SourceNameProperty, value);
            }
        }

        public object Source
        {
            get
            {
                object obj = AssociatedObject;
                if (SourceObject != null)
                    obj = SourceObject;
                else if (IsSourceNameSet)
                {
                    obj = SourceNameResolver.Object;
                    if (obj != null && !SourceTypeConstraint.IsInstanceOfType(obj))
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "RetargetedTypeConstraintViolatedExceptionMessage"+" "+ (object)GetType().Name+" "+obj.GetType()+" "+SourceTypeConstraint+" "+ (object)"Source"));
                }
                return obj;
            }
        }

        private NameResolver SourceNameResolver { get; }

        private bool IsSourceChangedRegistered { get; set; }

        private bool IsSourceNameSet
        {
            get
            {
                if (string.IsNullOrEmpty(SourceName))
                    return ReadLocalValue(SourceNameProperty) != DependencyProperty.UnsetValue;
                return true;
            }
        }

        private bool IsLoadedRegistered { get; set; }

        internal EventTriggerBase(Type sourceTypeConstraint)
            : base(typeof(DependencyObject))
        {
            SourceTypeConstraint = sourceTypeConstraint;
            SourceNameResolver = new NameResolver();
            RegisterSourceChanged();
        }
        
        protected abstract string GetEventName();
        
        protected virtual void OnEvent(EventArgs eventArgs)
        {
            InvokeActions(eventArgs);
        }

        private void OnSourceChanged(object oldSource, object newSource)
        {
            if (AssociatedObject == null)
                return;
            OnSourceChangedImpl(oldSource, newSource);
        }
        
        internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
        {
            if (string.IsNullOrEmpty(GetEventName()) || string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0)
                return;
            if (oldSource != null && SourceTypeConstraint.IsInstanceOfType(oldSource))
                UnregisterEvent(oldSource, GetEventName());
            if (newSource == null || !SourceTypeConstraint.IsInstanceOfType(newSource))
                return;
            RegisterEvent(newSource, GetEventName());
        }
        
        protected override void OnAttached()
        {
            base.OnAttached();
            var associatedObject1 = AssociatedObject;
            var behavior = associatedObject1 as Behavior;
            var frameworkElement = associatedObject1 as FrameworkElement;
            RegisterSourceChanged();
            if (behavior != null)
            {
                //var associatedObject2 = ((IAttachedObject)behavior).AssociatedObject;
                behavior.AssociatedObjectChanged += OnBehaviorHostChanged;
            }
            else
            {
                if (SourceObject == null)
                {
                    if (frameworkElement != null)
                    {
                        SourceNameResolver.NameScopeReferenceElement = frameworkElement;
                        goto label_7;
                    }
                }
                try
                {
                    OnSourceChanged(null, Source);
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            label_7:
            if (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) != 0 || frameworkElement == null || Interaction.IsElementLoaded(frameworkElement))
                return;
            RegisterLoaded(frameworkElement);
        }
        
        protected override void OnDetaching()
        {
            base.OnDetaching();
            var behavior = AssociatedObject as Behavior;
            var associatedElement = AssociatedObject as FrameworkElement;
            try
            {
                OnSourceChanged(Source, null);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
            }
            UnregisterSourceChanged();
            if (behavior != null)
                behavior.AssociatedObjectChanged -= OnBehaviorHostChanged;
            SourceNameResolver.NameScopeReferenceElement = null;
            if (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) != 0 || associatedElement == null)
                return;
            UnregisterLoaded(associatedElement);
        }

        private void OnBehaviorHostChanged(object sender, EventArgs e)
        {
            SourceNameResolver.NameScopeReferenceElement = ((IAttachedObject)sender).AssociatedObject as FrameworkElement;
        }

        private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var eventTriggerBase = (EventTriggerBase)obj;
            object newSource = eventTriggerBase.SourceNameResolver.Object;
            if (args.NewValue == null)
            {
                eventTriggerBase.OnSourceChanged(args.OldValue, newSource);
            }
            else
            {
                if (args.OldValue == null && newSource != null)
                    eventTriggerBase.UnregisterEvent(newSource, eventTriggerBase.GetEventName());
                eventTriggerBase.OnSourceChanged(args.OldValue, args.NewValue);
            }
        }

        private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((EventTriggerBase)obj).SourceNameResolver.Name = (string)args.NewValue;
        }

        private void RegisterSourceChanged()
        {
            if (IsSourceChangedRegistered)
                return;
            SourceNameResolver.ResolvedElementChanged += OnSourceNameResolverElementChanged;
            IsSourceChangedRegistered = true;
        }

        private void UnregisterSourceChanged()
        {
            if (!IsSourceChangedRegistered)
                return;
            SourceNameResolver.ResolvedElementChanged -= OnSourceNameResolverElementChanged;
            IsSourceChangedRegistered = false;
        }

        private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
        {
            if (SourceObject != null)
                return;
            OnSourceChanged(e.OldObject, e.NewObject);
        }

        private void RegisterLoaded(FrameworkElement associatedElement)
        {
            if (IsLoadedRegistered || associatedElement == null)
                return;
            associatedElement.Loaded += OnEventImpl;
            IsLoadedRegistered = true;
        }

        private void UnregisterLoaded(FrameworkElement associatedElement)
        {
            if (!IsLoadedRegistered || associatedElement == null)
                return;
            associatedElement.Loaded -= OnEventImpl;
            IsLoadedRegistered = false;
        }

        private void RegisterEvent(object obj, string eventName)
        {
            var @event = obj.GetType().GetEvent(eventName);
            if (@event == null)
            {
                if (SourceObject != null)
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "EventTriggerCannotFindEventNameExceptionMessage"+" "+(object) eventName+" "+(object) obj.GetType().Name));
            }
            else if (!IsValidEvent(@event))
            {
                if (SourceObject != null)
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "EventTriggerBaseInvalidEventExceptionMessage"+" "+ (object) eventName +" "+(object) obj.GetType().Name));
            }
            else
            {
                eventHandlerMethodInfo = typeof(EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.Instance | BindingFlags.NonPublic);
                @event.AddEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, eventHandlerMethodInfo));
            }
        }

        private static bool IsValidEvent(EventInfo eventInfo)
        {
            var eventHandlerType = eventInfo.EventHandlerType;
            if (!typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
                return false;
            var parameters = eventHandlerType.GetMethod("Invoke").GetParameters();
            if (parameters.Length == 2 && typeof(object).IsAssignableFrom(parameters[0].ParameterType))
                return typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
            return false;
        }

        private void UnregisterEvent(object obj, string eventName)
        {
            if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) == 0)
            {
                var associatedElement = obj as FrameworkElement;
                if (associatedElement == null)
                    return;
                UnregisterLoaded(associatedElement);
            }
            else
                UnregisterEventImpl(obj, eventName);
        }

        private void UnregisterEventImpl(object obj, string eventName)
        {
            var type = obj.GetType();
            if (eventHandlerMethodInfo == null)
                return;
            var @event = type.GetEvent(eventName);
            @event.RemoveEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, eventHandlerMethodInfo));
            eventHandlerMethodInfo = null;
        }

        private void OnEventImpl(object sender, EventArgs eventArgs)
        {
            OnEvent(eventArgs);
        }

        internal void OnEventNameChanged(string oldEventName, string newEventName)
        {
            if (AssociatedObject == null)
                return;
            var associatedElement = Source as FrameworkElement;
            if (associatedElement != null && string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0)
                UnregisterLoaded(associatedElement);
            else if (!string.IsNullOrEmpty(oldEventName))
                UnregisterEvent(Source, oldEventName);
            if (associatedElement != null && string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0)
            {
                RegisterLoaded(associatedElement);
            }
            else
            {
                if (string.IsNullOrEmpty(newEventName))
                    return;
                RegisterEvent(Source, newEventName);
            }
        }
    }

    public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
    {
        public new T Source => (T)base.Source;

        protected EventTriggerBase()
            : base(typeof(T))
        {
        }

        internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
        {
            base.OnSourceChangedImpl(oldSource, newSource);
            OnSourceChanged(oldSource as T, newSource as T);
        }

        protected virtual void OnSourceChanged(T oldSource, T newSource)
        {
        }
    }
}