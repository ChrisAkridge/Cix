using System;
using System.Collections.Generic;
using System.Text;
using Cix.Errors;

namespace Cix.Text
{
    internal sealed class StringLiteralFinder
    {
        private readonly IErrorListProvider errorList;

        public StringLiteralFinder(IErrorListProvider errorList) => this.errorList = errorList;

        public IReadOnlyList<SpanIndices> FindStringLiterals(string file)
        {
            var indices = new List<SpanIndices>();
            var inStringLiteral = false;
            var currentLiteralStart = -1;

            for (var i = 0; i < file.Length; i++)
            {
                var current = file[i];
                var last = (i > 0) ? file[i - 1] : (char?)null;

                if (current != '\"') { continue; }

                if (last == '\\')
                {
                    if (!inStringLiteral)
                    {
                        errorList.AddError(ErrorSource.StringLiteralFinder, 2,
                            "Escaped double quote found outside string literal.",
                            "TODO: Non-line errors aren't supported yet", -1);
                    }
                }
                else
                {
                    if (inStringLiteral)
                    {
                        inStringLiteral = false;
                        indices.Add(new SpanIndices(currentLiteralStart, i));
                    }
                    else
                    {
                        inStringLiteral = true;
                        currentLiteralStart = i;
                    }
                }
            }

            if (inStringLiteral)
            {
                errorList.AddError(ErrorSource.StringLiteralFinder, 1, "File ends with unterminated string literal.", "TODO: Non-line errors aren't supported yet", -1);
            }

            return indices.AsReadOnly();
        }
    }
}
