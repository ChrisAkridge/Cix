# Code Generation

Code generation is the final stage of Cix compilation. The code generator receives an abstract syntax tree containing every struct, global, and function defined in a preprocessed Cix file. It then generates an IronArc assembly file from the code of the abstract syntax tree. This assembly file then can be fed to IronAssembler to produce IronArc executable code.

One of the more powerful facts about Cix code is that every part of the code is designed to be as independent from each other as possible. For example, the body of an `if` block can have its code generated without needing to know where the `if` block is or what it's nested in.

The code generator for Cix, as of right now, does not perform optimizations against the code, neither for speed or for size. This may change in future versions.

## Globals Generation

As the code generator converts the AST to IronAssembler, it keeps a number of data structures and classes to represent various stages of the code generation that do not end up in the final assembler file. These data structures are called **compilation assistant** (CA) structures. Among these are the struct declarations.

The code generator first generates a list of all defined structs and the offsets of all their members. These offsets are used to calculate addresses to read or write data from, but no information about the structs are written to the IronAssembler file.

The size, in bytes, of all global variables, is added together and used to generate the `globals:` block in the IronAssembler file. A CA is created to store the addresses of all the global variables in the program.

For example, given the following globals:

```
global int i;
global void* memBase;
global MyStruct structDef;	// assume MyStruct is 20 bytes
```

...the following CA would be created to keep track of the globals during code generation (`HeaderEnd` being the first byte after the IronArc executable header):

```
Global 0: {Name: "i", Type: int, Address: HeaderEnd + 0}
Global 1: {Name: "memBase", Type: void*, Address: HeaderEnd + 4}
Global 2: {Name: "structDef", Type: MyStruct, Address: HeaderEnd + 12}
```

...and produce the following `globals:` block in the IronAssembler file:

```
globals: 32
```

Globals will be defined in the CA in the order they're defined in the AST, which is the order they're defined in the preprocessed source. Any usage of a global variable first requires some code to add the address to the `ERP` register so that, if the program is loaded at an address other than 0, the address is correct.

## Structs and Offsets

Another CA records information about structs declared in the AST. This information is the struct's name, size in bytes, and list of members. Each member stores its name, type (which may be a primitive type or another struct), size in bytes, and offset.

The offset of a struct member is `0` for the first member, the size of the first member for the second, the size of the second member for the third, and so forth. For example:

```c
struct Offsets
{
    int a;      // Offset 0
    short b;    // Offset 4
    ulong c;    // Offset 6
    void* d;    // Offset 14
}
```

Pointers, including function pointer, always have a size of 8 bytes.

## String Literals

In an IronArc executable file, string literals are stored as length-prefixed UTF-8 strings at the end of the file. In assembly, the strings are stored in the `strings:` section at the end of the file. To refer to these strings, instruction operands use the `str:0` notation, where the number is the index of the string in the table. These indices are rewritten into addresses into the string table during assembly.

Before code generation, the AST is walked to find all string literals, which are added to a list. The indices of this list are used as indices in operands using strings and in the order of the generated `strings:` section.

## Initialization Code
Cix expects that any preprocessed source file contains one function named `main`. It must have no return type or arguments. The function `main` may appear anywhere in the Cix code, but it will be the first function in the IronArc assembly file.

The first two instructions of the `main` function are inserted by the compiler automatically, and they are `mov QWORD eax ebp` and `mov QWORD eax esp`; this sets up the stack to just after the program. Following these two instructions is the instructions of the rest of the `main` function.

## Function, Statement, and Block Generation
A number of CA structures are used in the code generation of functions and their blocks. The first CA structure is the **function context**, one for each function.

The function context stores the name of the function, a list of at least one **assembly block**, a **virtual stack**, and a set of counters for each kind of assembly block.

An assembly block is a CA that stores the assembly code for one block or section of the code. Assembly blocks could map to code at the top level of a function, code inside a `while` loop, or the condition of an `if` block. Assembly blocks contain a name, the kind of block this assembly block holds the code for, and a string containing the instructions of the block.

The kinds of assembly blocks are:
* Function
* `if` condition
* `if` body
* `else if` condition
* `else if` body
* `else` body
* `do-while` condition
* `do-while` body
* `switch` condition
* `switch` case evaluation
* `case` body
* `while` condition
* `while` body

