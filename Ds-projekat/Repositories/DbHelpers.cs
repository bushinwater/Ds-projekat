using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat.Repositories
{
    internal static class DbHelpers
    {
        public static bool ReadBool(object value)
        {
            if (value is bool b) return b;
            if (value is byte bt) return bt != 0;   // MySQL često vraća tinyint
            if (value is sbyte sb) return sb != 0;
            if (value is int i) return i != 0;
            return Convert.ToBoolean(value);
        }

        public static int? ReadNullableInt(object value)
        {
            if (value == DBNull.Value) return null;
            return Convert.ToInt32(value);
        }

        public static string ReadNullableString(object value)
        {
            if (value == DBNull.Value) return null;
            return value.ToString();
        }
    }
}
