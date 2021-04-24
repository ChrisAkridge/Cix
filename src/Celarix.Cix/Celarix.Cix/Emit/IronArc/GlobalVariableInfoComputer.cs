using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Emit.IronArc.Models;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;
using NLog;

namespace Celarix.Cix.Compiler.Emit.IronArc
{
    internal static class GlobalVariableInfoComputer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static IDictionary<string, GlobalVariableInfo> ComputeGlobalVariableInfos(IEnumerable<GlobalVariableDeclaration> globals, IDictionary<string, NamedTypeInfo> declaredTypes)
        {
            var globalOffsetCounter = 0;
            var declaredGlobals = new Dictionary<string, GlobalVariableInfo>();

            foreach (var global in globals)
            {
                Helpers.TypesDeclaredOrThrow(global.Type, declaredTypes);

                var globalInfo = new GlobalVariableInfo
                {
                    Name = global.Name,
                    UsageType = UsageTypeInfo.FromTypeInfo(Helpers.GetDeclaredType(global.Type, declaredTypes), global.Type.PointerLevel),
                    OffsetFromERPPlusHeader = globalOffsetCounter
                };

                globalOffsetCounter += globalInfo.UsageType.Size;
                declaredGlobals.Add(global.Name, globalInfo);
                
                logger.Trace($"Global variable {globalInfo.Name} has size {globalInfo.UsageType.Size} at offset ERP+{globalInfo.OffsetFromERPPlusHeader}");
            }

            logger.Trace($"Global variables take {globalOffsetCounter} bytes");
            return declaredGlobals;
        }
    }
}
