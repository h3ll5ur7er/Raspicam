using System.Windows;

namespace MVVMBase.Interactivity
{
    public interface IAttachedObject
    {
        DependencyObject AssociatedObject { get; }
        
        void Attach(DependencyObject dependencyObject);
        
        void Detach();
    }
}