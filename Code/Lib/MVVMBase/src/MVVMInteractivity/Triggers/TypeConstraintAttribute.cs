using System;

namespace MVVMBase.Interactivity
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TypeConstraintAttribute : Attribute
    {
        public Type Constraint { get; private set; }
        
        public TypeConstraintAttribute(Type constraint)
        {
            Constraint = constraint;
        }
    }
}