// Stolen from C and C# grammars on GitHub

grammar Cix;

primaryExpression
    :   Identifier
    |   StringLiteral
	|	number
    |   '(' expression ')'
	;

postfixExpression
    :   primaryExpression
    |   postfixExpression '[' expression ']'
	|	postfixExpression '(' argumentExpressionList? ')'
    |   postfixExpression '.' Identifier
    |   postfixExpression '->' Identifier
    |   postfixExpression '++'
    |   postfixExpression '--'
    ;
	
argumentExpressionList
	: assignmentExpression
	| argumentExpressionList ',' assignmentExpression
	;

unaryExpression
    :   postfixExpression
    |   '++' unaryExpression
    |   '--' unaryExpression
    |   unaryOperator castExpression
    |   'sizeof' unaryExpression
    |   'sizeof' '(' typeName ')'
    ;

unaryOperator
    :   '&' | '*' | '+' | '-' | '~' | '!'
    ;

castExpression
    :   '(' typeName ')' castExpression
    |   unaryExpression
    ;

multiplicativeExpression
    :   castExpression
    |   multiplicativeExpression '*' castExpression
    |   multiplicativeExpression '/' castExpression
    |   multiplicativeExpression '%' castExpression
    ;

additiveExpression
    :   multiplicativeExpression
    |   additiveExpression '+' multiplicativeExpression
    |   additiveExpression '-' multiplicativeExpression
    ;

shiftExpression
    :   additiveExpression
    |   shiftExpression '<<' additiveExpression
    |   shiftExpression '>>' additiveExpression
    ;

relationalExpression
    :   shiftExpression
    |   relationalExpression '<' shiftExpression
    |   relationalExpression '>' shiftExpression
    |   relationalExpression '<=' shiftExpression
    |   relationalExpression '>=' shiftExpression
    ;

equalityExpression
    :   relationalExpression
    |   equalityExpression '==' relationalExpression
    |   equalityExpression '!=' relationalExpression
    ;

andExpression
    :   equalityExpression
    |   andExpression '&' equalityExpression
    ;

exclusiveOrExpression
    :   andExpression
    |   exclusiveOrExpression '^' andExpression
    ;

inclusiveOrExpression
    :   exclusiveOrExpression
    |   inclusiveOrExpression '|' exclusiveOrExpression
    ;

logicalAndExpression
    :   inclusiveOrExpression
    |   logicalAndExpression '&&' inclusiveOrExpression
    ;

logicalOrExpression
    :   logicalAndExpression
    |   logicalOrExpression '||' logicalAndExpression
    ;

conditionalExpression
    :   logicalOrExpression ('?' expression ':' conditionalExpression)?
    ;

assignmentExpression
    :   conditionalExpression
    |   unaryExpression assignmentOperator assignmentExpression
    ;

assignmentOperator
    :   '=' | '*=' | '/=' | '%=' | '+=' | '-=' | '<<=' | '>>=' | '&=' | '^=' | '|='
    ;

expression
    :   assignmentExpression
    ;

constantExpression
    :   conditionalExpression
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