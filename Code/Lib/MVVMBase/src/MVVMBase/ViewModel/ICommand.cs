using System.Windows.Input;

namespace MVVMBase.VM
{
    public interface ICommand<in T> : ICommand
    {
        void Execute(T parameter);
    }
    public interface ICommand<in T1, in T2> : ICommand
    {
        void Execute(T1 p1, T2 p2);
    }
}