The virtual stack imitates the actual IronArc stack when the function is run. Each **virtual stack element** (VSE) represents an actual value on the IronArc stack during execution. VSEs may be local variables, function arguments, or interstital values produced as expressions are evaluated.

Each of the above block kinds gets a counter that starts at 0 and increments each time a block of that kind is found in the function. This gives the indices for the label names for each assembly block.

### Naming of Assembly Blocks
Each function receives at least one assembly block, which contains the instructions of the first top-level elements in the function (or `ret` if the function has no code in it). These assembly blocks have the same name as the function does (that is, a function called `void InitRNG(...)` would have a block labelled `InitRNG:`).

Each kind of block (`if`, `while`, `switch`, etc.) gets one or more assembly blocks. The name of these blocks starts with the function name, two underscores, the type of the block, another underscore, then a zero-based index to specify which number block of this type this is, then another underscore, then a word indicating the part of the block this assembly block represents.

For example, consider an `if` block. Such a block has a condition and a sequence of code to execute if the condition is true. If the `if` block occurs in a function named `InitRNG`, two assembly blocks are defined with the names `InitRNG__if_0_condition` and `InitRNG__if_0_true`.

Any code following a block is suffixed with "after", i.e. `InitRNG__if_0_after`. If a block is followed immediately by another block (i.e. an `if` followed by a `while`), the after block contains only a jump to the start of the second block.

### Determing the Kind of an Assembly Block
An assembly block has a kind, taken from the list above. The kind of the assembly block is usually pretty easy to determine, as it is set when the code generator enters a particular block. The only complicated kind of assembly blocks are the ones that come after blocks; the `_after` blocks.

The kind of an `_after` assembly block is the same as the "parent" assembly block. That is to say, if there's an `if` block directly inside a function, the `_after` block is the Function kind.

Determining the parent kind is done by checking back with the AST. Whatever block we're in has a counter in the function context (or it's the top level of the function), and we can use that counter to find the assembly block label, and use the kind of that block.

### Code Generation for a Function

When the compiler begins to generate code for a function, it generates a new function context, a new assembly block to put in that context, and a virtual stack. The virtual stack is then populated with VSEs for each of the function arguments, such that the last argument is at the top of the stack.

For a function called `RandomInt(int min, int max)`, here's what the VSEs would look like:

```
0: Kind: Variable, Type: int, Name: "min", Size: 4, Address: *ebp+0
1: Kind: Variable: Type: int, Name: "max", Size: 4, Address: *ebp+4
```

An assembly block with the label `RandomInt` is generated. Then, each element inside the function is evaluated and has code generated for it. Let's see how code is generated for each kind of AST element.

#### Break Statement
The `break;` statement is only legal inside loop and case bodies. For this statement, the code generator emits an unconditional jump to the `_after` block for this assembly block (i.e. if the current block is labelled `main__while_0_body`, the jump will be to `main__while_0_after`).

#### Conditional Block
When the code generator finds an `if`/`else if`/`else` block, it must generate one to three new assembly blocks: one to check the condition, and one to execute the code if the condition is true, as well as one to hold the code that comes after the conditional block.

When an `if` block is found, an unconditional jump to a label named `funcName__if_#_condition` is generated, where `funcName` is the name of the function and `#` is the index of this `if` block. An assembly block by that name is generated and code to evaluate the expression and check if it's non-zero is emitted inside it.

Code is then emitted that has a conditional jump to `funcName__if_#_true` if the condition is true, followed by an unconditional jump to either:

* `funcName__if_#_after` if there are no `else` blocks
* `funcName__if_#_false` if there is an `else` block.

The `funcName__if_#_true` block contains the code for the `if` block's body and the `funcName__if_#_false` block contains the code for the `else` block, if there is one.

#### Continue
The `continue;` statement is only legal inside loops. When encountered, the code generator emits an unconditional jump to `funcName__while_#_condition` or `funcName__dowhile_#_condition`.

#### Do-While Loop
A `do while` loop runs a loop body once, checks for a condition, then runs it again, as long as the condition remains true.

When a `do` block is found, an unconditional jump to a label named `funcName__dowhile_#_body` is generated, along with the assembly block with that label. The code inside the `do` block is then compiled, then an unconditional jump to an assembly block named `funcName__dowhile_#_condition` is generated.

In the latter assembly block, the code to evaluate and check the condition is emitted. A conditional jump-if-not-equal is emitted to `funcName__dowhile_#_body` is emitted, then an unconditional jump to `funcName__dowhile_#_after` is emitted.

