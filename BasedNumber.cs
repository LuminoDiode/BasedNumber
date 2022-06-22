using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuminoDiodeBasedNumber
{
	// Класс предоставляющий удобную обёртку для работы с BasedNumberStatic
	public partial struct BasedNumber:IEquatable<BasedNumber>, IComparable<BasedNumber>
	{
		public double DecimalValue { get; private set; }

		public BasedNumber(double DecimalValue)
		{
			this.DecimalValue = DecimalValue;
		}
		public BasedNumber(string ValueString, int Base)
		{
			this.DecimalValue = BasedNumberStatic.ToDecimal(ValueString, Base);
		}

		#region ToString
		[Obsolete("Use ToString instead this method")]
		public string GetValueString(int Base=10) => BasedNumberStatic.FromDecimalToNewBase(this.DecimalValue, Base);
		[Obsolete("Use ToHexString instead this method")]
		public string GetHexString() => this.GetValueString(16);
		[Obsolete("Use ToDecString instead this method")]
		public string GetDecString() => this.GetValueString(10);
		[Obsolete("Use ToOctString instead this method")]
		public string GetOctString() => this.GetValueString(8);
		[Obsolete("Use ToBinString instead this method")]
		public string GetBinString() => this.GetValueString(2);

		public string ToString(int Base) => BasedNumberStatic.FromDecimalToNewBase(this.DecimalValue, Base);
		public string ToHexString() => this.ToString(16);
		public string ToDecString() => this.ToString(10);
		public string ToOctString() => this.ToString(8);
		public string ToBinString() => this.ToString(2);
		public override string ToString() => this.ToDecString();
		#endregion

		#region explicit/implicit operators
		public static explicit operator double(BasedNumber BasedNum) => BasedNum.DecimalValue;
		public static implicit operator BasedNumber(double DoubleVal) => new BasedNumber(DoubleVal);
		#endregion

		#region Math operators
		public static BasedNumber operator +(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue + Num2.DecimalValue);
		public static BasedNumber operator -(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue - Num2.DecimalValue);
		public static BasedNumber operator *(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue * Num2.DecimalValue);
		public static BasedNumber operator /(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue / Num2.DecimalValue);
		#endregion

		#region bool operators
		public static bool operator <(BasedNumber Num1, BasedNumber Num2)=> Num1.DecimalValue < Num2.DecimalValue;
		public static bool operator >(BasedNumber Num1, BasedNumber Num2) => Num1.DecimalValue > Num2.DecimalValue;
		public static bool operator ==(BasedNumber Num1, BasedNumber Num2) => Num1.DecimalValue == Num2.DecimalValue;
		public static bool operator !=(BasedNumber Num1, BasedNumber Num2) => Num1.DecimalValue != Num2.DecimalValue;
		#endregion

		public override int GetHashCode() => this.DecimalValue.GetHashCode();
		public bool Equals(BasedNumber Bn) =>(this.DecimalValue.Equals(Bn.DecimalValue));
		public override bool Equals(object obj) => obj is BasedNumber ? this.Equals((BasedNumber)obj) : false;

		public int CompareTo(BasedNumber Bn)
		{
			if (this.DecimalValue > Bn.DecimalValue) return 1;
			else if (this.DecimalValue < Bn.DecimalValue) return -1;
			else return 0;
		}
	}
}
