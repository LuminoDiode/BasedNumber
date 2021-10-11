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
		public const int MinBase = 2;
		public const int MaxBase = 'Z' - 'A' + 1 + 10;
		public const int DecimalBase = 10;
		public const int MaxFractionalDigits = 46; // Проблемы с точностью

		public static char GetCharForDecimalValue(int Value)
		{
			if (!ValueIsValid(Value))
				throw new ArgumentException($"Values in range [0;36] only can be converted to chars. Passed value is {Value}.");

			if (Value < 10)
				return (char)(Value + '0');
			else
				return (char)('A' + (Value - 10));
		}
		public static int GetDecimalValueForChar(char Symbol)
		{
			if (!CharIsValid(Symbol))
				throw new ArgumentException($"Chars in range [0;9] & [A;Z] only can be converted to decimals. Passed value is {Symbol}.");

			if (char.IsDigit(Symbol))
				return Symbol - '0';
			else
				return 10 + (Symbol - 'A');
		}

		/// <summary>
		/// Проверка возможности записи переданного символа как десятичного числа
		/// </summary>
		public static bool CharIsValid(char Chr) => Char.IsDigit(Chr) || (Chr >= 'A' && Chr <= 'Z');
		/// <summary>
		/// Проверка возможности записи переданного десятичного числа как символа
		/// </summary>
		public static bool ValueIsValid(int Val) => Val >= 0 && Val <= ('Z' - 'A' + 1 + 10);
		/// <summary>
		/// Проверка корректности строки-числа без проверки соответствия его основанию системы счисления.
		/// </summary>
		public static bool StringIsValid(string Str)
		{
			if (string.IsNullOrEmpty(Str)) return false;

			var ExcludeValidChars = Str.Where(c => !CharIsValid(c));
			if (!((ExcludeValidChars.Count() == 1 && (ExcludeValidChars.First() == '.' || ExcludeValidChars.First() == ','))
				|| ExcludeValidChars.Count() == 0)) return false;

			return true;
		}
		/// <summary>
		/// Проверка допустимости числового значения основания. 2 <= Base <= 36
		/// </summary>
		public static bool BaseIsValid(int Base) => Base >= MinBase && Base <= MaxBase;
		/// <summary>
		/// Проверка отсутствия в строке символов, не входящих в систему счисления с переданным основанием.
		/// </summary>
		public static bool StringBaseIsValid(string Str, int Base) =>
			Base <= 10 ?
				Str.All(c => c >= '0' && c <= ('0' + (Base - 1)) || c=='.' || c == ',') :
				Str.All(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'A' + (Base - 10 - 1)) || c == '.' || c == ',');

		#region From any base to decimal base
		public static double ToDecimal(string Value, int CurrentBase)
		{
			if (!StringIsValid(Value))
				throw new FormatException("Invalid format of input Value string");
			if(!StringBaseIsValid(Value,CurrentBase))
				throw new FormatException("Passed string does not currespond with passed base");

			var ValueSplitted = Value.Split(",.".ToCharArray());

			return ValueSplitted.Length == 1 ?
				IntPartToDecimal(ValueSplitted[0], CurrentBase) :
				IntPartToDecimal(ValueSplitted[0], CurrentBase) + FractionalPartToDecimal(ValueSplitted[1], CurrentBase);
		}
		public static double IntPartToDecimal(string IntPartOfValue, int CurrentBase)
		{
			double OutValue = 0;

			for (int i = 0; i < IntPartOfValue.Length; i++)
			{
				OutValue += GetDecimalValueForChar(IntPartOfValue[i]) * Math.Pow(CurrentBase, (IntPartOfValue.Length - 1 - i));
			}

			return OutValue;
		}
		public static double FractionalPartToDecimal(string FractionalPartOfValue, int CurrentBase)
		{
			double OutValue = 0;

			for (int i = 0; i < FractionalPartOfValue.Length; i++)
			{
				OutValue += GetDecimalValueForChar(FractionalPartOfValue[i]) * Math.Pow(CurrentBase, -i - 1);
			}

			return OutValue;
		}
		#endregion

		#region From decimal base to any base
		public static int DigitsInIntegerPart(double DecimalValue)
		{
			int Count = 0;
			for (; DecimalValue >= 1; Count++)
			{
				DecimalValue /= 10;
			}
			return Count;
		}
		public static int DigitsInFractionalPart(double DecimalValue)
		{
			int Count = 0;
			for (; ((DecimalValue % 1) * Math.Pow(10, MaxFractionalDigits)) > 1 && Count<MaxFractionalDigits; Count++)
			{
				DecimalValue *= 10;
			}
			return Count;
		}

		public static string FromDecimalToNewBase(double DecimalValue, int NewBase)
		{
			if (NewBase == 10) return DecimalValue.ToString();
			if (!(MinBase <= NewBase && NewBase <= MaxBase))
				throw new ArgumentException("Invalid base");

			string OutValue=string.Empty;

			var IntDigits = DigitsInIntegerPart(DecimalValue);
			var FractDigits = DigitsInFractionalPart(DecimalValue);

			var IntPart = (int)DecimalValue;
			for(int i = 0; IntPart>0; i++)
			{
				OutValue += GetCharForDecimalValue(IntPart % NewBase);
				IntPart /= NewBase;
			}
			OutValue=new string(OutValue.Reverse().ToArray());
			OutValue += '.';
			var FractPart = (DecimalValue % 1)*NewBase;
			int CalcTimes = MaxFractionalDigits - NewBase;
			for(int i = 0; i< CalcTimes; i++)
			{
				OutValue += GetCharForDecimalValue((int)(FractPart));
				FractPart = (FractPart%1 * NewBase);
			}

			return OutValue;
		}

		#endregion

	}
}
