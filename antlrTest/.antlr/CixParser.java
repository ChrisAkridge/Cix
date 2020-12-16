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
		RULE_primaryExpression = 0, RULE_postfixExpression = 1, RULE_argumentExpressionList = 2, 
		RULE_unaryExpression = 3, RULE_unaryOperator = 4, RULE_castExpression = 5, 
		RULE_multiplicativeExpression = 6, RULE_additiveExpression = 7, RULE_shiftExpression = 8, 
		RULE_relationalExpression = 9, RULE_equalityExpression = 10, RULE_andExpression = 11, 
		RULE_exclusiveOrExpression = 12, RULE_inclusiveOrExpression = 13, RULE_logicalAndExpression = 14, 
		RULE_logicalOrExpression = 15, RULE_conditionalExpression = 16, RULE_assignmentExpression = 17, 
		RULE_assignmentOperator = 18, RULE_expression = 19, RULE_constantExpression = 20, 
		RULE_typeName = 21, RULE_funcptrTypeName = 22, RULE_typeNameList = 23, 
		RULE_primitiveType = 24, RULE_pointerAsteriskList = 25, RULE_variableDeclarationStatement = 26, 
		RULE_variableDeclarationWithInitializationStatement = 27, RULE_struct = 28, 
		RULE_structMember = 29, RULE_structArraySize = 30, RULE_globalVariableDeclaration = 31, 
		RULE_function = 32, RULE_functionParameterList = 33, RULE_functionParameter = 34, 
		RULE_statement = 35, RULE_block = 36, RULE_breakStatement = 37, RULE_conditionalStatement = 38, 
		RULE_continueStatement = 39, RULE_elseStatement = 40, RULE_doWhileStatement = 41, 
		RULE_expressionStatement = 42, RULE_forStatement = 43, RULE_returnStatement = 44, 
		RULE_switchStatement = 45, RULE_caseStatement = 46, RULE_literalCaseStatement = 47, 
		RULE_defaultCaseStatement = 48, RULE_whileStatement = 49, RULE_number = 50, 
		RULE_sourceFile = 51;
	private static String[] makeRuleNames() {
		return new String[] {
			"primaryExpression", "postfixExpression", "argumentExpressionList", "unaryExpression", 
			"unaryOperator", "castExpression", "multiplicativeExpression", "additiveExpression", 
			"shiftExpression", "relationalExpression", "equalityExpression", "andExpression", 
			"exclusiveOrExpression", "inclusiveOrExpression", "logicalAndExpression", 
			"logicalOrExpression", "conditionalExpression", "assignmentExpression", 
			"assignmentOperator", "expression", "constantExpression", "typeName", 
			"funcptrTypeName", "typeNameList", "primitiveType", "pointerAsteriskList", 
			"variableDeclarationStatement", "variableDeclarationWithInitializationStatement", 
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

	public static class PrimaryExpressionContext extends ParserRuleContext {
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode StringLiteral() { return getToken(CixParser.StringLiteral, 0); }
		public NumberContext number() {
			return getRuleContext(NumberContext.class,0);
		}
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public PrimaryExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primaryExpression; }
	}

	public final PrimaryExpressionContext primaryExpression() throws RecognitionException {
		PrimaryExpressionContext _localctx = new PrimaryExpressionContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_primaryExpression);
		try {
			setState(111);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Identifier:
				enterOuterAlt(_localctx, 1);
				{
				setState(104);
				match(Identifier);
				}
				break;
			case StringLiteral:
				enterOuterAlt(_localctx, 2);
				{
				setState(105);
				match(StringLiteral);
				}
				break;
			case Integer:
			case FloatingPoint:
				enterOuterAlt(_localctx, 3);
				{
				setState(106);
				number();
				}
				break;
			case LeftParen:
				enterOuterAlt(_localctx, 4);
				{
				setState(107);
				match(LeftParen);
				setState(108);
				expression();
				setState(109);
				match(RightParen);
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

	public static class PostfixExpressionContext extends ParserRuleContext {
		public PrimaryExpressionContext primaryExpression() {
			return getRuleContext(PrimaryExpressionContext.class,0);
		}
		public PostfixExpressionContext postfixExpression() {
			return getRuleContext(PostfixExpressionContext.class,0);
		}
		public TerminalNode LeftBracket() { return getToken(CixParser.LeftBracket, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RightBracket() { return getToken(CixParser.RightBracket, 0); }
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public ArgumentExpressionListContext argumentExpressionList() {
			return getRuleContext(ArgumentExpressionListContext.class,0);
		}
		public TerminalNode DirectMemberAccess() { return getToken(CixParser.DirectMemberAccess, 0); }
		public TerminalNode Identifier() { return getToken(CixParser.Identifier, 0); }
		public TerminalNode PointerMemberAccess() { return getToken(CixParser.PointerMemberAccess, 0); }
		public TerminalNode Increment() { return getToken(CixParser.Increment, 0); }
		public TerminalNode Decrement() { return getToken(CixParser.Decrement, 0); }
		public PostfixExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_postfixExpression; }
	}

	public final PostfixExpressionContext postfixExpression() throws RecognitionException {
		return postfixExpression(0);
	}

	private PostfixExpressionContext postfixExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		PostfixExpressionContext _localctx = new PostfixExpressionContext(_ctx, _parentState);
		PostfixExpressionContext _prevctx = _localctx;
		int _startState = 2;
		enterRecursionRule(_localctx, 2, RULE_postfixExpression, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(114);
			primaryExpression();
			}
			_ctx.stop = _input.LT(-1);
			setState(139);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,3,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(137);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,2,_ctx) ) {
					case 1:
						{
						_localctx = new PostfixExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_postfixExpression);
						setState(116);
						if (!(precpred(_ctx, 6))) throw new FailedPredicateException(this, "precpred(_ctx, 6)");
						setState(117);
						match(LeftBracket);
						setState(118);
						expression();
						setState(119);
						match(RightBracket);
						}
						break;
					case 2:
						{
						_localctx = new PostfixExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_postfixExpression);
						setState(121);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(122);
						match(LeftParen);
						setState(124);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (((((_la - 24)) & ~0x3f) == 0 && ((1L << (_la - 24)) & ((1L << (Sizeof - 24)) | (1L << (LeftParen - 24)) | (1L << (Plus - 24)) | (1L << (Increment - 24)) | (1L << (Minus - 24)) | (1L << (Decrement - 24)) | (1L << (Asterisk - 24)) | (1L << (Ampersand - 24)) | (1L << (LogicalNot - 24)) | (1L << (BitwiseNot - 24)) | (1L << (Integer - 24)) | (1L << (FloatingPoint - 24)) | (1L << (StringLiteral - 24)) | (1L << (Identifier - 24)))) != 0)) {
							{
							setState(123);
							argumentExpressionList(0);
							}
						}

						setState(126);
						match(RightParen);
						}
						break;
					case 3:
						{
						_localctx = new PostfixExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_postfixExpression);
						setState(127);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(128);
						match(DirectMemberAccess);
						setState(129);
						match(Identifier);
						}
						break;
					case 4:
						{
						_localctx = new PostfixExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_postfixExpression);
						setState(130);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(131);
						match(PointerMemberAccess);
						setState(132);
						match(Identifier);
						}
						break;
					case 5:
						{
						_localctx = new PostfixExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_postfixExpression);
						setState(133);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(134);
						match(Increment);
						}
						break;
					case 6:
						{
						_localctx = new PostfixExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_postfixExpression);
						setState(135);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(136);
						match(Decrement);
						}
						break;
					}
					} 
				}
				setState(141);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,3,_ctx);
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

	public static class ArgumentExpressionListContext extends ParserRuleContext {
		public AssignmentExpressionContext assignmentExpression() {
			return getRuleContext(AssignmentExpressionContext.class,0);
		}
		public ArgumentExpressionListContext argumentExpressionList() {
			return getRuleContext(ArgumentExpressionListContext.class,0);
		}
		public TerminalNode Comma() { return getToken(CixParser.Comma, 0); }
		public ArgumentExpressionListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_argumentExpressionList; }
	}

	public final ArgumentExpressionListContext argumentExpressionList() throws RecognitionException {
		return argumentExpressionList(0);
	}

	private ArgumentExpressionListContext argumentExpressionList(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ArgumentExpressionListContext _localctx = new ArgumentExpressionListContext(_ctx, _parentState);
		ArgumentExpressionListContext _prevctx = _localctx;
		int _startState = 4;
		enterRecursionRule(_localctx, 4, RULE_argumentExpressionList, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(143);
			assignmentExpression();
			}
			_ctx.stop = _input.LT(-1);
			setState(150);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,4,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new ArgumentExpressionListContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_argumentExpressionList);
					setState(145);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(146);
					match(Comma);
					setState(147);
					assignmentExpression();
					}
					} 
				}
				setState(152);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,4,_ctx);
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

	public static class UnaryExpressionContext extends ParserRuleContext {
		public PostfixExpressionContext postfixExpression() {
			return getRuleContext(PostfixExpressionContext.class,0);
		}
		public TerminalNode Increment() { return getToken(CixParser.Increment, 0); }
		public UnaryExpressionContext unaryExpression() {
			return getRuleContext(UnaryExpressionContext.class,0);
		}
		public TerminalNode Decrement() { return getToken(CixParser.Decrement, 0); }
		public UnaryOperatorContext unaryOperator() {
			return getRuleContext(UnaryOperatorContext.class,0);
		}
		public CastExpressionContext castExpression() {
			return getRuleContext(CastExpressionContext.class,0);
		}
		public TerminalNode Sizeof() { return getToken(CixParser.Sizeof, 0); }
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public UnaryExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unaryExpression; }
	}

	public final UnaryExpressionContext unaryExpression() throws RecognitionException {
		UnaryExpressionContext _localctx = new UnaryExpressionContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_unaryExpression);
		try {
			setState(168);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,5,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(153);
				postfixExpression(0);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(154);
				match(Increment);
				setState(155);
				unaryExpression();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(156);
				match(Decrement);
				setState(157);
				unaryExpression();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(158);
				unaryOperator();
				setState(159);
				castExpression();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(161);
				match(Sizeof);
				setState(162);
				unaryExpression();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(163);
				match(Sizeof);
				setState(164);
				match(LeftParen);
				setState(165);
				typeName();
				setState(166);
				match(RightParen);
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

	public static class UnaryOperatorContext extends ParserRuleContext {
		public TerminalNode Ampersand() { return getToken(CixParser.Ampersand, 0); }
		public TerminalNode Asterisk() { return getToken(CixParser.Asterisk, 0); }
		public TerminalNode Plus() { return getToken(CixParser.Plus, 0); }
		public TerminalNode Minus() { return getToken(CixParser.Minus, 0); }
		public TerminalNode BitwiseNot() { return getToken(CixParser.BitwiseNot, 0); }
		public TerminalNode LogicalNot() { return getToken(CixParser.LogicalNot, 0); }
		public UnaryOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_unaryOperator; }
	}

	public final UnaryOperatorContext unaryOperator() throws RecognitionException {
		UnaryOperatorContext _localctx = new UnaryOperatorContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_unaryOperator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(170);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << Plus) | (1L << Minus) | (1L << Asterisk) | (1L << Ampersand) | (1L << LogicalNot) | (1L << BitwiseNot))) != 0)) ) {
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

	public static class CastExpressionContext extends ParserRuleContext {
		public TerminalNode LeftParen() { return getToken(CixParser.LeftParen, 0); }
		public TypeNameContext typeName() {
			return getRuleContext(TypeNameContext.class,0);
		}
		public TerminalNode RightParen() { return getToken(CixParser.RightParen, 0); }
		public CastExpressionContext castExpression() {
			return getRuleContext(CastExpressionContext.class,0);
		}
		public UnaryExpressionContext unaryExpression() {
			return getRuleContext(UnaryExpressionContext.class,0);
		}
		public CastExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_castExpression; }
	}

	public final CastExpressionContext castExpression() throws RecognitionException {
		CastExpressionContext _localctx = new CastExpressionContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_castExpression);
		try {
			setState(178);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(172);
				match(LeftParen);
				setState(173);
				typeName();
				setState(174);
				match(RightParen);
				setState(175);
				castExpression();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(177);
				unaryExpression();
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

	public static class MultiplicativeExpressionContext extends ParserRuleContext {
		public CastExpressionContext castExpression() {
			return getRuleContext(CastExpressionContext.class,0);
		}
		public MultiplicativeExpressionContext multiplicativeExpression() {
			return getRuleContext(MultiplicativeExpressionContext.class,0);
		}
		public TerminalNode Asterisk() { return getToken(CixParser.Asterisk, 0); }
		public TerminalNode Divide() { return getToken(CixParser.Divide, 0); }
		public TerminalNode Modulus() { return getToken(CixParser.Modulus, 0); }
		public MultiplicativeExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_multiplicativeExpression; }
	}

	public final MultiplicativeExpressionContext multiplicativeExpression() throws RecognitionException {
		return multiplicativeExpression(0);
	}

	private MultiplicativeExpressionContext multiplicativeExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		MultiplicativeExpressionContext _localctx = new MultiplicativeExpressionContext(_ctx, _parentState);
		MultiplicativeExpressionContext _prevctx = _localctx;
		int _startState = 12;
		enterRecursionRule(_localctx, 12, RULE_multiplicativeExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(181);
			castExpression();
			}
			_ctx.stop = _input.LT(-1);
			setState(194);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(192);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,7,_ctx) ) {
					case 1:
						{
						_localctx = new MultiplicativeExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_multiplicativeExpression);
						setState(183);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(184);
						match(Asterisk);
						setState(185);
						castExpression();
						}
						break;
					case 2:
						{
						_localctx = new MultiplicativeExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_multiplicativeExpression);
						setState(186);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(187);
						match(Divide);
						setState(188);
						castExpression();
						}
						break;
					case 3:
						{
						_localctx = new MultiplicativeExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_multiplicativeExpression);
						setState(189);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(190);
						match(Modulus);
						setState(191);
						castExpression();
						}
						break;
					}
					} 
				}
				setState(196);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
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

	public static class AdditiveExpressionContext extends ParserRuleContext {
		public MultiplicativeExpressionContext multiplicativeExpression() {
			return getRuleContext(MultiplicativeExpressionContext.class,0);
		}
		public AdditiveExpressionContext additiveExpression() {
			return getRuleContext(AdditiveExpressionContext.class,0);
		}
		public TerminalNode Plus() { return getToken(CixParser.Plus, 0); }
		public TerminalNode Minus() { return getToken(CixParser.Minus, 0); }
		public AdditiveExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_additiveExpression; }
	}

	public final AdditiveExpressionContext additiveExpression() throws RecognitionException {
		return additiveExpression(0);
	}

	private AdditiveExpressionContext additiveExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		AdditiveExpressionContext _localctx = new AdditiveExpressionContext(_ctx, _parentState);
		AdditiveExpressionContext _prevctx = _localctx;
		int _startState = 14;
		enterRecursionRule(_localctx, 14, RULE_additiveExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(198);
			multiplicativeExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(208);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,10,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(206);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,9,_ctx) ) {
					case 1:
						{
						_localctx = new AdditiveExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_additiveExpression);
						setState(200);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(201);
						match(Plus);
						setState(202);
						multiplicativeExpression(0);
						}
						break;
					case 2:
						{
						_localctx = new AdditiveExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_additiveExpression);
						setState(203);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(204);
						match(Minus);
						setState(205);
						multiplicativeExpression(0);
						}
						break;
					}
					} 
				}
				setState(210);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,10,_ctx);
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

	public static class ShiftExpressionContext extends ParserRuleContext {
		public AdditiveExpressionContext additiveExpression() {
			return getRuleContext(AdditiveExpressionContext.class,0);
		}
		public ShiftExpressionContext shiftExpression() {
			return getRuleContext(ShiftExpressionContext.class,0);
		}
		public TerminalNode ShiftLeft() { return getToken(CixParser.ShiftLeft, 0); }
		public TerminalNode ShiftRight() { return getToken(CixParser.ShiftRight, 0); }
		public ShiftExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_shiftExpression; }
	}

	public final ShiftExpressionContext shiftExpression() throws RecognitionException {
		return shiftExpression(0);
	}

	private ShiftExpressionContext shiftExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ShiftExpressionContext _localctx = new ShiftExpressionContext(_ctx, _parentState);
		ShiftExpressionContext _prevctx = _localctx;
		int _startState = 16;
		enterRecursionRule(_localctx, 16, RULE_shiftExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(212);
			additiveExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(222);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,12,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(220);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,11,_ctx) ) {
					case 1:
						{
						_localctx = new ShiftExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_shiftExpression);
						setState(214);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(215);
						match(ShiftLeft);
						setState(216);
						additiveExpression(0);
						}
						break;
					case 2:
						{
						_localctx = new ShiftExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_shiftExpression);
						setState(217);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(218);
						match(ShiftRight);
						setState(219);
						additiveExpression(0);
						}
						break;
					}
					} 
				}
				setState(224);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,12,_ctx);
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

	public static class RelationalExpressionContext extends ParserRuleContext {
		public ShiftExpressionContext shiftExpression() {
			return getRuleContext(ShiftExpressionContext.class,0);
		}
		public RelationalExpressionContext relationalExpression() {
			return getRuleContext(RelationalExpressionContext.class,0);
		}
		public TerminalNode LessThan() { return getToken(CixParser.LessThan, 0); }
		public TerminalNode GreaterThan() { return getToken(CixParser.GreaterThan, 0); }
		public TerminalNode LessThanOrEqualTo() { return getToken(CixParser.LessThanOrEqualTo, 0); }
		public TerminalNode GreaterThanOrEqualTo() { return getToken(CixParser.GreaterThanOrEqualTo, 0); }
		public RelationalExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_relationalExpression; }
	}

	public final RelationalExpressionContext relationalExpression() throws RecognitionException {
		return relationalExpression(0);
	}

	private RelationalExpressionContext relationalExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		RelationalExpressionContext _localctx = new RelationalExpressionContext(_ctx, _parentState);
		RelationalExpressionContext _prevctx = _localctx;
		int _startState = 18;
		enterRecursionRule(_localctx, 18, RULE_relationalExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(226);
			shiftExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(242);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,14,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(240);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,13,_ctx) ) {
					case 1:
						{
						_localctx = new RelationalExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_relationalExpression);
						setState(228);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(229);
						match(LessThan);
						setState(230);
						shiftExpression(0);
						}
						break;
					case 2:
						{
						_localctx = new RelationalExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_relationalExpression);
						setState(231);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(232);
						match(GreaterThan);
						setState(233);
						shiftExpression(0);
						}
						break;
					case 3:
						{
						_localctx = new RelationalExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_relationalExpression);
						setState(234);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(235);
						match(LessThanOrEqualTo);
						setState(236);
						shiftExpression(0);
						}
						break;
					case 4:
						{
						_localctx = new RelationalExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_relationalExpression);
						setState(237);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(238);
						match(GreaterThanOrEqualTo);
						setState(239);
						shiftExpression(0);
						}
						break;
					}
					} 
				}
				setState(244);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,14,_ctx);
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

	public static class EqualityExpressionContext extends ParserRuleContext {
		public RelationalExpressionContext relationalExpression() {
			return getRuleContext(RelationalExpressionContext.class,0);
		}
		public EqualityExpressionContext equalityExpression() {
			return getRuleContext(EqualityExpressionContext.class,0);
		}
		public TerminalNode Equals() { return getToken(CixParser.Equals, 0); }
		public TerminalNode NotEquals() { return getToken(CixParser.NotEquals, 0); }
		public EqualityExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_equalityExpression; }
	}

	public final EqualityExpressionContext equalityExpression() throws RecognitionException {
		return equalityExpression(0);
	}

	private EqualityExpressionContext equalityExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		EqualityExpressionContext _localctx = new EqualityExpressionContext(_ctx, _parentState);
		EqualityExpressionContext _prevctx = _localctx;
		int _startState = 20;
		enterRecursionRule(_localctx, 20, RULE_equalityExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(246);
			relationalExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(256);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,16,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(254);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,15,_ctx) ) {
					case 1:
						{
						_localctx = new EqualityExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_equalityExpression);
						setState(248);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(249);
						match(Equals);
						setState(250);
						relationalExpression(0);
						}
						break;
					case 2:
						{
						_localctx = new EqualityExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_equalityExpression);
						setState(251);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(252);
						match(NotEquals);
						setState(253);
						relationalExpression(0);
						}
						break;
					}
					} 
				}
				setState(258);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,16,_ctx);
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

	public static class AndExpressionContext extends ParserRuleContext {
		public EqualityExpressionContext equalityExpression() {
			return getRuleContext(EqualityExpressionContext.class,0);
		}
		public AndExpressionContext andExpression() {
			return getRuleContext(AndExpressionContext.class,0);
		}
		public TerminalNode Ampersand() { return getToken(CixParser.Ampersand, 0); }
		public AndExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_andExpression; }
	}

	public final AndExpressionContext andExpression() throws RecognitionException {
		return andExpression(0);
	}

	private AndExpressionContext andExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		AndExpressionContext _localctx = new AndExpressionContext(_ctx, _parentState);
		AndExpressionContext _prevctx = _localctx;
		int _startState = 22;
		enterRecursionRule(_localctx, 22, RULE_andExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(260);
			equalityExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(267);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new AndExpressionContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_andExpression);
					setState(262);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(263);
					match(Ampersand);
					setState(264);
					equalityExpression(0);
					}
					} 
				}
				setState(269);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
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

	public static class ExclusiveOrExpressionContext extends ParserRuleContext {
		public AndExpressionContext andExpression() {
			return getRuleContext(AndExpressionContext.class,0);
		}
		public ExclusiveOrExpressionContext exclusiveOrExpression() {
			return getRuleContext(ExclusiveOrExpressionContext.class,0);
		}
		public TerminalNode BitwiseXor() { return getToken(CixParser.BitwiseXor, 0); }
		public ExclusiveOrExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_exclusiveOrExpression; }
	}

	public final ExclusiveOrExpressionContext exclusiveOrExpression() throws RecognitionException {
		return exclusiveOrExpression(0);
	}

	private ExclusiveOrExpressionContext exclusiveOrExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExclusiveOrExpressionContext _localctx = new ExclusiveOrExpressionContext(_ctx, _parentState);
		ExclusiveOrExpressionContext _prevctx = _localctx;
		int _startState = 24;
		enterRecursionRule(_localctx, 24, RULE_exclusiveOrExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(271);
			andExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(278);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,18,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new ExclusiveOrExpressionContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_exclusiveOrExpression);
					setState(273);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(274);
					match(BitwiseXor);
					setState(275);
					andExpression(0);
					}
					} 
				}
				setState(280);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,18,_ctx);
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

	public static class InclusiveOrExpressionContext extends ParserRuleContext {
		public ExclusiveOrExpressionContext exclusiveOrExpression() {
			return getRuleContext(ExclusiveOrExpressionContext.class,0);
		}
		public InclusiveOrExpressionContext inclusiveOrExpression() {
			return getRuleContext(InclusiveOrExpressionContext.class,0);
		}
		public TerminalNode BitwiseOr() { return getToken(CixParser.BitwiseOr, 0); }
		public InclusiveOrExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_inclusiveOrExpression; }
	}

	public final InclusiveOrExpressionContext inclusiveOrExpression() throws RecognitionException {
		return inclusiveOrExpression(0);
	}

	private InclusiveOrExpressionContext inclusiveOrExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		InclusiveOrExpressionContext _localctx = new InclusiveOrExpressionContext(_ctx, _parentState);
		InclusiveOrExpressionContext _prevctx = _localctx;
		int _startState = 26;
		enterRecursionRule(_localctx, 26, RULE_inclusiveOrExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(282);
			exclusiveOrExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(289);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,19,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new InclusiveOrExpressionContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_inclusiveOrExpression);
					setState(284);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(285);
					match(BitwiseOr);
					setState(286);
					exclusiveOrExpression(0);
					}
					} 
				}
				setState(291);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,19,_ctx);
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

	public static class LogicalAndExpressionContext extends ParserRuleContext {
		public InclusiveOrExpressionContext inclusiveOrExpression() {
			return getRuleContext(InclusiveOrExpressionContext.class,0);
		}
		public LogicalAndExpressionContext logicalAndExpression() {
			return getRuleContext(LogicalAndExpressionContext.class,0);
		}
		public TerminalNode LogicalAnd() { return getToken(CixParser.LogicalAnd, 0); }
		public LogicalAndExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logicalAndExpression; }
	}

	public final LogicalAndExpressionContext logicalAndExpression() throws RecognitionException {
		return logicalAndExpression(0);
	}

	private LogicalAndExpressionContext logicalAndExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		LogicalAndExpressionContext _localctx = new LogicalAndExpressionContext(_ctx, _parentState);
		LogicalAndExpressionContext _prevctx = _localctx;
		int _startState = 28;
		enterRecursionRule(_localctx, 28, RULE_logicalAndExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(293);
			inclusiveOrExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(300);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,20,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new LogicalAndExpressionContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_logicalAndExpression);
					setState(295);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(296);
					match(LogicalAnd);
					setState(297);
					inclusiveOrExpression(0);
					}
					} 
				}
				setState(302);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,20,_ctx);
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

	public static class LogicalOrExpressionContext extends ParserRuleContext {
		public LogicalAndExpressionContext logicalAndExpression() {
			return getRuleContext(LogicalAndExpressionContext.class,0);
		}
		public LogicalOrExpressionContext logicalOrExpression() {
			return getRuleContext(LogicalOrExpressionContext.class,0);
		}
		public TerminalNode LogicalOr() { return getToken(CixParser.LogicalOr, 0); }
		public LogicalOrExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logicalOrExpression; }
	}

	public final LogicalOrExpressionContext logicalOrExpression() throws RecognitionException {
		return logicalOrExpression(0);
	}

	private LogicalOrExpressionContext logicalOrExpression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		LogicalOrExpressionContext _localctx = new LogicalOrExpressionContext(_ctx, _parentState);
		LogicalOrExpressionContext _prevctx = _localctx;
		int _startState = 30;
		enterRecursionRule(_localctx, 30, RULE_logicalOrExpression, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(304);
			logicalAndExpression(0);
			}
			_ctx.stop = _input.LT(-1);
			setState(311);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,21,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new LogicalOrExpressionContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_logicalOrExpression);
					setState(306);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(307);
					match(LogicalOr);
					setState(308);
					logicalAndExpression(0);
					}
					} 
				}
				setState(313);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,21,_ctx);
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

	public static class ConditionalExpressionContext extends ParserRuleContext {
		public LogicalOrExpressionContext logicalOrExpression() {
			return getRuleContext(LogicalOrExpressionContext.class,0);
		}
		public TerminalNode Question() { return getToken(CixParser.Question, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode Colon() { return getToken(CixParser.Colon, 0); }
		public ConditionalExpressionContext conditionalExpression() {
			return getRuleContext(ConditionalExpressionContext.class,0);
		}
		public ConditionalExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_conditionalExpression; }
	}

	public final ConditionalExpressionContext conditionalExpression() throws RecognitionException {
		ConditionalExpressionContext _localctx = new ConditionalExpressionContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_conditionalExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(314);
			logicalOrExpression(0);
			setState(320);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,22,_ctx) ) {
			case 1:
				{
				setState(315);
				match(Question);
				setState(316);
				expression();
				setState(317);
				match(Colon);
				setState(318);
				conditionalExpression();
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

	public static class AssignmentExpressionContext extends ParserRuleContext {
		public ConditionalExpressionContext conditionalExpression() {
			return getRuleContext(ConditionalExpressionContext.class,0);
		}
		public UnaryExpressionContext unaryExpression() {
			return getRuleContext(UnaryExpressionContext.class,0);
		}
		public AssignmentOperatorContext assignmentOperator() {
			return getRuleContext(AssignmentOperatorContext.class,0);
		}
		public AssignmentExpressionContext assignmentExpression() {
			return getRuleContext(AssignmentExpressionContext.class,0);
		}
		public AssignmentExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_assignmentExpression; }
	}

	public final AssignmentExpressionContext assignmentExpression() throws RecognitionException {
		AssignmentExpressionContext _localctx = new AssignmentExpressionContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_assignmentExpression);
		try {
			setState(327);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,23,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(322);
				conditionalExpression();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(323);
				unaryExpression();
				setState(324);
				assignmentOperator();
				setState(325);
				assignmentExpression();
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

	public static class AssignmentOperatorContext extends ParserRuleContext {
		public TerminalNode Assign() { return getToken(CixParser.Assign, 0); }
		public TerminalNode MultiplyAssign() { return getToken(CixParser.MultiplyAssign, 0); }
		public TerminalNode DivideAssign() { return getToken(CixParser.DivideAssign, 0); }
		public TerminalNode ModulusAssign() { return getToken(CixParser.ModulusAssign, 0); }
		public TerminalNode AddAssign() { return getToken(CixParser.AddAssign, 0); }
		public TerminalNode SubtractAssign() { return getToken(CixParser.SubtractAssign, 0); }
		public TerminalNode ShiftLeftAssign() { return getToken(CixParser.ShiftLeftAssign, 0); }
		public TerminalNode ShiftRightAssign() { return getToken(CixParser.ShiftRightAssign, 0); }
		public TerminalNode BitwiseAndAssign() { return getToken(CixParser.BitwiseAndAssign, 0); }
		public TerminalNode BitwiseXorAssign() { return getToken(CixParser.BitwiseXorAssign, 0); }
		public TerminalNode BitwiseOrAssign() { return getToken(CixParser.BitwiseOrAssign, 0); }
		public AssignmentOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_assignmentOperator; }
	}

	public final AssignmentOperatorContext assignmentOperator() throws RecognitionException {
		AssignmentOperatorContext _localctx = new AssignmentOperatorContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_assignmentOperator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(329);
			_la = _input.LA(1);
			if ( !(((((_la - 58)) & ~0x3f) == 0 && ((1L << (_la - 58)) & ((1L << (Assign - 58)) | (1L << (MultiplyAssign - 58)) | (1L << (DivideAssign - 58)) | (1L << (ModulusAssign - 58)) | (1L << (AddAssign - 58)) | (1L << (SubtractAssign - 58)) | (1L << (ShiftLeftAssign - 58)) | (1L << (ShiftRightAssign - 58)) | (1L << (BitwiseAndAssign - 58)) | (1L << (BitwiseXorAssign - 58)) | (1L << (BitwiseOrAssign - 58)))) != 0)) ) {
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

	public static class ExpressionContext extends ParserRuleContext {
		public AssignmentExpressionContext assignmentExpression() {
			return getRuleContext(AssignmentExpressionContext.class,0);
		}
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
	}

	public final ExpressionContext expression() throws RecognitionException {
		ExpressionContext _localctx = new ExpressionContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_expression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(331);
			assignmentExpression();
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

	public static class ConstantExpressionContext extends ParserRuleContext {
		public ConditionalExpressionContext conditionalExpression() {
			return getRuleContext(ConditionalExpressionContext.class,0);
		}
		public ConstantExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constantExpression; }
	}

	public final ConstantExpressionContext constantExpression() throws RecognitionException {
		ConstantExpressionContext _localctx = new ConstantExpressionContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_constantExpression);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(333);
			conditionalExpression();
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
		enterRule(_localctx, 42, RULE_typeName);
		int _la;
		try {
			setState(347);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Identifier:
				enterOuterAlt(_localctx, 1);
				{
				setState(335);
				match(Identifier);
				setState(337);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==Asterisk) {
					{
					setState(336);
					pointerAsteriskList();
					}
				}

				}
				break;
			case T__0:
				enterOuterAlt(_localctx, 2);
				{
				setState(339);
				funcptrTypeName();
				setState(341);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==Asterisk) {
					{
					setState(340);
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
				setState(343);
				primitiveType();
				setState(345);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==Asterisk) {
					{
					setState(344);
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
		enterRule(_localctx, 44, RULE_funcptrTypeName);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(349);
			match(T__0);
			setState(350);
			typeNameList();
			setState(351);
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
		enterRule(_localctx, 46, RULE_typeNameList);
		try {
			setState(358);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,28,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(353);
				typeName();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(354);
				typeName();
				setState(355);
				match(Comma);
				setState(356);
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
		enterRule(_localctx, 48, RULE_primitiveType);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(360);
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
		enterRule(_localctx, 50, RULE_pointerAsteriskList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(363); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(362);
				match(Asterisk);
				}
				}
				setState(365); 
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
		enterRule(_localctx, 52, RULE_variableDeclarationStatement);
		try {
			setState(374);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,30,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(367);
				typeName();
				setState(368);
				match(Identifier);
				setState(369);
				match(Semicolon);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(371);
				variableDeclarationWithInitializationStatement();
				setState(372);
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
		enterRule(_localctx, 54, RULE_variableDeclarationWithInitializationStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(376);
			typeName();
			setState(377);
			match(Identifier);
			setState(378);
			match(Assign);
			setState(379);
			expression();
			setState(380);
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
		enterRule(_localctx, 56, RULE_struct);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(382);
			match(Struct);
			setState(383);
			match(Identifier);
			setState(384);
			match(OpenScope);
			setState(386); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(385);
				structMember();
				}
				}
				setState(388); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << Double) | (1L << Float) | (1L << Int) | (1L << Long) | (1L << Short) | (1L << Void))) != 0) || _la==Identifier );
			setState(390);
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
		enterRule(_localctx, 58, RULE_structMember);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(392);
			typeName();
			setState(393);
			match(Identifier);
			setState(395);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LeftBracket) {
				{
				setState(394);
				structArraySize();
				}
			}

			setState(397);
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
		enterRule(_localctx, 60, RULE_structArraySize);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(399);
			match(LeftBracket);
			setState(400);
			match(Integer);
			setState(401);
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
		enterRule(_localctx, 62, RULE_globalVariableDeclaration);
		try {
			setState(407);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,33,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(403);
				match(Global);
				setState(404);
				variableDeclarationStatement();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(405);
				match(Global);
				setState(406);
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
		enterRule(_localctx, 64, RULE_function);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(409);
			typeName();
			setState(410);
			match(Identifier);
			setState(411);
			match(LeftParen);
			setState(413);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << Double) | (1L << Float) | (1L << Int) | (1L << Long) | (1L << Short) | (1L << Void))) != 0) || _la==Identifier) {
				{
				setState(412);
				functionParameterList();
				}
			}

			setState(415);
			match(RightParen);
			setState(416);
			match(OpenScope);
			setState(420);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << T__6) | (1L << Break) | (1L << Continue) | (1L << Do) | (1L << Double) | (1L << Float) | (1L << For) | (1L << If) | (1L << Int) | (1L << Long) | (1L << Return) | (1L << Short) | (1L << Sizeof) | (1L << Void) | (1L << While) | (1L << LeftParen) | (1L << OpenScope) | (1L << Plus) | (1L << Increment) | (1L << Minus) | (1L << Decrement) | (1L << Asterisk) | (1L << Ampersand) | (1L << LogicalNot) | (1L << BitwiseNot))) != 0) || ((((_la - 73)) & ~0x3f) == 0 && ((1L << (_la - 73)) & ((1L << (Integer - 73)) | (1L << (FloatingPoint - 73)) | (1L << (StringLiteral - 73)) | (1L << (Identifier - 73)))) != 0)) {
				{
				{
				setState(417);
				statement();
				}
				}
				setState(422);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(423);
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
		enterRule(_localctx, 66, RULE_functionParameterList);
		try {
			setState(430);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,36,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(425);
				functionParameter();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(426);
				functionParameter();
				setState(427);
				match(Comma);
				setState(428);
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
		enterRule(_localctx, 68, RULE_functionParameter);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(432);
			typeName();
			setState(433);
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
		enterRule(_localctx, 70, RULE_statement);
		try {
			setState(447);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,37,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(435);
				block();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(436);
				breakStatement();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(437);
				conditionalStatement();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(438);
				continueStatement();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(439);
				doWhileStatement();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(440);
				expressionStatement();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(441);
				forStatement();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(442);
				returnStatement();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(443);
				switchStatement();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(444);
				variableDeclarationStatement();
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(445);
				variableDeclarationWithInitializationStatement();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(446);
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
		enterRule(_localctx, 72, RULE_block);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(449);
			match(OpenScope);
			setState(453);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << T__6) | (1L << Break) | (1L << Continue) | (1L << Do) | (1L << Double) | (1L << Float) | (1L << For) | (1L << If) | (1L << Int) | (1L << Long) | (1L << Return) | (1L << Short) | (1L << Sizeof) | (1L << Void) | (1L << While) | (1L << LeftParen) | (1L << OpenScope) | (1L << Plus) | (1L << Increment) | (1L << Minus) | (1L << Decrement) | (1L << Asterisk) | (1L << Ampersand) | (1L << LogicalNot) | (1L << BitwiseNot))) != 0) || ((((_la - 73)) & ~0x3f) == 0 && ((1L << (_la - 73)) & ((1L << (Integer - 73)) | (1L << (FloatingPoint - 73)) | (1L << (StringLiteral - 73)) | (1L << (Identifier - 73)))) != 0)) {
				{
				{
				setState(450);
				statement();
				}
				}
				setState(455);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(456);
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
		enterRule(_localctx, 74, RULE_breakStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(458);
			match(Break);
			setState(459);
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
		enterRule(_localctx, 76, RULE_conditionalStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(461);
			match(If);
			setState(462);
			match(LeftParen);
			setState(463);
			expression();
			setState(464);
			match(RightParen);
			setState(465);
			statement();
			setState(467);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,39,_ctx) ) {
			case 1:
				{
				setState(466);
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
		enterRule(_localctx, 78, RULE_continueStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(469);
			match(Continue);
			setState(470);
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
		enterRule(_localctx, 80, RULE_elseStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(472);
			match(Else);
			setState(473);
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
		enterRule(_localctx, 82, RULE_doWhileStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(475);
			match(Do);
			setState(476);
			statement();
			setState(477);
			match(While);
			setState(478);
			match(LeftParen);
			setState(479);
			expression();
			setState(480);
			match(RightParen);
			setState(481);
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
		enterRule(_localctx, 84, RULE_expressionStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(483);
			expression();
			setState(484);
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
		enterRule(_localctx, 86, RULE_forStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(486);
			match(For);
			setState(487);
			match(LeftParen);
			setState(488);
			expression();
			setState(489);
			match(Semicolon);
			setState(490);
			expression();
			setState(491);
			match(Semicolon);
			setState(492);
			expression();
			setState(493);
			match(Semicolon);
			setState(494);
			match(RightParen);
			setState(495);
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
		enterRule(_localctx, 88, RULE_returnStatement);
		try {
			setState(503);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,40,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(497);
				match(Return);
				setState(498);
				match(Semicolon);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(499);
				match(Return);
				setState(500);
				expression();
				setState(501);
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
		enterRule(_localctx, 90, RULE_switchStatement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(505);
			match(T__6);
			setState(506);
			match(LeftParen);
			setState(507);
			expression();
			setState(508);
			match(RightParen);
			setState(509);
			match(OpenScope);
			setState(511); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(510);
				caseStatement();
				}
				}
				setState(513); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==Case || _la==Default );
			setState(515);
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
		enterRule(_localctx, 92, RULE_caseStatement);
		try {
			setState(519);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case Case:
				enterOuterAlt(_localctx, 1);
				{
				setState(517);
				literalCaseStatement();
				}
				break;
			case Default:
				enterOuterAlt(_localctx, 2);
				{
				setState(518);
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
		enterRule(_localctx, 94, RULE_literalCaseStatement);
		try {
			setState(529);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,43,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(521);
				match(Case);
				setState(522);
				match(Integer);
				setState(523);
				match(Colon);
				setState(524);
				statement();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(525);
				match(Case);
				setState(526);
				match(StringLiteral);
				setState(527);
				match(Colon);
				setState(528);
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
		enterRule(_localctx, 96, RULE_defaultCaseStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(531);
			match(Default);
			setState(532);
			match(Colon);
			setState(533);
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
		enterRule(_localctx, 98, RULE_whileStatement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(535);
			match(While);
			setState(536);
			match(LeftParen);
			setState(537);
			expression();
			setState(538);
			match(RightParen);
			setState(539);
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
		enterRule(_localctx, 100, RULE_number);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(541);
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
		enterRule(_localctx, 102, RULE_sourceFile);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(546); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				setState(546);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case Struct:
					{
					setState(543);
					struct();
					}
					break;
				case Global:
					{
					setState(544);
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
					setState(545);
					function();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				setState(548); 
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
		case 1:
			return postfixExpression_sempred((PostfixExpressionContext)_localctx, predIndex);
		case 2:
			return argumentExpressionList_sempred((ArgumentExpressionListContext)_localctx, predIndex);
		case 6:
			return multiplicativeExpression_sempred((MultiplicativeExpressionContext)_localctx, predIndex);
		case 7:
			return additiveExpression_sempred((AdditiveExpressionContext)_localctx, predIndex);
		case 8:
			return shiftExpression_sempred((ShiftExpressionContext)_localctx, predIndex);
		case 9:
			return relationalExpression_sempred((RelationalExpressionContext)_localctx, predIndex);
		case 10:
			return equalityExpression_sempred((EqualityExpressionContext)_localctx, predIndex);
		case 11:
			return andExpression_sempred((AndExpressionContext)_localctx, predIndex);
		case 12:
			return exclusiveOrExpression_sempred((ExclusiveOrExpressionContext)_localctx, predIndex);
		case 13:
			return inclusiveOrExpression_sempred((InclusiveOrExpressionContext)_localctx, predIndex);
		case 14:
			return logicalAndExpression_sempred((LogicalAndExpressionContext)_localctx, predIndex);
		case 15:
			return logicalOrExpression_sempred((LogicalOrExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean postfixExpression_sempred(PostfixExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 6);
		case 1:
			return precpred(_ctx, 5);
		case 2:
			return precpred(_ctx, 4);
		case 3:
			return precpred(_ctx, 3);
		case 4:
			return precpred(_ctx, 2);
		case 5:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean argumentExpressionList_sempred(ArgumentExpressionListContext _localctx, int predIndex) {
		switch (predIndex) {
		case 6:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean multiplicativeExpression_sempred(MultiplicativeExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 7:
			return precpred(_ctx, 3);
		case 8:
			return precpred(_ctx, 2);
		case 9:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean additiveExpression_sempred(AdditiveExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 10:
			return precpred(_ctx, 2);
		case 11:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean shiftExpression_sempred(ShiftExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 12:
			return precpred(_ctx, 2);
		case 13:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean relationalExpression_sempred(RelationalExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 14:
			return precpred(_ctx, 4);
		case 15:
			return precpred(_ctx, 3);
		case 16:
			return precpred(_ctx, 2);
		case 17:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean equalityExpression_sempred(EqualityExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 18:
			return precpred(_ctx, 2);
		case 19:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean andExpression_sempred(AndExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 20:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean exclusiveOrExpression_sempred(ExclusiveOrExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 21:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean inclusiveOrExpression_sempred(InclusiveOrExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 22:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean logicalAndExpression_sempred(LogicalAndExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 23:
			return precpred(_ctx, 1);
		}
		return true;
	}
	private boolean logicalOrExpression_sempred(LogicalOrExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 24:
			return precpred(_ctx, 1);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3R\u0229\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\4\64\t"+
		"\64\4\65\t\65\3\2\3\2\3\2\3\2\3\2\3\2\3\2\5\2r\n\2\3\3\3\3\3\3\3\3\3\3"+
		"\3\3\3\3\3\3\3\3\3\3\3\3\5\3\177\n\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3"+
		"\3\3\3\3\3\7\3\u008c\n\3\f\3\16\3\u008f\13\3\3\4\3\4\3\4\3\4\3\4\3\4\7"+
		"\4\u0097\n\4\f\4\16\4\u009a\13\4\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3"+
		"\5\3\5\3\5\3\5\3\5\3\5\5\5\u00ab\n\5\3\6\3\6\3\7\3\7\3\7\3\7\3\7\3\7\5"+
		"\7\u00b5\n\7\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\7\b\u00c3"+
		"\n\b\f\b\16\b\u00c6\13\b\3\t\3\t\3\t\3\t\3\t\3\t\3\t\3\t\3\t\7\t\u00d1"+
		"\n\t\f\t\16\t\u00d4\13\t\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\7\n\u00df"+
		"\n\n\f\n\16\n\u00e2\13\n\3\13\3\13\3\13\3\13\3\13\3\13\3\13\3\13\3\13"+
		"\3\13\3\13\3\13\3\13\3\13\3\13\7\13\u00f3\n\13\f\13\16\13\u00f6\13\13"+
		"\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\7\f\u0101\n\f\f\f\16\f\u0104\13\f"+
		"\3\r\3\r\3\r\3\r\3\r\3\r\7\r\u010c\n\r\f\r\16\r\u010f\13\r\3\16\3\16\3"+
		"\16\3\16\3\16\3\16\7\16\u0117\n\16\f\16\16\16\u011a\13\16\3\17\3\17\3"+
		"\17\3\17\3\17\3\17\7\17\u0122\n\17\f\17\16\17\u0125\13\17\3\20\3\20\3"+
		"\20\3\20\3\20\3\20\7\20\u012d\n\20\f\20\16\20\u0130\13\20\3\21\3\21\3"+
		"\21\3\21\3\21\3\21\7\21\u0138\n\21\f\21\16\21\u013b\13\21\3\22\3\22\3"+
		"\22\3\22\3\22\3\22\5\22\u0143\n\22\3\23\3\23\3\23\3\23\3\23\5\23\u014a"+
		"\n\23\3\24\3\24\3\25\3\25\3\26\3\26\3\27\3\27\5\27\u0154\n\27\3\27\3\27"+
		"\5\27\u0158\n\27\3\27\3\27\5\27\u015c\n\27\5\27\u015e\n\27\3\30\3\30\3"+
		"\30\3\30\3\31\3\31\3\31\3\31\3\31\5\31\u0169\n\31\3\32\3\32\3\33\6\33"+
		"\u016e\n\33\r\33\16\33\u016f\3\34\3\34\3\34\3\34\3\34\3\34\3\34\5\34\u0179"+
		"\n\34\3\35\3\35\3\35\3\35\3\35\3\35\3\36\3\36\3\36\3\36\6\36\u0185\n\36"+
		"\r\36\16\36\u0186\3\36\3\36\3\37\3\37\3\37\5\37\u018e\n\37\3\37\3\37\3"+
		" \3 \3 \3 \3!\3!\3!\3!\5!\u019a\n!\3\"\3\"\3\"\3\"\5\"\u01a0\n\"\3\"\3"+
		"\"\3\"\7\"\u01a5\n\"\f\"\16\"\u01a8\13\"\3\"\3\"\3#\3#\3#\3#\3#\5#\u01b1"+
		"\n#\3$\3$\3$\3%\3%\3%\3%\3%\3%\3%\3%\3%\3%\3%\3%\5%\u01c2\n%\3&\3&\7&"+
		"\u01c6\n&\f&\16&\u01c9\13&\3&\3&\3\'\3\'\3\'\3(\3(\3(\3(\3(\3(\5(\u01d6"+
		"\n(\3)\3)\3)\3*\3*\3*\3+\3+\3+\3+\3+\3+\3+\3+\3,\3,\3,\3-\3-\3-\3-\3-"+
		"\3-\3-\3-\3-\3-\3-\3.\3.\3.\3.\3.\3.\5.\u01fa\n.\3/\3/\3/\3/\3/\3/\6/"+
		"\u0202\n/\r/\16/\u0203\3/\3/\3\60\3\60\5\60\u020a\n\60\3\61\3\61\3\61"+
		"\3\61\3\61\3\61\3\61\3\61\5\61\u0214\n\61\3\62\3\62\3\62\3\62\3\63\3\63"+
		"\3\63\3\63\3\63\3\63\3\64\3\64\3\65\3\65\3\65\6\65\u0225\n\65\r\65\16"+
		"\65\u0226\3\65\2\16\4\6\16\20\22\24\26\30\32\34\36 \66\2\4\6\b\n\f\16"+
		"\20\22\24\26\30\32\34\36 \"$&(*,.\60\62\64\668:<>@BDFHJLNPRTVXZ\\^`bd"+
		"fh\2\6\7\2**,,..\61\61\66\67\3\2<F\b\2\4\b\20\20\22\22\26\27\31\31\34"+
		"\34\3\2KL\2\u023b\2q\3\2\2\2\4s\3\2\2\2\6\u0090\3\2\2\2\b\u00aa\3\2\2"+
		"\2\n\u00ac\3\2\2\2\f\u00b4\3\2\2\2\16\u00b6\3\2\2\2\20\u00c7\3\2\2\2\22"+
		"\u00d5\3\2\2\2\24\u00e3\3\2\2\2\26\u00f7\3\2\2\2\30\u0105\3\2\2\2\32\u0110"+
		"\3\2\2\2\34\u011b\3\2\2\2\36\u0126\3\2\2\2 \u0131\3\2\2\2\"\u013c\3\2"+
		"\2\2$\u0149\3\2\2\2&\u014b\3\2\2\2(\u014d\3\2\2\2*\u014f\3\2\2\2,\u015d"+
		"\3\2\2\2.\u015f\3\2\2\2\60\u0168\3\2\2\2\62\u016a\3\2\2\2\64\u016d\3\2"+
		"\2\2\66\u0178\3\2\2\28\u017a\3\2\2\2:\u0180\3\2\2\2<\u018a\3\2\2\2>\u0191"+
		"\3\2\2\2@\u0199\3\2\2\2B\u019b\3\2\2\2D\u01b0\3\2\2\2F\u01b2\3\2\2\2H"+
		"\u01c1\3\2\2\2J\u01c3\3\2\2\2L\u01cc\3\2\2\2N\u01cf\3\2\2\2P\u01d7\3\2"+
		"\2\2R\u01da\3\2\2\2T\u01dd\3\2\2\2V\u01e5\3\2\2\2X\u01e8\3\2\2\2Z\u01f9"+
		"\3\2\2\2\\\u01fb\3\2\2\2^\u0209\3\2\2\2`\u0213\3\2\2\2b\u0215\3\2\2\2"+
		"d\u0219\3\2\2\2f\u021f\3\2\2\2h\u0224\3\2\2\2jr\7P\2\2kr\7O\2\2lr\5f\64"+
		"\2mn\7\36\2\2no\5(\25\2op\7\37\2\2pr\3\2\2\2qj\3\2\2\2qk\3\2\2\2ql\3\2"+
		"\2\2qm\3\2\2\2r\3\3\2\2\2st\b\3\1\2tu\5\2\2\2u\u008d\3\2\2\2vw\f\b\2\2"+
		"wx\7 \2\2xy\5(\25\2yz\7!\2\2z\u008c\3\2\2\2{|\f\7\2\2|~\7\36\2\2}\177"+
		"\5\6\4\2~}\3\2\2\2~\177\3\2\2\2\177\u0080\3\2\2\2\u0080\u008c\7\37\2\2"+
		"\u0081\u0082\f\6\2\2\u0082\u0083\7J\2\2\u0083\u008c\7P\2\2\u0084\u0085"+
		"\f\5\2\2\u0085\u0086\7I\2\2\u0086\u008c\7P\2\2\u0087\u0088\f\4\2\2\u0088"+
		"\u008c\7+\2\2\u0089\u008a\f\3\2\2\u008a\u008c\7-\2\2\u008bv\3\2\2\2\u008b"+
		"{\3\2\2\2\u008b\u0081\3\2\2\2\u008b\u0084\3\2\2\2\u008b\u0087\3\2\2\2"+
		"\u008b\u0089\3\2\2\2\u008c\u008f\3\2\2\2\u008d\u008b\3\2\2\2\u008d\u008e"+
		"\3\2\2\2\u008e\5\3\2\2\2\u008f\u008d\3\2\2\2\u0090\u0091\b\4\1\2\u0091"+
		"\u0092\5$\23\2\u0092\u0098\3\2\2\2\u0093\u0094\f\3\2\2\u0094\u0095\7;"+
		"\2\2\u0095\u0097\5$\23\2\u0096\u0093\3\2\2\2\u0097\u009a\3\2\2\2\u0098"+
		"\u0096\3\2\2\2\u0098\u0099\3\2\2\2\u0099\7\3\2\2\2\u009a\u0098\3\2\2\2"+
		"\u009b\u00ab\5\4\3\2\u009c\u009d\7+\2\2\u009d\u00ab\5\b\5\2\u009e\u009f"+
		"\7-\2\2\u009f\u00ab\5\b\5\2\u00a0\u00a1\5\n\6\2\u00a1\u00a2\5\f\7\2\u00a2"+
		"\u00ab\3\2\2\2\u00a3\u00a4\7\32\2\2\u00a4\u00ab\5\b\5\2\u00a5\u00a6\7"+
		"\32\2\2\u00a6\u00a7\7\36\2\2\u00a7\u00a8\5,\27\2\u00a8\u00a9\7\37\2\2"+
		"\u00a9\u00ab\3\2\2\2\u00aa\u009b\3\2\2\2\u00aa\u009c\3\2\2\2\u00aa\u009e"+
		"\3\2\2\2\u00aa\u00a0\3\2\2\2\u00aa\u00a3\3\2\2\2\u00aa\u00a5\3\2\2\2\u00ab"+
		"\t\3\2\2\2\u00ac\u00ad\t\2\2\2\u00ad\13\3\2\2\2\u00ae\u00af\7\36\2\2\u00af"+
		"\u00b0\5,\27\2\u00b0\u00b1\7\37\2\2\u00b1\u00b2\5\f\7\2\u00b2\u00b5\3"+
		"\2\2\2\u00b3\u00b5\5\b\5\2\u00b4\u00ae\3\2\2\2\u00b4\u00b3\3\2\2\2\u00b5"+
		"\r\3\2\2\2\u00b6\u00b7\b\b\1\2\u00b7\u00b8\5\f\7\2\u00b8\u00c4\3\2\2\2"+
		"\u00b9\u00ba\f\5\2\2\u00ba\u00bb\7.\2\2\u00bb\u00c3\5\f\7\2\u00bc\u00bd"+
		"\f\4\2\2\u00bd\u00be\7/\2\2\u00be\u00c3\5\f\7\2\u00bf\u00c0\f\3\2\2\u00c0"+
		"\u00c1\7\60\2\2\u00c1\u00c3\5\f\7\2\u00c2\u00b9\3\2\2\2\u00c2\u00bc\3"+
		"\2\2\2\u00c2\u00bf\3\2\2\2\u00c3\u00c6\3\2\2\2\u00c4\u00c2\3\2\2\2\u00c4"+
		"\u00c5\3\2\2\2\u00c5\17\3\2\2\2\u00c6\u00c4\3\2\2\2\u00c7\u00c8\b\t\1"+
		"\2\u00c8\u00c9\5\16\b\2\u00c9\u00d2\3\2\2\2\u00ca\u00cb\f\4\2\2\u00cb"+
		"\u00cc\7*\2\2\u00cc\u00d1\5\16\b\2\u00cd\u00ce\f\3\2\2\u00ce\u00cf\7,"+
		"\2\2\u00cf\u00d1\5\16\b\2\u00d0\u00ca\3\2\2\2\u00d0\u00cd\3\2\2\2\u00d1"+
		"\u00d4\3\2\2\2\u00d2\u00d0\3\2\2\2\u00d2\u00d3\3\2\2\2\u00d3\21\3\2\2"+
		"\2\u00d4\u00d2\3\2\2\2\u00d5\u00d6\b\n\1\2\u00d6\u00d7\5\20\t\2\u00d7"+
		"\u00e0\3\2\2\2\u00d8\u00d9\f\4\2\2\u00d9\u00da\7(\2\2\u00da\u00df\5\20"+
		"\t\2\u00db\u00dc\f\3\2\2\u00dc\u00dd\7)\2\2\u00dd\u00df\5\20\t\2\u00de"+
		"\u00d8\3\2\2\2\u00de\u00db\3\2\2\2\u00df\u00e2\3\2\2\2\u00e0\u00de\3\2"+
		"\2\2\u00e0\u00e1\3\2\2\2\u00e1\23\3\2\2\2\u00e2\u00e0\3\2\2\2\u00e3\u00e4"+
		"\b\13\1\2\u00e4\u00e5\5\22\n\2\u00e5\u00f4\3\2\2\2\u00e6\u00e7\f\6\2\2"+
		"\u00e7\u00e8\7$\2\2\u00e8\u00f3\5\22\n\2\u00e9\u00ea\f\5\2\2\u00ea\u00eb"+
		"\7&\2\2\u00eb\u00f3\5\22\n\2\u00ec\u00ed\f\4\2\2\u00ed\u00ee\7%\2\2\u00ee"+
		"\u00f3\5\22\n\2\u00ef\u00f0\f\3\2\2\u00f0\u00f1\7\'\2\2\u00f1\u00f3\5"+
		"\22\n\2\u00f2\u00e6\3\2\2\2\u00f2\u00e9\3\2\2\2\u00f2\u00ec\3\2\2\2\u00f2"+
		"\u00ef\3\2\2\2\u00f3\u00f6\3\2\2\2\u00f4\u00f2\3\2\2\2\u00f4\u00f5\3\2"+
		"\2\2\u00f5\25\3\2\2\2\u00f6\u00f4\3\2\2\2\u00f7\u00f8\b\f\1\2\u00f8\u00f9"+
		"\5\24\13\2\u00f9\u0102\3\2\2\2\u00fa\u00fb\f\4\2\2\u00fb\u00fc\7G\2\2"+
		"\u00fc\u0101\5\24\13\2\u00fd\u00fe\f\3\2\2\u00fe\u00ff\7H\2\2\u00ff\u0101"+
		"\5\24\13\2\u0100\u00fa\3\2\2\2\u0100\u00fd\3\2\2\2\u0101\u0104\3\2\2\2"+
		"\u0102\u0100\3\2\2\2\u0102\u0103\3\2\2\2\u0103\27\3\2\2\2\u0104\u0102"+
		"\3\2\2\2\u0105\u0106\b\r\1\2\u0106\u0107\5\26\f\2\u0107\u010d\3\2\2\2"+
		"\u0108\u0109\f\3\2\2\u0109\u010a\7\61\2\2\u010a\u010c\5\26\f\2\u010b\u0108"+
		"\3\2\2\2\u010c\u010f\3\2\2\2\u010d\u010b\3\2\2\2\u010d\u010e\3\2\2\2\u010e"+
		"\31\3\2\2\2\u010f\u010d\3\2\2\2\u0110\u0111\b\16\1\2\u0111\u0112\5\30"+
		"\r\2\u0112\u0118\3\2\2\2\u0113\u0114\f\3\2\2\u0114\u0115\7\65\2\2\u0115"+
		"\u0117\5\30\r\2\u0116\u0113\3\2\2\2\u0117\u011a\3\2\2\2\u0118\u0116\3"+
		"\2\2\2\u0118\u0119\3\2\2\2\u0119\33\3\2\2\2\u011a\u0118\3\2\2\2\u011b"+
		"\u011c\b\17\1\2\u011c\u011d\5\32\16\2\u011d\u0123\3\2\2\2\u011e\u011f"+
		"\f\3\2\2\u011f\u0120\7\62\2\2\u0120\u0122\5\32\16\2\u0121\u011e\3\2\2"+
		"\2\u0122\u0125\3\2\2\2\u0123\u0121\3\2\2\2\u0123\u0124\3\2\2\2\u0124\35"+
		"\3\2\2\2\u0125\u0123\3\2\2\2\u0126\u0127\b\20\1\2\u0127\u0128\5\34\17"+
		"\2\u0128\u012e\3\2\2\2\u0129\u012a\f\3\2\2\u012a\u012b\7\63\2\2\u012b"+
		"\u012d\5\34\17\2\u012c\u0129\3\2\2\2\u012d\u0130\3\2\2\2\u012e\u012c\3"+
		"\2\2\2\u012e\u012f\3\2\2\2\u012f\37\3\2\2\2\u0130\u012e\3\2\2\2\u0131"+
		"\u0132\b\21\1\2\u0132\u0133\5\36\20\2\u0133\u0139\3\2\2\2\u0134\u0135"+
		"\f\3\2\2\u0135\u0136\7\64\2\2\u0136\u0138\5\36\20\2\u0137\u0134\3\2\2"+
		"\2\u0138\u013b\3\2\2\2\u0139\u0137\3\2\2\2\u0139\u013a\3\2\2\2\u013a!"+
		"\3\2\2\2\u013b\u0139\3\2\2\2\u013c\u0142\5 \21\2\u013d\u013e\78\2\2\u013e"+
		"\u013f\5(\25\2\u013f\u0140\79\2\2\u0140\u0141\5\"\22\2\u0141\u0143\3\2"+
		"\2\2\u0142\u013d\3\2\2\2\u0142\u0143\3\2\2\2\u0143#\3\2\2\2\u0144\u014a"+
		"\5\"\22\2\u0145\u0146\5\b\5\2\u0146\u0147\5&\24\2\u0147\u0148\5$\23\2"+
		"\u0148\u014a\3\2\2\2\u0149\u0144\3\2\2\2\u0149\u0145\3\2\2\2\u014a%\3"+
		"\2\2\2\u014b\u014c\t\3\2\2\u014c\'\3\2\2\2\u014d\u014e\5$\23\2\u014e)"+
		"\3\2\2\2\u014f\u0150\5\"\22\2\u0150+\3\2\2\2\u0151\u0153\7P\2\2\u0152"+
		"\u0154\5\64\33\2\u0153\u0152\3\2\2\2\u0153\u0154\3\2\2\2\u0154\u015e\3"+
		"\2\2\2\u0155\u0157\5.\30\2\u0156\u0158\5\64\33\2\u0157\u0156\3\2\2\2\u0157"+
		"\u0158\3\2\2\2\u0158\u015e\3\2\2\2\u0159\u015b\5\62\32\2\u015a\u015c\5"+
		"\64\33\2\u015b\u015a\3\2\2\2\u015b\u015c\3\2\2\2\u015c\u015e\3\2\2\2\u015d"+
		"\u0151\3\2\2\2\u015d\u0155\3\2\2\2\u015d\u0159\3\2\2\2\u015e-\3\2\2\2"+
		"\u015f\u0160\7\3\2\2\u0160\u0161\5\60\31\2\u0161\u0162\7&\2\2\u0162/\3"+
		"\2\2\2\u0163\u0169\5,\27\2\u0164\u0165\5,\27\2\u0165\u0166\7;\2\2\u0166"+
		"\u0167\5\60\31\2\u0167\u0169\3\2\2\2\u0168\u0163\3\2\2\2\u0168\u0164\3"+
		"\2\2\2\u0169\61\3\2\2\2\u016a\u016b\t\4\2\2\u016b\63\3\2\2\2\u016c\u016e"+
		"\7.\2\2\u016d\u016c\3\2\2\2\u016e\u016f\3\2\2\2\u016f\u016d\3\2\2\2\u016f"+
		"\u0170\3\2\2\2\u0170\65\3\2\2\2\u0171\u0172\5,\27\2\u0172\u0173\7P\2\2"+
		"\u0173\u0174\7:\2\2\u0174\u0179\3\2\2\2\u0175\u0176\58\35\2\u0176\u0177"+
		"\7:\2\2\u0177\u0179\3\2\2\2\u0178\u0171\3\2\2\2\u0178\u0175\3\2\2\2\u0179"+
		"\67\3\2\2\2\u017a\u017b\5,\27\2\u017b\u017c\7P\2\2\u017c\u017d\7<\2\2"+
		"\u017d\u017e\5(\25\2\u017e\u017f\7:\2\2\u017f9\3\2\2\2\u0180\u0181\7\33"+
		"\2\2\u0181\u0182\7P\2\2\u0182\u0184\7\"\2\2\u0183\u0185\5<\37\2\u0184"+
		"\u0183\3\2\2\2\u0185\u0186\3\2\2\2\u0186\u0184\3\2\2\2\u0186\u0187\3\2"+
		"\2\2\u0187\u0188\3\2\2\2\u0188\u0189\7#\2\2\u0189;\3\2\2\2\u018a\u018b"+
		"\5,\27\2\u018b\u018d\7P\2\2\u018c\u018e\5> \2\u018d\u018c\3\2\2\2\u018d"+
		"\u018e\3\2\2\2\u018e\u018f\3\2\2\2\u018f\u0190\7:\2\2\u0190=\3\2\2\2\u0191"+
		"\u0192\7 \2\2\u0192\u0193\7K\2\2\u0193\u0194\7!\2\2\u0194?\3\2\2\2\u0195"+
		"\u0196\7\23\2\2\u0196\u019a\5\66\34\2\u0197\u0198\7\23\2\2\u0198\u019a"+
		"\58\35\2\u0199\u0195\3\2\2\2\u0199\u0197\3\2\2\2\u019aA\3\2\2\2\u019b"+
		"\u019c\5,\27\2\u019c\u019d\7P\2\2\u019d\u019f\7\36\2\2\u019e\u01a0\5D"+
		"#\2\u019f\u019e\3\2\2\2\u019f\u01a0\3\2\2\2\u01a0\u01a1\3\2\2\2\u01a1"+
		"\u01a2\7\37\2\2\u01a2\u01a6\7\"\2\2\u01a3\u01a5\5H%\2\u01a4\u01a3\3\2"+
		"\2\2\u01a5\u01a8\3\2\2\2\u01a6\u01a4\3\2\2\2\u01a6\u01a7\3\2\2\2\u01a7"+
		"\u01a9\3\2\2\2\u01a8\u01a6\3\2\2\2\u01a9\u01aa\7#\2\2\u01aaC\3\2\2\2\u01ab"+
		"\u01b1\5F$\2\u01ac\u01ad\5F$\2\u01ad\u01ae\7;\2\2\u01ae\u01af\5D#\2\u01af"+
		"\u01b1\3\2\2\2\u01b0\u01ab\3\2\2\2\u01b0\u01ac\3\2\2\2\u01b1E\3\2\2\2"+
		"\u01b2\u01b3\5,\27\2\u01b3\u01b4\7P\2\2\u01b4G\3\2\2\2\u01b5\u01c2\5J"+
		"&\2\u01b6\u01c2\5L\'\2\u01b7\u01c2\5N(\2\u01b8\u01c2\5P)\2\u01b9\u01c2"+
		"\5T+\2\u01ba\u01c2\5V,\2\u01bb\u01c2\5X-\2\u01bc\u01c2\5Z.\2\u01bd\u01c2"+
		"\5\\/\2\u01be\u01c2\5\66\34\2\u01bf\u01c2\58\35\2\u01c0\u01c2\5d\63\2"+
		"\u01c1\u01b5\3\2\2\2\u01c1\u01b6\3\2\2\2\u01c1\u01b7\3\2\2\2\u01c1\u01b8"+
		"\3\2\2\2\u01c1\u01b9\3\2\2\2\u01c1\u01ba\3\2\2\2\u01c1\u01bb\3\2\2\2\u01c1"+
		"\u01bc\3\2\2\2\u01c1\u01bd\3\2\2\2\u01c1\u01be\3\2\2\2\u01c1\u01bf\3\2"+
		"\2\2\u01c1\u01c0\3\2\2\2\u01c2I\3\2\2\2\u01c3\u01c7\7\"\2\2\u01c4\u01c6"+
		"\5H%\2\u01c5\u01c4\3\2\2\2\u01c6\u01c9\3\2\2\2\u01c7\u01c5\3\2\2\2\u01c7"+
		"\u01c8\3\2\2\2\u01c8\u01ca\3\2\2\2\u01c9\u01c7\3\2\2\2\u01ca\u01cb\7#"+
		"\2\2\u01cbK\3\2\2\2\u01cc\u01cd\7\13\2\2\u01cd\u01ce\7:\2\2\u01ceM\3\2"+
		"\2\2\u01cf\u01d0\7\25\2\2\u01d0\u01d1\7\36\2\2\u01d1\u01d2\5(\25\2\u01d2"+
		"\u01d3\7\37\2\2\u01d3\u01d5\5H%\2\u01d4\u01d6\5R*\2\u01d5\u01d4\3\2\2"+
		"\2\u01d5\u01d6\3\2\2\2\u01d6O\3\2\2\2\u01d7\u01d8\7\r\2\2\u01d8\u01d9"+
		"\7:\2\2\u01d9Q\3\2\2\2\u01da\u01db\7\21\2\2\u01db\u01dc\5H%\2\u01dcS\3"+
		"\2\2\2\u01dd\u01de\7\17\2\2\u01de\u01df\5H%\2\u01df\u01e0\7\35\2\2\u01e0"+
		"\u01e1\7\36\2\2\u01e1\u01e2\5(\25\2\u01e2\u01e3\7\37\2\2\u01e3\u01e4\7"+
		":\2\2\u01e4U\3\2\2\2\u01e5\u01e6\5(\25\2\u01e6\u01e7\7:\2\2\u01e7W\3\2"+
		"\2\2\u01e8\u01e9\7\24\2\2\u01e9\u01ea\7\36\2\2\u01ea\u01eb\5(\25\2\u01eb"+
		"\u01ec\7:\2\2\u01ec\u01ed\5(\25\2\u01ed\u01ee\7:\2\2\u01ee\u01ef\5(\25"+
		"\2\u01ef\u01f0\7:\2\2\u01f0\u01f1\7\37\2\2\u01f1\u01f2\5H%\2\u01f2Y\3"+
		"\2\2\2\u01f3\u01f4\7\30\2\2\u01f4\u01fa\7:\2\2\u01f5\u01f6\7\30\2\2\u01f6"+
		"\u01f7\5(\25\2\u01f7\u01f8\7:\2\2\u01f8\u01fa\3\2\2\2\u01f9\u01f3\3\2"+
		"\2\2\u01f9\u01f5\3\2\2\2\u01fa[\3\2\2\2\u01fb\u01fc\7\t\2\2\u01fc\u01fd"+
		"\7\36\2\2\u01fd\u01fe\5(\25\2\u01fe\u01ff\7\37\2\2\u01ff\u0201\7\"\2\2"+
		"\u0200\u0202\5^\60\2\u0201\u0200\3\2\2\2\u0202\u0203\3\2\2\2\u0203\u0201"+
		"\3\2\2\2\u0203\u0204\3\2\2\2\u0204\u0205\3\2\2\2\u0205\u0206\7#\2\2\u0206"+
		"]\3\2\2\2\u0207\u020a\5`\61\2\u0208\u020a\5b\62\2\u0209\u0207\3\2\2\2"+
		"\u0209\u0208\3\2\2\2\u020a_\3\2\2\2\u020b\u020c\7\f\2\2\u020c\u020d\7"+
		"K\2\2\u020d\u020e\79\2\2\u020e\u0214\5H%\2\u020f\u0210\7\f\2\2\u0210\u0211"+
		"\7O\2\2\u0211\u0212\79\2\2\u0212\u0214\5H%\2\u0213\u020b\3\2\2\2\u0213"+
		"\u020f\3\2\2\2\u0214a\3\2\2\2\u0215\u0216\7\16\2\2\u0216\u0217\79\2\2"+
		"\u0217\u0218\5H%\2\u0218c\3\2\2\2\u0219\u021a\7\35\2\2\u021a\u021b\7\36"+
		"\2\2\u021b\u021c\5(\25\2\u021c\u021d\7\37\2\2\u021d\u021e\5H%\2\u021e"+
		"e\3\2\2\2\u021f\u0220\t\5\2\2\u0220g\3\2\2\2\u0221\u0225\5:\36\2\u0222"+
		"\u0225\5@!\2\u0223\u0225\5B\"\2\u0224\u0221\3\2\2\2\u0224\u0222\3\2\2"+
		"\2\u0224\u0223\3\2\2\2\u0225\u0226\3\2\2\2\u0226\u0224\3\2\2\2\u0226\u0227"+
		"\3\2\2\2\u0227i\3\2\2\2\60q~\u008b\u008d\u0098\u00aa\u00b4\u00c2\u00c4"+
		"\u00d0\u00d2\u00de\u00e0\u00f2\u00f4\u0100\u0102\u010d\u0118\u0123\u012e"+
		"\u0139\u0142\u0149\u0153\u0157\u015b\u015d\u0168\u016f\u0178\u0186\u018d"+
		"\u0199\u019f\u01a6\u01b0\u01c1\u01c7\u01d5\u01f9\u0203\u0209\u0213\u0224"+
		"\u0226";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}