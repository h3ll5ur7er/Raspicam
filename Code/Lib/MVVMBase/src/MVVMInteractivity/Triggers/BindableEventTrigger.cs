using System.Windows;

namespace MVVMBase.Interactivity
{
    public class BindableEventTrigger : EventTriggerBase<object>
    {
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(BindableEventTrigger), new FrameworkPropertyMetadata("Loaded", OnEventNameChanged));
        
        public string EventName
        {
            get
            {
                return (string)GetValue(EventNameProperty);
            }
            set
            {
                SetValue(EventNameProperty, value);
            }
        }
        
        public BindableEventTrigger()
        {
        }
        
        public BindableEventTrigger(string eventName)
        {
            EventName = eventName;
        }

        protected override string GetEventName()
        {
            return EventName;
        }

        private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((EventTriggerBase)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
        }
    }
}