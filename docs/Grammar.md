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
	Space (U+0020) or
	Tab (U+0009)
	
basic-numeric-literal:
	Zero or one - then
	One to eighteen of {0123456789}
	
basic-hexadecimal-literal:
	Zero or one - then
	"0x" or "0X" then
	One to sixteen of {0123456789ABCDEFabcdef}
	
suffixed-numeric-literal:
	basic-numeric-literal then
	One of {u l ul}

suffixed-hexadecimal-literal:
	basic-hexadecimal-literal then
	One of {u l ul}

floating-literal-with-decimal-point:
	Zero or one - then
	One or more of {0123456789} then
	. then
	One or more of {0123456789}
	
suffixed-floating-literal:
	basic-numeric-literal or floating-literal-with-decimal-point then
	One of {f d}
	
floating-literal-with-exponent:
	floating-literal-with-decimal-point then
	e then
	+ or - then
	One to three of {0123456789}
	
numeric-literal:
	basic-numeric-literal or
	basic-hexadecimal-literal or
	suffixed-numeric-literal or
	suffixed-hexadecimal-literal or
	floating-literal-with-decimal-point or
	suffixed-floating-literal or
	floating-literal-with-exponent
	
string-literal:
	" then
	Zero or more string-literal-characters then
	"

string-literal-character:
	Any character except {\ " '} or string-literal-escaped-character

string-literal-escaped-character:
	\ then
	One of { single-character-escape unicode-short-escape unicode-long-escape }

single-character-escape:
	One of { \ " a b f n r t v }

unicode-short-escape:
	\u then
	Four hexadecimal-digits
	
unicode-long-espace:
	\U then
	Eight hexadecimal-digits

hexadecimal-digit:
	One of { 0 1 2 3 4 5 6 7 8 9 A a B b C c D d E e F f }

identifier:
	One character from {ABCDEFGHIKLMNOPQRSTUVWXYZabcdefghijlmnopqrstuvwxyz_} then
	Zero or more characters from {ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghihjklmnopqrstuvwxyz0123456789_}

open-scope:
	{

close-scope:
	}

preprocessor-directive:
	# then
	Directive Name (case insensitive, one of {define, undefine, ifdef, ifndef, else, endif, include}) then
	One or more whitespaces then
	One or more of { compile-time-constant, compile-time-substitution, local-include-path, global-include-path } then
	newline

compile-time-constant:
	identifier

compile-time-substitution:
	identifier then
	One or more whitespace then
	compile-time-substitution-replacement

compile-time-substitution-replacement:
	One or more characters from {ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghihjklmnopqrstuvwxyz0123456789_}

local-include-path:
	" then
	include-path then
	"

global-include-path:
	< then
	include-path then
	>
	
include-path:
	One or more characters from {ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghihjklmnopqrstuvwxyz0123456789_:/.-()!#$%^&*+=,}
	
structure-declaration:
	struct then
	One or more whitespaces then
	identifier then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more whitespaces or newlines then
	One or more structure-member-declarations then
	Zero or more whitespaces or newlines then
	close-scope then
	One or more newlines

structure-member-declaration:
	type-name then
	Zero or one array-size-declaration then
	One or more whitespaces then
	identifier then
	Semicolon (U+003B) then
	One or more whitespaces or newlines

type-name:
	identifier or funcptr-type then
	Zero or more *
	
funcptr-type:
	@ then
	"funcptr" then
	< then
	Zero or more whitespaces then
	funcptr-returnonly-type or funcptr-type-list then
	Zero or more whitespaces then
	>
	
funcptr-returnonly-type:
	type-name
	
funcptr-type-list:
	Two or more of funcptr-type
	
funcptr-type:
	type-name or pointer-type-name then
	, if not the last type in the list
	Zero or more whitespaces
	
global-variable-declaration:
		global-variable-declaration-no-initializer or global-variable-declaration-with-initializer or global-variable-array-declaration

global-variable-declaration-core:
	global then
	One or more whitespaces then
	type-name then
	One or more whitespaces then
	identifier then
	Zero or more whitespaces then	

global-variable-declaration-no-initializer:
	global-variable-declaration-core then
	;
	
global-variable-declaration-with-initializer:
	global-variable-declaration-core then
	= then
	Zero or more whitespaces then
	numeric-literal
	
global-variable-array-declaration:
	global then
	One or more whitespaces then
	type-name then
	array-size-declaration then
	One or more whitespaces then
	identifier then
	;

function:
	function-declaration then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more statements then
	Zero or more whitespaces or newlines then
	close-scope then
	Zero or more whitespaces or newlines

function-declaration:
	type-name then
	One or more whitespaces or newlines then
	identifier then
	Zero or more whitespaces or newlines then
	( then
	Zero or more whitespaces then
	Zero or one function-parameter-list then
	Zero or more whitespaces or newlines then
	)

function-parameter-list:
	One or more function-parameter

function-parameter:
	type-name then
	One or more whitespaces or newlines then
	identifier then
	, if not last in the list of function-parameters then
	Zero or more whitespaces or newlines

statement:
	One of { open-scope, close-scope, single-statement, variable-declaration, variable-declaration-initalization, expression, conditional-if, conditional-elseif, conditional-else, loop-for, loop-while, loop-do, goto, switch } then
	Zero or more whitespaces or newlines

single-statement:
	One of { break, continue }

variable-declaration:
	type-name then
	One or more whitespaces or newlines then
	identifier then
	Semicolon (U+003B)

variable-declaration-initialization:
	type-name then
	One or more whitespaces or newlines then
	identifier then
	Zero or more whitespaces or newlines then
	= then
	Zero or more whitespaces or newlines then
	Expression then
	;

expression:
	Any expression element then
	Zero or more whitespaces or newlines then
	Any expression element then...
	
expression-value:
	One of { expression-variable expression-literal }

expression-variable:
	identifier
	
expression-literal:
	One of { numeric-literal string-literal }

unary-prefix-element:
	One of { + - ! ~ ++ -- * & sizeof } or typecast then
	One of { expression-variable expression-literal }

unary-postfix-element:
	One of { function-call-element array-member-access-element pointer-member-access-element dot-member-access-element postfix-increment-element postfix-decrement-element }

binary-expression:
	One of { expression expression-value unary-prefix-element unary-postfix-element } then
	Zero or more whitespaces or newlines then
	One of { * / % + - << >> < <= > >= == != & ^ | && || = += -= *= /= %= >>= <<= &= |= ^= } then
	Zero or more whitespaces or newlines then
	One of { expression expression-value unary-prefix-element unary-postfix-element }

function-call-element:
	function-expression then
	Zero or more whitespaces then
	Left parentheses (U+0028) then
	function-call-arguments then
	Zero or more whitespaces then
	Right parentheses (U+0029)
	
function-expression:
	identifier or hwcall-intrinsic or function-pointer-expression
	
	
hwcall-intrinsic:
	"@hwcall"
	
function-pointer-expression:
	( then
	Zero or more whitespaces then
	expression then
	Zero or more whitespaces then
	)

function-call-arguments:
	Zero or more function-call-argument then
	, if not last argument in list then
	Zero or more whitespaces

function-call-argument:
	One of { expression-literal expression-variable expression }

array-member-access-element:
	One of { expression-variable expression } then
	[ then
	One of { numeric-literal expression-variable expression } then
	] then

pointer-member-access-element:
	One of { expression-variable expression } then
	-> then
	identifier

dot-member-access-element:
	One of { expression-variable expression } then
	. then
	identifier

postfix-increment-element:
	One of { expression-variable expression } then
	++

postfix-decrement-element:
	One of { expression-variable expression } then
	--

conditional-if:
	if then
	Zero or more whitespaces or newlines then
	( then
	Zero or more whitespaces or newlines then
	One of { expression-literal expression-variable expression } then
	) then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more statements then
	close-scope

conditional-elseif:
	else then
	One or more whitespaces then
	conditional-if

conditional-else:
	else then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more statements then
	close-scope

loop-for:
	for then
	Zero or more whitespaces or newlines then
	( then
	Zero or more whitespaces or newlines then
	One of { variable-declaration-initialization expression } then
	; then
	Zero or more whitespaces or newlines then
	expression then
	; then
	Zero or more whitespaces or newlines then
	Expression then
	) then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more statements then
	close-scope

loop-while:
	while then
	Zero or more whitespaces or newlines then
	( then
	expression then
	) then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more statements then
	close-scope
	
loop-do:
	do then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more statements then
	close-scope then
	Zero or more whitespaces or newlines then
	( then
	Zero or more whitespaces or newlines then
	Expression then
	Zero or more whitespaces or newlines then
	) (U+0029)

switch:
	switch then
	Zero or more whitespaces or newlines then
	open-scope then
	Zero or more switch-statements then
	Zero or one switch-default-statement then
	close-scope

switch-statement:
	case then
	One or more whitespaces or newlines then
	expression-literal then
	: then
	One or more whitespaces or newlines then
	Zero or more statements

switch-default-statement:
	default then
	: then
	One or more whitespaces or newlines then
	Zero or more statements
```