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

### Finding String Literals
...

### Preprocessor
Preprocessing is performed line-by-line. Thus, when an error is encountered, skip to the next line and continue the preprocessing from there.

### Comment Stage
Certain comment-like constructs, like single forward slashes at the end of a line, are not valid.

### Type Rewriting
...

### ANTLR4 Parsing

Cix uses an ANTLR4 grammar and related generated classes to parse a preprocessed Cix file. The errors that the ANTLR types report are recorded here.

### AST Generation

After ANTLR parses the preprocessed Cix file, Cix then converts ANTLR's parse tree classes into the AST types.

## Error List
### I/O Errors

* IO001: Null or empty file path.
* IO002: File {file} is not a valid path or the file does not exist.
* IO003: File {file} could not be opened for reading. Check permissions or if the file is already opened.
* IO004: File {file} is blank or empty.
* IO005: I/O exception occurred. (include exception details)

### Finding String Literals

* SL001: File ends with unterminated string literal.
* SL002: Escaped double quote found outside string literal.

## Comment Remover Errors

* CR001: Single forward slash at end of line is not a valid comment.

### Preprocessor Errors

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
* PR012: The include file {file} already includes previously included file {file}.
* PR013: An #else was found without a matching #ifdef or #ifndef.
* PR014: An #endif was found without a matching #ifdef or #ifndef.
* PR015: File "file.cix" was already included.

### ANTLR4 Parsing Errors

* PA001: Syntax error: {message}