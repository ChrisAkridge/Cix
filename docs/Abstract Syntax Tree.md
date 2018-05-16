# Abstract Syntax Tree

In Cix, the **abstract syntax tree** is a tree data structures that describes a preprocessed, lexed, and tokenized Cix file. The **AST generator** transforms a tokenized list of words into a tree.

The AST contains tree nodes that, themselves, contain other nodes. The nodes at the top of the tree are struct definitions, global variable declarations, and functions. Struct definitions then own elements describing their members, globals own elements describing their type and name, and functions own elements describing the statements and expressions within them.

## Types of Elements

An **element** is a single node on the syntax tree. Some elements own other elements, others do not. Elements that don't own other elements are called **terminal** elements. Every path down the tree must either end in a terminal element or end in a non-terminal element containing no other elements (i.e. an empty block).

Additionally, some elements are **expression** elements, which can be used in an expression. Non-expression elements are also called **statements** or **blocks**.

The types of elements are as follows:

* Break: Represents a `break` statement.
* Conditional Block: Represents a single `if`/`else`/`else if` block. Contains the conditional expression and the statements inside the block. Multiple conditions appear in order in whatever element contains the conditional block.
* Continue: Represents a `continue` statement.
* Data Type: Represents a type for a local or global variable, function argument or return type, or structure member. Is composed of the type's name, pointer level (`int` is at level 0, `int**` is at level 2), and the size in bytes of the type.
	* Function Pointer Type: Represents a type for a function pointer. In addition to the other Data Type properties, this element contains a return type and a list of zero or more parameter types.
