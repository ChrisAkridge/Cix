# Cix Preprocessor
Cix files may include *preprocessor directives*, which are directives that transform the text of the Cix file before it is given to the lexer. These directives can replace an identifier with another identifier or number, include or exclude parts of the code based on the declaration of certain flags, or include the text of other Cix files in the place of the directive.

Each preprocessor directive is on a single line

## Types of Preprocessor Directives
### `#define`: Define Preprocessor Flag or Substitution
The `#define` directive is used to either declare a preprocessor flag or substitute an identifier for another identifier or number.

Preprocessor flags are identifiers that, when declared, are considered "set" for the remaining portion of the source file. These flags can be used with `#ifdef` directives to conditionally include or exclude code. Preprocessor flags can be defined by placing an identifier after the `#define`: `#define IDENTIFIER`. It is customary that preprocessor flags are expressed in allcaps and words are separated with underscores (`DARA_TRANSFER_READ`).

The same flag cannot be defined twice in a row, but it can be defined again after undefining it. (See `#undefine` below)

**Invalid:**
```
#define IDENTIFIER
#define IDENTIFIER
```

**Valid:**
```
#define IDENTIFIER
#undefine IDENTIFIER
#define IDENTIFIER		
```

Substitutions can state that any instance of an identifier should be replaced with another identifier or with an integer. Any defined substitution will cause the preprocessor to substitute that identifier until either the end of the file or until the substitution is undefined.

To declare a substitution:
```
#define DATA_TRANSFER_READ 0
#define DATA_TRANSFER_WRITE 1
```

The above example substitutions would then transform this example code:
```
	File* read = OpenFile(path1, DATA_TRANSFER_READ);
	File* write = OpenFile(path2, DATA_TRANSFER_WRITE);
```

...into this one:
```
	File* read = OpenFile(path1, 0);
	File* write = OpenFile(path2, 1);
```

Like preprocessor flags, substitutions can be undefined using the `#undefine` directive. The same substitution cannot be declared twice in a row, nor can it be declared again with a different identifier/integer, without first undefining it.

### `#undefine`: Undefine a Preprocessor Flag or Substitution
As mentioned above, `#undefine` undefines a preprocessor flag or substitution from the line of the preprocessor up to the end of the file (or its redefinition, if present). Undefinitions for both preprocessor flags and substitutions take the same form:

```
#undefine IDENTIFIER
#undefine DATA_TRANSFER_READ
#undefine DATA_TRANSFER_WRITE
```

### `#ifdef`, `#ifndef`, `#else`, and `#endif`: Conditional Directives
These four directives start, continue, or end a conditional block in which normal code or other preprocessor directives may be within. Code within these blocks will be included in the preprocessed source file based on the definition (or lack thereof) of preprocessor flags.

`#ifdef` includes the code in its block if a given preprocessor flag is defined. `#ifndef` includes the code in the block if a given preprocessor flag is NOT defined. `#else`, whose block always follows an `#ifdef` or `#ifndef` block, includes the code within its block if the preceding block's code was not included. `#endif` marks the end of the conditional statement. Conditional blocks can be nested inside other conditional blocks, and other preprocessor directives can be used inside conditional blocks, leading to things like conditional flag definition and conditional file inclusion.

In the following example, assume that only the preprocessor flag `MEMORY_MAPPING_ENABLED` is defined.

```
#ifdef MEMORY_MAPPING_ENABLED
	// the code here will be included in the preprocessed source file
#else
	// the code here will NOT be included in the preprocessed source file
#endif

#ifndef USE_ASCII_ONLY
	// the code here will be included in the preprocessed source file
#endif
```

### `#include`: Include the Text of a Cix File
`#include <filepath.cix>` or `#include "filepath.cix"` (both forms are treated as identical) finds and loads a text file at the given path in the folder that the Cix file being processed is in. The text of this loaded file is then preprocessed, then replaces the `#include` directive, and an extra newline is added at the end of the included file. The file path supports files in child and parent folders, too: `#include "io/file.cix"`, `#include "../../program.cix"`.

There cannot be an inclusion loop: `a.cix` cannot include `b.cix` if `b.cix` already includes `a.cix`.