#### For Loop
All `for` loops were rewritten to `while` loops back in the lowering phase. Please see the "Lowering" document for more details.

#### Return
The `return;` statement returns from a function. It doesn't return a value and can only be used in functions whose return type is `void`.

When a `return;` statement is found, the code generator emits the following instructions:

```
mov QWORD ebp esp  # To reset the stack to what it was before the function call
ret                # To perform the return
```

#### Return Value
The return value statement returns a value computed from a given expression. It can only be used in functions whose return is not `void`. The type of the expression must be the same as the function's return type, or can be implicitly converted to the function's return type.

When a return value statement is found, the code generator first emits code to evaluate the expression. The resulting value of this expression would then sit at the top of the stack.

If the return type is a primitive type or a pointer (that is to say, 8 bytes or less), code is emitted that pops it into a register. Then, the stack is reset using `mov QWORD ebp esp`, an instruction pushes the return value from the register onto the stack, and the `ret` instruction is emitted.

If the return type is a struct type, code is emitted to perform the following steps:

1. Move the address of the struct to return into a register.
2. Reset the stack with `mov QWORD ebp esp`
3. Use the `movln` instruction to copy the struct to the start of the stack.
4. Add the struct's size to ESP to ensure the stack's length is correct.
5. Return from the function.

#### Switch Block
The switch block evaluates an expression, then compares the result with a number of constants. When a given constant is equal to the result, code inside a block associated with the constant is ran. Control is then passed out of the switch block.

When a switch block is found, an unconditional jump is generated to a new assembly block labelled `funcName__switch_#_eval`. This assembly block will have the code to evaluate the expression. Then, a sequence of comparisons between the result and the constants are performed, each comparison acting as follows:

1. Duplicate the result of the expression by pushing it again.
2. Push the constant.
3. Emit a `cmp` instruction.
4. If they are equal, jump to `funcName__switch_#_case_*` where `*` is the index of the case in the switch block.
5. At the end of the assembly block, emit a jump to either `funcName__switch_#_case_default` if there is a default block, or `funcName__switch_#_after` if there is no default block.

Then, the code generator creates assembly blocks for every `case` block, as named in step 4 above.

Note that case block generation is an exception to the typical block arrangement: **case blocks are stored contiguously in the generated code, with their child blocks, if any, occurring after all the case blocks**. This is to allow fall-through.

#### Variable Declaration
A variable declaration creates a new variable without assigning it a value.

When a variable declaration is encountered, the code generator emits code to add the size of the variable to `ESP`. The code generator also adds a VSE to the virtual stack representing this local variable.

#### Variable Declaration with Initialization
A variable declaration with initialization creates a new variable and assigns it a value based on an expression.

The code generator emits the same code as it does for a normal variable declaration, but it then emits code to evaluate the expression, then code to store the result in the variable.

#### While Loop
A `while` loop checks a condition, runs a body of code if the condition is true, then checks the condition again, in a loop.

When a `while` loop is found, an unconditional jump to a new assembly block labelled `funcName__while_#_condition` is generated. This assembly block contains code to evaluate the expression and compare it to zero, emitting a conditional jump-if-not-equal to `funcName__while_#_body` followed by an unconditional jump to `funcName__while_#_after`.

The body of the loop then has its statements compiled. Finally, an unconditional jump to `funcName__while_#_condition` is emitted.

## Types and Expressions

### Implicit and Explicit Conversions

Values of a given type may be convertable to values of a different type. Some conversions are implicit; that is, a value of one type can be used in place of a different type without any explicit conversion. Other conversions are explicit; they need a typecast. The remaining conversions are illegal.

The primitive types can be separated into the following categories:
* Signed integer types: `sbyte`, `short`, `int`, `long`
* Unsigned integer types: `byte`, `ushort`, `uint`, `ulong`
* Floating-point types: `float`, `double`
* Pointers: `T*` and its variants of increasing asteriskitivity

Some types are wider (have more bytes) than others. For example, a short is 2 bytes, and an int is 4 bytes.
Conversions from a narrow type to a wider type are always implicit as long as both types have the same signedness. That is, a `short` -> `int` conversion, an `sbyte` -> `long` conversion, and a `uint` -> `ulong` conversion are all implicit. This is because a wider type is always able to represent all values of a narrower type.

