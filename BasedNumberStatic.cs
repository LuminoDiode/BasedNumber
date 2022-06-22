using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LuminoDiodeBasedNumber
{
	public partial class BasedNumberStatic
	{
		#region Consts
		public const int MinBase = 2;
		public const int MaxBase = 'Z' - 'A' + 1 + 10;
		public const int MaxFractionalDigits = 40; // Precision problems?
		#endregion

		#region Is-checks
		/// <summary>
		/// Returns true if char exists in any supported base, otherwise false.
		/// </summary>
		public static bool CharIsValid(char Chr) => (Chr >= '0' && Chr <= '9') || (Chr >= 'A' && Chr <= 'Z');

		/// <summary>
		/// Returns true if value exists as char in any supported base, otherwise false.
		/// </summary>
		public static bool CharValueIsValid(int Val) => Val >= 0 && Val <= (MaxBase - 1);

		/// <summary>
		/// Returns true if the passed Value-string exists in any supported base, otherwise false
		/// </summary>
		public static bool StringIsValid(string ValueString)
		{
			if (string.IsNullOrEmpty(ValueString)) return false;
			if (ValueString[0] == '-' && ValueString.Length == 1) return false;

			bool DotOrComaSeen = false;
			for (int i = ValueString[0] == '-' ? 1 : 0; i < ValueString.Length; i++)
			{
				if (ValueString[i] == '.' || ValueString[i] == ',')
				{
					if (DotOrComaSeen) return false;
					else DotOrComaSeen = true;
					continue;
				}
				if (!CharIsValid(ValueString[i])) return false;
			}

			return true;
		}

		/// <summary>
		/// Returns true if the passed base is supported, otherwise false.
		/// </summary>
		public static bool BaseIsValid(int Base) => Base >= MinBase && Base <= MaxBase;

		/// <summary>
		/// Returns true if the passed char exists in the passed Base, otherwise false.
		/// </summary>
		public static bool CharBaseIsValid(char Chr, int Base)
		{
			if (Base <= 10)
			{
				if (!(Chr >= '0' && Chr <= ('0' + (Base - 1))))
					return false;
			}
			else
			{
				if (!((Chr >= '0' && Chr <= '9') || (Chr >= 'A' && Chr <= 'A' + (Base - 10 - 1))))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Returns true if the passed Value-string exists in the passed Base, otherwise false.
		/// </summary>
		public static bool StringBaseIsValid(string Str, int Base)
		{
			if (string.IsNullOrEmpty(Str)) return false;

			for (int i = Str.StartsWith("-") ? 1 : 0; i < Str.Length; i++)
				if (!(Str[i] == ',' || Str[i] == '.'))
					if (!CharBaseIsValid(Str[i], Base))
						return false;

			return true;
		}
		#endregion

		#region GetCharForDecimal & GetDecimalForChar
		/// <summary>
		/// Returns char representing the passed decimal value.
		/// </summary>
		public static char GetCharForDecimalValue(int Value)
		{
			if (!CharValueIsValid(Value))
				throw new ArgumentException($"Values in range [0;36] only can be converted to chars. Passed value is {Value}.");

			if (Value < 10)
				return (char)(Value + '0');
			else
				return (char)('A' + (Value - 10));
		}

		/// <summary>
		/// Returns decimal value representing the passed char.
		/// </summary>
		public static int GetDecimalValueForChar(char Symbol)
		{
			if (!CharIsValid(Symbol))
				throw new ArgumentException($"Chars in range [0;9] & [A;Z] only can be converted to decimals. Passed char is {Symbol}.");

			if (char.IsDigit(Symbol))
				return Symbol - '0';
			else
				return 10 + (Symbol - 'A');
		}
		#endregion

		// Strings are being processed without minus char, it is being inserted back at the end of the function.
		#region From any base to decimal base

		/// <summary>
		/// Converts Value-string with passed base to decimal value and returns it as double.
		/// </summary>
		public static double ToDecimal(string Value, int CurrentBase)
		{
			bool neg = Value.StartsWith("-");
			if (neg) Value = Value.Substring(1);

			if (!StringIsValid(Value))
				throw new FormatException("Invalid format of input Value string");
			if (!StringBaseIsValid(Value, CurrentBase))
				throw new FormatException("Passed string does not currespond with passed base");

			var ValueSplitted = Value.Split('.');

			return (neg ? -1 : 1) * (ValueSplitted.Length == 1 ?
				IntPartToDecimal(ValueSplitted[0], CurrentBase) :
				IntPartToDecimal(ValueSplitted[0], CurrentBase) + FractionalPartToDecimal(ValueSplitted[1], CurrentBase));
		}
		private static double IntPartToDecimal(string IntPartOfValue, int CurrentBase)
		{
			double OutValue = 0;

			for (int i = 0; i < IntPartOfValue.Length; i++)
				OutValue += GetDecimalValueForChar(IntPartOfValue[i]) * Math.Pow(CurrentBase, (IntPartOfValue.Length - 1 - i));

			return OutValue;
		}
		private static double FractionalPartToDecimal(string FractionalPartOfValue, int CurrentBase)
		{
			double OutValue = 0;

			for (int i = 0; i < FractionalPartOfValue.Length; i++)
				OutValue += GetDecimalValueForChar(FractionalPartOfValue[i]) * Math.Pow(CurrentBase, -i - 1);

			return OutValue;
		}
		#endregion

		#region From decimal base to any base
		private static int DigitsInIntegerPart(double DecimalValue)
		{
			int Count = 0;
			for (; DecimalValue >= 1; Count++)
			{
				DecimalValue /= 10;
			}
			return Count;
		}
		private static int DigitsInFractionalPart(double DecimalValue)
		{
			int Count = 0;
			for (; ((DecimalValue % 1) * Math.Pow(10, MaxFractionalDigits)) > 1 && Count < MaxFractionalDigits; Count++)
			{
				DecimalValue *= 10;
			}
			return Count;
		}

		/// <summary>
		/// Converts decimal value to value string with passed base.
		/// </summary>
		public static string FromDecimalToNewBase(double DecimalValue, int NewBase)
		{
			var neg = DecimalValue < 0;
			if (neg) DecimalValue *= -1;

			if (NewBase == 10) return (neg ? "-" : String.Empty) + DecimalValue.ToString();
			if (!(MinBase <= NewBase && NewBase <= MaxBase))
				throw new ArgumentException("Invalid base");


			string OutValue = string.Empty;

			var IntDigits = DigitsInIntegerPart(DecimalValue);
			var FractDigits = DigitsInFractionalPart(DecimalValue);

			var IntPart = (int)DecimalValue;
			for (int i = 0; IntPart > 0; i++)
			{
				OutValue += GetCharForDecimalValue(IntPart % NewBase);
				IntPart /= NewBase;
			}
			OutValue = new string(OutValue.Reverse().ToArray());
			OutValue += '.';
			var FractPart = (DecimalValue % 1) * NewBase;
			int CalcTimes = MaxFractionalDigits - NewBase;
			for (int i = 0; i < CalcTimes; i++)
			{
				OutValue += GetCharForDecimalValue((int)(FractPart));
				FractPart = (FractPart % 1 * NewBase);
			}

			Console.Write(String.Empty);
			return (neg ? "-" : String.Empty) + OutValue;
		}

		/// <summary>
		/// Converts value string from one base to another.
		/// </summary>
		public static string ToNewBase(string Value, int CurrentBase,int NewBase)
		{
			return FromDecimalToNewBase(ToDecimal(Value, CurrentBase), NewBase);
		}
		#endregion
	}
}
