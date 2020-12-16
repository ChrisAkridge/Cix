// Generated from d:\Documents\GitHub\Cix\antlrTest\Cix.g4 by ANTLR 4.8
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class CixParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.8", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, Whitespace=8, 
		Break=9, Case=10, Continue=11, Default=12, Do=13, Double=14, Else=15, 
		Float=16, Global=17, For=18, If=19, Int=20, Long=21, Return=22, Short=23, 
		Sizeof=24, Struct=25, Void=26, While=27, LeftParen=28, RightParen=29, 
		LeftBracket=30, RightBracket=31, OpenScope=32, CloseScope=33, LessThan=34, 
		LessThanOrEqualTo=35, GreaterThan=36, GreaterThanOrEqualTo=37, ShiftLeft=38, 
		ShiftRight=39, Plus=40, Increment=41, Minus=42, Decrement=43, Asterisk=44, 
		Divide=45, Modulus=46, Ampersand=47, BitwiseOr=48, LogicalAnd=49, LogicalOr=50, 
		BitwiseXor=51, LogicalNot=52, BitwiseNot=53, Question=54, Colon=55, Semicolon=56, 
		Comma=57, Assign=58, MultiplyAssign=59, DivideAssign=60, ModulusAssign=61, 
		AddAssign=62, SubtractAssign=63, ShiftLeftAssign=64, ShiftRightAssign=65, 
		BitwiseAndAssign=66, BitwiseXorAssign=67, BitwiseOrAssign=68, Equals=69, 
		NotEquals=70, PointerMemberAccess=71, DirectMemberAccess=72, Integer=73, 
		FloatingPoint=74, Digit=75, HexDigit=76, StringLiteral=77, Identifier=78, 
		IdentifierFirstChar=79, IdentifierChar=80;
	public static final int
		RULE_expressionAtom = 0, RULE_unaryPrefixOperator = 1, RULE_typeCast = 2, 
		RULE_unaryPostfixOperator = 3, RULE_argumentList = 4, RULE_binaryOperator = 5, 
		RULE_ternaryFirstOperator = 6, RULE_ternarySecondOperator = 7, RULE_expression = 8, 
		RULE_typeName = 9, RULE_funcptrTypeName = 10, RULE_typeNameList = 11, 
		RULE_primitiveType = 12, RULE_pointerAsteriskList = 13, RULE_variableDeclarationStatement = 14, 
		RULE_variableDeclarationWithInitializationStatement = 15, RULE_struct = 16, 
		RULE_structMember = 17, RULE_structArraySize = 18, RULE_globalVariableDeclaration = 19, 
		RULE_function = 20, RULE_functionParameterList = 21, RULE_functionParameter = 22, 
		RULE_statement = 23, RULE_block = 24, RULE_breakStatement = 25, RULE_conditionalStatement = 26, 
		RULE_continueStatement = 27, RULE_elseStatement = 28, RULE_doWhileStatement = 29, 
		RULE_expressionStatement = 30, RULE_forStatement = 31, RULE_returnStatement = 32, 
		RULE_switchStatement = 33, RULE_caseStatement = 34, RULE_literalCaseStatement = 35, 
		RULE_defaultCaseStatement = 36, RULE_whileStatement = 37, RULE_number = 38, 
		RULE_sourceFile = 39;
	private static String[] makeRuleNames() {
		return new String[] {
			"expressionAtom", "unaryPrefixOperator", "typeCast", "unaryPostfixOperator", 
			"argumentList", "binaryOperator", "ternaryFirstOperator", "ternarySecondOperator", 
			"expression", "typeName", "funcptrTypeName", "typeNameList", "primitiveType", 
			"pointerAsteriskList", "variableDeclarationStatement", "variableDeclarationWithInitializationStatement", 
			"struct", "structMember", "structArraySize", "globalVariableDeclaration", 
			"function", "functionParameterList", "functionParameter", "statement", 
			"block", "breakStatement", "conditionalStatement", "continueStatement", 
			"elseStatement", "doWhileStatement", "expressionStatement", "forStatement", 
			"returnStatement", "switchStatement", "caseStatement", "literalCaseStatement", 
			"defaultCaseStatement", "whileStatement", "number", "sourceFile"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'@funcptr<'", "'byte'", "'sbyte'", "'ushort'", "'uint'", "'ulong'", 
			"'switch'", null, "'break'", "'case'", "'continue'", "'default'", "'do'", 
			"'double'", "'else'", "'float'", "'global'", "'for'", "'if'", "'int'", 
			"'long'", "'return'", "'short'", "'sizeof'", "'struct'", "'void'", "'while'", 
			"'('", "')'", "'['", "']'", "'{'", "'}'", "'<'", "'<='", "'>'", "'>='", 
			"'<<'", "'>>'", "'+'", "'++'", "'-'", "'--'", "'*'", "'/'", "'%'", "'&'", 
			"'|'", "'&&'", "'||'", "'^'", "'!'", "'~'", "'?'", "':'", "';'", "','", 
			"'='", "'*='", "'/='", "'%='", "'+='", "'-='", "'<<='", "'>>='", "'&='", 
			"'^='", "'|='", "'=='", "'!='", "'->'", "'.'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, "Whitespace", "Break", 
			"Case", "Continue", "Default", "Do", "Double", "Else", "Float", "Global", 
			"For", "If", "Int", "Long", "Return", "Short", "Sizeof", "Struct", "Void", 
			"While", "LeftParen", "RightParen", "LeftBracket", "RightBracket", "OpenScope", 
			"CloseScope", "LessThan", "LessThanOrEqualTo", "GreaterThan", "GreaterThanOrEqualTo", 
			"ShiftLeft", "ShiftRight", "Plus", "Increment", "Minus", "Decrement", 
			"Asterisk", "Divide", "Modulus", "Ampersand", "BitwiseOr", "LogicalAnd", 
			"LogicalOr", "BitwiseXor", "LogicalNot", "BitwiseNot", "Question", "Colon", 
			"Semicolon", "Comma", "Assign", "MultiplyAssign", "DivideAssign", "ModulusAssign", 
			"AddAssign", "SubtractAssign", "ShiftLeftAssign", "ShiftRightAssign", 
			"BitwiseAndAssign", "BitwiseXorAssign", "BitwiseOrAssign", "Equals", 
			"NotEquals", "PointerMemberAccess", "DirectMemberAccess", "Integer", 
			"FloatingPoint", "Digit", "HexDigit", "StringLiteral", "Identifier", 
			"IdentifierFirstChar", "IdentifierChar"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "Cix.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public CixParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class ExpressionAtomContext extends ParserRuleContext {
		public NumberContext number() {
			return getRuleContext(NumberContext.class,0);
		}
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode StringLiteral() { return getToken(CixParser.StringLiteral, 0); }
		public ExpressionAtomContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expressionAtom; }
	}

	public final ExpressionAtomContext expressionAtom() throws RecognitionException {
		ExpressionAtomContext _localctx = new ExpressionAtomContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_expressionAtom);
		try {
			setState(83);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Integer:
			case FloatingPoint:
				enterOuterAlt(_localctx, 1);
				{
				setState(80);
				number();
				}
				break;
			case Identifier:
				enterOuterAlt(_localctx, 2);
				{
				setState(81);
				match(Identifier);
				}
				break;
			case StringLiteral:
				enterOuterAlt(_localctx, 3);
				{
				setState(82);
				match(StringLiteral);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class UnaryPrefixOperatorContext extends ParserRuleContext {
		public TerminalNode Plus() { return getToken(CixParser.Plus, 0); }
		public TerminalNode Minus() { return getToken(CixParser.Minus, 0); }
		public TerminalNode BitwiseNot() { return getToken(CixParser.BitwiseNot, 0); }
		public TerminalNode LogicalNot() { return getToken(CixParser.LogicalNot, 0); }
		public TerminalNode Asterisk() { return getToken(CixParser.Asterisk, 0); }
		public TerminalNode Ampersand() { return getToken(CixParser.Ampersand, 0); }
		public TerminalNode Increment() { return getToken(CixParser.Increment, 0); }
		public TerminalNode Decrement() { return getToken(CixParser.Decrement, 0); }
		public TypeCastContext typeCast() {
			return getRuleContext(TypeCastContext.class,0);
		}
		public UnaryPrefixOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unaryPrefixOperator; }
	}

	public final UnaryPrefixOperatorContext unaryPrefixOperator() throws RecognitionException {
		UnaryPrefixOperatorContext _localctx = new UnaryPrefixOperatorContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_unaryPrefixOperator);
		try {
			setState(94);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Plus:
				enterOuterAlt(_localctx, 1);
				{
				setState(85);
				match(Plus);
				}
				break;
			case Minus:
				enterOuterAlt(_localctx, 2);
				{
				setState(86);
				match(Minus);
				}
				break;
			case BitwiseNot:
				enterOuterAlt(_localctx, 3);
				{
				setState(87);
				match(BitwiseNot);
				}
				break;
			case LogicalNot:
				enterOuterAlt(_localctx, 4);
				{
				setState(88);
				match(LogicalNot);
				}
				break;
			case Asterisk:
				enterOuterAlt(_localctx, 5);
				{
				setState(89);
				match(Asterisk);
				}
				break;
			case Ampersand:
				enterOuterAlt(_localctx, 6);
				{
				setState(90);
				match(Ampersand);
				}
				break;
			case Increment:
				enterOuterAlt(_localctx, 7);
				{
				setState(91);
				match(Increment);
				}
				break;
			case Decrement:
				enterOuterAlt(_localctx, 8);
				{
				setState(92);
				match(Decrement);
				}
				break;
			case LeftParen:
				enterOuterAlt(_localctx, 9);
				{
				setState(93);
				typeCast();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeCastContext extends ParserRuleContext {
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TypeCastContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeCast; }
	}

	public final TypeCastContext typeCast() throws RecognitionException {
		TypeCastContext _localctx = new TypeCastContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_typeCast);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(96);
			match(LeftParen);
			setState(97);
			typeName();
			setState(98);
			match(RightParen);
			setState(99);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class UnaryPostfixOperatorContext extends ParserRuleContext {
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public ArgumentListContext argumentList() {
			return getRuleContext(ArgumentListContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public TerminalNode LeftBracket() { return getToken(CixParser.LeftBracket, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RightBracket() { return getToken(CixParser.RightBracket, 0); }
		public TerminalNode Increment() { return getToken(CixParser.Increment, 0); }
		public TerminalNode Decrement() { return getToken(CixParser.Decrement, 0); }
		public UnaryPostfixOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unaryPostfixOperator; }
	}

	public final UnaryPostfixOperatorContext unaryPostfixOperator() throws RecognitionException {
		UnaryPostfixOperatorContext _localctx = new UnaryPostfixOperatorContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_unaryPostfixOperator);
		try {
			setState(111);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LeftParen:
				enterOuterAlt(_localctx, 1);
				{
				setState(101);
				match(LeftParen);
				setState(102);
				argumentList();
				setState(103);
				match(RightParen);
				}
				break;
			case LeftBracket:
				enterOuterAlt(_localctx, 2);
				{
				setState(105);
				match(LeftBracket);
				setState(106);
				expression(0);
				setState(107);
				match(RightBracket);
				}
				break;
			case Increment:
				enterOuterAlt(_localctx, 3);
				{
				setState(109);
				match(Increment);
				}
				break;
			case Decrement:
				enterOuterAlt(_localctx, 4);
				{
				setState(110);
				match(Decrement);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArgumentListContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode Comma() { return getToken(CixParser.Comma, 0); }
		public ArgumentListContext argumentList() {
			return getRuleContext(ArgumentListContext.class,0);
		}
		public ArgumentListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_argumentList; }
	}

	public final ArgumentListContext argumentList() throws RecognitionException {
		ArgumentListContext _localctx = new ArgumentListContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_argumentList);
		try {
			setState(118);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,3,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(113);
				expression(0);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(114);
				expression(0);
				setState(115);
				match(Comma);
				setState(116);
				argumentList();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class BinaryOperatorContext extends ParserRuleContext {
		public TerminalNode Plus() { return getToken(CixParser.Plus, 0); }
		public TerminalNode Minus() { return getToken(CixParser.Minus, 0); }
		public TerminalNode Asterisk() { return getToken(CixParser.Asterisk, 0); }
		public TerminalNode Divide() { return getToken(CixParser.Divide, 0); }
		public TerminalNode Modulus() { return getToken(CixParser.Modulus, 0); }
		public TerminalNode ShiftLeft() { return getToken(CixParser.ShiftLeft, 0); }
		public TerminalNode ShiftRight() { return getToken(CixParser.ShiftRight, 0); }
		public TerminalNode LessThan() { return getToken(CixParser.LessThan, 0); }
		public TerminalNode LessThanOrEqualTo() { return getToken(CixParser.LessThanOrEqualTo, 0); }
		public TerminalNode GreaterThan() { return getToken(CixParser.GreaterThan, 0); }
		public TerminalNode GreaterThanOrEqualTo() { return getToken(CixParser.GreaterThanOrEqualTo, 0); }
		public TerminalNode Equals() { return getToken(CixParser.Equals, 0); }
		public TerminalNode NotEquals() { return getToken(CixParser.NotEquals, 0); }
		public TerminalNode BitwiseOr() { return getToken(CixParser.BitwiseOr, 0); }
		public TerminalNode BitwiseXor() { return getToken(CixParser.BitwiseXor, 0); }
		public TerminalNode LogicalAnd() { return getToken(CixParser.LogicalAnd, 0); }
		public TerminalNode LogicalOr() { return getToken(CixParser.LogicalOr, 0); }
		public TerminalNode Assign() { return getToken(CixParser.Assign, 0); }
		public TerminalNode AddAssign() { return getToken(CixParser.AddAssign, 0); }
		public TerminalNode SubtractAssign() { return getToken(CixParser.SubtractAssign, 0); }
		public TerminalNode MultiplyAssign() { return getToken(CixParser.MultiplyAssign, 0); }
		public TerminalNode DivideAssign() { return getToken(CixParser.DivideAssign, 0); }
		public TerminalNode ModulusAssign() { return getToken(CixParser.ModulusAssign, 0); }
		public TerminalNode ShiftLeftAssign() { return getToken(CixParser.ShiftLeftAssign, 0); }
		public TerminalNode ShiftRightAssign() { return getToken(CixParser.ShiftRightAssign, 0); }
		public TerminalNode BitwiseAndAssign() { return getToken(CixParser.BitwiseAndAssign, 0); }
		public TerminalNode BitwiseOrAssign() { return getToken(CixParser.BitwiseOrAssign, 0); }
		public TerminalNode BitwiseXorAssign() { return getToken(CixParser.BitwiseXorAssign, 0); }
		public BinaryOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_binaryOperator; }
	}

	public final BinaryOperatorContext binaryOperator() throws RecognitionException {
		BinaryOperatorContext _localctx = new BinaryOperatorContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_binaryOperator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(120);
			_la = _input.LA(1);
			if ( !(((((_la - 34)) & ~0x3f) == 0 && ((1L << (_la - 34)) & ((1L << (LessThan - 34)) | (1L << (LessThanOrEqualTo - 34)) | (1L << (GreaterThan - 34)) | (1L << (GreaterThanOrEqualTo - 34)) | (1L << (ShiftLeft - 34)) | (1L << (ShiftRight - 34)) | (1L << (Plus - 34)) | (1L << (Minus - 34)) | (1L << (Asterisk - 34)) | (1L << (Divide - 34)) | (1L << (Modulus - 34)) | (1L << (BitwiseOr - 34)) | (1L << (LogicalAnd - 34)) | (1L << (LogicalOr - 34)) | (1L << (BitwiseXor - 34)) | (1L << (Assign - 34)) | (1L << (MultiplyAssign - 34)) | (1L << (DivideAssign - 34)) | (1L << (ModulusAssign - 34)) | (1L << (AddAssign - 34)) | (1L << (SubtractAssign - 34)) | (1L << (ShiftLeftAssign - 34)) | (1L << (ShiftRightAssign - 34)) | (1L << (BitwiseAndAssign - 34)) | (1L << (BitwiseXorAssign - 34)) | (1L << (BitwiseOrAssign - 34)) | (1L << (Equals - 34)) | (1L << (NotEquals - 34)))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TernaryFirstOperatorContext extends ParserRuleContext {
		public TerminalNode Question() { return getToken(CixParser.Question, 0); }
		public TernaryFirstOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ternaryFirstOperator; }
	}

	public final TernaryFirstOperatorContext ternaryFirstOperator() throws RecognitionException {
		TernaryFirstOperatorContext _localctx = new TernaryFirstOperatorContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_ternaryFirstOperator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(122);
			match(Question);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TernarySecondOperatorContext extends ParserRuleContext {
		public TerminalNode Colon() { return getToken(CixParser.Colon, 0); }
		public TernarySecondOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ternarySecondOperator; }
	}

	public final TernarySecondOperatorContext ternarySecondOperator() throws RecognitionException {
		TernarySecondOperatorContext _localctx = new TernarySecondOperatorContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_ternarySecondOperator);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(124);
			match(Colon);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionContext extends ParserRuleContext {
		public ExpressionAtomContext expressionAtom() {
			return getRuleContext(ExpressionAtomContext.class,0);
		}
		public UnaryPrefixOperatorContext unaryPrefixOperator() {
			return getRuleContext(UnaryPrefixOperatorContext.class,0);
		}
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public BinaryOperatorContext binaryOperator() {
			return getRuleContext(BinaryOperatorContext.class,0);
		}
		public UnaryPostfixOperatorContext unaryPostfixOperator() {
			return getRuleContext(UnaryPostfixOperatorContext.class,0);
		}
		public TernaryFirstOperatorContext ternaryFirstOperator() {
			return getRuleContext(TernaryFirstOperatorContext.class,0);
		}
		public TernarySecondOperatorContext ternarySecondOperator() {
			return getRuleContext(TernarySecondOperatorContext.class,0);
		}
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
	}

	public final ExpressionContext expression() throws RecognitionException {
		return expression(0);
	}

	private ExpressionContext expression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressionContext _localctx = new ExpressionContext(_ctx, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 16;
		enterRecursionRule(_localctx, 16, RULE_expression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(132);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,4,_ctx) ) {
			case 1:
				{
				setState(127);
				expressionAtom();
				}
				break;
			case 2:
				{
				setState(128);
				unaryPrefixOperator();
				setState(129);
				expression(5);
				}
				break;
			case 3:
				{
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(147);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,6,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(145);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,5,_ctx) ) {
					case 1:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(134);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(135);
						binaryOperator();
						setState(136);
						expression(4);
						}
						break;
					case 2:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(138);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(139);
						unaryPostfixOperator();
						}
						break;
					case 3:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(140);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(141);
						ternaryFirstOperator();
						setState(142);
						expression(0);
						setState(143);
						ternarySecondOperator();
						}
						break;
					}
					} 
				}
				setState(149);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,6,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class TypeNameContext extends ParserRuleContext {
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public PointerAsteriskListContext pointerAsteriskList() {
			return getRuleContext(PointerAsteriskListContext.class,0);
		}
		public FuncptrTypeNameContext funcptrTypeName() {
			return getRuleContext(FuncptrTypeNameContext.class,0);
		}
		public PrimitiveTypeContext primitiveType() {
			return getRuleContext(PrimitiveTypeContext.class,0);
		}
		public TypeNameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeName; }
	}

	public final TypeNameContext typeName() throws RecognitionException {
		TypeNameContext _localctx = new TypeNameContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_typeName);
		int _la;
		try {
			setState(162);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Identifier:
				enterOuterAlt(_localctx, 1);
				{
				setState(150);
				match(Identifier);
				setState(152);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==Asterisk) {
					{
					setState(151);
					pointerAsteriskList();
					}
				}

				}
				break;
			case T__0:
				enterOuterAlt(_localctx, 2);
				{
				setState(154);
				funcptrTypeName();
				setState(156);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==Asterisk) {
					{
					setState(155);
					pointerAsteriskList();
					}
				}

				}
				break;
			case T__1:
			case T__2:
			case T__3:
			case T__4:
			case T__5:
			case Double:
			case Float:
			case Int:
			case Long:
			case Short:
			case Void:
				enterOuterAlt(_localctx, 3);
				{
				setState(158);
				primitiveType();
				setState(160);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==Asterisk) {
					{
					setState(159);
					pointerAsteriskList();
					}
				}

				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FuncptrTypeNameContext extends ParserRuleContext {
		public TypeNameListContext typeNameList() {
			return getRuleContext(TypeNameListContext.class,0);
		}
		public TerminalNode GreaterThan() { return getToken(CixParser.GreaterThan, 0); }
		public FuncptrTypeNameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_funcptrTypeName; }
	}

	public final FuncptrTypeNameContext funcptrTypeName() throws RecognitionException {
		FuncptrTypeNameContext _localctx = new FuncptrTypeNameContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_funcptrTypeName);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(164);
			match(T__0);
			setState(165);
			typeNameList();
			setState(166);
			match(GreaterThan);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeNameListContext extends ParserRuleContext {
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode Comma() { return getToken(CixParser.Comma, 0); }
		public TypeNameListContext typeNameList() {
			return getRuleContext(TypeNameListContext.class,0);
		}
		public TypeNameListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeNameList; }
	}

	public final TypeNameListContext typeNameList() throws RecognitionException {
		TypeNameListContext _localctx = new TypeNameListContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_typeNameList);
		try {
			setState(173);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,11,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(168);
				typeName();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(169);
				typeName();
				setState(170);
				match(Comma);
				setState(171);
				typeNameList();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PrimitiveTypeContext extends ParserRuleContext {
		public TerminalNode Void() { return getToken(CixParser.Void, 0); }
		public TerminalNode Short() { return getToken(CixParser.Short, 0); }
		public TerminalNode Int() { return getToken(CixParser.Int, 0); }
		public TerminalNode Long() { return getToken(CixParser.Long, 0); }
		public TerminalNode Float() { return getToken(CixParser.Float, 0); }
		public TerminalNode Double() { return getToken(CixParser.Double, 0); }
		public PrimitiveTypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primitiveType; }
	}

	public final PrimitiveTypeContext primitiveType() throws RecognitionException {
		PrimitiveTypeContext _localctx = new PrimitiveTypeContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_primitiveType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(175);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << Double) | (1L << Float) | (1L << Int) | (1L << Long) | (1L << Short) | (1L << Void))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PointerAsteriskListContext extends ParserRuleContext {
		public List<TerminalNode> Asterisk() { return getTokens(CixParser.Asterisk); }
		public TerminalNode Asterisk(int i) {
			return getToken(CixParser.Asterisk, i);
		}
		public PointerAsteriskListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_pointerAsteriskList; }
	}

	public final PointerAsteriskListContext pointerAsteriskList() throws RecognitionException {
		PointerAsteriskListContext _localctx = new PointerAsteriskListContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_pointerAsteriskList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(178); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(177);
				match(Asterisk);
				}
				}
				setState(180); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==Asterisk );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableDeclarationStatementContext extends ParserRuleContext {
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public VariableDeclarationWithInitializationStatementContext variableDeclarationWithInitializationStatement() {
			return getRuleContext(VariableDeclarationWithInitializationStatementContext.class,0);
		}
		public VariableDeclarationStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variableDeclarationStatement; }
	}

	public final VariableDeclarationStatementContext variableDeclarationStatement() throws RecognitionException {
		VariableDeclarationStatementContext _localctx = new VariableDeclarationStatementContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_variableDeclarationStatement);
		try {
			setState(189);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,13,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(182);
				typeName();
				setState(183);
				match(Identifier);
				setState(184);
				match(Semicolon);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(186);
				variableDeclarationWithInitializationStatement();
				setState(187);
				match(Semicolon);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableDeclarationWithInitializationStatementContext extends ParserRuleContext {
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode Assign() { return getToken(CixParser.Assign, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public VariableDeclarationWithInitializationStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variableDeclarationWithInitializationStatement; }
	}

	public final VariableDeclarationWithInitializationStatementContext variableDeclarationWithInitializationStatement() throws RecognitionException {
		VariableDeclarationWithInitializationStatementContext _localctx = new VariableDeclarationWithInitializationStatementContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_variableDeclarationWithInitializationStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(191);
			typeName();
			setState(192);
			match(Identifier);
			setState(193);
			match(Assign);
			setState(194);
			expression(0);
			setState(195);
			match(Semicolon);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StructContext extends ParserRuleContext {
		public TerminalNode Struct() { return getToken(CixParser.Struct, 0); }
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode OpenScope() { return getToken(CixParser.OpenScope, 0); }
		public TerminalNode CloseScope() { return getToken(CixParser.CloseScope, 0); }
		public List<StructMemberContext> structMember() {
			return getRuleContexts(StructMemberContext.class);
		}
		public StructMemberContext structMember(int i) {
			return getRuleContext(StructMemberContext.class,i);
		}
		public StructContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_struct; }
	}

	public final StructContext struct() throws RecognitionException {
		StructContext _localctx = new StructContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_struct);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(197);
			match(Struct);
			setState(198);
			match(Identifier);
			setState(199);
			match(OpenScope);
			setState(201); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(200);
				structMember();
				}
				}
				setState(203); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << Double) | (1L << Float) | (1L << Int) | (1L << Long) | (1L << Short) | (1L << Void))) != 0) || _la==Identifier );
			setState(205);
			match(CloseScope);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StructMemberContext extends ParserRuleContext {
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public StructArraySizeContext structArraySize() {
			return getRuleContext(StructArraySizeContext.class,0);
		}
		public StructMemberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structMember; }
	}

	public final StructMemberContext structMember() throws RecognitionException {
		StructMemberContext _localctx = new StructMemberContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_structMember);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(207);
			typeName();
			setState(208);
			match(Identifier);
			setState(210);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LeftBracket) {
				{
				setState(209);
				structArraySize();
				}
			}

			setState(212);
			match(Semicolon);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StructArraySizeContext extends ParserRuleContext {
		public TerminalNode LeftBracket() { return getToken(CixParser.LeftBracket, 0); }
		public TerminalNode Integer() { return getToken(CixParser.Integer, 0); }
		public TerminalNode RightBracket() { return getToken(CixParser.RightBracket, 0); }
		public StructArraySizeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_structArraySize; }
	}

	public final StructArraySizeContext structArraySize() throws RecognitionException {
		StructArraySizeContext _localctx = new StructArraySizeContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_structArraySize);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(214);
			match(LeftBracket);
			setState(215);
			match(Integer);
			setState(216);
			match(RightBracket);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class GlobalVariableDeclarationContext extends ParserRuleContext {
		public TerminalNode Global() { return getToken(CixParser.Global, 0); }
		public VariableDeclarationStatementContext variableDeclarationStatement() {
			return getRuleContext(VariableDeclarationStatementContext.class,0);
		}
		public VariableDeclarationWithInitializationStatementContext variableDeclarationWithInitializationStatement() {
			return getRuleContext(VariableDeclarationWithInitializationStatementContext.class,0);
		}
		public GlobalVariableDeclarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_globalVariableDeclaration; }
	}

	public final GlobalVariableDeclarationContext globalVariableDeclaration() throws RecognitionException {
		GlobalVariableDeclarationContext _localctx = new GlobalVariableDeclarationContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_globalVariableDeclaration);
		try {
			setState(222);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,16,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(218);
				match(Global);
				setState(219);
				variableDeclarationStatement();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(220);
				match(Global);
				setState(221);
				variableDeclarationWithInitializationStatement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FunctionContext extends ParserRuleContext {
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public TerminalNode OpenScope() { return getToken(CixParser.OpenScope, 0); }
		public TerminalNode CloseScope() { return getToken(CixParser.CloseScope, 0); }
		public FunctionParameterListContext functionParameterList() {
			return getRuleContext(FunctionParameterListContext.class,0);
		}
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public FunctionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_function; }
	}

	public final FunctionContext function() throws RecognitionException {
		FunctionContext _localctx = new FunctionContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_function);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(224);
			typeName();
			setState(225);
			match(Identifier);
			setState(226);
			match(LeftParen);
			setState(228);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << Double) | (1L << Float) | (1L << Int) | (1L << Long) | (1L << Short) | (1L << Void))) != 0) || _la==Identifier) {
				{
				setState(227);
				functionParameterList();
				}
			}

			setState(230);
			match(RightParen);
			setState(231);
			match(OpenScope);
			setState(235);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,18,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(232);
					statement();
					}
					} 
				}
				setState(237);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,18,_ctx);
			}
			setState(238);
			match(CloseScope);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FunctionParameterListContext extends ParserRuleContext {
		public FunctionParameterContext functionParameter() {
			return getRuleContext(FunctionParameterContext.class,0);
		}
		public TerminalNode Comma() { return getToken(CixParser.Comma, 0); }
		public FunctionParameterListContext functionParameterList() {
			return getRuleContext(FunctionParameterListContext.class,0);
		}
		public FunctionParameterListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_functionParameterList; }
	}

	public final FunctionParameterListContext functionParameterList() throws RecognitionException {
		FunctionParameterListContext _localctx = new FunctionParameterListContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_functionParameterList);
		try {
			setState(245);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,19,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(240);
				functionParameter();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(241);
				functionParameter();
				setState(242);
				match(Comma);
				setState(243);
				functionParameterList();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FunctionParameterContext extends ParserRuleContext {
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public FunctionParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_functionParameter; }
	}

	public final FunctionParameterContext functionParameter() throws RecognitionException {
		FunctionParameterContext _localctx = new FunctionParameterContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_functionParameter);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(247);
			typeName();
			setState(248);
			match(Identifier);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StatementContext extends ParserRuleContext {
		public BlockContext block() {
			return getRuleContext(BlockContext.class,0);
		}
		public BreakStatementContext breakStatement() {
			return getRuleContext(BreakStatementContext.class,0);
		}
		public ConditionalStatementContext conditionalStatement() {
			return getRuleContext(ConditionalStatementContext.class,0);
		}
		public ContinueStatementContext continueStatement() {
			return getRuleContext(ContinueStatementContext.class,0);
		}
		public DoWhileStatementContext doWhileStatement() {
			return getRuleContext(DoWhileStatementContext.class,0);
		}
		public ExpressionStatementContext expressionStatement() {
			return getRuleContext(ExpressionStatementContext.class,0);
		}
		public ForStatementContext forStatement() {
			return getRuleContext(ForStatementContext.class,0);
		}
		public ReturnStatementContext returnStatement() {
			return getRuleContext(ReturnStatementContext.class,0);
		}
		public SwitchStatementContext switchStatement() {
			return getRuleContext(SwitchStatementContext.class,0);
		}
		public VariableDeclarationStatementContext variableDeclarationStatement() {
			return getRuleContext(VariableDeclarationStatementContext.class,0);
		}
		public VariableDeclarationWithInitializationStatementContext variableDeclarationWithInitializationStatement() {
			return getRuleContext(VariableDeclarationWithInitializationStatementContext.class,0);
		}
		public WhileStatementContext whileStatement() {
			return getRuleContext(WhileStatementContext.class,0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_statement);
		try {
			setState(262);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,20,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(250);
				block();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(251);
				breakStatement();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(252);
				conditionalStatement();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(253);
				continueStatement();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(254);
				doWhileStatement();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(255);
				expressionStatement();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(256);
				forStatement();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(257);
				returnStatement();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(258);
				switchStatement();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(259);
				variableDeclarationStatement();
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(260);
				variableDeclarationWithInitializationStatement();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(261);
				whileStatement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class BlockContext extends ParserRuleContext {
		public TerminalNode OpenScope() { return getToken(CixParser.OpenScope, 0); }
		public TerminalNode CloseScope() { return getToken(CixParser.CloseScope, 0); }
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public BlockContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_block; }
	}

	public final BlockContext block() throws RecognitionException {
		BlockContext _localctx = new BlockContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_block);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(264);
			match(OpenScope);
			setState(268);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,21,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(265);
					statement();
					}
					} 
				}
				setState(270);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,21,_ctx);
			}
			setState(271);
			match(CloseScope);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class BreakStatementContext extends ParserRuleContext {
		public TerminalNode Break() { return getToken(CixParser.Break, 0); }
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public BreakStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_breakStatement; }
	}

	public final BreakStatementContext breakStatement() throws RecognitionException {
		BreakStatementContext _localctx = new BreakStatementContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_breakStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(273);
			match(Break);
			setState(274);
			match(Semicolon);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ConditionalStatementContext extends ParserRuleContext {
		public TerminalNode If() { return getToken(CixParser.If, 0); }
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public ElseStatementContext elseStatement() {
			return getRuleContext(ElseStatementContext.class,0);
		}
		public ConditionalStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_conditionalStatement; }
	}

	public final ConditionalStatementContext conditionalStatement() throws RecognitionException {
		ConditionalStatementContext _localctx = new ConditionalStatementContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_conditionalStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(276);
			match(If);
			setState(277);
			match(LeftParen);
			setState(278);
			expression(0);
			setState(279);
			match(RightParen);
			setState(280);
			statement();
			setState(282);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,22,_ctx) ) {
			case 1:
				{
				setState(281);
				elseStatement();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ContinueStatementContext extends ParserRuleContext {
		public TerminalNode Continue() { return getToken(CixParser.Continue, 0); }
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public ContinueStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_continueStatement; }
	}

	public final ContinueStatementContext continueStatement() throws RecognitionException {
		ContinueStatementContext _localctx = new ContinueStatementContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_continueStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(284);
			match(Continue);
			setState(285);
			match(Semicolon);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ElseStatementContext extends ParserRuleContext {
		public TerminalNode Else() { return getToken(CixParser.Else, 0); }
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public ElseStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_elseStatement; }
	}

	public final ElseStatementContext elseStatement() throws RecognitionException {
		ElseStatementContext _localctx = new ElseStatementContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_elseStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(287);
			match(Else);
			setState(288);
			statement();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DoWhileStatementContext extends ParserRuleContext {
		public TerminalNode Do() { return getToken(CixParser.Do, 0); }
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public TerminalNode While() { return getToken(CixParser.While, 0); }
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public DoWhileStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_doWhileStatement; }
	}

	public final DoWhileStatementContext doWhileStatement() throws RecognitionException {
		DoWhileStatementContext _localctx = new DoWhileStatementContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_doWhileStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(290);
			match(Do);
			setState(291);
			statement();
			setState(292);
			match(While);
			setState(293);
			match(LeftParen);
			setState(294);
			expression(0);
			setState(295);
			match(RightParen);
			setState(296);
			match(Semicolon);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionStatementContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public ExpressionStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expressionStatement; }
	}

	public final ExpressionStatementContext expressionStatement() throws RecognitionException {
		ExpressionStatementContext _localctx = new ExpressionStatementContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_expressionStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(298);
			expression(0);
			setState(299);
			match(Semicolon);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ForStatementContext extends ParserRuleContext {
		public TerminalNode For() { return getToken(CixParser.For, 0); }
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public List<TerminalNode> Semicolon() { return getTokens(CixParser.Semicolon); }
		public TerminalNode Semicolon(int i) {
			return getToken(CixParser.Semicolon, i);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public ForStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_forStatement; }
	}

	public final ForStatementContext forStatement() throws RecognitionException {
		ForStatementContext _localctx = new ForStatementContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_forStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(301);
			match(For);
			setState(302);
			match(LeftParen);
			setState(303);
			expression(0);
			setState(304);
			match(Semicolon);
			setState(305);
			expression(0);
			setState(306);
			match(Semicolon);
			setState(307);
			expression(0);
			setState(308);
			match(Semicolon);
			setState(309);
			match(RightParen);
			setState(310);
			statement();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ReturnStatementContext extends ParserRuleContext {
		public TerminalNode Return() { return getToken(CixParser.Return, 0); }
		public TerminalNode Semicolon() { return getToken(CixParser.Semicolon, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public ReturnStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_returnStatement; }
	}

	public final ReturnStatementContext returnStatement() throws RecognitionException {
		ReturnStatementContext _localctx = new ReturnStatementContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_returnStatement);
		try {
			setState(318);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,23,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(312);
				match(Return);
				setState(313);
				match(Semicolon);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(314);
				match(Return);
				setState(315);
				expression(0);
				setState(316);
				match(Semicolon);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SwitchStatementContext extends ParserRuleContext {
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public TerminalNode OpenScope() { return getToken(CixParser.OpenScope, 0); }
		public TerminalNode CloseScope() { return getToken(CixParser.CloseScope, 0); }
		public List<CaseStatementContext> caseStatement() {
			return getRuleContexts(CaseStatementContext.class);
		}
		public CaseStatementContext caseStatement(int i) {
			return getRuleContext(CaseStatementContext.class,i);
		}
		public SwitchStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_switchStatement; }
	}

	public final SwitchStatementContext switchStatement() throws RecognitionException {
		SwitchStatementContext _localctx = new SwitchStatementContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_switchStatement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(320);
			match(T__6);
			setState(321);
			match(LeftParen);
			setState(322);
			expression(0);
			setState(323);
			match(RightParen);
			setState(324);
			match(OpenScope);
			setState(326); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(325);
				caseStatement();
				}
				}
				setState(328); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==Case || _la==Default );
			setState(330);
			match(CloseScope);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class CaseStatementContext extends ParserRuleContext {
		public LiteralCaseStatementContext literalCaseStatement() {
			return getRuleContext(LiteralCaseStatementContext.class,0);
		}
		public DefaultCaseStatementContext defaultCaseStatement() {
			return getRuleContext(DefaultCaseStatementContext.class,0);
		}
		public CaseStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_caseStatement; }
	}

	public final CaseStatementContext caseStatement() throws RecognitionException {
		CaseStatementContext _localctx = new CaseStatementContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_caseStatement);
		try {
			setState(334);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Case:
				enterOuterAlt(_localctx, 1);
				{
				setState(332);
				literalCaseStatement();
				}
				break;
			case Default:
				enterOuterAlt(_localctx, 2);
				{
				setState(333);
				defaultCaseStatement();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LiteralCaseStatementContext extends ParserRuleContext {
		public TerminalNode Case() { return getToken(CixParser.Case, 0); }
		public TerminalNode Integer() { return getToken(CixParser.Integer, 0); }
		public TerminalNode Colon() { return getToken(CixParser.Colon, 0); }
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public TerminalNode StringLiteral() { return getToken(CixParser.StringLiteral, 0); }
		public LiteralCaseStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_literalCaseStatement; }
	}

	public final LiteralCaseStatementContext literalCaseStatement() throws RecognitionException {
		LiteralCaseStatementContext _localctx = new LiteralCaseStatementContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_literalCaseStatement);
		try {
			setState(344);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,26,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(336);
				match(Case);
				setState(337);
				match(Integer);
				setState(338);
				match(Colon);
				setState(339);
				statement();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(340);
				match(Case);
				setState(341);
				match(StringLiteral);
				setState(342);
				match(Colon);
				setState(343);
				statement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DefaultCaseStatementContext extends ParserRuleContext {
		public TerminalNode Default() { return getToken(CixParser.Default, 0); }
		public TerminalNode Colon() { return getToken(CixParser.Colon, 0); }
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public DefaultCaseStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_defaultCaseStatement; }
	}

	public final DefaultCaseStatementContext defaultCaseStatement() throws RecognitionException {
		DefaultCaseStatementContext _localctx = new DefaultCaseStatementContext(_ctx, getState());
		enterRule(_localctx, 72, RULE_defaultCaseStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(346);
			match(Default);
			setState(347);
			match(Colon);
			setState(348);
			statement();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class WhileStatementContext extends ParserRuleContext {
		public TerminalNode While() { return getToken(CixParser.While, 0); }
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public StatementContext statement() {
			return getRuleContext(StatementContext.class,0);
		}
		public WhileStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_whileStatement; }
	}

	public final WhileStatementContext whileStatement() throws RecognitionException {
		WhileStatementContext _localctx = new WhileStatementContext(_ctx, getState());
		enterRule(_localctx, 74, RULE_whileStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(350);
			match(While);
			setState(351);
			match(LeftParen);
			setState(352);
			expression(0);
			setState(353);
			match(RightParen);
			setState(354);
			statement();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class NumberContext extends ParserRuleContext {
		public TerminalNode Integer() { return getToken(CixParser.Integer, 0); }
		public TerminalNode FloatingPoint() { return getToken(CixParser.FloatingPoint, 0); }
		public NumberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_number; }
	}

	public final NumberContext number() throws RecognitionException {
		NumberContext _localctx = new NumberContext(_ctx, getState());
		enterRule(_localctx, 76, RULE_number);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(356);
			_la = _input.LA(1);
			if ( !(_la==Integer || _la==FloatingPoint) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SourceFileContext extends ParserRuleContext {
		public List<StructContext> struct() {
			return getRuleContexts(StructContext.class);
		}
		public StructContext struct(int i) {
			return getRuleContext(StructContext.class,i);
		}
		public List<GlobalVariableDeclarationContext> globalVariableDeclaration() {
			return getRuleContexts(GlobalVariableDeclarationContext.class);
		}
		public GlobalVariableDeclarationContext globalVariableDeclaration(int i) {
			return getRuleContext(GlobalVariableDeclarationContext.class,i);
		}
		public List<FunctionContext> function() {
			return getRuleContexts(FunctionContext.class);
		}
		public FunctionContext function(int i) {
			return getRuleContext(FunctionContext.class,i);
		}
		public SourceFileContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sourceFile; }
	}

	public final SourceFileContext sourceFile() throws RecognitionException {
		SourceFileContext _localctx = new SourceFileContext(_ctx, getState());
		enterRule(_localctx, 78, RULE_sourceFile);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(361); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				setState(361);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case Struct:
					{
					setState(358);
					struct();
					}
					break;
				case Global:
					{
					setState(359);
					globalVariableDeclaration();
					}
					break;
				case T__0:
				case T__1:
				case T__2:
				case T__3:
				case T__4:
				case T__5:
				case Double:
				case Float:
				case Int:
				case Long:
				case Short:
				case Void:
				case Identifier:
					{
					setState(360);
					function();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				setState(363); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << Double) | (1L << Float) | (1L << Global) | (1L << Int) | (1L << Long) | (1L << Short) | (1L << Struct) | (1L << Void))) != 0) || _la==Identifier );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 8:
			return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 3);
		case 1:
			return precpred(_ctx, 4);
		case 2:
			return precpred(_ctx, 2);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3R\u0170\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\3\2\3\2\3\2\5"+
		"\2V\n\2\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\5\3a\n\3\3\4\3\4\3\4\3\4\3"+
		"\4\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\5\5r\n\5\3\6\3\6\3\6\3\6\3"+
		"\6\5\6y\n\6\3\7\3\7\3\b\3\b\3\t\3\t\3\n\3\n\3\n\3\n\3\n\3\n\5\n\u0087"+
		"\n\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\7\n\u0094\n\n\f\n\16"+
		"\n\u0097\13\n\3\13\3\13\5\13\u009b\n\13\3\13\3\13\5\13\u009f\n\13\3\13"+
		"\3\13\5\13\u00a3\n\13\5\13\u00a5\n\13\3\f\3\f\3\f\3\f\3\r\3\r\3\r\3\r"+
		"\3\r\5\r\u00b0\n\r\3\16\3\16\3\17\6\17\u00b5\n\17\r\17\16\17\u00b6\3\20"+
		"\3\20\3\20\3\20\3\20\3\20\3\20\5\20\u00c0\n\20\3\21\3\21\3\21\3\21\3\21"+
		"\3\21\3\22\3\22\3\22\3\22\6\22\u00cc\n\22\r\22\16\22\u00cd\3\22\3\22\3"+
		"\23\3\23\3\23\5\23\u00d5\n\23\3\23\3\23\3\24\3\24\3\24\3\24\3\25\3\25"+
		"\3\25\3\25\5\25\u00e1\n\25\3\26\3\26\3\26\3\26\5\26\u00e7\n\26\3\26\3"+
		"\26\3\26\7\26\u00ec\n\26\f\26\16\26\u00ef\13\26\3\26\3\26\3\27\3\27\3"+
		"\27\3\27\3\27\5\27\u00f8\n\27\3\30\3\30\3\30\3\31\3\31\3\31\3\31\3\31"+
		"\3\31\3\31\3\31\3\31\3\31\3\31\3\31\5\31\u0109\n\31\3\32\3\32\7\32\u010d"+
		"\n\32\f\32\16\32\u0110\13\32\3\32\3\32\3\33\3\33\3\33\3\34\3\34\3\34\3"+
		"\34\3\34\3\34\5\34\u011d\n\34\3\35\3\35\3\35\3\36\3\36\3\36\3\37\3\37"+
		"\3\37\3\37\3\37\3\37\3\37\3\37\3 \3 \3 \3!\3!\3!\3!\3!\3!\3!\3!\3!\3!"+
		"\3!\3\"\3\"\3\"\3\"\3\"\3\"\5\"\u0141\n\"\3#\3#\3#\3#\3#\3#\6#\u0149\n"+
		"#\r#\16#\u014a\3#\3#\3$\3$\5$\u0151\n$\3%\3%\3%\3%\3%\3%\3%\3%\5%\u015b"+
		"\n%\3&\3&\3&\3&\3\'\3\'\3\'\3\'\3\'\3\'\3(\3(\3)\3)\3)\6)\u016c\n)\r)"+
		"\16)\u016d\3)\2\3\22*\2\4\6\b\n\f\16\20\22\24\26\30\32\34\36 \"$&(*,."+
		"\60\62\64\668:<>@BDFHJLNP\2\5\7\2$*,,.\60\62\65<H\b\2\4\b\20\20\22\22"+
		"\26\27\31\31\34\34\3\2KL\2\u017c\2U\3\2\2\2\4`\3\2\2\2\6b\3\2\2\2\bq\3"+
		"\2\2\2\nx\3\2\2\2\fz\3\2\2\2\16|\3\2\2\2\20~\3\2\2\2\22\u0086\3\2\2\2"+
		"\24\u00a4\3\2\2\2\26\u00a6\3\2\2\2\30\u00af\3\2\2\2\32\u00b1\3\2\2\2\34"+
		"\u00b4\3\2\2\2\36\u00bf\3\2\2\2 \u00c1\3\2\2\2\"\u00c7\3\2\2\2$\u00d1"+
		"\3\2\2\2&\u00d8\3\2\2\2(\u00e0\3\2\2\2*\u00e2\3\2\2\2,\u00f7\3\2\2\2."+
		"\u00f9\3\2\2\2\60\u0108\3\2\2\2\62\u010a\3\2\2\2\64\u0113\3\2\2\2\66\u0116"+
		"\3\2\2\28\u011e\3\2\2\2:\u0121\3\2\2\2<\u0124\3\2\2\2>\u012c\3\2\2\2@"+
		"\u012f\3\2\2\2B\u0140\3\2\2\2D\u0142\3\2\2\2F\u0150\3\2\2\2H\u015a\3\2"+
		"\2\2J\u015c\3\2\2\2L\u0160\3\2\2\2N\u0166\3\2\2\2P\u016b\3\2\2\2RV\5N"+
		"(\2SV\7P\2\2TV\7O\2\2UR\3\2\2\2US\3\2\2\2UT\3\2\2\2V\3\3\2\2\2Wa\7*\2"+
		"\2Xa\7,\2\2Ya\7\67\2\2Za\7\66\2\2[a\7.\2\2\\a\7\61\2\2]a\7+\2\2^a\7-\2"+
		"\2_a\5\6\4\2`W\3\2\2\2`X\3\2\2\2`Y\3\2\2\2`Z\3\2\2\2`[\3\2\2\2`\\\3\2"+
		"\2\2`]\3\2\2\2`^\3\2\2\2`_\3\2\2\2a\5\3\2\2\2bc\7\36\2\2cd\5\24\13\2d"+
		"e\7\37\2\2ef\5\22\n\2f\7\3\2\2\2gh\7\36\2\2hi\5\n\6\2ij\7\37\2\2jr\3\2"+
		"\2\2kl\7 \2\2lm\5\22\n\2mn\7!\2\2nr\3\2\2\2or\7+\2\2pr\7-\2\2qg\3\2\2"+
		"\2qk\3\2\2\2qo\3\2\2\2qp\3\2\2\2r\t\3\2\2\2sy\5\22\n\2tu\5\22\n\2uv\7"+
		";\2\2vw\5\n\6\2wy\3\2\2\2xs\3\2\2\2xt\3\2\2\2y\13\3\2\2\2z{\t\2\2\2{\r"+
		"\3\2\2\2|}\78\2\2}\17\3\2\2\2~\177\79\2\2\177\21\3\2\2\2\u0080\u0081\b"+
		"\n\1\2\u0081\u0087\5\2\2\2\u0082\u0083\5\4\3\2\u0083\u0084\5\22\n\7\u0084"+
		"\u0087\3\2\2\2\u0085\u0087\3\2\2\2\u0086\u0080\3\2\2\2\u0086\u0082\3\2"+
		"\2\2\u0086\u0085\3\2\2\2\u0087\u0095\3\2\2\2\u0088\u0089\f\5\2\2\u0089"+
		"\u008a\5\f\7\2\u008a\u008b\5\22\n\6\u008b\u0094\3\2\2\2\u008c\u008d\f"+
		"\6\2\2\u008d\u0094\5\b\5\2\u008e\u008f\f\4\2\2\u008f\u0090\5\16\b\2\u0090"+
		"\u0091\5\22\n\2\u0091\u0092\5\20\t\2\u0092\u0094\3\2\2\2\u0093\u0088\3"+
		"\2\2\2\u0093\u008c\3\2\2\2\u0093\u008e\3\2\2\2\u0094\u0097\3\2\2\2\u0095"+
		"\u0093\3\2\2\2\u0095\u0096\3\2\2\2\u0096\23\3\2\2\2\u0097\u0095\3\2\2"+
		"\2\u0098\u009a\7P\2\2\u0099\u009b\5\34\17\2\u009a\u0099\3\2\2\2\u009a"+
		"\u009b\3\2\2\2\u009b\u00a5\3\2\2\2\u009c\u009e\5\26\f\2\u009d\u009f\5"+
		"\34\17\2\u009e\u009d\3\2\2\2\u009e\u009f\3\2\2\2\u009f\u00a5\3\2\2\2\u00a0"+
		"\u00a2\5\32\16\2\u00a1\u00a3\5\34\17\2\u00a2\u00a1\3\2\2\2\u00a2\u00a3"+
		"\3\2\2\2\u00a3\u00a5\3\2\2\2\u00a4\u0098\3\2\2\2\u00a4\u009c\3\2\2\2\u00a4"+
		"\u00a0\3\2\2\2\u00a5\25\3\2\2\2\u00a6\u00a7\7\3\2\2\u00a7\u00a8\5\30\r"+
		"\2\u00a8\u00a9\7&\2\2\u00a9\27\3\2\2\2\u00aa\u00b0\5\24\13\2\u00ab\u00ac"+
		"\5\24\13\2\u00ac\u00ad\7;\2\2\u00ad\u00ae\5\30\r\2\u00ae\u00b0\3\2\2\2"+
		"\u00af\u00aa\3\2\2\2\u00af\u00ab\3\2\2\2\u00b0\31\3\2\2\2\u00b1\u00b2"+
		"\t\3\2\2\u00b2\33\3\2\2\2\u00b3\u00b5\7.\2\2\u00b4\u00b3\3\2\2\2\u00b5"+
		"\u00b6\3\2\2\2\u00b6\u00b4\3\2\2\2\u00b6\u00b7\3\2\2\2\u00b7\35\3\2\2"+
		"\2\u00b8\u00b9\5\24\13\2\u00b9\u00ba\7P\2\2\u00ba\u00bb\7:\2\2\u00bb\u00c0"+
		"\3\2\2\2\u00bc\u00bd\5 \21\2\u00bd\u00be\7:\2\2\u00be\u00c0\3\2\2\2\u00bf"+
		"\u00b8\3\2\2\2\u00bf\u00bc\3\2\2\2\u00c0\37\3\2\2\2\u00c1\u00c2\5\24\13"+
		"\2\u00c2\u00c3\7P\2\2\u00c3\u00c4\7<\2\2\u00c4\u00c5\5\22\n\2\u00c5\u00c6"+
		"\7:\2\2\u00c6!\3\2\2\2\u00c7\u00c8\7\33\2\2\u00c8\u00c9\7P\2\2\u00c9\u00cb"+
		"\7\"\2\2\u00ca\u00cc\5$\23\2\u00cb\u00ca\3\2\2\2\u00cc\u00cd\3\2\2\2\u00cd"+
		"\u00cb\3\2\2\2\u00cd\u00ce\3\2\2\2\u00ce\u00cf\3\2\2\2\u00cf\u00d0\7#"+
		"\2\2\u00d0#\3\2\2\2\u00d1\u00d2\5\24\13\2\u00d2\u00d4\7P\2\2\u00d3\u00d5"+
		"\5&\24\2\u00d4\u00d3\3\2\2\2\u00d4\u00d5\3\2\2\2\u00d5\u00d6\3\2\2\2\u00d6"+
		"\u00d7\7:\2\2\u00d7%\3\2\2\2\u00d8\u00d9\7 \2\2\u00d9\u00da\7K\2\2\u00da"+
		"\u00db\7!\2\2\u00db\'\3\2\2\2\u00dc\u00dd\7\23\2\2\u00dd\u00e1\5\36\20"+
		"\2\u00de\u00df\7\23\2\2\u00df\u00e1\5 \21\2\u00e0\u00dc\3\2\2\2\u00e0"+
		"\u00de\3\2\2\2\u00e1)\3\2\2\2\u00e2\u00e3\5\24\13\2\u00e3\u00e4\7P\2\2"+
		"\u00e4\u00e6\7\36\2\2\u00e5\u00e7\5,\27\2\u00e6\u00e5\3\2\2\2\u00e6\u00e7"+
		"\3\2\2\2\u00e7\u00e8\3\2\2\2\u00e8\u00e9\7\37\2\2\u00e9\u00ed\7\"\2\2"+
		"\u00ea\u00ec\5\60\31\2\u00eb\u00ea\3\2\2\2\u00ec\u00ef\3\2\2\2\u00ed\u00eb"+
		"\3\2\2\2\u00ed\u00ee\3\2\2\2\u00ee\u00f0\3\2\2\2\u00ef\u00ed\3\2\2\2\u00f0"+
		"\u00f1\7#\2\2\u00f1+\3\2\2\2\u00f2\u00f8\5.\30\2\u00f3\u00f4\5.\30\2\u00f4"+
		"\u00f5\7;\2\2\u00f5\u00f6\5,\27\2\u00f6\u00f8\3\2\2\2\u00f7\u00f2\3\2"+
		"\2\2\u00f7\u00f3\3\2\2\2\u00f8-\3\2\2\2\u00f9\u00fa\5\24\13\2\u00fa\u00fb"+
		"\7P\2\2\u00fb/\3\2\2\2\u00fc\u0109\5\62\32\2\u00fd\u0109\5\64\33\2\u00fe"+
		"\u0109\5\66\34\2\u00ff\u0109\58\35\2\u0100\u0109\5<\37\2\u0101\u0109\5"+
		"> \2\u0102\u0109\5@!\2\u0103\u0109\5B\"\2\u0104\u0109\5D#\2\u0105\u0109"+
		"\5\36\20\2\u0106\u0109\5 \21\2\u0107\u0109\5L\'\2\u0108\u00fc\3\2\2\2"+
		"\u0108\u00fd\3\2\2\2\u0108\u00fe\3\2\2\2\u0108\u00ff\3\2\2\2\u0108\u0100"+
		"\3\2\2\2\u0108\u0101\3\2\2\2\u0108\u0102\3\2\2\2\u0108\u0103\3\2\2\2\u0108"+
		"\u0104\3\2\2\2\u0108\u0105\3\2\2\2\u0108\u0106\3\2\2\2\u0108\u0107\3\2"+
		"\2\2\u0109\61\3\2\2\2\u010a\u010e\7\"\2\2\u010b\u010d\5\60\31\2\u010c"+
		"\u010b\3\2\2\2\u010d\u0110\3\2\2\2\u010e\u010c\3\2\2\2\u010e\u010f\3\2"+
		"\2\2\u010f\u0111\3\2\2\2\u0110\u010e\3\2\2\2\u0111\u0112\7#\2\2\u0112"+
		"\63\3\2\2\2\u0113\u0114\7\13\2\2\u0114\u0115\7:\2\2\u0115\65\3\2\2\2\u0116"+
		"\u0117\7\25\2\2\u0117\u0118\7\36\2\2\u0118\u0119\5\22\n\2\u0119\u011a"+
		"\7\37\2\2\u011a\u011c\5\60\31\2\u011b\u011d\5:\36\2\u011c\u011b\3\2\2"+
		"\2\u011c\u011d\3\2\2\2\u011d\67\3\2\2\2\u011e\u011f\7\r\2\2\u011f\u0120"+
		"\7:\2\2\u01209\3\2\2\2\u0121\u0122\7\21\2\2\u0122\u0123\5\60\31\2\u0123"+
		";\3\2\2\2\u0124\u0125\7\17\2\2\u0125\u0126\5\60\31\2\u0126\u0127\7\35"+
		"\2\2\u0127\u0128\7\36\2\2\u0128\u0129\5\22\n\2\u0129\u012a\7\37\2\2\u012a"+
		"\u012b\7:\2\2\u012b=\3\2\2\2\u012c\u012d\5\22\n\2\u012d\u012e\7:\2\2\u012e"+
		"?\3\2\2\2\u012f\u0130\7\24\2\2\u0130\u0131\7\36\2\2\u0131\u0132\5\22\n"+
		"\2\u0132\u0133\7:\2\2\u0133\u0134\5\22\n\2\u0134\u0135\7:\2\2\u0135\u0136"+
		"\5\22\n\2\u0136\u0137\7:\2\2\u0137\u0138\7\37\2\2\u0138\u0139\5\60\31"+
		"\2\u0139A\3\2\2\2\u013a\u013b\7\30\2\2\u013b\u0141\7:\2\2\u013c\u013d"+
		"\7\30\2\2\u013d\u013e\5\22\n\2\u013e\u013f\7:\2\2\u013f\u0141\3\2\2\2"+
		"\u0140\u013a\3\2\2\2\u0140\u013c\3\2\2\2\u0141C\3\2\2\2\u0142\u0143\7"+
		"\t\2\2\u0143\u0144\7\36\2\2\u0144\u0145\5\22\n\2\u0145\u0146\7\37\2\2"+
		"\u0146\u0148\7\"\2\2\u0147\u0149\5F$\2\u0148\u0147\3\2\2\2\u0149\u014a"+
		"\3\2\2\2\u014a\u0148\3\2\2\2\u014a\u014b\3\2\2\2\u014b\u014c\3\2\2\2\u014c"+
		"\u014d\7#\2\2\u014dE\3\2\2\2\u014e\u0151\5H%\2\u014f\u0151\5J&\2\u0150"+
		"\u014e\3\2\2\2\u0150\u014f\3\2\2\2\u0151G\3\2\2\2\u0152\u0153\7\f\2\2"+
		"\u0153\u0154\7K\2\2\u0154\u0155\79\2\2\u0155\u015b\5\60\31\2\u0156\u0157"+
		"\7\f\2\2\u0157\u0158\7O\2\2\u0158\u0159\79\2\2\u0159\u015b\5\60\31\2\u015a"+
		"\u0152\3\2\2\2\u015a\u0156\3\2\2\2\u015bI\3\2\2\2\u015c\u015d\7\16\2\2"+
		"\u015d\u015e\79\2\2\u015e\u015f\5\60\31\2\u015fK\3\2\2\2\u0160\u0161\7"+
		"\35\2\2\u0161\u0162\7\36\2\2\u0162\u0163\5\22\n\2\u0163\u0164\7\37\2\2"+
		"\u0164\u0165\5\60\31\2\u0165M\3\2\2\2\u0166\u0167\t\4\2\2\u0167O\3\2\2"+
		"\2\u0168\u016c\5\"\22\2\u0169\u016c\5(\25\2\u016a\u016c\5*\26\2\u016b"+
		"\u0168\3\2\2\2\u016b\u0169\3\2\2\2\u016b\u016a\3\2\2\2\u016c\u016d\3\2"+
		"\2\2\u016d\u016b\3\2\2\2\u016d\u016e\3\2\2\2\u016eQ\3\2\2\2\37U`qx\u0086"+
		"\u0093\u0095\u009a\u009e\u00a2\u00a4\u00af\u00b6\u00bf\u00cd\u00d4\u00e0"+
		"\u00e6\u00ed\u00f7\u0108\u010e\u011c\u0140\u014a\u0150\u015a\u016b\u016d";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}