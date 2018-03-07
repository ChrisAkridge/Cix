# Error Handling

Compilers typically scan through code, noting any errors and warnings it comes across, instead of failing as soon as it finds the first error. This helps users avoid a long and painful fix-and-compile cycle.

Errors are invalid syntatic or semantic constructs that cannot be correctly parsed or understood by the compiler. Warnings are syntatically and semantically valid code that may not be what the user expects.

Errors and warnings are composed of the following elements:
* Error Code: A two-character prefix to indicate which stage the error came from, followed by a three-digit number to indicate the error code.
* Message: A message including what the error is, what specifically is wrong, and, optionally, what a valid example looks like.
* File, line, and character position: The position of the first character of the error.

In the Cix compiler, errors and warnings populate an error list. This list is returned at the completion of the compilation process. Errors can come from any stage of compilation, and later stages rely on earlier stages to be correct and complete before continuing. Thus, all errors from a single stage must be placed in the error list instead of failing on the first error. Once the stage completes with errors, the compilation can fail entirely before reaching a later stage.

## Continue Processing with Errors on Each Stage

Each stage of compilation can encouter errors any time during the stage. Each stage also requires differing methods of continuing the compilation process after finding an error. Below are the consideration for each stage:

### I/O Stage
No special considerations.

### Preprocessor
Preprocessing is performed line-by-line. Thus, when an error is encountered, skip to the next line and continue the preprocessing from there.

### Lexer
Many, many character-and-context combinations are not valid in the grammar. Skip to the next character (perhaps the character after the next for some combinations), and continue lexing with the same context.

### Tokenizer
Similar to the lexer, the tokenizer works on each lexed word with a context. When an error is encountered, continue processing from the next token with the same context.

### First-Pass AST Generator (structs, globals, function declarations)
Each part of the first pass processes each individual thing (all structs, then all globals, then all function declarations). Each element is also made of smaller elements (structure members, global type and name, function name/return type/arguments).

Each struct, global, or function declaration should be parsed as far as possible, with errors in each component placed in the error list.

### Second-Pass AST Generator (statements and expressions)

## Error List
### I/O Errors:
* IO001: Null or empty file path.
* IO002: File {file} is not a valid path or the file does not exist.
* IO003: File {file} could not be opened for reading. Check permissions or if the file is already opened.
* IO004: File {file} is blank or empty.
* IO005: I/O exception occurred. (include exception details)

### Preprocessor Errors:
* PR001: "{defineLine}" isn't valid; must be "#define SYMBOL" or "#define THIS THAT".
* PR002: #define symbol {symbol} is not an identifier.
* PR003: Symbol {symbol} is already defined.
* PR004: "{substitution}" for symbol {symbol} is not valid; must be a single word or an integer.
* PR005: "{undefineLine}" isn't valid; must be "#undefine SYMBOL"
* PR006: Cannot undefine {symbol} as it was not previously defined.
* PR007: "{ifdefLine}" isn't valid; must be "#ifdef SYMBOL"
* PR008: "{ifndefLine}" isn't valid; must be "#ifndef SYMBOL"
* PR009: "{includeLine}" isn't valid; must be "#include "file"" or "#include <file>"
* PR010: The include file {file} doesn't exist or has an invalid path.
* PR011: The include file {file} couldn't be opened for reading.

### Lexer Errors:
* LX001: Invalid character '{char}'.
* LX002: The character '{char}' isn't a valid numeric literal suffix; valid suffixes are "u", "l", "ul", "f", and "d".

### Tokenizer Errors:
* TK001: The word "{word}" cannot be parsed.
* TK002: The operator "{word}" cannot appear in this location.
* TK003: The word "{word}" cannot be in the place of an operator.
* TK004: The type "{typeNameWithAsterisks}" must be followed by a name or a close parenthesis.
* TK005: Multiple asterisks must follow a type name, not "{word}".

### First-Pass AST Generator Errors
* AA001: AST already generated; did you accidentally call GenerateFirstPassAST again?
* AA002: There is already a structure named "{struct}".
* AA003: Return type "{type}" is not defined.
* AA004: Function name "{name}" is not valid.
* AA005: Expected "(".
* AA006: Token "{token}" appears between arguments and openscope of {funcName}.
* AA007: Function {funcName} has no closescope.
* AA008: Invalid type for {structName}.{memberName}. lpstring- or void-typed members cannot be in structures. Consider a pointer to these types instead.
* AA009: Invalid token {token} of type {type} after type.
* AA010: The size of the array {structAndMember} was not declared.
* AA011: The size of the array {structAndMember} is out of range ({arraySize}). Array sizes must be between 0 and 2.1 billion.
* AA012: Expected ']'.
* AA013: Invalid type for {structAndMember}. Type {type} is not defined.
* AA014: Maximum struct nesting depth of {depth} reached. Look for circular struct members.
* AA015: Invalid type for {globalVariableName}. Type "void" is not valid; perhaps you meant "void*"?
* AA016: Invalid type for {globalVariableName}. "{word}" is a {thing}, not a type.
* AA017: Invalid type for {globalVariableName}. The type {type} is not defined.
* AA018: Invalid type for {globalVariableName}. No global may be "lpstring", perhaps you meant "lpstring*"?
* AA019: Type "{type}" is not defined.
* AA020: Token "{type}" is not actually a type, it's a {thing}.
* AA021: 