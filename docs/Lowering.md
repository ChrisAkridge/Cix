# Lowering

Lowering is the process of converting certain kinds of elements in a Cix abstract syntax tree into other, simpler elements while keeping the same meaning of the original code. This allows the code generator to only have to deal with the simpler construct.

New lowerings may be added to Cix at any future point.

## Convert For Loops into While Loops
Any for loop can be rewritten as a while loop. For example...

```c
code before;
for (initializer; condition; iterator)
{
	loop code;
}
code after;
```

can be rewritten as
```c
code before;
initializer;
while (condition)
{
	loop code;
	iterator;
}
code after;
```

To convert for loops into while loops, the initializer must be lifted out of the `for` header line, and the iterator must be put as the last statement in the loop.

Such a conversion is simple when done against the AST. The `for` loop above, expressed as a list of AST elements, is as follows:

```
<code before>
For Loop
	Initializer: initializer
	Condition: condition
	Iterator: iterator
	Body:
		loop code
<code after>
```

After the lowering, this becomes:

```
<code before>
Expression: initializer
While Loop
	Condition: condition
	Body:
		loop code
		iterator
code after
```