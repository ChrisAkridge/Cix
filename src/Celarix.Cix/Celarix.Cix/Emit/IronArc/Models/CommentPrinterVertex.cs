using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = System.Environment;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class CommentPrinterVertex : ControlFlowVertex
    {
        public string CommentText { get; set; }

        public CommentPrinterVertex(string commentText) => CommentText = commentText;

        public override string GenerateInstructionText() =>
            !IsJumpTarget
                ? $"{Environment.NewLine}# {CommentText}"
                : $"{Environment.NewLine}{JumpLabel}:{Environment.NewLine}# {CommentText}";

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => CommentText;
    }
}
