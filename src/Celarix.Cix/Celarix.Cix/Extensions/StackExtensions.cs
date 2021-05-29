using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Extensions
{
    internal static class StackExtensions
    {
        public static bool TryPeek<T>(this Stack<T> stack, out T item)
        {
            if (stack.Count == 0)
            {
                item = default;

                return false;
            }

            item = stack.Peek();

            return true;
        }
    }
}
