using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kkbac.Tools.Extensions.IEnumerables
{
    public static class IEnumerables
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            return source.Any();
        }
    }
}
