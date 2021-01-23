﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Extensions;
using Celarix.Cix.Compiler.IO.Models;
using NLog;

namespace Celarix.Cix.Compiler.Preparse
{
    internal static class CommentRemover
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private enum CommentRemoverStates
        {
            Default,
            StringLiteral,
            SingleLineComment,
            MultilineComment
        }

        public static void RemoveComments(IEnumerable<Line> lines)
        {
            bool inMultilineComment = false;

            logger.Trace("Removing comments...");
            
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line.Text)) { continue; }
                
                var lineBuilder = new StringBuilder(line.Text.Length);
                bool inSingleLineComment = false;

                for (int i = 0; i < line.Text.Length; i++)
                {
                    if (line.StringLiteralLocations.Any(sll => i >= sll.Start.Value && i <= sll.End.Value))
                    {
                        lineBuilder.Append(line.Text[i]);

                        continue;
                    }
                    
                    var prev = (i > 0) ? line.Text[i - 1] : (char?)null;
                    var curr = line.Text[i];
                    var next = (i < line.Text.Length - 1) ? line.Text[i + 1] : (char?)null;

                    if (inSingleLineComment)
                    {
                        lineBuilder.Append(' ');
                    }
                    else if (inMultilineComment)
                    {
                        if (prev == '*' && curr == '/')
                        {
                            inMultilineComment = false;
                        }

                        lineBuilder.Append(' ');
                    }
                    else
                    {
                        switch (prev)
                        {
                            case '/' when curr == '/':
                                inSingleLineComment = true;
                                lineBuilder.Append(' ');

                                break;
                            case '/' when curr == '*':
                                inMultilineComment = true;
                                lineBuilder.Append(' ');

                                break;
                            default:
                                lineBuilder.Append((curr != '/' && (next != '*' || next != '/')) ? curr : ' ');

                                break;
                        }
                    }
                }

                line.Text = lineBuilder.ToString();
            }
            
            logger.Trace("Removed comments");
        }
    }
}
