# Grammar

This document describes the grammar of the Cix language. The grammar defines what every higher-level construct (code statements, structure declaractions, etc.) is made out of, down to the very basics, called *terminal symbols*, which are individual characters or groups of characters.

```
cix-file:
	Zero or more of { preprocessor-directive structure-declaration global-variable-declaration function }

newline:
	Carriage return character (U+000D) or
	Line feed character (U+000A) or
	Carriage return character (U+000D) followed by linefeed character (U+000A)

whitespace:
	Space (U+0020)
	Tab (U+0009)
	
decimal-numeric-constant:
	One or more characters from {0123456789}
	
hexadecimal-numeric-constant:
	"0x" or "0X" then
	One or more characters from {0123456789ABCDEF}

numeric-literal:
	decimal-numeric-constant or hexadecimal-numeric-constant then
	Zero or one of { u ul f d }

string-literal:
	Quotation Mark (U+0022) then
	Zero or more string-literal-characters then
	Quotation Mark (U+0022)

string-literal-character:
	Any character except \ or string-literal-escaped-character

string-literal-escaped-character:
	Reverse Solidus (U+005C) then
	One of { single-character-escape unicode-character }

single-character-escape:
	One of { \ " a b f n r t v }

unicode-character:
	Reverse Solidus (U+005C) then
	One of { U u } then
	Four hexadecimal-digits

hexadecimal-digit:
	One of { 0 1 2 3 4 5 6 7 8 9 A a B b C c D d E e F f }

identifier:
	One character from {ABCDEFGHIKLMNOPQRSTUVWXYZabcdefghijlmnopqrstuvwxyz_} then
	Zero or more characters from {ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghihjklmnopqrstuvwxyz0123456789_}

open-scope:
	Left Curly Bracket (U+007B)

close-scope:
	Right Curly Bracket (U+007D)

preprocessor-directive:
	Number Sign (U+0023) then
	Directive Name (case insensitive, one of {define, undefine, ifdef, ifndef, else, endif, include}) then
	One or more whitespaces then
	One or more of { compile-time-constant, compile-time-substitution, local-include-path, global-include-path } then
	newline

compile-time-constant:
	Follows only one of [#define #undefine #ifdef #ifndef] then one or more whitespaces
	identifier

compile-time-substitution:
	Follows only #define then one or more whitespaces
	Starts with compile-time-substitution-to-replace then
	One or more whitespace then
	compile-time-substitution-replacement

compile-time-substution-to-replace:
	identifier

compile-time-substitution-replacement:
	One or more characters from {ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghihjklmnopqrstuvwxyz0123456789_}

local-include-path:
	Follows only #include then one or more whitespaces
	Starts with Quotation Mark (U+0022) then
	One or more characters from {ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghihjklmnopqrstuvwxyz0123456789_:/.-()!#$%^&*+=,} then
	Quotation Mark (U+0022)

global-include-path:
	Follows only #include then one or more whitespaces
	Starts with Less Than Sign (U+003C) then
	One or more characters from {ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghihjklmnopqrstuvwxyz0123456789_:/.-()!#$%^&*+=,} then
	Greater Than Sign (U+003E)

structure-declaration:
	The string "struct" then
	One or more whitespaces then
	identifier then
	Zero or more whitespaces or line terminators then
	open-scope then
	Zero or more whitespaces or line terminators then
	One or more structure-member-declarations then
	Zero or more whitespaces or line terminators then
	close-scope then
	One or more line terminators

structure-member-declaration:
	type-name or pointer-type-name then
	One or more whitespaces then
	identifier then
	Optional structure-member-offset
	Semicolon (U+003B) then
	One or more whitespaces or line terminators

type-name:
	identifier or
	funcptr-type
	
funcptr-type:
	At Sign (U+0040) then
	"funcptr" then
	Less-Than Sign (U+003C) then
	Zero or more whitespaces then
	funcptr-returnonly-type or funcptr-type-list then
	Zero or more whitespaces then
	Greater-Than Sign (U+003E)
	
funcptr-returnonly-type:
	type-name
	
funcptr-type-list:
	Two or more of funcptr-type
	
funcptr-type:
	type-name or pointer-type-name then
	Comma (U+002C) if not the last type in the list
	Zero or more whitespaces
	
pointer-type-name:
	type-name then
	Zero or more whitespaces then
	One or more Asterisks (U+002A)

structure-member-offset:
	At Sign (U+0040) then
	One or more whitespaces then
	decimal-numeric-constant
	
global-variable-declaration:
	global then
	One or more whitespaces then
	type-name or pointer-type-name then
	One or more whitespaces then
	identifier then
	Zero or more whitespaces then
	semicolon

function:
	function-declaration then
	Zero or more whitespaces or line terminators then
	open-scope then
	Zero or more statements then
	Zero or more whitespaces or line terminators then
	close-scope then
	Zero or more whitespaces or line terminators

function-declaration:
	type-name or pointer-type-name then
	One or more whitespaces or line terminators then
	identifier then
	Zero or more whitespaces or line terminators then
	Left parentheses (U+0028) then
	Zero or more whitespaces then
	Zero or one function-parameter-list then
	Zero or more whitespaces or line terminators then
	Right parentheses (U+0029)

function-parameter-list:
	One or more function-parameter then
	Zero or one instance of "..."

function-parameter:
	type-name or pointer-type-name then
	One or more whitespaces or line terminators then
	identifier then
	Comma (U+002C) if not last in the list of function-parameters then
	Zero or more whitespaces or line terminators

statement:
	One of { open-scope, close-scope, single-statement, variable-declaration, variable-declaration-initalization, expression, conditional-if, conditional-elseif, conditional-else, loop-for, loop-while, loop-do, goto, switch } then
	Zero or more whitespaces or line terminators

single-statement:
	One of { break, continue }

variable-declaration:
	Zero or one of { const, register } then
	One or more whitespaces or line terminators then
	type-name or pointer-type-name then
	One or more whitespaces or line terminators then
	identifier then
	Semicolon (U+003B)

variable-declaration-initialization:
	Optionally register then
	One or more whitespaces or line terminators then
	type-name or pointer-type-name then
	One or more whitespaces or line terminators then
	identifier then
	Zero or more whitespaces or line terminators then
	Equals Sign (U+003D) then
	Zero or more whitespaces or line terminators then
	Expression then
	Semicolon (U+003B)

expression:
	Any expression element then
	Zero or more whitespaces or line terminators then
	Any expression element then...
	
expression-value:
	One of { expression-variable expression-literal }

expression-variable:
	identifier
	
expression-literal:
	One of { numeric-literal string-literal }

unary-prefix-element:
	One of { + - ! ~ ++ -- typecast * & sizeof } then
	One of { expression-variable expression-literal }

unary-postfix-element:
	One of { function-call-element array-member-access-element pointer-member-access-element dot-member-access-element postfix-increment-element postfix-decrement-element }

binary-expression:
	One of { expression expression-value unary-prefix-element unary-postfix-element } then
	Zero or more whitespaces or line terminators then
	One of { * / % + - << >> < <= > >= == != & ^ | && || = += -= *= /= %= >>= <<= &= |= ^= } then
	Zero or more whitespaces or line terminators then
	One of { expression expression-value unary-prefix-element unary-postfix-element }

ternary-expression:
	One of { expression expression-value unary-prefix-element unary-postfix-element } then
	Zero or more whitespaces or line terminators then
	Question Mark (U+003F) then
	Zero or more whitespaces or line terminators then
	One of { expression expression-value unary-prefix-element unary-postfix-element } then
	Colon (U+003A) then
	Zero or more whitespaces or line terminators then
	One of { expression expression-value unary-prefix-element unary-postfix-element }

function-call-element:
	identifier then
	Zero or more whitespaces then
	Left parentheses (U+0028) then
	function-call-arguments then
	Zero or more whitespaces then
	Right parentheses (U+0029)

function-call-arguments:
	Zero or more function-call-argument then
	Comma (U+002C) if not last argument in list then
	Zero or more whitespaces

function-call-argument:
	One of { expression-literal expression-variable expression }

array-member-access-element:
	One of { expression-variable expression } then
	Left Square Bracket (U+005B) then
	One of { numeric-literal expression-variable expression } then
	Right Square Bracket (U+005D) then

pointer-member-access-element:
	One of { expression-variable expression } then
	-> then
	identifier

dot-member-access-element:
	One of { expression-variable expression } then
	Full stop (U+002E) then
	identifier

postfix-increment-element:
	One of { expression-variable expression } then
	++

postfix-decrement-element:
	One of { expression-variable expression } then
	--

conditional-if:
	if then
	Zero or more whitespaces or line terminators then
	Left Parentheses (U+0028) then
	Zero or more whitespaces or line terminators then
	One of { expression-literal expression-variable expression } then
	Right Parentehses (U+0029) then
	Zero or more whitespaces or line terminators then
	open-scope then
	Zero or more statements then
	close-scope

conditional-elseif:
	else then
	One or more whitespaces then
	conditional-if

conditional-else:
	else then
	Zero or more whitespaces or line terminators then
	open-scope then
	Zero or more statements then
	close-scope

loop-for:
	for then
	Zero or more whitespaces or line terminators then
	Left Parentheses (U+0028) then
	Zero or more whitespaces or line terminators then
	One of { variable-declaration-initialization expression } then
	Semicolon (U+003B) then
	Zero or more whitespaces or line terminators then
	expression then
	Semicolon (U+003B) then
	Zero or more whitespaces or line terminators then
	Expression then
	Right Parentheses (U+0029) then
	Zero or more whitespaces or line terminators then
	scope-open then
	Zero or more statements then
	scope-close

loop-while:
	while then
	Zero or more whitespaces or line terminators then
	Left Parentheses (U+0028) then
	expression then
	Right Parentheses (U+0029) then
	Zero or more whitespaces or line terminators then
	scope-open then
	Zero or more statements then
	scope-close
	
loop-do:
	do then
	Zero or more whitespaces or line terminators then
	scope-open then
	Zero or more statements then
	scope-close then
	Zero or more whitespaces or line terminators then
	Left Parentheses (U+0028) then
	Zero or more whitespaces or line terminators then
	Expression then
	Zero or more whitespaces or line terminators then
	Right Parentheses (U+0029)

switch:
	switch then
	Zero or more whitespaces or line terminators then
	open-scope then
	Zero or more switch-statements then
	Zero or one switch-default-statement then
	close-scope

switch-statement:
	case then
	One or more whitespaces or line terminators then
	expression-literal then
	Colon (U+003A) then
	One or more whitespaces or line terminators then
	Zero or more statements

switch-default-statement:
	default then
	Colon (U+003A) then
	One or more whitespaces or line terminators then
	Zero or more statements
```