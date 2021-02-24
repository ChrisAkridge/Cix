// Generated from d:\Documents\GitHub\Cix\src\Celarix.Cix\Celarix.Cix\Parse\ANTLR\Cix.g4 by ANTLR 4.8
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class CixLexer extends Lexer {
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
		FloatingPoint=74, Digit=75, StringLiteral=76, Identifier=77, IdentifierFirstChar=78, 
		IdentifierChar=79, HexDigit=80;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "Whitespace", 
			"Break", "Case", "Continue", "Default", "Do", "Double", "Else", "Float", 
			"Global", "For", "If", "Int", "Long", "Return", "Short", "Sizeof", "Struct", 
			"Void", "While", "LeftParen", "RightParen", "LeftBracket", "RightBracket", 
			"OpenScope", "CloseScope", "LessThan", "LessThanOrEqualTo", "GreaterThan", 
			"GreaterThanOrEqualTo", "ShiftLeft", "ShiftRight", "Plus", "Increment", 
			"Minus", "Decrement", "Asterisk", "Divide", "Modulus", "Ampersand", "BitwiseOr", 
			"LogicalAnd", "LogicalOr", "BitwiseXor", "LogicalNot", "BitwiseNot", 
			"Question", "Colon", "Semicolon", "Comma", "Assign", "MultiplyAssign", 
			"DivideAssign", "ModulusAssign", "AddAssign", "SubtractAssign", "ShiftLeftAssign", 
			"ShiftRightAssign", "BitwiseAndAssign", "BitwiseXorAssign", "BitwiseOrAssign", 
			"Equals", "NotEquals", "PointerMemberAccess", "DirectMemberAccess", "Integer", 
			"FloatingPoint", "IntegerSuffix", "FloatingSuffix", "DecimalInteger", 
			"HexadecimalInteger", "DecimalFloatingPoint", "ExponentFloatingPoint", 
			"Exponent", "ExponentLetter", "ExponentSign", "Digit", "StringLiteral", 
			"Identifier", "IdentifierFirstChar", "IdentifierChar", "HexDigit"
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
			"FloatingPoint", "Digit", "StringLiteral", "Identifier", "IdentifierFirstChar", 
			"IdentifierChar", "HexDigit"
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


	public CixLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "Cix.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2R\u0222\b\1\4\2\t"+
		"\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\4\64\t"+
		"\64\4\65\t\65\4\66\t\66\4\67\t\67\48\t8\49\t9\4:\t:\4;\t;\4<\t<\4=\t="+
		"\4>\t>\4?\t?\4@\t@\4A\tA\4B\tB\4C\tC\4D\tD\4E\tE\4F\tF\4G\tG\4H\tH\4I"+
		"\tI\4J\tJ\4K\tK\4L\tL\4M\tM\4N\tN\4O\tO\4P\tP\4Q\tQ\4R\tR\4S\tS\4T\tT"+
		"\4U\tU\4V\tV\4W\tW\4X\tX\4Y\tY\4Z\tZ\3\2\3\2\3\2\3\2\3\2\3\2\3\2\3\2\3"+
		"\2\3\2\3\3\3\3\3\3\3\3\3\3\3\4\3\4\3\4\3\4\3\4\3\4\3\5\3\5\3\5\3\5\3\5"+
		"\3\5\3\5\3\6\3\6\3\6\3\6\3\6\3\7\3\7\3\7\3\7\3\7\3\7\3\b\3\b\3\b\3\b\3"+
		"\b\3\b\3\b\3\t\6\t\u00e5\n\t\r\t\16\t\u00e6\3\t\3\t\3\n\3\n\3\n\3\n\3"+
		"\n\3\n\3\13\3\13\3\13\3\13\3\13\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3"+
		"\r\3\r\3\r\3\r\3\r\3\r\3\r\3\r\3\16\3\16\3\16\3\17\3\17\3\17\3\17\3\17"+
		"\3\17\3\17\3\20\3\20\3\20\3\20\3\20\3\21\3\21\3\21\3\21\3\21\3\21\3\22"+
		"\3\22\3\22\3\22\3\22\3\22\3\22\3\23\3\23\3\23\3\23\3\24\3\24\3\24\3\25"+
		"\3\25\3\25\3\25\3\26\3\26\3\26\3\26\3\26\3\27\3\27\3\27\3\27\3\27\3\27"+
		"\3\27\3\30\3\30\3\30\3\30\3\30\3\30\3\31\3\31\3\31\3\31\3\31\3\31\3\31"+
		"\3\32\3\32\3\32\3\32\3\32\3\32\3\32\3\33\3\33\3\33\3\33\3\33\3\34\3\34"+
		"\3\34\3\34\3\34\3\34\3\35\3\35\3\36\3\36\3\37\3\37\3 \3 \3!\3!\3\"\3\""+
		"\3#\3#\3$\3$\3$\3%\3%\3&\3&\3&\3\'\3\'\3\'\3(\3(\3(\3)\3)\3*\3*\3*\3+"+
		"\3+\3,\3,\3,\3-\3-\3.\3.\3/\3/\3\60\3\60\3\61\3\61\3\62\3\62\3\62\3\63"+
		"\3\63\3\63\3\64\3\64\3\65\3\65\3\66\3\66\3\67\3\67\38\38\39\39\3:\3:\3"+
		";\3;\3<\3<\3<\3=\3=\3=\3>\3>\3>\3?\3?\3?\3@\3@\3@\3A\3A\3A\3A\3B\3B\3"+
		"B\3B\3C\3C\3C\3D\3D\3D\3E\3E\3E\3F\3F\3F\3G\3G\3G\3H\3H\3H\3I\3I\3J\3"+
		"J\5J\u01cc\nJ\3J\3J\5J\u01d0\nJ\5J\u01d2\nJ\3K\3K\5K\u01d6\nK\3K\3K\5"+
		"K\u01da\nK\5K\u01dc\nK\3L\3L\3L\3L\3L\5L\u01e3\nL\3M\3M\3N\6N\u01e8\n"+
		"N\rN\16N\u01e9\3O\3O\3O\3O\6O\u01f0\nO\rO\16O\u01f1\3P\6P\u01f5\nP\rP"+
		"\16P\u01f6\3P\3P\7P\u01fb\nP\fP\16P\u01fe\13P\3Q\3Q\3Q\3R\3R\3R\3R\3S"+
		"\3S\3T\3T\3U\3U\3V\3V\7V\u020f\nV\fV\16V\u0212\13V\3V\3V\3W\3W\7W\u0218"+
		"\nW\fW\16W\u021b\13W\3X\3X\3Y\3Y\3Z\3Z\2\2[\3\3\5\4\7\5\t\6\13\7\r\b\17"+
		"\t\21\n\23\13\25\f\27\r\31\16\33\17\35\20\37\21!\22#\23%\24\'\25)\26+"+
		"\27-\30/\31\61\32\63\33\65\34\67\359\36;\37= ?!A\"C#E$G%I&K\'M(O)Q*S+"+
		"U,W-Y.[/]\60_\61a\62c\63e\64g\65i\66k\67m8o9q:s;u<w=y>{?}@\177A\u0081"+
		"B\u0083C\u0085D\u0087E\u0089F\u008bG\u008dH\u008fI\u0091J\u0093K\u0095"+
		"L\u0097\2\u0099\2\u009b\2\u009d\2\u009f\2\u00a1\2\u00a3\2\u00a5\2\u00a7"+
		"\2\u00a9M\u00abN\u00adO\u00afP\u00b1Q\u00b3R\3\2\f\5\2\13\f\17\17\"\""+
		"\6\2NNWWnnww\6\2FFHHffhh\4\2GGgg\4\2--//\3\2\62;\5\2\f\f\17\17$$\4\2C"+
		"\\c|\6\2\62;C\\aac|\5\2\62;CHch\2\u0227\2\3\3\2\2\2\2\5\3\2\2\2\2\7\3"+
		"\2\2\2\2\t\3\2\2\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2\2\21\3\2\2\2"+
		"\2\23\3\2\2\2\2\25\3\2\2\2\2\27\3\2\2\2\2\31\3\2\2\2\2\33\3\2\2\2\2\35"+
		"\3\2\2\2\2\37\3\2\2\2\2!\3\2\2\2\2#\3\2\2\2\2%\3\2\2\2\2\'\3\2\2\2\2)"+
		"\3\2\2\2\2+\3\2\2\2\2-\3\2\2\2\2/\3\2\2\2\2\61\3\2\2\2\2\63\3\2\2\2\2"+
		"\65\3\2\2\2\2\67\3\2\2\2\29\3\2\2\2\2;\3\2\2\2\2=\3\2\2\2\2?\3\2\2\2\2"+
		"A\3\2\2\2\2C\3\2\2\2\2E\3\2\2\2\2G\3\2\2\2\2I\3\2\2\2\2K\3\2\2\2\2M\3"+
		"\2\2\2\2O\3\2\2\2\2Q\3\2\2\2\2S\3\2\2\2\2U\3\2\2\2\2W\3\2\2\2\2Y\3\2\2"+
		"\2\2[\3\2\2\2\2]\3\2\2\2\2_\3\2\2\2\2a\3\2\2\2\2c\3\2\2\2\2e\3\2\2\2\2"+
		"g\3\2\2\2\2i\3\2\2\2\2k\3\2\2\2\2m\3\2\2\2\2o\3\2\2\2\2q\3\2\2\2\2s\3"+
		"\2\2\2\2u\3\2\2\2\2w\3\2\2\2\2y\3\2\2\2\2{\3\2\2\2\2}\3\2\2\2\2\177\3"+
		"\2\2\2\2\u0081\3\2\2\2\2\u0083\3\2\2\2\2\u0085\3\2\2\2\2\u0087\3\2\2\2"+
		"\2\u0089\3\2\2\2\2\u008b\3\2\2\2\2\u008d\3\2\2\2\2\u008f\3\2\2\2\2\u0091"+
		"\3\2\2\2\2\u0093\3\2\2\2\2\u0095\3\2\2\2\2\u00a9\3\2\2\2\2\u00ab\3\2\2"+
		"\2\2\u00ad\3\2\2\2\2\u00af\3\2\2\2\2\u00b1\3\2\2\2\2\u00b3\3\2\2\2\3\u00b5"+
		"\3\2\2\2\5\u00bf\3\2\2\2\7\u00c4\3\2\2\2\t\u00ca\3\2\2\2\13\u00d1\3\2"+
		"\2\2\r\u00d6\3\2\2\2\17\u00dc\3\2\2\2\21\u00e4\3\2\2\2\23\u00ea\3\2\2"+
		"\2\25\u00f0\3\2\2\2\27\u00f5\3\2\2\2\31\u00fe\3\2\2\2\33\u0106\3\2\2\2"+
		"\35\u0109\3\2\2\2\37\u0110\3\2\2\2!\u0115\3\2\2\2#\u011b\3\2\2\2%\u0122"+
		"\3\2\2\2\'\u0126\3\2\2\2)\u0129\3\2\2\2+\u012d\3\2\2\2-\u0132\3\2\2\2"+
		"/\u0139\3\2\2\2\61\u013f\3\2\2\2\63\u0146\3\2\2\2\65\u014d\3\2\2\2\67"+
		"\u0152\3\2\2\29\u0158\3\2\2\2;\u015a\3\2\2\2=\u015c\3\2\2\2?\u015e\3\2"+
		"\2\2A\u0160\3\2\2\2C\u0162\3\2\2\2E\u0164\3\2\2\2G\u0166\3\2\2\2I\u0169"+
		"\3\2\2\2K\u016b\3\2\2\2M\u016e\3\2\2\2O\u0171\3\2\2\2Q\u0174\3\2\2\2S"+
		"\u0176\3\2\2\2U\u0179\3\2\2\2W\u017b\3\2\2\2Y\u017e\3\2\2\2[\u0180\3\2"+
		"\2\2]\u0182\3\2\2\2_\u0184\3\2\2\2a\u0186\3\2\2\2c\u0188\3\2\2\2e\u018b"+
		"\3\2\2\2g\u018e\3\2\2\2i\u0190\3\2\2\2k\u0192\3\2\2\2m\u0194\3\2\2\2o"+
		"\u0196\3\2\2\2q\u0198\3\2\2\2s\u019a\3\2\2\2u\u019c\3\2\2\2w\u019e\3\2"+
		"\2\2y\u01a1\3\2\2\2{\u01a4\3\2\2\2}\u01a7\3\2\2\2\177\u01aa\3\2\2\2\u0081"+
		"\u01ad\3\2\2\2\u0083\u01b1\3\2\2\2\u0085\u01b5\3\2\2\2\u0087\u01b8\3\2"+
		"\2\2\u0089\u01bb\3\2\2\2\u008b\u01be\3\2\2\2\u008d\u01c1\3\2\2\2\u008f"+
		"\u01c4\3\2\2\2\u0091\u01c7\3\2\2\2\u0093\u01d1\3\2\2\2\u0095\u01db\3\2"+
		"\2\2\u0097\u01e2\3\2\2\2\u0099\u01e4\3\2\2\2\u009b\u01e7\3\2\2\2\u009d"+
		"\u01eb\3\2\2\2\u009f\u01f4\3\2\2\2\u00a1\u01ff\3\2\2\2\u00a3\u0202\3\2"+
		"\2\2\u00a5\u0206\3\2\2\2\u00a7\u0208\3\2\2\2\u00a9\u020a\3\2\2\2\u00ab"+
		"\u020c\3\2\2\2\u00ad\u0215\3\2\2\2\u00af\u021c\3\2\2\2\u00b1\u021e\3\2"+
		"\2\2\u00b3\u0220\3\2\2\2\u00b5\u00b6\7B\2\2\u00b6\u00b7\7h\2\2\u00b7\u00b8"+
		"\7w\2\2\u00b8\u00b9\7p\2\2\u00b9\u00ba\7e\2\2\u00ba\u00bb\7r\2\2\u00bb"+
		"\u00bc\7v\2\2\u00bc\u00bd\7t\2\2\u00bd\u00be\7>\2\2\u00be\4\3\2\2\2\u00bf"+
		"\u00c0\7d\2\2\u00c0\u00c1\7{\2\2\u00c1\u00c2\7v\2\2\u00c2\u00c3\7g\2\2"+
		"\u00c3\6\3\2\2\2\u00c4\u00c5\7u\2\2\u00c5\u00c6\7d\2\2\u00c6\u00c7\7{"+
		"\2\2\u00c7\u00c8\7v\2\2\u00c8\u00c9\7g\2\2\u00c9\b\3\2\2\2\u00ca\u00cb"+
		"\7w\2\2\u00cb\u00cc\7u\2\2\u00cc\u00cd\7j\2\2\u00cd\u00ce\7q\2\2\u00ce"+
		"\u00cf\7t\2\2\u00cf\u00d0\7v\2\2\u00d0\n\3\2\2\2\u00d1\u00d2\7w\2\2\u00d2"+
		"\u00d3\7k\2\2\u00d3\u00d4\7p\2\2\u00d4\u00d5\7v\2\2\u00d5\f\3\2\2\2\u00d6"+
		"\u00d7\7w\2\2\u00d7\u00d8\7n\2\2\u00d8\u00d9\7q\2\2\u00d9\u00da\7p\2\2"+
		"\u00da\u00db\7i\2\2\u00db\16\3\2\2\2\u00dc\u00dd\7u\2\2\u00dd\u00de\7"+
		"y\2\2\u00de\u00df\7k\2\2\u00df\u00e0\7v\2\2\u00e0\u00e1\7e\2\2\u00e1\u00e2"+
		"\7j\2\2\u00e2\20\3\2\2\2\u00e3\u00e5\t\2\2\2\u00e4\u00e3\3\2\2\2\u00e5"+
		"\u00e6\3\2\2\2\u00e6\u00e4\3\2\2\2\u00e6\u00e7\3\2\2\2\u00e7\u00e8\3\2"+
		"\2\2\u00e8\u00e9\b\t\2\2\u00e9\22\3\2\2\2\u00ea\u00eb\7d\2\2\u00eb\u00ec"+
		"\7t\2\2\u00ec\u00ed\7g\2\2\u00ed\u00ee\7c\2\2\u00ee\u00ef\7m\2\2\u00ef"+
		"\24\3\2\2\2\u00f0\u00f1\7e\2\2\u00f1\u00f2\7c\2\2\u00f2\u00f3\7u\2\2\u00f3"+
		"\u00f4\7g\2\2\u00f4\26\3\2\2\2\u00f5\u00f6\7e\2\2\u00f6\u00f7\7q\2\2\u00f7"+
		"\u00f8\7p\2\2\u00f8\u00f9\7v\2\2\u00f9\u00fa\7k\2\2\u00fa\u00fb\7p\2\2"+
		"\u00fb\u00fc\7w\2\2\u00fc\u00fd\7g\2\2\u00fd\30\3\2\2\2\u00fe\u00ff\7"+
		"f\2\2\u00ff\u0100\7g\2\2\u0100\u0101\7h\2\2\u0101\u0102\7c\2\2\u0102\u0103"+
		"\7w\2\2\u0103\u0104\7n\2\2\u0104\u0105\7v\2\2\u0105\32\3\2\2\2\u0106\u0107"+
		"\7f\2\2\u0107\u0108\7q\2\2\u0108\34\3\2\2\2\u0109\u010a\7f\2\2\u010a\u010b"+
		"\7q\2\2\u010b\u010c\7w\2\2\u010c\u010d\7d\2\2\u010d\u010e\7n\2\2\u010e"+
		"\u010f\7g\2\2\u010f\36\3\2\2\2\u0110\u0111\7g\2\2\u0111\u0112\7n\2\2\u0112"+
		"\u0113\7u\2\2\u0113\u0114\7g\2\2\u0114 \3\2\2\2\u0115\u0116\7h\2\2\u0116"+
		"\u0117\7n\2\2\u0117\u0118\7q\2\2\u0118\u0119\7c\2\2\u0119\u011a\7v\2\2"+
		"\u011a\"\3\2\2\2\u011b\u011c\7i\2\2\u011c\u011d\7n\2\2\u011d\u011e\7q"+
		"\2\2\u011e\u011f\7d\2\2\u011f\u0120\7c\2\2\u0120\u0121\7n\2\2\u0121$\3"+
		"\2\2\2\u0122\u0123\7h\2\2\u0123\u0124\7q\2\2\u0124\u0125\7t\2\2\u0125"+
		"&\3\2\2\2\u0126\u0127\7k\2\2\u0127\u0128\7h\2\2\u0128(\3\2\2\2\u0129\u012a"+
		"\7k\2\2\u012a\u012b\7p\2\2\u012b\u012c\7v\2\2\u012c*\3\2\2\2\u012d\u012e"+
		"\7n\2\2\u012e\u012f\7q\2\2\u012f\u0130\7p\2\2\u0130\u0131\7i\2\2\u0131"+
		",\3\2\2\2\u0132\u0133\7t\2\2\u0133\u0134\7g\2\2\u0134\u0135\7v\2\2\u0135"+
		"\u0136\7w\2\2\u0136\u0137\7t\2\2\u0137\u0138\7p\2\2\u0138.\3\2\2\2\u0139"+
		"\u013a\7u\2\2\u013a\u013b\7j\2\2\u013b\u013c\7q\2\2\u013c\u013d\7t\2\2"+
		"\u013d\u013e\7v\2\2\u013e\60\3\2\2\2\u013f\u0140\7u\2\2\u0140\u0141\7"+
		"k\2\2\u0141\u0142\7|\2\2\u0142\u0143\7g\2\2\u0143\u0144\7q\2\2\u0144\u0145"+
		"\7h\2\2\u0145\62\3\2\2\2\u0146\u0147\7u\2\2\u0147\u0148\7v\2\2\u0148\u0149"+
		"\7t\2\2\u0149\u014a\7w\2\2\u014a\u014b\7e\2\2\u014b\u014c\7v\2\2\u014c"+
		"\64\3\2\2\2\u014d\u014e\7x\2\2\u014e\u014f\7q\2\2\u014f\u0150\7k\2\2\u0150"+
		"\u0151\7f\2\2\u0151\66\3\2\2\2\u0152\u0153\7y\2\2\u0153\u0154\7j\2\2\u0154"+
		"\u0155\7k\2\2\u0155\u0156\7n\2\2\u0156\u0157\7g\2\2\u01578\3\2\2\2\u0158"+
		"\u0159\7*\2\2\u0159:\3\2\2\2\u015a\u015b\7+\2\2\u015b<\3\2\2\2\u015c\u015d"+
		"\7]\2\2\u015d>\3\2\2\2\u015e\u015f\7_\2\2\u015f@\3\2\2\2\u0160\u0161\7"+
		"}\2\2\u0161B\3\2\2\2\u0162\u0163\7\177\2\2\u0163D\3\2\2\2\u0164\u0165"+
		"\7>\2\2\u0165F\3\2\2\2\u0166\u0167\7>\2\2\u0167\u0168\7?\2\2\u0168H\3"+
		"\2\2\2\u0169\u016a\7@\2\2\u016aJ\3\2\2\2\u016b\u016c\7@\2\2\u016c\u016d"+
		"\7?\2\2\u016dL\3\2\2\2\u016e\u016f\7>\2\2\u016f\u0170\7>\2\2\u0170N\3"+
		"\2\2\2\u0171\u0172\7@\2\2\u0172\u0173\7@\2\2\u0173P\3\2\2\2\u0174\u0175"+
		"\7-\2\2\u0175R\3\2\2\2\u0176\u0177\7-\2\2\u0177\u0178\7-\2\2\u0178T\3"+
		"\2\2\2\u0179\u017a\7/\2\2\u017aV\3\2\2\2\u017b\u017c\7/\2\2\u017c\u017d"+
		"\7/\2\2\u017dX\3\2\2\2\u017e\u017f\7,\2\2\u017fZ\3\2\2\2\u0180\u0181\7"+
		"\61\2\2\u0181\\\3\2\2\2\u0182\u0183\7\'\2\2\u0183^\3\2\2\2\u0184\u0185"+
		"\7(\2\2\u0185`\3\2\2\2\u0186\u0187\7~\2\2\u0187b\3\2\2\2\u0188\u0189\7"+
		"(\2\2\u0189\u018a\7(\2\2\u018ad\3\2\2\2\u018b\u018c\7~\2\2\u018c\u018d"+
		"\7~\2\2\u018df\3\2\2\2\u018e\u018f\7`\2\2\u018fh\3\2\2\2\u0190\u0191\7"+
		"#\2\2\u0191j\3\2\2\2\u0192\u0193\7\u0080\2\2\u0193l\3\2\2\2\u0194\u0195"+
		"\7A\2\2\u0195n\3\2\2\2\u0196\u0197\7<\2\2\u0197p\3\2\2\2\u0198\u0199\7"+
		"=\2\2\u0199r\3\2\2\2\u019a\u019b\7.\2\2\u019bt\3\2\2\2\u019c\u019d\7?"+
		"\2\2\u019dv\3\2\2\2\u019e\u019f\7,\2\2\u019f\u01a0\7?\2\2\u01a0x\3\2\2"+
		"\2\u01a1\u01a2\7\61\2\2\u01a2\u01a3\7?\2\2\u01a3z\3\2\2\2\u01a4\u01a5"+
		"\7\'\2\2\u01a5\u01a6\7?\2\2\u01a6|\3\2\2\2\u01a7\u01a8\7-\2\2\u01a8\u01a9"+
		"\7?\2\2\u01a9~\3\2\2\2\u01aa\u01ab\7/\2\2\u01ab\u01ac\7?\2\2\u01ac\u0080"+
		"\3\2\2\2\u01ad\u01ae\7>\2\2\u01ae\u01af\7>\2\2\u01af\u01b0\7?\2\2\u01b0"+
		"\u0082\3\2\2\2\u01b1\u01b2\7@\2\2\u01b2\u01b3\7@\2\2\u01b3\u01b4\7?\2"+
		"\2\u01b4\u0084\3\2\2\2\u01b5\u01b6\7(\2\2\u01b6\u01b7\7?\2\2\u01b7\u0086"+
		"\3\2\2\2\u01b8\u01b9\7`\2\2\u01b9\u01ba\7?\2\2\u01ba\u0088\3\2\2\2\u01bb"+
		"\u01bc\7~\2\2\u01bc\u01bd\7?\2\2\u01bd\u008a\3\2\2\2\u01be\u01bf\7?\2"+
		"\2\u01bf\u01c0\7?\2\2\u01c0\u008c\3\2\2\2\u01c1\u01c2\7#\2\2\u01c2\u01c3"+
		"\7?\2\2\u01c3\u008e\3\2\2\2\u01c4\u01c5\7/\2\2\u01c5\u01c6\7@\2\2\u01c6"+
		"\u0090\3\2\2\2\u01c7\u01c8\7\60\2\2\u01c8\u0092\3\2\2\2\u01c9\u01cb\5"+
		"\u009bN\2\u01ca\u01cc\5\u0097L\2\u01cb\u01ca\3\2\2\2\u01cb\u01cc\3\2\2"+
		"\2\u01cc\u01d2\3\2\2\2\u01cd\u01cf\5\u009dO\2\u01ce\u01d0\5\u0097L\2\u01cf"+
		"\u01ce\3\2\2\2\u01cf\u01d0\3\2\2\2\u01d0\u01d2\3\2\2\2\u01d1\u01c9\3\2"+
		"\2\2\u01d1\u01cd\3\2\2\2\u01d2\u0094\3\2\2\2\u01d3\u01d5\5\u009fP\2\u01d4"+
		"\u01d6\5\u0099M\2\u01d5\u01d4\3\2\2\2\u01d5\u01d6\3\2\2\2\u01d6\u01dc"+
		"\3\2\2\2\u01d7\u01d9\5\u00a1Q\2\u01d8\u01da\5\u0099M\2\u01d9\u01d8\3\2"+
		"\2\2\u01d9\u01da\3\2\2\2\u01da\u01dc\3\2\2\2\u01db\u01d3\3\2\2\2\u01db"+
		"\u01d7\3\2\2\2\u01dc\u0096\3\2\2\2\u01dd\u01e3\t\3\2\2\u01de\u01df\7w"+
		"\2\2\u01df\u01e3\7n\2\2\u01e0\u01e1\7W\2\2\u01e1\u01e3\7N\2\2\u01e2\u01dd"+
		"\3\2\2\2\u01e2\u01de\3\2\2\2\u01e2\u01e0\3\2\2\2\u01e3\u0098\3\2\2\2\u01e4"+
		"\u01e5\t\4\2\2\u01e5\u009a\3\2\2\2\u01e6\u01e8\5\u00a9U\2\u01e7\u01e6"+
		"\3\2\2\2\u01e8\u01e9\3\2\2\2\u01e9\u01e7\3\2\2\2\u01e9\u01ea\3\2\2\2\u01ea"+
		"\u009c\3\2\2\2\u01eb\u01ec\7\62\2\2\u01ec\u01ed\7z\2\2\u01ed\u01ef\3\2"+
		"\2\2\u01ee\u01f0\5\u00b3Z\2\u01ef\u01ee\3\2\2\2\u01f0\u01f1\3\2\2\2\u01f1"+
		"\u01ef\3\2\2\2\u01f1\u01f2\3\2\2\2\u01f2\u009e\3\2\2\2\u01f3\u01f5\5\u009b"+
		"N\2\u01f4\u01f3\3\2\2\2\u01f5\u01f6\3\2\2\2\u01f6\u01f4\3\2\2\2\u01f6"+
		"\u01f7\3\2\2\2\u01f7\u01f8\3\2\2\2\u01f8\u01fc\7\60\2\2\u01f9\u01fb\5"+
		"\u009bN\2\u01fa\u01f9\3\2\2\2\u01fb\u01fe\3\2\2\2\u01fc\u01fa\3\2\2\2"+
		"\u01fc\u01fd\3\2\2\2\u01fd\u00a0\3\2\2\2\u01fe\u01fc\3\2\2\2\u01ff\u0200"+
		"\5\u009fP\2\u0200\u0201\5\u00a3R\2\u0201\u00a2\3\2\2\2\u0202\u0203\5\u00a5"+
		"S\2\u0203\u0204\5\u00a7T\2\u0204\u0205\5\u009bN\2\u0205\u00a4\3\2\2\2"+
		"\u0206\u0207\t\5\2\2\u0207\u00a6\3\2\2\2\u0208\u0209\t\6\2\2\u0209\u00a8"+
		"\3\2\2\2\u020a\u020b\t\7\2\2\u020b\u00aa\3\2\2\2\u020c\u0210\7$\2\2\u020d"+
		"\u020f\n\b\2\2\u020e\u020d\3\2\2\2\u020f\u0212\3\2\2\2\u0210\u020e\3\2"+
		"\2\2\u0210\u0211\3\2\2\2\u0211\u0213\3\2\2\2\u0212\u0210\3\2\2\2\u0213"+
		"\u0214\7$\2\2\u0214\u00ac\3\2\2\2\u0215\u0219\5\u00afX\2\u0216\u0218\5"+
		"\u00b1Y\2\u0217\u0216\3\2\2\2\u0218\u021b\3\2\2\2\u0219\u0217\3\2\2\2"+
		"\u0219\u021a\3\2\2\2\u021a\u00ae\3\2\2\2\u021b\u0219\3\2\2\2\u021c\u021d"+
		"\t\t\2\2\u021d\u00b0\3\2\2\2\u021e\u021f\t\n\2\2\u021f\u00b2\3\2\2\2\u0220"+
		"\u0221\t\13\2\2\u0221\u00b4\3\2\2\2\21\2\u00e6\u01cb\u01cf\u01d1\u01d5"+
		"\u01d9\u01db\u01e2\u01e9\u01f1\u01f6\u01fc\u0210\u0219\3\b\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}