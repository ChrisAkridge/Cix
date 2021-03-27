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
        public StructMemberInfo ReferentStructMember { get; set; }
        
        public IdentifierReferentKind ReferentKind { get; set; }
        public string Name { get; set; }

        public override UsageTypeInfo ComputeType(TypeComputationContext context, TypedExpression parent)
        {
            UsageTypeInfo computedType;
            
            switch (ReferentKind)
            {
                case IdentifierReferentKind.LocalVariable:
                case IdentifierReferentKind.FunctionArgument:
                    var stackEntry = context.CurrentStack.GetEntry(Name);
                    ReferentVariable = stackEntry;
                    computedType = stackEntry.UsageType;
                    break;
                case IdentifierReferentKind.GlobalVariable:
                    if (!context.DeclaredGlobals.TryGetValue(Name, out var matchingGlobal))
                    {
                        throw new InvalidOperationException("No global with this name exists");
                    }

                    ReferentGlobal = matchingGlobal;
                    computedType = matchingGlobal.UsageType;
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

                    break;
                default:
                    throw new InvalidOperationException("Internal compiler error: Unrecognized referent kind");
            }

            ComputedType = computedType;
            return computedType;
        }
    }
}