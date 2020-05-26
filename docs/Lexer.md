# Lexer

The lexer scans over a preprocessed Cix file character-by-character and is the second stage of compilation. It builds individual words by determining if a character is in a legal position. It removes comments and whitespace, then separates a file into a list of words.

## Contexts

The lexer's context is its state. The context can be set by the character most recently scanned. The contexts are:

* Root: No context has been set yet.
* Whitespace: The last character was a whitespace or line terminator.
* Word: The last charater was a character in a word or identifier not inside a string literal.
* Operator: The last character was an operator character.
* NumericLiteral: A number after whitespace not inside a string literal was found, and no . character, {u l f d} character, or whitespace has been reached yet.
* NumericLiteralFraction. A . character was found during a NumericLiteral context, and no {u l f d} character or whitespace has been reached yet.
* NumericLiteralSuffix: A {u l f d} character was found during a NumericLiteral or NumericLiteralFraction context, and no whitespace has been reached yet.
* HexadecimalNumericLiteral: An 'x' character was found during a NumericLiteral context, and no whitespace has been reached yet.
* StringLiteral: An unescaped " character not inside a string literal was found, and no matching unescaped " character has been reached yet.

## What to Do for Each Character
### Whitespace or Line Terminator
* Root: Ignore.
* Whitespace: Ignore.
* Word: Yield the builder.
* Operator: Yield the builder.
* NumericLiteral: Yield the builder.
* NumericLiteralFraction: Yield the builder.
* NumericLiteralSuffix: Yield the builder.
* StringLiteral: Append "\r\n" to the builder.

### Brace, Bracket, or Parenthesis
* Root: INVALID. These cannot appear as the first character. Add an error to the error list.
* Whitespace: Yield the character.
* Word: Yield the word, and then the character.
* Operator: INVALID. These do not succeed operators. Add an error to the error list.
* NumericLiteral: INVALID. Braces do not appear immediately after numeric literals. Brackets and parentheses only follow a number if it's part of a word. Add an error to the error list.
* NumericLiteralFraction: Same as NumericLiteral.
* NumericLiteralSuffix: Same as NumericLiteral.
* StringLiteral: Append to the builder.

### At Sign (@)
* Root: Starts an intrinsic. Switch to Word and append to builder.
* Whitespace: Starts an intrinsic. Switch to Word and append to builder.
* Word: INVALID. The At Sign can only start a word. Add an error to the error list.
* Operator: Yield and clear the builder. Append to builder and change context to Word.
* NumericLiteral: INVALID. At Signs require whitespace after a number to be parsed as an intrinsic. Add an error to the error list.
* NumericLiteralFraction: INVALID. At Signs require whitespace after a number to be parsed as an intrinsic. Add an error to the error list.
* NumericLiteralSuffix: INVALID. At Signs require whitespace after a number to be parsed as an intrinsic. Add an error to the error list.
* HexadecimalNumericLiteral: INVALID. At Signs require whitespace after a number to be parsed as an intrinsic. Add an error to the error list.
* StringLiteral: Append to builder.

### Letter or Underscore
* Root: Starts an identifier. Switch to Word and append to builder.
* Whitespace: Starts an identifier. Switch to Word and append to builder.
* Word: Append to builder.
* Operator: Yield and clear the builder. Append to builder and change context to Word.
* NumericLiteral: Is it {u l f d}? Switch to NumericLiteralSuffix and append to builder. No? INVALID. No other letters appear within numeric literals. Add an error to the error list.
* NumericLiteralFraction: Same as in NumericLiteral.
* NumericLiteralSuffix: Is it l and the last letter u? Append to builder and yield. No? INVALID. No other character appears after u and no other character is used as a second character. Throw
* StringLiteral: Append to builder.

### Quotation Mark (")
* Root: INVALID. String literals cannot be started in a root context. Add an error to the error list.
* Whitespace: Append the quote to the builder and change context to StringLiteral.
* Word: INVALID. Quotation marks may not appear in words. Add an error to the error list.
* Operator: Yield and clear the builder. Append to builder and change context to StringLiteral.
* NumericLiteral: Same as in Word.
* NumericLiteralFraction: Same as in Word.
* NumericLiteralSuffix: Same as in Word.
* StringLiteral: Check previous character. If it's a \, append a quote to the builder. If it's not, append to builder, yield, and switch context to Whitespace.

### Plus Sign (+)
* Root: INVALID. This character is invalid in a root context. Add an error to the error list.
* Whitespace: Append to the builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Check if last character was +. If so, append to builder, yield, change context to Whitespace, otherwise add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Same as in NumericLiteral.
* NumericLiteralSuffix: Same as in NumericLiteral.
* StringLiteral: Append to builder.

