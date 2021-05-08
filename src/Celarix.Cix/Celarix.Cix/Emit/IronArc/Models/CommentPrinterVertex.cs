using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models
{
    internal sealed class CommentPrinterVertex : ControlFlowVertex
    {
        public string CommentText { get; set; }

        public CommentPrinterVertex(string commentText) => CommentText = commentText;

        public override string GenerateInstructionText() => $"{Environment.NewLine}# {CommentText}";
    }
}