Conversions from a wider type to a narrower type, or between types of different signedness, are always explicit and require a cast. This is because narrower types cannot store all the values of wider types, and because a signed negative integer would convert to an unsigned positive integer and vice versa.

Casting to a narrower type removes the top bytes of the value. For example, a cast from `long` (8 bytes) to `short` (2 bytes) would store only the lowest two bytes in the resulting `short`.

The conversion from `float` to `double` is implicit, as every float can be represented as a double. The reverse conversion, however, is explicit, as doubles have values that are either out of range (resulting in `+/-Infinity`), or too precise (resulting in the nearest float) for floats.

Conversions from integer types to float or double is explicit, as some integers cannot be accurately represnted as a float or double. For example, 8,388,603 is the last odd integer that can be stored in a float.

Conversions from floating types to an integer is explicit, as floating types may have a fractional part (which is truncated in the conversion), or be out of range (which results in either the lowest possible value for the integer, or the highest).

Pointers conversions are explicit, except for `void*`, which can be converted to any other type implicitly.

Struct types cannot be converted to or from anything.

Function pointers cannot be cast to or from anything except `void*`. `void*` can be cast to any function pointer type.

Widening conversions are handled as follows:

1. Push a `0` value of the right size onto the stack, creating a VSE for it.
2. Use `mov` to move the right number of bytes to the widened variable at the right location.

Narrowing conversions are similar:

1. Push a `0` value of the right size onto the stack, creating a VSE for it.
2. Use `mov` to move the right number of bytes to the narrowed variable from the right location in the original.

### Determining the Type of an Expression

Every expression has a type; this type is what results from an expression's evaluation. For instance, the expression `5 + 3` has type `int`. Additionally, each subexpression in an expression has its own type, which may not be the same as the expression's type. For instance, the expression `5 + (int)(3.0f + 1.0f)` has type `int`, but the parenthesized expression has type `float`.

We'll start from the bottom: literals and references to variables, both direct and to members of a struct. Integer and floating point literals store their type (`int`, `uint`, `long`, `ulong`, `float`, or `double`), and therefore their size. String literals have type `byte*` - string literals are described below.

Variable references have the type of their declared variable. Access to struct members, either directly or through a pointer, have the type of that member.

Let's now specify the types of each expression node:

* Array Access: Applied to members of type `T*`, has the type of `T`.
* Binary Expression: Except for `.` and `->`, whose behavior is described above, a binary expression takes the widest type of the two operands - that is, `long + int` would be `long`. The operands' types must be implicitly convertible, or one side must be cast to a convertible type if not.
* Cast Expression: Has the type the operand is casted to.
* Function Invocation: Has the return type of the function being invoked. Expressions producing a function pointer have their return type explicitly specified.
* Hardware Call Returning a Value: Has the return type of the function call.
* Size-Of Expression: Has the type `long`.
* Ternary Expression: For `? :`, the first operand must have an integral type. The second and third operands must be the same type, or the third operand must be implicitly convertible to the second operand's type.
* Unary Expression:
    * `&`: Operates on `T`, returns `T*`.
    * `*`: Operates on `T*`, returns `T`.
    * `~`, `!`, `-`, `+`, `++`, `--` (both pre- and postfix): The same type as the operand.

## Expression Generation

Expression generation produces assembly code that evaluates an expression. As expressions are stored in the AST in RPN form, the order of operations is already known and parentheses are not present. Generating the code for each expression element works as follows:

### Array Accesses

An array access is an operator with an expression producing an index of type `int`. This index is then applied to a pointer to the start of an array. First, the code to evaluate the index is generated. That index lives atop the stack.

Next, code is generated that multiplies the index by the size of an element of the array's type. For instance, if the code accesses an element of a `long` array, the index is multiplied by `sizeof(long) == 8`.

Finally, code is produced that copies the desired array element to atop the stack. This may be a simple `mov` for primitive types, or a `movln` for struct types. `ESP` is then adjusted to include this result.

### Binary Expressions

Generally, a binary expression is handled as follows:

1. Push the first operand. If it needs to be widened, emit code to do the widening.
2. Push the second operand. If it needs to be widened, emit code to do the widening.
3. Perform the required operation. The result is atop the stack.

Some operators require special handling:

