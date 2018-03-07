# Cix Documentation

This folder contains documentation for the Cix compiler platform for IronArc. This documentation is separated into the following documents:

1. Grammar: Defines the proper syntax for Cix files using a form of Backus-Naur grammar.
2. Error Handling: Defines how a compiler should record multiple errors from a file, instead of failing on the first error.
3. Preprocessing: Defines how the preprocessor parses the file, includes other Cix files, and handles conditional code inclusion and token substitutions.
4. Lexing: Defines how a Cix file is parsed, character-by-character, to remove comments and whitespace and to separate the file into words.
5. Tokenization: Defines how the words in Cix files are converted into a token that notes what kind of token it is.
6. Abstract Syntax Tree Generation: Defines how the tokens in a Cix file are converted into a tree that contains the structures, global variables, and the functions with their definitions, statements, and expressions all parsed.
	a. First Pass: Defines how the structures, global variables, and function headers are parsed.
	b. Second Pass: Defines how the statements and expressions inside functions are parsed.
7. Lowering: Defines how the compiler converts more complicated code constructs into simpler, more performant variations.
8. Code Generation: Defines how the compiler converts an abstract syntax tree into an IronArc assembly file.
	a. Structures and Globals Generation: Defines how the compiler determines how structures are laid out in memory and how global variables are allocated within an IronArc assembly file.
	b. Statement Generation: Defines how the compiler generates code for control flow statements such as switch, if, or while.
	c. Expression Type Determination: Defines how the compiler determines the type of individual expression elements and subexpressions made from combining elements and operators.
	d. Expression Generation: Defines how the compiler generates code for expressions.