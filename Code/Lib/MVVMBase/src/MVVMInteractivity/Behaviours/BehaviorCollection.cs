using System.Windows;

namespace MVVMBase.Interactivity
{
    public sealed class BehaviorCollection : AttachableCollection<Behavior>
    {
        internal BehaviorCollection()
        {
        }
        
        protected override void OnAttached()
        {
            foreach (var behavior in this)
                behavior.Attach(AssociatedObject);
        }
        
        protected override void OnDetaching()
        {
            foreach (var behavior in this)
                behavior.Detach();
        }

        protected override void ItemAdded(Behavior item)
        {
            if (AssociatedObject == null)
                return;
            item.Attach(AssociatedObject);
        }

        protected override void ItemRemoved(Behavior item)
        {
            if (((IAttachedObject)item).AssociatedObject == null)
                return;
            item.Detach();
        }
        
        protected override Freezable CreateInstanceCore()
        {
            return new BehaviorCollection();
        }
    }
}