# Lexer

The lexer scans over a preprocessed Cix file character-by-character and is the second stage of compilation. It builds individual words by determining if a character is in a legal position. It removes comments and whitespace, then separates a file into a list of words.

## Contexts

The lexer's context is its state. The context can be set by the character most recently scanned. The contexts are:


* Root: No context has been set yet.
* Whitespace: The last character was a whitespace or line terminator.
* Comment: A // or /* sequence not inside a string literal was found and no line terminator or */ sequence has been reached yet.
* Word: The last charater was a character in a word or identifier not inside a string literal.
* Directive: A # character not inside a string literal was found, and no whitespace has been reached yet.
* Operator: The last character was an operator character.
* NumericLiteral: A number after whitespace not inside a string literal was found, and no . character, {u l f d} character, or whitespace has been reached yet.
* NumericLiteralFraction. A . character was found during a NumericLiteral context, and no {u l f d} character or whitespace has been reached yet.
* NumericLiteralSuffix: A {u l f d} character was found during a NumericLiteral or NumericLiteralFraction context, and no whitespace has been reached yet.
* StringLiteral: An unescaped " character not inside a string literal was found, and no matching unescaped " character has been reached yet.

## What to Do for Each Character
### Whitespace or Line Terminator
* Root: Ignore.
* Whitespace: Ignore.
* Comment: Ignore whitespace. On Line Terminator, if commentKind is CommentKind.SingleLine, change context to Whitespace.
* Word: Yield the builder.
* Directive: Yield the builder.
* Operator: Yield the builder.
* NumericLiteral: Yield the builder.
* NumericLiteralFraction: Yield the builder.
* NumericLiteralSuffix: Yield the builder.
* StringLiteral: Append "\r\n" to the builder.

### Brace, Bracket, or Parenthesis
* Root: INVALID. These cannot appear as the first character. Add an error to the error list.
* Whitespace: Yield the character.
* Comment: Ignore.
* Word: Yield the word, and then the character.
* Directive: INVALID. These do not appear in a preprocessor directive. Add an error to the error list.
* Operator: INVALID. These do not succeed operators. Add an error to the error list.
* NumericLiteral: INVALID. Braces do not appear immediately after numeric literals. Brackets and parentheses only follow a number if it's part of a word. Add an error to the error list.
* NumericLiteralFraction: Same as NumericLiteral.
* NumericLiteralSuffix: Same as NumericLiteral.
* StringLiteral: Append to the builder.

### Letter or Underscore
* In Root: Starts an identifier. Switch to Word and append to builder.
* In Whitespace: Starts an identifier. Switch to Word and append to builder.
* In Comment: Ignore.
* In Word: Append to builder.
* In Directive: Is it not an underscore? Append to builder. No? INVALID. Underscores do not appear in preprocessor directives. Add an error to the error list.
* In Operator: Yield and clear the builder. Append to builder and change context to Word.
* In NumericLiteral: Is it {u l f d}? Switch to NumericLiteralSuffix and append to builder. No? INVALID. No other letters appear within numeric literals. Add an error to the error list.
* In NumericLiteralFraction: Same as in NumericLiteral.
* In NumericLiteralSuffix: Is it l and the last letter u? Append to builder and yield. No? INVALID. No other character appears after u and no other character is used as a second character. Throw
* In StringLiteral: Append to builder.

### Quotation Mark
* In Root: INVALID. String literals cannot be started in a root context. Add an error to the error list.
* In Whitespace: Append the quote to the builder and change context to StringLiteral.
* In Comment: Ignore.
* In Word: INVALID. Quotation marks may not appear in words. Add an error to the error list.
* In Directive: Same as in Word.
* In Operator: Yield and clear the builder. Append to builder and change context to StringLiteral.
* In NumericLiteral: Same as in Word.
* In NumericLiteralFraction: Same as in Word.
* In NumericLiteralSuffix: Same as in Word.
* In StringLiteral: Check previous character. If it's a \, append a quote to the builder. If it's not, append to builder, yield, and switch context to Whitespace.

