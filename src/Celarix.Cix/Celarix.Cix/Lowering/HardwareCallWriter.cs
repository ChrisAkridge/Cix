using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.Lowering.Models;
using Celarix.Cix.Compiler.Parse.Models.AST.v1;

namespace Celarix.Cix.Compiler.Lowering
{
    internal static class HardwareCallWriter
    {
        public static IList<Function> WriteHardwareCallFunctions(HardwareDefinition hardware)
        {
            var functions = new List<Function>();
            foreach (var hardwareDevice in hardware.HardwareDevices)
            {
                functions.AddRange(hardwareDevice.HardwareCalls.Select(c =>
                    WriteHardwareCallFunction(c, hardwareDevice.DeviceName)));
            }

            return functions;
        }

        private static Function WriteHardwareCallFunction(HardwareCall call, string deviceName)
        {
            var function = new Function
            {
                Name = $"HW_{deviceName}_{call.CallName}",
                Parameters = call.Parameters
                    .Select(p => new FunctionParameter
                    {
                        Name = p.ParameterName,
                        Type = new NamedDataType { Name = p.Type.TypeName, PointerLevel = p.Type.PointerLevel }
                    })
                    .ToList()
            };

            if (call.ReturnType == null)
            {
                function.ReturnType = new NamedDataType { Name = "void", PointerLevel = 0 };

                function.Statements = new List<Statement>
                {
                    new ExpressionStatement
                    {
                        Expression = new HardwareCallVoidInternal
                        {
                            CallName = call.CallName,
                            ParameterTypes = call.Parameters.Select(p => (DataType)new NamedDataType
                                {
                                    Name = p.Type.TypeName, PointerLevel = p.Type.PointerLevel
                                })
                                .ToList()
                        }
                    }
                };
            }
            else
            {
                var returnType = new NamedDataType
                {
                    Name = call.ReturnType.TypeName,
                    PointerLevel = call.ReturnType.PointerLevel
                };
                function.ReturnType = returnType;
                
                function.Statements = new List<Statement>
                {
                    new ReturnStatement
                    {
                        ReturnValue = new HardwareCallReturnsInternal
                        {
                            CallName = call.CallName,
                            ReturnType = returnType,
                            ParameterTypes = call.Parameters
                                .Select(p => (DataType)new NamedDataType
                                {
                                    Name = p.Type.TypeName,
                                    PointerLevel = p.Type.PointerLevel
                                })
                                .ToList()
                        }
                    }
                };
            }

            return function;
        }
    }
}
