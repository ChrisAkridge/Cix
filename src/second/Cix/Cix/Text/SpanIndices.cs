using System;
using System.Collections.Generic;
using System.Text;

namespace Cix.Text
{
    internal readonly struct SpanIndices
    {
        public int Start { get; }
        public int End { get; }

        public SpanIndices(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
