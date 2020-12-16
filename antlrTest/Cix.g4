// Stolen from C and C# grammars on GitHub

grammar Cix;

expressionAtom
	: number
	| Identifier
	| StringLiteral
	;

unaryPrefixOperator
	: Plus
	| Minus
	| BitwiseNot
	| LogicalNot
	| Asterisk
	| Ampersand
	| Increment
	| Decrement
	| typeCast
	;

typeCast
	: '(' typeName ')' expression
	;

unaryPostfixOperator
	: '(' argumentList ')'
	| '[' expression ']'
	| Increment
	| Decrement
	;

argumentList
	: expression
	| expression ',' argumentList
	;

binaryOperator
	: Plus
	| Minus
	| Asterisk
	| Divide
	| Modulus
	| ShiftLeft
	| ShiftRight
	| LessThan
	| LessThanOrEqualTo
	| GreaterThan
	| GreaterThanOrEqualTo
	| Equals
	| NotEquals
	| BitwiseOr
	| BitwiseXor
	| LogicalAnd
	| LogicalOr
	| Assign
	| AddAssign
	| SubtractAssign
	| MultiplyAssign
	| DivideAssign
	| ModulusAssign
	| ShiftLeftAssign
	| ShiftRightAssign
	| BitwiseAndAssign
	| BitwiseOrAssign
	| BitwiseXorAssign
	;

ternaryFirstOperator
	: Question
	;

ternarySecondOperator
	: Colon
	;

expression
	: expressionAtom
	| unaryPrefixOperator expression
	| expression unaryPostfixOperator
	| expression binaryOperator expression
	| expression ternaryFirstOperator expression ternarySecondOperator
	|
	;
	
typeName
	: Identifier pointerAsteriskList?
	| funcptrTypeName pointerAsteriskList?
	| primitiveType pointerAsteriskList?
	;
	
funcptrTypeName
	: '@funcptr<' typeNameList '>'
	;
	
typeNameList
	: typeName
	| typeName ',' typeNameList
	;
	
primitiveType
	: 'void'
	| 'byte'
	| 'sbyte'
	| 'short'
	| 'ushort'
	| 'int'
	| 'uint'
	| 'long'
	| 'ulong'
	| 'float'
	| 'double'
	;
	
pointerAsteriskList
	: Asterisk+
	;
	
variableDeclarationStatement
	: typeName Identifier ';'
	| variableDeclarationWithInitializationStatement ';'
	;

variableDeclarationWithInitializationStatement
	: typeName Identifier '=' expression ';'
	;

struct
	: 'struct' Identifier '{' structMember+ '}'
	;
	
structMember
	: typeName Identifier structArraySize? ';'
	;
	
structArraySize
	: '[' Integer ']'
	;
	
globalVariableDeclaration
	: 'global' variableDeclarationStatement
	| 'global' variableDeclarationWithInitializationStatement
	;

function
	: typeName Identifier '(' functionParameterList? ')' '{' statement* '}'
	;

functionParameterList
	: functionParameter
	| functionParameter ',' functionParameterList
	;
	
functionParameter
	: typeName Identifier
	;
	
statement
	: block
	| breakStatement
	| conditionalStatement
	| continueStatement
	| doWhileStatement
	| expressionStatement
	| forStatement
	| returnStatement
	| switchStatement
	| variableDeclarationStatement
	| variableDeclarationWithInitializationStatement
	| whileStatement
	;

block
	: '{' statement* '}'
	;
	
breakStatement
	: 'break' ';'
	;

conditionalStatement
	: 'if' '(' expression ')' statement elseStatement?
	;
	
continueStatement
	: 'continue' ';'
	;

elseStatement
	: 'else' statement
	;

doWhileStatement
	: 'do' statement 'while' '(' expression ')' ';'
	;
	
expressionStatement
	: expression ';'
	;

forStatement
	: 'for' '(' expression ';' expression ';' expression ';' ')' statement
	;

