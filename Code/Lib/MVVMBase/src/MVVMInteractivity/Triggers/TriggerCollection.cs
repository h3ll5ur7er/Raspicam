using System.Windows;

namespace MVVMBase.Interactivity
{
    public sealed class TriggerCollection : AttachableCollection<TriggerBase>
    {
        internal TriggerCollection()
        {
        }

        protected override void OnAttached()
        {
            foreach (var triggerBase in this)
                triggerBase.Attach(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            foreach (var triggerBase in this)
                triggerBase.Detach();
        }

        protected override void ItemAdded(TriggerBase item)
        {
            if (AssociatedObject == null)
                return;
            item.Attach(AssociatedObject);
        }

        protected override void ItemRemoved(TriggerBase item)
        {
            if (((IAttachedObject)item).AssociatedObject == null)
                return;
            item.Detach();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TriggerCollection();
        }
    }
}