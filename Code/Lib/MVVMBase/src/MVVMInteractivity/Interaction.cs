using System;
using System.Windows;

namespace MVVMBase.Interactivity
{
    public static class Interaction
    {
        private static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("ShadowTriggers", typeof(TriggerCollection), typeof(Interaction), new FrameworkPropertyMetadata(OnTriggersChanged));
        private static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("ShadowBehaviors", typeof(BehaviorCollection), typeof(Interaction), new FrameworkPropertyMetadata(OnBehaviorsChanged));
        internal static bool ShouldRunInDesignMode { get; set; }
        public static TriggerCollection GetTriggers(DependencyObject obj)
        {
            var triggerCollection = (TriggerCollection)obj.GetValue(TriggersProperty);
            if (triggerCollection != null) return triggerCollection;
            triggerCollection = new TriggerCollection();
            obj.SetValue(TriggersProperty, triggerCollection);
            return triggerCollection;
        }
        public static BehaviorCollection GetBehaviors(DependencyObject obj)
        {
            var behaviorCollection = (BehaviorCollection)obj.GetValue(BehaviorsProperty);
            if (behaviorCollection != null) return behaviorCollection;
            behaviorCollection = new BehaviorCollection();
            obj.SetValue(BehaviorsProperty, behaviorCollection);
            return behaviorCollection;
        }
        private static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var behaviorCollection1 = (BehaviorCollection)args.OldValue;
            var behaviorCollection2 = (BehaviorCollection)args.NewValue;
            if (Equals(behaviorCollection1, behaviorCollection2))
                return;
            if (behaviorCollection1?.AssociatedObject != null)
                behaviorCollection1.Detach();
            if (behaviorCollection2 == null || obj == null)
                return;
            if (behaviorCollection2.AssociatedObject != null)
                throw new InvalidOperationException("CannotHostBehaviorCollectionMultipleTimesExceptionMessage");
            behaviorCollection2.Attach(obj);
        }
        private static void OnTriggersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var triggerCollection1 = args.OldValue as TriggerCollection;
            var triggerCollection2 = args.NewValue as TriggerCollection;
            if (Equals(triggerCollection1, triggerCollection2))
                return;
            if (triggerCollection1?.AssociatedObject != null)
                triggerCollection1.Detach();
            if (triggerCollection2 == null || obj == null)
                return;
            if (triggerCollection2.AssociatedObject != null)
                throw new InvalidOperationException("CannotHostTriggerCollectionMultipleTimesExceptionMessage");
            triggerCollection2.Attach(obj);
        }
        internal static bool IsElementLoaded(FrameworkElement element)
        {
            return element.IsLoaded;
        }
    }
}