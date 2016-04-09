using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tol.Common
{
    public static class EnumHelper
    {
        public static T IsEnum<T>(int id) where T : struct
        {
            foreach (FieldInfo value in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if ((int)value.GetRawConstantValue() == id)
                {
                    return (T)value.GetValue(value);
                }
            }
            return default(T);
        }
    }
}
