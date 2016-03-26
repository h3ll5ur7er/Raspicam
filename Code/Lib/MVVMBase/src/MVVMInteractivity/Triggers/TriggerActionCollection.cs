using System;
using System.Windows;

namespace MVVMBase.Interactivity
{
    public class TriggerActionCollection : AttachableCollection<TriggerAction>
    {
        internal TriggerActionCollection()
        {
        }

        protected override void OnAttached()
        {
            foreach (var triggerAction in this)
                triggerAction.Attach(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            foreach (var triggerAction in this)
                triggerAction.Detach();
        }

        protected override void ItemAdded(TriggerAction item)
        {
            if (item.IsHosted)
                throw new InvalidOperationException("CannotHostTriggerActionMultipleTimesExceptionMessage");
            if (AssociatedObject != null)
                item.Attach(AssociatedObject);
            item.IsHosted = true;
        }

        protected override void ItemRemoved(TriggerAction item)
        {
            if (((IAttachedObject)item).AssociatedObject != null)
                item.Detach();
            item.IsHosted = false;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TriggerActionCollection();
        }
    }
}