### Cix Code Generator for IronArc

This article documents the semantics of generating assembly code for the IronArc platform from a Cix source file.

## Before Code Generation
At the point where Cix is able to generate code, several things have already occurred: the source file has been loaded, uncommented, and preprocessed, following by lexing, tokenizing, and, finally, the generation of the Abstract Syntax Tree (AST).

The AST of a Cix program describes a program in a tree structure using various types to represent different language constructs such as structs, functions, and control structures. Expressions are not stored in tree form, but rather as `ExpressionElements` in postfix notation, as parsed by a version of the shunting-yard algorithm. In expressions with side effects (assignments and pre/post increment/decrement of values), certain expression elements are marked as *lvalues* (can be assigned to) and all others are marked as *rvalues* (cannot be assigned to).

Assignable statements take many forms:

* Assignment to local: `x = 1;`
* Assignment to struct member of local: `x.y = 1;`
* Assignment to memory at pointer: `*x = 1;`
* Assignment to struct member at pointer: `x->y = 1;`
* Assignment to memory at memory address: `*(x + 4) = 1;`
* Pre/post Increment/Decrement `x++;`

Multiple lvalues may be chained together, for instance `y = x = *(*(z + 12)->w->q.r) = 1;`

## The Shunting Yard Algorithm In This Document
Certain expressions expressed in infix form (`3 + 4`) may have a diagram showing their transformation into postfix form (`3 4 +`) in order to demonstrate the composition of the expression and of the resulting assembly code.

For instance:

```
[Input] -> [Output] [Operators]
```

## The Code Generator
The Cix Code Generator receives its input as a fully constructed Abstract Syntax Tree (AST) representing a valid Cix program and outputs an IronArc assembly file containing the compiled code of the program. The assembled file can then be passed to an IronArc assembler in order to create an executable IronArc program.

### Internal Data Structures and their uses
During code generation, the code generator uses data structures to keep track of the state of a "virtual" IronArc machine, including its registers, data stack, and memory stack. This allows the code generator to resolve the location of local variables.

#### The Virtual Execution Stack
The Virtual Execution Stack (VES) is a stack that models what the actual IronArc stack will appear as during execution. The VES is composed of a `Stack<StackElement>` and two ulong values representing the values of the `EBP` and `ESP` registers (the stack base pointer and stack pointer, respectively). When code is generated that manipulates values on the stack, the VES and its `ESP` value are adjusted in the same way.

The `StackElement` stores knowable information about each stack item, including the name of the Cix object it represents (local variable, argument, etc.), and its size. The StackElement does not concern itself with the value of the data on the stack, as that cannot be definitively known until runtime anyway. If no name exists for a stack element (consider the result of 3 * 4; there is no defined name for that), then no name will be assigned.

Stack indexes are a core method of accessing stack elements in IronArc. The indexes for an object change as the stack grows or shrinks. The top of the stack (the element immediately before `ESP`) always has index 0, every element behind it has indices increasing by 1 for each element.

```
EBP = 0							ESP = 16
|							    |
+-------+-------+-------+-------+
|index 3|index 2|index 1|index 0|
+-------+-------+-------+-------+
```

```
push 1:s4

EBP = 0									ESP = 20
|										|
+-------+-------+-------+-------+-------+
|index 4|index 3|index 2|index 1|index 0|
+-------+-------+-------+-------+-------+
```

#### The Virtual Size Stack
The Virtual Size Stack (VSS) reflects the IronArc size stack which is an internal stack that maintains the sizes of the elements on the execution stack. Each element on the size stack stores the size in bytes of the element with the same index on the execution stack. Each element on the size stack is a 32-bit signed little-endian integer. The size stack and the VSS maintain two more registers, `SBP` and `SSP` (size stack base pointer and size stack pointer, respectively).

#### Nametable