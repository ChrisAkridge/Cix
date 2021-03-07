# Tokenizer

The Cix Tokenizer takes a list of words made from the lexer, where each word is part of a preprocessed Cix file. The tokenizer then associates each word with what kind of token it is. For instance, `myVariable` is an identifier, `+` is an addition operator.

## Definitions

* A **word** is a string containing an indivisible unit of text from the preprocessed Cix file. Examples of words are `int`, `myVariable`, `+`, `@funcptr`, and `[`.
* A **token** is a word associated with a token type, such as Identifier, OpAdd, or OpenBracket.
* An **operator** is an element of an expression that combines one or two *operands* (such as numbers, identifiers, or other expressions) into one operand. For example, in `5 + 3`, `+` is the operator.
* A **unary operator** is an operator that has one operand. It can appear before or after the operand. Some unary operators are identity (`+5`) and postincrement (`i++`).
* A **binary operator** is an operator that has two operands. It appears between the operands. Some binary operators are multiplication (`2 * 2`) and modulus assignment (`t %= 60`).

## Sequence

The tokenization categorizes each word by the content of the words immediately before and after each word. These words are called the *previous*, *current*, and *next* words.

Each word is categorized in the order provided by the lexer.

### Basic Tokens
* The word is an **identifier** if:
    * It is made only of letters, numbers, and underscores and the first character is not a number.
    * It begins with a single at sign `@` and is followed only by letters, numbers, and underscores.
* The word is a **semicolon** if the word is only a `;`.
* The word is an **openscope** (left curly brace) if the word is only a `{`.
* The word is a **closescope** (right curly brace) if the word is only a `}`.
* The word is an **openparen** (left parentheses) if the word is only a `(`.
* The word is a **closeparen** (right parentheses) if the word is only a `)`.
* The word is a **left bracket** if the word is only a `[`.
* The word is a **right bracket** if the word is only a `]`.
* The word is a **comma** if the word is only a `,`.

### Reserved Keyword Tokens
A number of keywords are reserved by Cix:

```
break byte case const continue default do double else float for goto if int long return sbyte short sizeof struct switch uint ulong ushort void while
```

If a word is any of the above reserved words, it is classified as one of the token types for that keyword (for example, the word `int` has a token type of `KeyInt`).

### Numeric Literals

Numeric literals are classed into the following token types:

* **BasicNumericLiteral**: Consists only of one or more digits.
* **BasicHexadecimalLiteral**: Starts with `0x`, then is followed by one or more digits or letters `A` through `F` (any case).
* **SuffixedNumericLiteral**: A basic numeric literal suffixed with any of `u`, `l`, or `ul`.
* **SuffixedHexadecimalLiteral**: A hexadecimal literal suffixed with any of `u`, `l`, or `ul`.
* **FloatingLiteralWithDecimal**: A set of digits with a single `.` in them.
* **SuffixedFloatingLiteral**: A floating literal (with or without a `.`) suffixed with `f` or `d`. The period may not appear directly before the suffix.
* **FloatingLiteralWithExponent**: A floating literal (with or without a `.`), suffixed with an `e`, a `+` or `-`, then one or more digits. The period may not appear directly before the exponent.

### Operator Characters

Various words correspond to operators, such as addition or multiplication. Some words can represent different operators. For example, `+` can be either identity (`+x`) or addition (`x + y`). What each operator is can usually be determined by what words *precede* and *succeed* it.

Below is listed every type of operator word and what operator it is based on what precedes and succeeds it.

A plus sign `+` is an identity operator with:
* Preceding semicolon, closescope, openparen, comma, binary operator, or one of `- ! ~`. 
* Succeeding identifier, openparen, or one of `- ! ~ * &`.

A plus sign `+` is an addition operator with:
* Preceding identifier, closeparen, closebracket, one of `++ --`.
* Succeeding identifier, openparen, one of `- ! ~ -- & *`.

A minus sign `-` is an inverse operator with:
* Preceding semicolon, closescope, openparen, comma, binary operator, or one of `+ ! ~`.
* Succeeding identifier, openparen, or one of `+ ! ~ * &`.

A minus sign `-` is a subtraction operator with:
* Preceding identifier, closeparen, closebracket, one of `++ --`.
* Succeeding identifier, openparen, one of `+ ! ~ ++ & *`.

An exclamation mark `!` is a logical NOT with:
* Preceding semicolon, closescope, openparen, comma, binary operator, or one of `+ - ! ~`.
* Succeeding identifier, openparen, or one of `+ - ! ~ * &`.

A tilde `~` is a bitwise NOT with:
* Preceding semicolon, closescope, openparen, comma, binary operator, or one of `+ - ! ~`.
* Succeeding identifier, openparen, or one of `+ - ! ~ * &`.

A double plus sign `++` is a preincrement with:
* Preceding semicolon, closescope, openparen, comma, or binary operator. 
* Succeeding identifier.

A double plus sign `++` is a postincrement with:
* Preceding identifier or closebracket. 
* Succeeding binary operator or semicolon.

A double minus sign `--` is a predecrement with:
* Preceding semicolon, closescope, openparen, comma, or binary operator. 
* Succeeding identifier.

A double minus sign `--` is a postdecrement with:
* Preceding identifier or closebracket
* Succeeding binary operator or semicolon.

An asterisk `*` is a pointer dereference with:
* Preceding semicolon, closescope, openparen, comma, binary operator, or one of `+ - ! ~ & *`.
* Succeeding identifier, openparen, closeparen, or asterisk.

An asterisk `*` is a multiplication operator with:
* Preceding closeparen, closebracket, or one of `++ --`.
* Succeeding identifier, openparen, or one of `+ - ! ~ ++ -- & *`.

An asterisk `*` cannot have its type determined if none of the above conditions to determine if it's a multiplication operator apply. This is because the expression `x * y;` could mean `multiply x by y` or `create a variable of type pointer-to-x named y` based on whether `x` is the name of a type or not. Since we don't know which identifiers are types and which aren't yet, the only thing we can do is give it the token type of Indeterminate.

An ampersand `&` is a variable dereference with:
* Preceding semicolon, closescope, openparen, comma, binary operator, or one of `+ - ! ~ &`. 
* Succeeding identifier or one of `& *`.

An ampersand `&` is a bitwise AND with:
* Preceding identifier, closeparen, closebracket, one of `++ --`
* Succeeding identifier, openparen, or one of `+ - ! ~ ++ -- *`.

A dot `.` is a member access operator with:
* Preceding identifier, closeparen, closebracket.
* Succeeding identifier.

An arrow `->` is a pointer member access with:
* Preceding identifier, closeparen, closebracket.
* Succeeding identifier.

Any of `/ % << >> < <= > >= == != | ^ && || = += -= *= /= %= >>= <<= &= |= ^=` are various binary operators with:
* Preceding identifier, closeparen, closebracket, one of `++ --`. 
* Succeeding identifier, openparen, or one of `+ - ! ~ ++ -- & *`.

A word containing two or more asterisks denotes a pointer to a pointer to ... to a pointer to a type. The previous token must be an identifier, and the next token must be an identifier or a closeparen. Multiple asterisks are appended to the end of the token with the type name. For example, with words `int` and `***`, the token becomes `int***`.

A word beginning and ending in a double quotation mark `"` is a string literal.