returnStatement
	: 'return' ';'
	| 'return' expression ';'
	;

switchStatement
	: 'switch' '(' expression ')' '{' caseStatement+ '}'
	;

caseStatement
	: literalCaseStatement
	| defaultCaseStatement
	;
	
literalCaseStatement
	: 'case' Integer ':' statement
	| 'case' StringLiteral ':' statement
	;

defaultCaseStatement
	: 'default' ':' statement
	;
	
whileStatement
	: 'while' '(' expression ')' statement
	;
	
number
	: Integer
	| FloatingPoint
	;
	
sourceFile
	: (struct | globalVariableDeclaration | function)+
	;

Whitespace
    :   [ \t\r\n]+
        -> skip
    ;
	
Break: 'break';
Case: 'case';
Continue: 'continue';
Default: 'default';
Do: 'do';
Double: 'double';
Else: 'else';
Float: 'float';
Global: 'global';
For: 'for';
If: 'if';
Int: 'int';
Long: 'long';
Return: 'return';
Short: 'short';
Sizeof: 'sizeof';
Struct: 'struct';
Void: 'void';
While: 'while';

LeftParen: '(';
RightParen: ')';
LeftBracket: '[';
RightBracket: ']';
OpenScope: '{';
CloseScope: '}';

LessThan: '<';
LessThanOrEqualTo: '<=';
GreaterThan: '>';
GreaterThanOrEqualTo: '>=';
ShiftLeft: '<<';
ShiftRight: '>>';
	
Plus: '+';
Increment: '++';
Minus: '-';
Decrement: '--';
Asterisk: '*';
Divide: '/';
Modulus: '%';

Ampersand: '&';
BitwiseOr: '|';
LogicalAnd: '&&';
LogicalOr: '||';
BitwiseXor: '^';
LogicalNot: '!';
BitwiseNot: '~';

Question: '?';
Colon: ':';
Semicolon: ';';
Comma: ',';

Assign: '=';
MultiplyAssign: '*=';
DivideAssign: '/=';
ModulusAssign: '%=';
AddAssign: '+=';
SubtractAssign: '-=';
ShiftLeftAssign: '<<=';
ShiftRightAssign: '>>=';
BitwiseAndAssign: '&=';
BitwiseXorAssign: '^=';
BitwiseOrAssign: '|=';

Equals: '==';
NotEquals: '!=';

PointerMemberAccess: '->';
DirectMemberAccess: '.';

Integer
	: DecimalInteger IntegerSuffix?
	| HexadecimalInteger IntegerSuffix?
	;

FloatingPoint
	: DecimalFloatingPoint FloatingSuffix?
	| ExponentFloatingPoint FloatingSuffix?
	;
	
fragment IntegerSuffix
	: 'l' | 'L'
	| 'u' | 'U'
	| 'ul' | 'UL'
	;
	
fragment FloatingSuffix
	: 'f' | 'F'
	| 'd' | 'D'
	;
	
fragment DecimalInteger
	: Digit+
	;

fragment HexadecimalInteger
	: '0x' HexDigit+
	;

fragment DecimalFloatingPoint
	: DecimalInteger+ '.' DecimalInteger*
	;

fragment ExponentFloatingPoint
	: DecimalFloatingPoint Exponent
	;
	
fragment Exponent
	: ExponentLetter ExponentSign DecimalInteger
	;

fragment ExponentLetter
	: 'E'
	| 'e'
	;

fragment ExponentSign
	: '+'
	| '-'
	;

Digit
	: [0-9]
	;
	
HexDigit
	: [0-9A-Fa-f]
	;

StringLiteral
	: '"' ~[\r\n"]* '"'
	;

Identifier
	: IdentifierFirstChar IdentifierChar*
	;

IdentifierFirstChar
	: [A-Za-z]
	;

IdentifierChar
	: [A-Za-z0-9_]
	;