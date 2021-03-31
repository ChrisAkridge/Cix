using System;
using System.Collections.Generic;
using System.Linq;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Emit.IronArc.Models.TypedExpressions
{
    internal sealed class Identifier : TypedExpression
    {
        public Function ReferentFunction { get; set; }
        public VirtualStackEntry ReferentVariable { get; set; }
        public GlobalVariableInfo ReferentGlobal { get; set; }
        public StructInfo ReferentStruct { get; set; }
        public StructMemberInfo ReferentStructMember { get; set; }
        
        public IdentifierReferentKind ReferentKind { get; set; }
        public string Name { get; set; }

        public override UsageTypeInfo ComputeType(ExpressionEmitContext context, TypedExpression parent)
        {
            var referentKind = SetReferentKind(context, parent);
            UsageTypeInfo computedType;
            
            switch (referentKind)
            {
                case IdentifierReferentKind.LocalVariable:
                    var stackEntry = context.CurrentStack.GetEntry(Name);
                    ReferentVariable = stackEntry;
                    computedType = stackEntry.UsageType;
                    IsAssignable = true;
                    break;
                case IdentifierReferentKind.GlobalVariable:
                    if (!context.DeclaredGlobals.TryGetValue(Name, out var matchingGlobal))
                    {
                        throw new InvalidOperationException("No global with this name exists");
                    }

                    ReferentGlobal = matchingGlobal;
                    computedType = matchingGlobal.UsageType;
                    IsAssignable = true;
                    break;
                case IdentifierReferentKind.Function:
                    if (!context.Functions.TryGetValue(Name, out var matchingFunction))
                    {
                        throw new InvalidOperationException("No function with this name exists");
                    }

                    ReferentFunction = matchingFunction;

                    computedType = new UsageTypeInfo
                    {
                        DeclaredType = new FuncptrTypeInfo
                        {
                            ReturnType = context.LookupDataType(ReferentFunction.ReturnType),
                            ParameterTypes = ReferentFunction.Parameters.Select(pt => context.LookupDataType(pt.Type))
                                .ToList()
                        }
                    };
                    break;
                case IdentifierReferentKind.StructMember:
                    computedType = new UsageTypeInfo
                    {
                        DeclaredType = ReferentStructMember.UnderlyingType,
                        PointerLevel = (ReferentStructMember.ArraySize == 1)
                            ? ReferentStructMember.PointerLevel
                            : ReferentStructMember.PointerLevel + 1
                    };
                    IsAssignable = true;
                    break;
                default:
                    throw new InvalidOperationException("Internal compiler error: Unrecognized referent kind");
            }

            ComputedType = computedType;
            return computedType;
        }

        public override StartEndVertices Generate(ExpressionEmitContext context, TypedExpression parent)
        {
            bool parentPerformsAssignment = EmitHelpers.ExpressionPerformsAssignment(parent);

            switch (ReferentKind)
            {
                case IdentifierReferentKind.Function:
                    return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                    {
                        new InstructionVertex("push", OperandSize.Qword, new LabelOperand(ReferentFunction.Name)),
                    });
                case IdentifierReferentKind.GlobalVariable:
                {
                    var computePointerFlow = new IConnectable[]
                    {
                        new InstructionVertex("push", OperandSize.Qword, EmitHelpers.Register(Register.ERP)),
                        new InstructionVertex(
                            "push", OperandSize.Qword,
                            new IntegerOperand((ulong)ReferentGlobal.OffsetFromERPPlusHeader
                                + GlobalVariableInfo.IronArcHeaderSize)),
                        new InstructionVertex("add", OperandSize.Qword),
                    };

                    return GetValueOrPointerFlow(parentPerformsAssignment, ReferentGlobal.UsageType, computePointerFlow);
                }
                case IdentifierReferentKind.LocalVariable:
                {
                    var computePointerFlow = new IConnectable[]
                    {
                        new InstructionVertex("push", OperandSize.Qword, EmitHelpers.Register(Register.ESP)),
                        new InstructionVertex("push", OperandSize.Qword, new IntegerOperand(ReferentVariable.OffsetFromEBP)),
                        new InstructionVertex("add", OperandSize.Qword), 
                    };

                    return GetValueOrPointerFlow(parentPerformsAssignment, ReferentVariable.UsageType, computePointerFlow);
                }
                case IdentifierReferentKind.StructMember:
                {
                    var leftType = (parent as BinaryExpression).Left.ComputedType;
                    var leftStructType = leftType.DeclaredType as StructInfo;
                    var memberOffset = leftStructType.MemberInfos.Single(mi => mi.Name == Name).Offset;

                    return EmitHelpers.ConnectWithDirectFlow(new IConnectable[]
                    {
                        new InstructionVertex("push", OperandSize.Qword, new IntegerOperand(memberOffset)),
                    });
                }
                default:
                    throw new InvalidOperationException("Internal compiler error: invalid referent kind");
            }
        }

        private static StartEndVertices GetValueOrPointerFlow(bool parentPerformsAssignment, UsageTypeInfo usageType, IConnectable[] computePointerFlow)
        {
            if (!parentPerformsAssignment)
            {
                var writeValueFlow = (usageType.DeclaredType is StructInfo && usageType.PointerLevel == 0)
                    ? new IConnectable[]
                    {
                        new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                        new InstructionVertex("movln",
                            OperandSize.NotUsed,
                            EmitHelpers.Register(Register.EAX),
                            EmitHelpers.Register(Register.ESP),
                            new IntegerOperand(usageType.Size)),
                        new InstructionVertex("addl",
                            OperandSize.Qword,
                            EmitHelpers.Register(Register.ESP),
                            new IntegerOperand(usageType.Size),
                            EmitHelpers.Register(Register.ESP)),
                    }
                    : new IConnectable[]
                    {
                        new InstructionVertex("pop", OperandSize.Qword, EmitHelpers.Register(Register.EAX)),
                        new InstructionVertex("push", EmitHelpers.ToOperandSize(usageType.Size),
                            EmitHelpers.Register(Register.EAX, isPointer: true)),
                    };

                return EmitHelpers.ConnectWithDirectFlow(computePointerFlow.Concat(writeValueFlow));
            }
            else { return EmitHelpers.ConnectWithDirectFlow(computePointerFlow); }
        }

        private IdentifierReferentKind SetReferentKind(ExpressionEmitContext context, TypedExpression parent)
        {
            if (parent is BinaryExpression binaryExpression
                && (binaryExpression.Operator == "." || binaryExpression.Operator == "->"))
            {
                if (!(binaryExpression.Left.ComputedType.DeclaredType is StructInfo structInfo))
                {
                    throw new InvalidOperationException("Left-hand side of binary expression isn't a struct");
                }

                ReferentKind = IdentifierReferentKind.StructMember;
                ReferentStructMember = structInfo.MemberInfos.FirstOrDefault(mi => mi.Name == Name);

                return ReferentStructMember != null
                    ? ReferentKind
                    : throw new InvalidOperationException("No struct member has this name");
            }

            if (context.CurrentStack.Entries.Any(e => e.Name == Name))
            {
                ReferentKind = IdentifierReferentKind.LocalVariable;
                return ReferentKind;
            }
            else if (context.DeclaredGlobals.ContainsKey(Name))
            {
                ReferentKind = IdentifierReferentKind.GlobalVariable;
                return ReferentKind;
            }
            else if (context.Functions.ContainsKey(Name))
            {
                ReferentKind = IdentifierReferentKind.Function;
                return ReferentKind;
            }
            else
            {
                throw new InvalidOperationException("No local or global variable or function has this name");
            }
        }
    }
}