- Direct member access `x.y`: `x` must be a struct type here, directly on the stack. Push the address of `x` atop the stack and add the offset of `y`. `mov` or `movln` the value of `x.y` onto the top of the stack, adjusting `ESP`. Copy it in the place of the temporary holding `&(x.y)` and subtract `8` from `ESP`.
- Pointer member access `x->y`: `x` must be a pointer type here. Push `x` and then the offset of `y`. Add the two to get `&(x.y)`. `mov` or `movln` the value of `x.y` onto the top of the stack, adjusting `ESP`. Copy it in the place of the temporary holding `&(x.y)` and subtract `8` from `ESP`.
- Comparison operators:
    1. Emits `cmp` to perform the comparison, setting flags in `EFLAGS`. Then, `EFLAGS` is bitwise-ANDed with the following values to mask out the desired comparison flag:
        * `==`, `!=`: `0x40_00_00_00_00_00_00_00`
        * `<`: `0x20_00_00_00_00_00_00_00`
        * `>`: `0x10_00_00_00_00_00_00_00`
        * `<=`: `0x08_00_00_00_00_00_00_00`
        * `>=`: `0x04_00_00_00_00_00_00_00`
    2. If the operator is `!=`, we then push `0x40_00_00_00_00_00_00_00` again and bitwise-XOR it with the top of the stack to invert the equality value.
    3. We then shift the top of the stack right by `62` (`==`, `!=`), `61` (`<`), `60` (`>`), `59` (`<=`), or `58` (`>=`) to shift the comparison bit into the lowest bit.
- Assignment:
    - Assignment to a local variable or function argument simply moves the value atop the stack into the stack location of the local or argument.
    - Assignment to a global variable: Pushes `ERP`, then the offset of the global, then adds them together. Then, moves the value one beneath the top of the stack to the address pointed to by the value atop the stack, then moves `ESP` to remove the pointer off the stack.
    - Assignment to a pointer: Pushes the pointer, evaluating it if necessary, and moves the value as if it was a global variable pointer, described above. Then, `ESP` is moved to remove the pointer.
- Compound assignment: The value of the left-hand side is pushed atop the stack, then the assignment's operation is performed on it. Then the result is treated as an assignment. This ensures that the left-hand side is evaluated for its value only once.

### Cast Expressions

Casting a value to a wider or narrower type is done as noted in **Implicit and Explicit Conversions** above. Casting to a type of the same width is done by just pushing the value again - the only thing that changes is the type specified in the VSE.

### Literals

Integer literals just push the value onto the stack. Floating point literals push the value atop the stack using the `float()` or `double()` notation in IronArc assembly.

String literals are pushed by their index in the strings table, as generated before code generation: `push str:0` pushes a pointer to the start of the 0th string in the table.

### Size-Of Expressions

The type's size in bytes is pushed atop the stack.

### Ternary Conditional (`? :`)

This operator requires the generation of three new blocks: `funcName__ternary_#_true`,  `funcName__ternary_#_false`, and `funcName__ternary_#_after`. First, code to evaluate the condition is emitted, followed by a conditional jump to one of the first two blocks. The block then evaluates its expression, leaving it atop the stack and unconditionally jumping to the after block.

### Unary Expression

* Identity `+`: No code is emitted.
* Inverse `-`: A `0` value of the operand's type is pushed, then the value, then a `sub` instruction to compute the value's inverse.
* Bitwise NOT `~`: Emits a `bwnot` instruction.
* Logical NOT `!`: Emits an `lnot` instruction.
* Value-at `*`: Adds the type's size minus 8 to `ESP`, then emits `mov` or `movln` to move the value at the pointer atop the stack, overwriting the pointer atop the stack.
* Address-of `&`:
    - For local variables and function arguments, which are on the stack, push `EBP`, push the offset from `EBP` that's stored in the VSE, and add them together.
    - For global variables, push `ERP`, push the global's offset from the start of the program, then add them together.
    - For stack temporaries, we're forced to emit an error, unfortunately. Something like `&(x.y)` would not work at the moment, you'd have to do `&x + 1`.
* Pre-increment `++`, pre-decrement `--`: Changes the variable, then leaves the changed variable atop the stack. The emitted code is the same as what would be emitted if `x += 1` or `x -= 1`.
* Post-increment `++`, post-decrement `--`: Conceptually, this operator pushes the operand's value onto the stack, then changes the variable. The emitted code is the same as what would be emitted if `(x += 1) - 1` or `(x -= 1) + 1`.