### Plus Sign (+)
* In Root: INVALID. This character is invalid in a root context. Add an error to the error list.
* In Whitespace: Append to the builder and change context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Check if last character was +. If so, append to builder, yield, change context to Whitespace, otherwise add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Same as in NumericLiteral.
* In NumericLiteralSuffix: Same as in NumericLiteral.
* In StringLiteral: Append to builder.

### Minus Sign (-)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Check if last character was -. If so, append to builder, yield, and change context to Whitespace, otherwise add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Same as in NumericLiteral.
* In NumericLiteralSuffix: Same as in NumericLiteral.
* In StringLiteral: Append to builder.

### Exclamation Mark (!)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Is the next character =? If yes, yield the builder, switch context to Operator, and append to builder. If not, add an error to the error list.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Same as in NumericLiteral.
* In NumericLiteralSuffix: Same as in NumericLiteral.
* In StringLiteral: Append to builder.

### Tilde (~)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Is the next character =? If yes, yield the builder, switch context to Operator, and append to builder. If not, add an error to the error list.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Same as in Word.
* In NumericLiteralFraction: Same as in Word.
* In NumericLiteralSuffix: Same as in Word.
* In StringLiteral: Append to builder.

### Asterisk (*)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Same as in NumericLiteral.
* In NumericLiteralSuffix: Same as in NumericLiteral.
* In StringLiteral: Append to builder.

### Back Slash (/)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Is the next character { * / }? If so, change context to Comment. Otherwise, append to builder and change context to Operator.
* In Comment: Is the last character *? If so, change context to Whitespace. Otherwise, ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Percent Sign (%)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Less-Than Sign (<)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and switch context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Check if the last character was <; if so, append to builder, otherwise add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Greater-Than Sign
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and switch context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Check if the last character was one of { * > }; if so, append to builder, otherwise add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Ampersand (&)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and switch context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Check if the last character was &; if so, append to builder, otherwise add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Pipe (|)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and switch context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Check if the last character was |; if so, append to builder, otherwise add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Caret (^)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and switch context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Dot (.)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: INVALID. Add an error to the error list.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Append to builder and switch context to NumericLiteralFraction.
* In NumericLiteralFraction: INVALID. Add an error to the error list.
* In NumericLiteralSuffix: INVALID. Add an error to the error list.
* In StringLiteral: Append to builder.

### Question Mark (?)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Colon (:)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Equals Sign (=)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: Append to builder and change context to Operator.
* In Comment: Ignore.
* In Word: Yield and clear the builder. Append to builder and change context to Operator.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Check if last character was one of { < > = + * * / % < > & | ^ }. If so, append to builder and change context to Whitespace. If not, add an error to the error list.
* In NumericLiteral: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralFraction: Yield and clear the builder. Append to builder and change context to Operator.
* In NumericLiteralSuffix: Yield and clear the builder. Append to builder and change context to Operator.
* In StringLiteral: Append to builder.

### Forward Slash (\\)
* In Root: INVALID. Add an error to the error list.
* In Whitespace: INVALID. Add an error to the error list.
* In Comment: Ignore.
* In Word: INVALID. Add an error to the error list.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: INVALID. Add an error to the error list.
* In NumericLiteralFraction: INVALID. Add an error to the error list.
* In NumericLiteralSuffix: INVALID. Add an error to the error list.
* In StringLiteral: Append to builder.

### Semicolon (;)
* In Root: Append to builder and change context to Whitespace.
* In Whitespace: Same as in Root.
* In Comment: Ignore.
* In Word: Same as in Root.
* In Directive: INVALID. Add an error to the error list.
* In Operator: Same as in Root.
* In NumericLiteral: Same as in Root.
* In NumericLiteralFraction: Same as in Root.
* In NumericLiteralSuffix: Same as in Root.
* In StringLiteral: Append to builder.

### Any Other Character
* In Root: INVALID. Add an error to the error list.
* In Whitespace: INVALID. Add an error to the error list.
* In Comment: Ignore.
* In Word: INVALID. Add an error to the error list.
* In Directive: INVALID. Add an error to the error list.
* In Operator: INVALID. Add an error to the error list.
* In NumericLiteral: INVALID. Add an error to the error list.
* In NumericLiteralFraction: INVALID. Add an error to the error list.
* In NumericLiteralSuffix: INVALID. Add an error to the error list.
* In StringLiteral: Append to builder.