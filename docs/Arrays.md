# Arrays

In C, arrays and pointers are confusingly intertwined. For example, the indexing operator `a[i]` is really just a synonym for `*(a + i)` Arrays have a size, and indexing off the end of the array is undefined in C, even though it isn't always easy to know what size an array is.

Cix will simplify this problem... by mostly removing arrays entirely. Arrays in Cix work as follows:

## Struct Members

Any struct member can be an array:

```
struct MyStruct
{
	int[5] arrayMember;
}
```

Here we declare an array of size 5. The array size is of type `int` and is required. The five ints are stored contiguously in every struct declaration - had you declared it as a pointer, it wouldn't be included directly in the struct and will be null by default when you declare instances of the structure.

However, when you actually use this array member in functions, it behaves as a pointer to the start of the array.

## Global Variable Declarations

Any global variable can be an array:

```
global int[5] arrayGlobal;
```

Global array variables must also have a size declared, and the size is of type `int`. A global array variable cannot have an initializer. Much like with struct array members, using a global array member in a function actually gives you a pointer to the start of the array.

## Function Arguments and Local Variables

Function arguments and local variables **cannot** be array members; they must be pointers.

```
int[5] i;							// <-- invalid
int* i = malloc(sizeof(int) * 5);	// <--   valid
```

## Indexing Operator

The `[]` operator works as it does in C - `a[i]` is the same as `*(a + i)` where `a` is a pointer to any type and `i` is any numeric type.