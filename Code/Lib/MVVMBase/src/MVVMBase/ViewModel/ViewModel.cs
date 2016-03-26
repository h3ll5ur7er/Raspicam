using System.ComponentModel;
using System.Runtime.CompilerServices;
using MVVMBase.Annotations;

namespace MVVMBase.VM
{
    public abstract class ViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public abstract string[] PropertyNames { get; }
        public IViewModel Parent { get; protected set; }

        protected ViewModel(ViewModel parent)
        {
            Parent = parent;
            SetupEvents();
            SetupHandlers();
        }
        protected ViewModel() : this(null)
        {
        }

        protected abstract void SetupEvents();
        protected abstract void SetupHandlers();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}