* Do-While Loop: Represents a loop that performs an action at least once, then checks a condition to see if it should loop again. Contains the condition and the statements to loop over.
* For Loop: Represents a loop that runs an initializer, then checks a condition. If the condition is true, a set of statements are executed, followed by an iterator statement. This element contains the initializer, condition, and iterator expressions, and the statements to loop over.
* Function: Contains the function's name, return type, arguments, and the statements it contains.
* Function Argument: Contains the argument's name and type.
* Global Variable Declaration: Contains the global's type, name, and initial value, if any.
* Return: Represents a `return` statement.
* Return Value: Represents a `return` statement that returns a value.
* Struct Declaration: Contains the members of the struct.
* Struct Member Declaration: Contains the name and type of the member, along with its array size (1 if the member isn't an array) and the offset in bytes of the member in the struct.
* Switch Block: Contains a list of the cases in the Switch block.
* Switch Case: Contains a constant value for the case, a flag indicating if this switch case is the default, and the statements of the case.
* Varargs Function Argument: Represents an argument in a function that indicates this function accepts any number of arguments after the normal arguments.
* Variable Declaration: Contains the name and type of the variable.
* Variable Declaration with Initialization: Contains the name, type, and initializer expression.
* While Loop: Represents a loop that runs code if a condition is true, and then continues to run the code while the condition is true. Contains the condition and the statements.

The types of expression elements are as follows:

* Expression: Represents an entire expression. Contains a list of the elements in "postfix" notation (that is, `2 + 3` becomes `2 3 +`). For more information on expression parsing, please see Expression Parsing below.
* Array Access: Represents an array access. Array accesses themselves are expressions (i.e. `data[i * 4]`).
* Constant: Represents a constant or string literal, such as `5`, `3.14159d`, or `"Hello, world!"`.
* Function Call: Represents the invocation of a function or function pointer. Contains the name of the function/function pointer being invoked, as well as the parameters and the return type.
* Function Parameter: Represents an expression used as a parameter in a function call.
* Identifier: Names a global or local variable, function argument, or, used with the member access operators, a struct member.
* Operator: Represents a unary or binary operator, such as dereference, add, or multiplication assignment.
* Parentheses: Represents a left or right parentheses.
* Typecast: Represents an operator that converts a value of one type into another.

## AST First Pass Generator
The first-pass AST generator receives a tokenized list of words made from a preprocessed Cix source file. It then creates the trees for every struct and global variable, and also makes intermediate representations of functions, which includes their names, return types, arguments, and the indices of their starting and ending tokens.

This can be accomplished in any number of ways, so I'll just describe the resulting syntax tree. It contains:

* Full definitions of every struct
	* Contains the name of the struct
	* Contains a list of the members of the struct
		* Contains the name and type of the member
		* Contains the array size of the member. The array size is one if the member is not an array, or it's the size specified by the user (`int data[5];` has an array size of 5)
		* Contains the offset of the member, in bytes, from the start of any instance of the struct, where the first member has offset 0
* Full definitions of every global variable
	* Contains the name of the variable
	* Contains the type of the variable (which can be a pointer, but can't be `void`, `lpstring`, or an array)
	* Contains, if present, a numeric literal to initialize the global variable. This is only legal if the variable is a primitive numeric type
* Intermediate definitions of every function
	* Contains the name of the function
	* Contains the return type of the function
	* Contains the full definition of every function argument (last argument can be the Varargs Function Argument)
		* Contains the name and type of the argument
	* Contains the start token index, the index of the function's openscope (the first opening left curly brace of the function)
	* Contains the end token index, the index of the function's closescope (the last closing right curly brace of the function)

## AST Second Pass Generator
The second-pass AST generator receives the tree generated by the first pass and the tokenized list of words. It generates the syntax for function bodies, including all statements, blocks, and expressions.

The resulting syntax tree contains the same struct and global variable definitions as the tree made from the First Pass Generator. However, the intermediate functions are replaced with complete function definitions that contain all the code of the function bodies.

Function body parsing involves three fundamental objects: statements, blocks, and expressions:

* A statement is single piece of code such as `break;` or `return;`. Statements can contain expressions (`int i = 2 + 3`) or they can be entirely made of expressions (`InitRNG()`).
* An expression is a sequence of expression elements, such as identifiers and literals, with operators combining these elements in various ways. They are written in code as "infix expressions", which means that operators appear inline amongst the elements.
* A block is a list of statements. Most blocks are also associated with an expression; for example, an `if` block contains an expression and a list of statements to run if the condition is true.

Expression parsing is covered in the "Expression Parsing" section below. For now, we'll concern ourselves with generating a tree of function bodies, containing:

* A list of elements (blocks or statements) for every top-level element in the function. For example, the below lines marked with `<-` are top-level elements.

```
int main()
{
	if (3 < 5) <-
	{
		return 2;
	}
	return 0; <-
}
```

Each top-level element may be a statement that contains nothing else (in which case, it is considered a *terminal element*), or it may contain a list of elements within it (in which case, it is a *non-terminal statement*). Note that, in Cix, a statement that contains an expression (like `int y = x * z;`) is NOT considered to contain any other elements; it is considered terminal.

Thus, a tree of the above function should appear as (where indented lines are children of the line with one less indentation):

```
Function (name "main", returns int, 0 arguments)
	If Block (condition expression "3 < 5")
		Return Value (value 2)
	Return Value (value 0)
```

### Expression Parsing

#### Operators in Cix and How They're Evaluated

IronArc, the platform for which Cix is being built, largely evaluates expressions using a stack-based approach. For every binary operation, both operands are pushed onto the stack, then the operator instruction is called, which pops the top two values off the stack, performs the operation on them, then pushes the result.

For example, `3 + 5` assembles into:

```
push DWORD 5    Stack: 5
push DWORD 3    Stack: 5 3
add DWORD       Stack: 8
```

The top of the stack has a `DWORD` with the value of 8.

Different operations have different associativities - they "bind tighter" than other operations. For example, multiplication binds tighter than addition and thus should come first in any evaluation.

For example, `3 * 5 + 2` should calculate `3 * 5` first, then take that result and add it to two. You can think of the expression as having invisible parentheses: `(3 * 5) + 2`. This expression assembles down to:

```
push DWORD 3    Stack: 3
push DWORD 5    Stack: 5 3
mult DWORD      Stack: 15
push DWORD 2    Stack: 2 15
add DWORD       Stack: 17
```

Unary operations accept only one operand. Some unary operators come before the operand (`&value`), and some come after (`i--`). Unary operations are handled very similarly to binary operations: push the operand, then perform the action.

Almost all C-like languages have the ternary conditional operator `?:`. Cix does not, at least at this time.

Most such operators are pretty simple. Some operators, however, parse a bit differently. Consider the following:

* Direct Member Access (`a.b`): This expression is used to access members of structs. It results in an assignable expression (see "Assignable and Value Expressions") that points to member "b" in variable "a".
* Pointer Member Access (`a->b`): This expression takes a pointer to a struct and follows it to find the named member. It's equivalent to `(*a).b`, and produces an assignable expression much like Direct Member Access.
* Typecast (`(int)f`): This expression takes a sub-expression and converts it to a different type. Note that it isn't made of one token, but at least three (`(`, `int`, and `)`), and the middle token(s) can and will vary.
* Array Access (`arr[(i * width) + j]`): This expression takes an expression producing an array/pointer as one operand and an expression producing an index into that array (or offset beyond that pointer) as another, producing the value at that address in the array. The expression is equivalent to `*(arr + ((i * width) + j))`. This is not unlike the typecast operator, in that it has multiple tokens and the inner tokens vary in type and count.
* Function Invocation (`randomInt(1, 6)`): This takes a function name (or expression resulting in a function pointer), and a comma-delimited list of parameters, each an expression of its own, started and ended by parentheses.

#### Operator Precedence List

Each operator has a number called its precedence. The higher the precedence, the closer the operands bind to the operator. The highest precedence operations in an expression are evaluated first. If two operations have the same precedence, they are evaluated left-to-right.

The list of precedences is as follows:

* Precedence Level 12: Post-increment `++`, Post-decrement `--`, Array Access `[]`, Direct Member Access `.`, Pointer Member Access `->`
* Precedence Level 11: Pre-increment `++`, Pre-decrement `--`, Identity `+`, Inverse `-`, Logical NOT `!`, Bitwise NOT `~`, Dereference `*`, Address Of `&`
* Precedence Level 10: Multiplication `*`, Division `/`, Modulus Division `%`
* Precedence Level 9: Addition `+`, Subtraction `-`
* Precedence Level 8: Shift Left `<<`, Shift Right `>>`
* Precedence Level 7: Less Than `<`, Less Than or Equal To `<=`, Greater Than `>`, Greater Than or Equal To `>=`
* Precedence Level 6: Bitwise AND `&`
* Precedence Level 5: Bitwise XOR `^`
* Precedence Level 4: Bitwise OR `|`
* Precedence Level 3: Logical AND `&&`
* Precedence Level 2: Logical OR `||`
* Precedence Level 1: Assignments `= += -= *= /= %= <<= >>= &= |= ^=`

#### Operator Associativity
Operators that are left associative are evaluated left to right. That is, if operator `Q` is left associative, then the expression `a Q b Q c` is evaluated as `(a Q b) Q c`.

However, if `Q` is right associative, then `a Q b Q c` is evaluated as `a Q (b Q c)`.

Unary operators are considered right associative.

* Left-associative: Array Access `[]`, Direct and Pointer Member Access `.` `->`, arithmetic/bitwise/comparison operators `+ - * / % << >> < <= > >= == != & ^ | && ||`
* Right-associative: Assignment `= += -= *= /= %= <<= >>> &= ^= |=`

#### Assignable and Value Expressions
Some expressions produces things that can be assigned to. For example, if `p` is a pointer, then `p + 4` produces a pointer that can be assigned to (`(p + 4) = 2` would work, for instance). Such expressions are called **assignable expressions** (or lvalues).

Some expressions produce things that can only be used as a value. For example, as `5` is a number, you cannot assign a value to it (`5 = 2` doesn't work). Such expressions are called **value expressions** (or rvalues).

Assignable expressions represent an object in memory and can be used both for their value and to assign a new value to the object. Value expressions can only be used for their value.

The following expression elements are assignable or produce assignable expressions:

* The name of a variable (`myVar`), or an access to a member of a structure (`socket.port`, `thermometer->tempF`).
* Arithmetic operations on pointers (`p + 4`).
* Functions that return a pointer.

Any other element or operation only produces a value.

#### Parsing Expressions into a Tree

When "for each token" is used below, the order is left-to-right unless otherwise stated. The procedure for parsing an arbitrary expression from a list of tokens composing it into a tree is as follows:

##### First Pass: Mark the Parenthetical Subexpressions
A **parenthetical subexpression** (PS) is a sequence of expression elements enclosed in parentheses, optionally as part of a larger expression. Below is an example of a PS, marked by arrows:

```
i = 2 * (3 + 4);
        ^^^^^^^
```

We introduce a new token called the **expression element token** (EET). This token stores the following:
* Token type, text, filename and line number, as before
* Index in the token stream
* Parenthetical nesting level (tokens not in parentheses are at nesting level 0, and it goes up by 1 for every pair of parentheses the token is in)
* Matching parentheses index (nullable, only non-null if the token is a parentheses)
* Parenthetical subexpression type:
	* That hasn't yet been determined (default for all new EETs)
	* Token is not in a PS
	* Token is part of a typecast
	* Token is part of a function call
	* Token is in a normal PS

When an EET is created, the token type, text, and index are assigned.

We start with the following local variables:
* Current nesting level, starting at 0
* A stack of left parentheses indices
* A list of EETs to hold the processed tokens

1. For each token,
	* If the token is a left parenthesis
		1. Create a new left parenthesis EET.
		2. Assign the current nesting number to it
		3. Add it to the list.
		4. Push the index of the left paren onto the stack
		5. Increment the current nesting level.
	* If the token is a right parentheses:
		1. Create a new right parentheses EET.
		2. Assign it (current nesting level - 1).
		3. Pop the last left paren index off the top of the stack.
		4. Assign the left paren's index to the right paren EET's matching index and vice versa.
		5. Add the right paren EET to the list,
		6. Decrement the nesting level.
	* If the token is not a parentheses:
		1. Create a new EET.
		2. Assign it the current nesting level.
		3. Add it to the list.

This produces a list of EETs for which we can easily tell what PS each token is in. This can be done as the nesting level of all tokens inside a PS have the same value.

An example of this process is below. Given `i = (2 * 3 + (4 % ((*p + 22)(1, 2, "hello"))))`:

| Token Index | Token     | Nesting Level | Matching Paren Index |
|-------------|-----------|---------------|----------------------|
| 0           | `i`       | 0             |                      |
| 1           | `=`       | 0             |                      |
| 2           | `(`       | 0             | 26                   |
| 3           | `2`       | 1             |                      |
| 4           | `*`       | 1             |                      |
| 5           | `3`       | 1             |                      |
| 6           | `+`       | 1             |                      |
| 7           | `(`       | 1             | 25                   |
| 8           | `4`       | 2             |                      |
| 9           | `%`       | 2             |                      |
| 10          | `(`       | 2             | 24                   |
| 11          | `(`       | 3             | 16                   |
| 12          | `*`       | 4             |                      |
| 13          | `p`       | 4             |                      |
| 14          | `+`       | 4             |                      |
| 15          | `22`      | 4             |                      |
| 16          | `)`       | 3             | 11                   |
| 17          | `(`       | 3             | 23                   |
| 18          | `1`       | 4             |                      |
| 19          | `,`       | 4             |                      |
| 20          | `2`       | 4             |                      |
| 21          | `,`       | 4             |                      |
| 22          | `"hello"` | 4             |                      |
| 23          | `)`       | 3             | 17                   |
| 24          | `)`       | 2             | 10                   |
| 25          | `)`       | 1             | 7                    |
| 26          | `)`       | 0             | 2                    |

##### Second Pass: Finding and Marking Typecasts and Function Calls
Quick, what's a function call? `sqrt(22)` definitely is. If we want to find function calls, we can just search for identifiers followed by a left paren, right?

Well, what about `(*p+22)(1, 2, "hello")`? That's a PS producing a function pointer on the left, being called with three arguments on the right.

Okay, you say. Then, in addition to the first rule, any right paren followed by a left paren marks a function call.

Ah, yes, but what about `(int)(float)x`? That's not a function call, that's two typecasts.

Now we see what must be done to properly find function calls. And, with our more detailed token list, we have all the info we need to find them.

We do this by marking every EET with what kind of PS it's in. We do this by scanning through the list, looking for left parentheses. What follows the parentheses is key. In this order:

* If an identifier follows the parentheses, check if that identifier names a type or is `@funcptr`. If it does, mark every EET in the parenthetical block as a typecast. (If it's actually illegal syntax, we'll find out later). Note that variables cannot have the same name as any type.
* If an identifier precedes the parentheses, the subexpression and the identifier before it is a function call. Mark these EETs as a function call.
* If a right parentheses precedes the left parentheses, check if the right paren is a typecast or is a PS. (Function calls are an error, i.e. `randomInt(0, 4)(12)` is wrong).
	* If it's a typecast, this PS is just a plain PS. Mark every EET inside it as so.
	* Is it's a PS, this PS is a function argument list. Mark the EETs in it and the entire block before it (here called a **function-producing expression**) as a function call.

This marks every PS with what kind of PS it is. Using our expression from before:

```
i = (2 * 3 + (4 % ((*p + 22)(1, 2, "hello"))))
    NNNNNNNNNNNNNNFFFFFFFFFFFFFFFFFFFFFFFFFNNN

N = normal PS
F = function call PS
```

What does knowing what's a function call and what's not let us do? It lets us treat function call subexpressions as individual tokens we don't have to parse. Since we can parse any subexpression independent of whatever expression it's in (`(i + 2)` can be parsed without having to know it's part of `(i + 2) + 4`), we don't have to worry about accounting for function call tokens along with all the normal tokens. We can just parse the function call and its arguments separately and treat it no differently as a variable name.

We can do the same with typecasts. Since a typecast is a type in parentheses followed by an expression (here called the **typecast value expression**), we can treat typecasts as individual tokens which are processed the same as variable names. The expressions within the typecasts can be processed recursively.

Once the whole expression and any subexpressions in function calls and typecasts are converted into trees, we can merge the trees of the expressions and subexpressions together to make one unified tree.

There's a bit of cleanup we need to do before we can continue, though. We must convert all blocks of EETs corresponding to typecasts into individual token. These tokens then contain the list of expression elements that makes up their function-producing expression/function arguments/typecast value expression. These are treated identically to tokens that just contain a variable name. We can then work on creating a tree from the expression.

One last thing, though: nested function calls (i.e. `sqrt(cos(i))`). Since these new token types can contain other expression element tokens, and since these new token types *are* expression element tokens, they can contain other function calls or typecasts.

#### Converting the Expression to Postfix Form
Expressions in Cix are written in **infix** form; operators appear in between their operands. In order to produce a tree, we must first convert the expression from infix to **postfix** form, in which operators appear after their operands.

For example, `2 + 3` would become `2 3 +`, as mentioned above in the expression evaluation examples. Notably, postfix notation does not require parentheses.

To convert infix expressions into postfix expressions, we will use the **shunting yard algorithm**, invented by Edsger Dijkstra. This algorithm uses two variables: an operator stack and an output list of expression elements.

For each token in the expression (with function calls and typecasts converted to one token):

* If the token is a constant, string literal, variable name, typecast, or function call, add it to the output.
* If the token is a left parentheses, push it onto the operator stack.
* If the token is a right parentheses:
	1. Pop operators off the stack, adding each one to the output as you go.
	2. When you reach a left parentheses, pop it and discard it.
* If the token is an operator:
	* If the stack is empty/has a left parentheses on top, push the operator onto the stack.
	* If the stack has an operator on top (we'll call it B and the current token A):
		* If A has a higher precedence than B or A has the same precedence as B and is right-associative, push it on the stack.
		* If A has a lower precedence than B or A has the same precedence as B and is left associative, pop operators off the stack and add them to the list until this condition is no longer true (or the stack empties/reaches a left paren). Then, push A.

Finally, if the stack has any operators left, pop them off and add them to the output list.

This process would convert `i = (2 * 3 + (4 % ((*p + 22)(1, 2, "hello"))))` to postfix as follows:

| Current Token | Operator Stack | Output List                     |
|---------------|----------------|---------------------------------|
| `i`           | empty        | `i`                             |
| `=`           | `=`            | `i`                             |
| `(`           | `= (`          | `i`                             |
| `2`           | `= (`          | `i 2`                           |
| `*`           | `= ( *`        | `i 2`                           |
| `3`           | `= ( *`        | `i 2 3`                         |
| `+`           | `= ( +`        | `i 2 3 *`                       |
| `(`           | `= ( + (`      | `i 2 3 *`                       |
| `4`           | `= ( + (`      | `i 2 3 * 4`                     |
| `%`           | `= ( + ( %`    | `i 2 3 * 4`                     |
| function call | `= ( + ( %`    | `i 2 3 * 4 function call`       |
| `)`           | `= ( +`        | `i 2 3 * 4 function call %`     |
| `)`           | `=`            | `i 2 3 * 4 function call % +`   |
| `)`           | empty        | `i 2 3 * 4 function call % + =` |

Psuedo-IronArc assembly for this may look like:

```
push QWORD &i
push DWORD 2
push DWORD 3
mult DWORD
push DWORD 4
# prepare arguments and function here
call functionExpression
mod DWORD
add DWORD
pop *esp
```

The stack after each instruction would look like (assuming the function returns 22):

```
&i
2 &i
3 2 &i
6 &i
4 6 &i
22 4 6 &i
2 6 &i
8 &i
```

`i` would be assigned the value 8.

Here's another example, this one using unary operators: `*p + *q`. The shunting yard becomes:

| Current Token | Operator Stack | Output List |
|---------------|----------------|-------------|
| `*`           | `*`            | empty     |
| `p`           | `*`            | `p`         |
| `+`           | `+`            | `p *`       |
| `*`           | `+ *`          | `p *`       |
| `q`           | `+ *`          | `p * q`     |
| clear the stack | `+`            | `p * q *`   |
| clear the stack | empty        | `p * q * +` |

If `p` is an `int*` at `*ebp` and `q` is an `int*` at `*esp+8`, IronArc assembly for this expression becomes:

```
mov QWORD *ebp eax
push DWORD *eax
mov QWORD *ebp+8 eax
push DWORD *eax
add DWORD
```


#### Converting the Postfix Expression into a Tree
An expression tree is a tree containing a given expression:

![Credit to Wikimedia Commons](https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Expression_Tree.svg/800px-Expression_Tree.svg.png)

The above tree is a result of the expression `2 2 + 2 2 + + 3 3 + +` (postfix form) or `((2 + 2) + (2 + 2)) + (3 + 3)` (infix form).

We can generate an expression tree by taking up a postfix expression backwards. Note that some operator nodes may have two children (binary operators) and some may only have one (unary operators).

As an example, we'll use our postfix expression from the last section: `i 2 3 * 4 function call % + =`

Here, a **value** is any constant, string literal, identifier, typecast, or function call. An **operator** is any operator.

The process is as follows:

1. Check the last element.
	* If it's a value, the tree has only one node, the value itself.
	* If it's a binary operator:
		1. Create a new node with two children.
		2. Recurse back to step 1 to fill the right child node.
		3. Recurse back to step 1 again to fill the left child node.
	* If it's a unary operator:
		1. Create a new node with one child.
		2. Recurse back to step 1 to fill the child node.

With this algorithm, we can process our example expression into a tree:

1. We first find `=`, so we expect two children. Recurse:
	1. We first find `+`, so we expect two children. Recurse:
		1. We first find `%`, so we expect two children. Recurse:
			1. We first find `function call`, which becomes our right child. This tree now looks like
			```
			Root: %
				Left: <empty>
				Right: function call
			```
			2. We then find `4`, which becomes our left child. This tree now looks like:
			```
			Root: %
				Left: 4
				Right: function call
			```
			3. Return the tree.
		2. We then find `*`, so we expect two children. Recurse:
			1. We first find `3`, which becomes the right child. This tree now looks like:
			```
			Root: *
				Left: <empty>
				Right: 3
			```
			2. We then find `2`, which becomes the left child. This tree now looks like:
			```
			Root: *
				Left: 2
				Right: 3
			```
			3. Return the tree.
		3. This tree now looks like:
		```
		Root: +
			Left: *
				Left: 2
				Right: 3
			Right: %
				Left: 4
				Right: function call
		```
		4. Return this tree.
	2. We then find `i`, which becomes the left child. This tree is now:
	```
	Root: =
		Left: i
		Right: +
			Left: *
				Left: 2
				Right: 3
			Right: %
				Left: 4
				Right: function call
	```
	3. Return this tree.
2. Return the tree.

The tree looks like this (expressed as an image):

![](https://i.imgur.com/GO9ciJL.png)

For our other expression `p * q * +`:

1. We start with `+`. We need two children. Recurse:
	1. We start with `*`. We need one child. Recurse:
		1. We start with `q`. This becomes the child. Return this tree:
		```
		Root: *
			Child: q
		```
	2. We then find `*`. We need one child. Recurse:
		1. We start with `p`. This becomes the child. Return this tree:
		```
		Root: *
			Child: p
		```
	3. Return this tree:
	```
	Root: +
		Left: *
			Child: p
		Right: *
			Child: q
	```

The tree looks like this:

![](https://i.imgur.com/oqY56tO.png)

#### Generating Trees for Function Calls and Typecasts

Function call expression elements are in one of two forms: a **simple function call** (a function call to a named function) or a **expression function call** (a function call to an expression resulting in a function pointer). Both elements have a list of parameters, but they differ in how the function is expressed. A simple function call just has a string containing the identifier, while the expression function call has an expression generating the function pointer.

Typecast elements have two properties: the expression to convert, and the type to convert it to.

Nonetheless, generating trees for these are easy. Traverse the expression tree, and when a function call/typecast element is found, call the functions that generate a tree on every expression in the element.