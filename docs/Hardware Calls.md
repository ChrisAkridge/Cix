# Cix: Hardware Calls

An IronArc VM can have access to _hardware devices_ which provide methods to read and write data to the outside world. Hardware devices include terminals, storage devices, networking interfaces, and more. The methods that hardware devices are called _hardware calls_ and look mostly like functions: they are named, can return values, and accept zero or more arguments.

## Specifying Hardware Calls

In order for the Cix compiler to know what hardware calls exist, it is given a _hardware definition file_ in JSON format that lists the devices and the calls they have. The hardware definition file conforms to the below POCO types:

```csharp
class HardwareDefinition
{
    HardwareDevice[] HardwareDevices;
}

class HardwareDevice
{
    string DeviceName;
    HardwareCall[] HardwareCalls;
}

class HardwareCall
{
    HardwareCallDataType ReturnType;
    string CallName;
    HardwareCallParameter[] Parameters;
}

class HardwareCallParameter
{
    string ParameterName;
    HardwareCallDataType Type;
}

class HardwareCallDataType
{
    string TypeName;
    int PointerLevel;
}
```

The member `HardwareCall.ReturnType` can be `null` - if it is, the hardware call is specified not to return a value. No data type used in a hardware call may be a function pointer type, at least directly - you can pass and receive `void*` and cast it to the function pointer type directly.

### Providing the Hardware Definition file

A hardware definition file is provided to the compiler through the `#include` preprocessor directive. For more information on the inclusion process, please see the Preprocessor document.

## Adding Support for Hardware Calls in Cix Code

To actually perform hardware calls in Cix code, the compiler inserts functions that do not exist in the original source file during the AST generation. The functions have the return and parameter types as defined in each hardware call in the hardware definition file. The function's name is `HW_{name of device}_{name of call}`.

The bodies of the functions contain one of two AST elements: `HardwareCallVoidInternal` wrapped in an `ExpressionStatement` and `HardwareCallReturnsInternal` wrapped in a `ReturnValue`. They both store the call's name and parameter list. The `HardwareCallReturnsInternal` element is used for calls that return values, and, as such, includes the return type of the call.

Cix code can then invoke the hardware call as a normal function.