# Cix: Intrinsics and Function Pointers

## Intrinsic

An intrinsic is a keyword denoting a special, built-in object provided by the compiler. Intrinsics can be types or functions. Their implementation is provided by the compiler and their usage generates special code.

All intrinsics begin with an `@` symbol, like so: `@intrinsic`. This both signifies the locations of intrinsics to the compiler and allows the user to define normal identifiers with those names. They also allow for differing syntactic constructs for certain intrinsics.

Intrinsic keywords should be lexed as one word: `@intrinsic`, not `@` then `intrinsic`. Depending on the value of the intrinsic, other phases of the compiler may produce a different AST or generate different code than if a different identifier was used in its place.

## Function Pointers

In Cix, a function pointer is a pointer to any defined function. Functions can be derefenced to produce function pointers, and these pointers can be invoked to execute a function and return a result. A function pointer has special, generic-like syntax that names its return type and any argument types.

The type of a function pointer should be written as `@funcptr<ReturnType, Arg1Type, Arg2Type, ...>`. This generic-like syntax is only valid for the function pointer type; it cannot be used anywhere else. The return and argument types can be any Cix type, including primitives like `int` and `byte`, user-defined structures, pointers, arrays, or even other function pointers. When defining a function pointer to a function that takes or returns a function pointer, you must use the `@` symbol before every `funcptr`: `@funcptr<int, @funcptr<int, long, ulong>>`.

A function pointer type can be used in any place any other type can be used: as the type of global and local variables, the type of function arguments, and the type of a structure member.

Additionally, it is possible to have pointers to function pointers (`@funcptr<int, int>*`), and arrays of function pointers (`@funcptr<int, int>[]`). Pointers to functions returning nothing must use `void` as the return type: `@funcptr<void>`.

Function pointers support the following operations:
* Creation by dereference: When a function is dereferenced (`&main`), a function pointer with the proper return and argument types is produced (`@funcptr<int, int, byte**>`).
* Assignment: As with any other value, a function pointer can be assigned to a local or global variable or function parameter that has the same return and argument types.
* Invocation: A function pointer can be called just as a function:
```
int f(int i) { return i * 2; }

void main()
{
	@funcptr<int, int> ptr = &f;
	f(4);	// returns 8
}
```

Function pointers do not support the following operations:
* Pointer arithmetic: As functions are not considered to have constant sizes, pointer arithmetic on function pointers is meaningless.
* Casts: Function pointers cannot be cast to other function pointer types, nor can they be cast to any primitive or user-defined type.

One exception to not being able to cast function pointers is, as with every other type, the ability to cast a pointer to and from `void*`. For instance, the cast `(@funcptr<int, long, ulong>*)(void*)f` is a legal if erroneous cast..

### Code Generation

A recent change to IronAssembler added support for using block labels (which are used at the start of functions) in any location that can accept an address. Thus, to generate code to dereference a function to a function pointer, the process is rather simple:

```
	# @funcptr<int, int, byte**> ptr = &main;
	push QWORD main
```

Invocation involves simply calling the address stored in the function pointer:

```
	# Given a function int max(int a, int b),
	# @funcptr<int, int, int> maxPtr = &max
	push QWORD max

	# maxPtr(5, 3)
	stackargs
	push DWORD 3
	push DWORD 5
	call *ebp
```