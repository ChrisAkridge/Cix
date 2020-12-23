using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cix.Extensions
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            // https://stackoverflow.com/a/1290638/2709212
            int index = 0;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            foreach (T item in source)
            {
                if (comparer.Equals(item, value))
                {
                    return index;
                }

                index++;
            }
            return -1;
        }

        public static int IndexOf<TList, TOther>(this IEnumerable<TList> source, TOther value, Func<TList, TOther, bool> comparer)
        {
            int index = 0;
            foreach (TList item in source)
            {
                if (comparer(item, value))
                {
                    return index;
                }

                index++;
            }
            return -1;
        }
        
        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> list) => new ReadOnlyCollection<T>(list);
    }
}
