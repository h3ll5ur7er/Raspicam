using System.ComponentModel;

namespace MVVMBase.VM
{
    public interface IViewModel : INotifyPropertyChanged
    {
        IViewModel Parent { get; }

        string[] PropertyNames { get; }
    }
}