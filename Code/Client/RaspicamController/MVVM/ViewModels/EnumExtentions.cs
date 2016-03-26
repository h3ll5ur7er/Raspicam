using System;
using System.Collections.Generic;
using System.Linq;

namespace RaspicamController.MVVM.ViewModels
{
    public static class EnumExtentions
    {
        public static IEnumerable<T> GetAllMembers<T>(this Type t) where T : struct, IConvertible
        {
            if (t.IsEnum)
            {
                return Enum.GetValues(t).Cast<T>();
            }
            return default(IEnumerable<T>);
        }
    }
}