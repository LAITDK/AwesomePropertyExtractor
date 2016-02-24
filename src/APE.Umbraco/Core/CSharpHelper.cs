using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core
{
	/// <summary>
	/// The purpose of the code is to generate valid identifiers from Umbraco node type aliases.
	/// All code in this file is extracted from the source code of the packages Microsoft.CodeAnalysis and Microsoft.CodeAnalysis.CSharp.
	/// </summary>
	internal class CSharpHelper
	{
		// Extracted from Microsoft.CodeAnalysis.UnicodeCharacterUtilities / CodeAnalysis/UnicodeCharacterUtilities.cs.
		public static bool IsIdentifierStartCharacter(char ch)
		{
			// identifier-start-character:
			//   letter-character
			//   _ (the underscore character U+005F)

			if (ch < 'a') // '\u0061'
			{
				if (ch < 'A') // '\u0041'
				{
					return false;
				}

				return ch <= 'Z'  // '\u005A'
					|| ch == '_'; // '\u005F'
			}

			if (ch <= 'z') // '\u007A'
			{
				return true;
			}

			if (ch <= '\u007F') // max ASCII
			{
				return false;
			}

			return IsLetterChar(CharUnicodeInfo.GetUnicodeCategory(ch));
		}

		// Extracted from Microsoft.CodeAnalysis.UnicodeCharacterUtilities / CodeAnalysis/UnicodeCharacterUtilities.cs.
		public static bool IsIdentifierPartCharacter(char ch)
		{
			// identifier-part-character:
			//   letter-character
			//   decimal-digit-character
			//   connecting-character
			//   combining-character
			//   formatting-character

			if (ch < 'a') // '\u0061'
			{
				if (ch < 'A') // '\u0041'
				{
					return ch >= '0'  // '\u0030'
						&& ch <= '9'; // '\u0039'
				}

				return ch <= 'Z'  // '\u005A'
					|| ch == '_'; // '\u005F'
			}

			if (ch <= 'z') // '\u007A'
			{
				return true;
			}

			if (ch <= '\u007F') // max ASCII
			{
				return false;
			}

			UnicodeCategory cat = CharUnicodeInfo.GetUnicodeCategory(ch);
			return IsLetterChar(cat)
				|| IsDecimalDigitChar(cat)
				|| IsConnectingChar(cat)
				|| IsCombiningChar(cat)
				|| IsFormattingChar(cat);
		}

		// Extracted from Microsoft.CodeAnalysis.UnicodeCharacterUtilities / CodeAnalysis/UnicodeCharacterUtilities.cs.
		private static bool IsLetterChar(UnicodeCategory cat)
		{
			// letter-character:
			//   A Unicode character of classes Lu, Ll, Lt, Lm, Lo, or Nl 
			//   A Unicode-escape-sequence representing a character of classes Lu, Ll, Lt, Lm, Lo, or Nl

			switch (cat)
			{
				case UnicodeCategory.UppercaseLetter:
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.ModifierLetter:
				case UnicodeCategory.OtherLetter:
				case UnicodeCategory.LetterNumber:
					return true;
			}

			return false;
		}

		// Extracted from Microsoft.CodeAnalysis.UnicodeCharacterUtilities / CodeAnalysis/UnicodeCharacterUtilities.cs.
		private static bool IsDecimalDigitChar(UnicodeCategory cat)
		{
			// decimal-digit-character:
			//   A Unicode character of the class Nd 
			//   A unicode-escape-sequence representing a character of the class Nd

			return cat == UnicodeCategory.DecimalDigitNumber;           // 8
		}

		// Extracted from Microsoft.CodeAnalysis.UnicodeCharacterUtilities / CodeAnalysis/UnicodeCharacterUtilities.cs.
		private static bool IsConnectingChar(UnicodeCategory cat)
		{
			// connecting-character:  
			//   A Unicode character of the class Pc
			//   A unicode-escape-sequence representing a character of the class Pc

			return cat == UnicodeCategory.ConnectorPunctuation;     //18
		}

		// Extracted from Microsoft.CodeAnalysis.UnicodeCharacterUtilities / CodeAnalysis/UnicodeCharacterUtilities.cs.
		private static bool IsCombiningChar(UnicodeCategory cat)
		{
			// combining-character:
			//   A Unicode character of classes Mn or Mc 
			//   A Unicode-escape-sequence representing a character of classes Mn or Mc

			switch (cat)
			{
				case UnicodeCategory.NonSpacingMark:                //5
				case UnicodeCategory.SpacingCombiningMark:          //6
					return true;
			}

			return false;
		}

		// Extracted from Microsoft.CodeAnalysis.UnicodeCharacterUtilities / CodeAnalysis/UnicodeCharacterUtilities.cs.
		/// <summary>
		/// Returns true if the Unicode character is a formatting character (Unicode class Cf).
		/// </summary>
		/// <param name="cat">The Unicode character.</param>
		private static bool IsFormattingChar(UnicodeCategory cat)
		{
			// formatting-character:  
			//   A Unicode character of the class Cf
			//   A unicode-escape-sequence representing a character of the class Cf

			return cat == UnicodeCategory.Format;           //15
		}

		// From the namespace Microsoft.CodeAnalysis.CSharp.SyntaxFacts.
		// Extracted from Microsoft.CodeAnalysis.CSharp.SyntaxFacts / CSharpCodeAnalysis/Syntax/SyntaxKindFacts.cs / GetKeywordKind(...) and modified
		// to tell whether or not a string is a C# keyword.
		public static bool IsKeyword(string text)
		{
			switch (text)
			{
				case "bool":
				case "byte":
				case "sbyte":
				case "short":
				case "ushort":
				case "int":
				case "uint":
				case "long":
				case "ulong":
				case "double":
				case "float":
				case "decimal":
				case "string":
				case "char":
				case "void":
				case "object":
				case "typeof":
				case "sizeof":
				case "null":
				case "true":
				case "false":
				case "if":
				case "else":
				case "while":
				case "for":
				case "foreach":
				case "do":
				case "switch":
				case "case":
				case "default":
				case "lock":
				case "try":
				case "throw":
				case "catch":
				case "finally":
				case "goto":
				case "break":
				case "continue":
				case "return":
				case "public":
				case "private":
				case "internal":
				case "protected":
				case "static":
				case "readonly":
				case "sealed":
				case "const":
				case "fixed":
				case "stackalloc":
				case "volatile":
				case "new":
				case "override":
				case "abstract":
				case "virtual":
				case "event":
				case "extern":
				case "ref":
				case "out":
				case "in":
				case "is":
				case "as":
				case "params":
				case "__arglist":
				case "__makeref":
				case "__reftype":
				case "__refvalue":
				case "this":
				case "base":
				case "namespace":
				case "using":
				case "class":
				case "struct":
				case "interface":
				case "enum":
				case "delegate":
				case "checked":
				case "unchecked":
				case "unsafe":
				case "operator":
				case "implicit":
				case "explicit":
					return true;
				default:
					return false;
			}
		}
	}
}