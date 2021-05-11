using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Extensions;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal sealed class FinalEmitter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        private readonly int unvisitedIndicator;
        private readonly StartEndVertices startEndVertices;
        private readonly string functionName;

        public FinalEmitter(string functionName, StartEndVertices startEndVertices)
        {
            this.startEndVertices = startEndVertices;
            this.functionName = functionName;
            unvisitedIndicator = startEndVertices.Start.VisitCount;
        }

        public string GenerateInstructionsForControlFlow()
        {
            logger.Trace($"Generating final assembly for function {functionName}...");
            
            var builder = new StringBuilder();
            builder.AppendLine($"{functionName}:");
            var instructionList = GenerateInstructionList(startEndVertices.Start);

            foreach (var instruction in instructionList.Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                builder.AppendLine(instruction);
            }

            return builder.ToString();
        }

        private List<string> GenerateInstructionList(ControlFlowVertex start)
        {
            var directFlowInstructionTexts = new List<string>();
            var jumpedFlowsInstructionTexts = new List<List<string>>();
            var currentVertex = start;

            while (true)
            {
                if (currentVertex.VisitCount != unvisitedIndicator)
                {
                    // We've already generated this flow.
                    return new List<string>();
                }

                if (currentVertex is InstructionVertex instructionVertex
                    && instructionVertex.Mnemonic == "bwand")
                {
                    System.Diagnostics.Debugger.Break();
                }
                
                directFlowInstructionTexts.Add(currentVertex.GenerateInstructionText());

                foreach (var jumpFlowEdge in currentVertex.OutboundEdges.Where(e => e.FlowEdgeType != FlowEdgeType.DirectFlow))
                {
                    directFlowInstructionTexts.Add(jumpFlowEdge.GenerateInstructionText());
                    var jumpFlowInstructionTexts = GenerateInstructionList(jumpFlowEdge.Destination);

                    if (jumpFlowInstructionTexts.Any())
                    {
                        jumpedFlowsInstructionTexts.Add(jumpFlowInstructionTexts);
                    }
                }

                currentVertex.VisitCount += 1;

                var nextInstruction = currentVertex.OutboundEdges.Where(e => e.FlowEdgeType == FlowEdgeType.DirectFlow).SingleOrNone();
                if (nextInstruction == null) { break; }
                
                currentVertex = nextInstruction.Destination;
            }

            return directFlowInstructionTexts.Concat(jumpedFlowsInstructionTexts.SelectMany(list => list)).ToList();
        }
    }
}