### Minus Sign (-)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Check if last character was -. If so, append to builder, yield, and change context to Whitespace, otherwise add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Same as in NumericLiteral.
* NumericLiteralSuffix: Same as in NumericLiteral.
* StringLiteral: Append to builder.

### Exclamation Mark (!)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Is the next character =? If yes, yield the builder, switch context to Operator, and append to builder. If not, add an error to the error list.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Same as in NumericLiteral.
* NumericLiteralSuffix: Same as in NumericLiteral.
* StringLiteral: Append to builder.

### Tilde (~)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Is the next character =? If yes, yield the builder, switch context to Operator, and append to builder. If not, add an error to the error list.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: Same as in Word.
* NumericLiteralFraction: Same as in Word.
* NumericLiteralSuffix: Same as in Word.
* StringLiteral: Append to builder.

### Asterisk (*)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Invalid unless the lass character was an asterisk; otherwise, append to builder.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Same as in NumericLiteral.
* NumericLiteralSuffix: Same as in NumericLiteral.
* StringLiteral: Append to builder.

### Back Slash (/)
* Root: INVALID. Add an error to the error list.
* Whitespace: Is the next character { * / }? If so, change context to Comment. Otherwise, append to builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Percent Sign (%)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Less-Than Sign (<)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and switch context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Check if the last character was <; if so, append to builder, otherwise add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Greater-Than Sign (>)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and switch context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Check if the last character was one of { * > }; if so, append to builder, otherwise add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Ampersand (&)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and switch context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Check if the last character was &; if so, append to builder, otherwise add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Pipe (|)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and switch context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Check if the last character was |; if so, append to builder, otherwise add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Caret (^)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and switch context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Dot (.)
* Root: INVALID. Add an error to the error list.
* Whitespace: INVALID. Add an error to the error list.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: Append to builder and switch context to NumericLiteralFraction.
* NumericLiteralFraction: INVALID. Add an error to the error list.
* NumericLiteralSuffix: INVALID. Add an error to the error list.
* StringLiteral: Append to builder.

### Question Mark (?)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Colon (:)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Equals Sign (=)
* Root: INVALID. Add an error to the error list.
* Whitespace: Append to builder and change context to Operator.
* Word: Yield and clear the builder. Append to builder and change context to Operator.
* Operator: Check if last character was one of { < > = + * * / % < > & | ^ }. If so, append to builder and change context to Whitespace. If not, add an error to the error list.
* NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* StringLiteral: Append to builder.

### Forward Slash (\\)
* Root: INVALID. Add an error to the error list.
* Whitespace: INVALID. Add an error to the error list.
* Word: INVALID. Add an error to the error list.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: INVALID. Add an error to the error list.
* NumericLiteralFraction: INVALID. Add an error to the error list.
* NumericLiteralSuffix: INVALID. Add an error to the error list.
* StringLiteral: Append to builder.

### Semicolon (;)
* Root: Append to builder and change context to Whitespace.
* Whitespace: Same as in Root.
* Word: Same as in Root.
* Operator: Same as in Root.
* NumericLiteral: Same as in Root.
* NumericLiteralFraction: Same as in Root.
* NumericLiteralSuffix: Same as in Root.
* StringLiteral: Append to builder.

### At Sign (@)
* Root: Starts an identifier to be treated as an intrinsic. Switch context to Word and append to builder.
* Whitespace: Starts an identifier to be treated as an intrinsic. Switch context to Word and append to builder.
* Word: INVALID. At signs only appear at the start of intrinsic identifiers. Add an error to the error list.
* Operator: Yield and clear the builder. Append to builder and change context to Word.
* NumericLiteral: Is it {u l f d}? Switch to NumericLiteralSuffix and append to builder. No? INVALID. No other letters appear within numeric literals. Add an error to the error list.
* NumericLiteralFraction: Same as in NumericLiteral.
* NumericLiteralSuffix: Is it l and the last letter u? Append to builder and yield. No? INVALID. No other character appears after u and no other character is used as a second character. Throw
* StringLiteral: Append to builder.

### Any Other Character
* Root: INVALID. Add an error to the error list.
* Whitespace: INVALID. Add an error to the error list.
* Word: INVALID. Add an error to the error list.
* Operator: INVALID. Add an error to the error list.
* NumericLiteral: INVALID. Add an error to the error list.
* NumericLiteralFraction: INVALID. Add an error to the error list.
* NumericLiteralSuffix: INVALID. Add an error to the error list.
* StringLiteral: Append to builder.