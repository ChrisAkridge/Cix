using Cix.AST.Generator.IntermediateForms;
using Cix.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cix.AST.Generator.SecondPass
{
    public sealed class SecondPassGenerator
    {
        private readonly TokenEnumerator tokens;
        private readonly IErrorListProvider errorList;
        private readonly List<Element> tree;
        private bool astGenerated;

        public IReadOnlyList<Element> Tree => tree.AsReadOnly();

        public SecondPassGenerator(TokenEnumerator tokens, List<Element> tree, IErrorListProvider errorList)
        {
            this.tokens = tokens;
            this.tree = tree;
            this.errorList = errorList;
        }

        public void GenerateSecondPassAST()
        {
            if (astGenerated)
            {
                errorList.AddError(ErrorSource.ASTGenerator, 1,
                    "First-pass AST already generated; did you accidentally call GenerateFirstPassAST again?", "",
                    0);
                return;
            }

            foreach (var element in tree)
            {
                if (!(element is IntermediateFunction)) { continue; }

                var intermediateFunction = element as IntermediateFunction;
                var function = new Function(intermediateFunction.ReturnType,
                    intermediateFunction.Name,
                    intermediateFunction.Parameters);
                var functionTokens = tokens.Subset(intermediateFunction.StartTokenIndex,
                    intermediateFunction.EndTokenIndex);

                // Generate element trees

                // Expression Parser
                //  Mark parenthetical subexpressions
                //  Find and mark typecasts, function calls
                //  Convert expressions to postfix form
                //  Convert postfix-form expression into tree
                //  Generate expression trees type casts, function calls
            }

            astGenerated = true;
        }

        private IList<Element> ParseStatements(TokenEnumerator tokens, bool returnSingleElement)
        {
            // Block: found if the current token is an openscope
            // Break: found if the token is KeyBreak
            // ConditionalBlock: found if the token is a KeyIf
            // Continue: found if the token is a KeyContinue
            // DataType: found if the token is an Identifier in the NameTable and is a DataType
            throw new NotImplementedException();
        }
    }
}
