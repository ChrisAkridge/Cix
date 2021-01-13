# Cix Compiler Architecture

Let's try this again.

**Cix** is a C-like, low-level procedural language made for the IronArc platform. It features a simple syntax and semantics roughly equivalent to C, but greatly simplified. Cix files consist only of structs, functions, and global variable declarations. Cix files are joined together with the preprocessor, which can be used to include other Cix files and omit parts of code based on declared symbols. The type system consists of primitive types like `int` and `double`, user-defined types made with structs, and pointers, including function pointers with simplified syntax.

The Cix Compiler, `Celarix.Cix.Compiler`, compiles a Cix file into IronArc assembly. It also provides types that represent and analyze parsed Cix code. The compiler is used by frontends, such as `Celarix.Cix.Console`.

## Compilation Options

- `-i, --input` (required): A path to a single Cix file to compile. Typically, you would select a Cix file that consists only of includes for all the files you want to compile.
- `-o, --output` (required): A path to the IronArc assembly file to write.
- `-s, --symbols`: Declares all the symbols defined for the preprocessor to use. Consists of a comma-separated list of symbol names, such as `USE_FLOATING_POINT,STRING_INTERNING_ENABLED`
- `-t, --save-temps`: Saves intermediate files to the folder named in `--output`. Includes a preprocessed Cix file and a JSON representation of the parsed AST.

## Compiler Architecture

The compilation occurs in phases.

### Preparse Phase

#### I/O

A compilation starts by taking the path to the input file and loading the text of the file into memory. The file is then split into lines, where each line stores its file path, line number, and starting character index in the file.

#### Preprocessor

The preprocessor receives a list of lines and iterates through them, using preprocessor statements to include, remove, and substitute parts of code based on symbols declared by compilation options and `#define` directives in the code.

After preprocessing, the list of lines contains all the included files. Each line has both its line number inside its file, and the overall line number, as if the input file had had all its includes present all along. It also contains the starting character index and the overall starting character index, in a similar fashion.

As an example, consider a file `file1.cix` ending in LF newlines. The tuple at the beginning of each line stores the file line number, overall line number, file path, file starting character number, and overall starting character index:

```c
{ 1, 1, file1.cix, 0, 0 } 	#include <file2.cix>⏎
{ 2, 2, file1.cix, 22, 22 } ⏎
{ 3, 3, file1.cix, 23, 23 } int func()⏎
{ 4, 4, file1.cix, 34, 34 } {⏎
{ 5, 5, file1.cix, 36, 36 }     return 2;⏎
{ 6, 6, file1.cix, 50, 50 } }⏎
```

After preprocessing:

```c
{ 1, 1, file2.cix, 0, 0 }	int func2() { return 3; }⏎
{ 2, 1, file1.cix, 0, 26 }	⏎
{ 3, 2, file1.cix, 1, 27 }	int func()⏎
{ 4, 3, file1.cix, 12, 38 }	{⏎
{ 5, 4, file1.cix, 14, 40 }		return 2;⏎
{ 6, 5, file1.cix, 28, 54 }	}⏎
```

Note how the overall line and starting character indices increase with each line, while the file line and starting character indices reset at the start of each file.

#### String Literal Marker

This stage receives a list of lines. It then searches and marks the starting and ending locations of every string literal, so that future phases do not parse elements inside the literals. The result of this stage is a list of lines where each line also has a list of line character index ranges representing the start and end of each string literal on the line, including the double quotes. Line character indices start start from 0 for the beginning of each line.

#### Comment Remover

This stage receives a list of lines with string literals marked. It replaces all comments not in string literals with spaces. The output is a list of lines with string literals marked.

If `--save-temps` is used, the compiler will save a file to the folder named in `--output`. It saves a file containing the preprocessed Cix file. The file's name starts with the name of the input file (minus extension), followed by `_preprocessed.cix`.

#### Type Finder

This stage receives the uncommented list of lines and searches for the `struct` keyword outside of string literals. It then takes up the next token, which should be an identifier, and adds it to a list of type names that also contains the primitive types. The output is a list of lines alongside a list of all types declared in the file.

#### Pointer Type Rewriter

To disambiguate the grammar, this stage finds all the tokens that name a primitive or user-defined type. It then looks at the next token, and, if the token is one or more asterisks, replaces all the asterisks with backticks. This also rewrites the asterisks inside of function pointer types. This allows `x * y` to always mean `x` multiplied by `y` instead of possibly a variable of type `x*` named `y`.

### Parsing Phase

#### ANTLR Parsing

The compiler uses a parser created by ANTLR4 from a grammar. The output of this stage is a parse tree made from generated ANTLR types.

#### AST Generation

This stage takes the ANTLR tree and converts it into Cix AST types, along with file, line, and character information. The result is a tree of Cix AST elements.

If `--save-temps` is used, the compiler will save a file to the folder named in `--output`. It saves a JSON file containing the AST. The file's name starts with the name of the input file (minus extension), followed by `_ast.json`.

Consumers of the compiler library have access to a type that can pretty-print the AST as a Cix file.

### Code Generation

#### Lowering

This stage performs lowerings to simplify the rest of the compilation process. Currently, the lowerings supported are:

- Rewriting `for` loops as `while` loops.

#### Code Generation

This stage finally converts the AST into an IronArc assembly file. Finally, the assembly file is saved to the path specified in `--output`.

## Compiler Guidelines

### 

### Testing