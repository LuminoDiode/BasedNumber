using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuminoDiodeBasedNumber
{
	// Type T is closest to the all C# numeric types
	public partial class BasedNumber<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
	{
		public double DecimalValue { get; set; }
		public int CurrentBase { get; private set; }

		
		public void SetNewBase(int NewBase)
		{
			if (!BasedNumberStatic.BaseIsValid(NewBase))
				throw new ArgumentException($"Invalid base. MinBase is {BasedNumberStatic.MinBase}, MaxBase is {BasedNumberStatic.MaxBase}.");
			else
				this.CurrentBase = NewBase;
		}

		public BasedNumber(double DecimalValue)
		{
			this.DecimalValue = DecimalValue;
			this.CurrentBase = BasedNumberStatic.DecimalBase;
		}
		public BasedNumber(T DecimalValue)
		{
			this.DecimalValue = (double)(DecimalValue as dynamic);
			this.CurrentBase = BasedNumberStatic.DecimalBase;
		}
		public BasedNumber(string ValueString, int Base)
		{
			this.DecimalValue = BasedNumberStatic.ToDecimal(ValueString, Base);
			this.CurrentBase = Base;
		}

		public string ValueString(int Base) => BasedNumberStatic.FromDecimalToNewBase(this.DecimalValue, Base);
		public string HexString => this.ValueString(16);
		public string DecString => this.ValueString(10);
		public string OctString => this.ValueString(8);
		public string BinString => this.ValueString(2);
		public string AnyBaseString(int Base) =>
			BasedNumberStatic.FromDecimalToNewBase(this.DecimalValue, Base);

		public static implicit operator BasedNumber<T>(T Val) => new BasedNumber<T>(Val);
		public static explicit operator double(BasedNumber<T> bn) => bn.DecimalValue;
		public static BasedNumber<T> operator +(BasedNumber<T> Num1, BasedNumber<T> Num2) => new BasedNumber<T>(Num1.DecimalValue + Num2.DecimalValue);
		public static BasedNumber<T> operator -(BasedNumber<T> Num1, BasedNumber<T> Num2) => new BasedNumber<T>(Num1.DecimalValue - Num2.DecimalValue);
		public static BasedNumber<T> operator *(BasedNumber<T> Num1, BasedNumber<T> Num2) => new BasedNumber<T>(Num1.DecimalValue * Num2.DecimalValue);
		public static BasedNumber<T> operator /(BasedNumber<T> Num1, BasedNumber<T> Num2) => new BasedNumber<T>(Num1.DecimalValue / Num2.DecimalValue);

	}
}
