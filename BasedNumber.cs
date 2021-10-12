using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuminoDiodeBasedNumber
{
	// Класс предоставляющий удобную обёртку для работы с BasedNumberStatic
	public partial class BasedNumber
	{
		public double DecimalValue { get; set; }
		//public int CurrentBase { get; private set; }

		/*
		public void SetNewBase(int NewBase)
		{
			if (!BasedNumberStatic.BaseIsValid(NewBase))
				throw new ArgumentException($"Invalid base. MinBase is {BasedNumberStatic.MinBase}, MaxBase is {BasedNumberStatic.MaxBase}.");
			else
				this.CurrentBase = 10;
		}
		*/

		public BasedNumber(double DecimalValue)
		{
			this.DecimalValue = DecimalValue;
			//this.CurrentBase = 10;
		}
		public BasedNumber(string ValueString, int Base)
		{
			this.DecimalValue = BasedNumberStatic.ToDecimal(ValueString, Base);
			//this.CurrentBase = Base;
		}

		#region 
		public string GetValueString(int Base=10) => BasedNumberStatic.FromDecimalToNewBase(this.DecimalValue, Base);
		public string GetHexString() => this.GetValueString(16);
		public string GetDecString() => this.GetValueString(10);
		public string GetOctString() => this.GetValueString(8);
		public string GetBinString() => this.GetValueString(2);
		#endregion

		#region explicit operators
		public static explicit operator double(BasedNumber bn) => bn.DecimalValue;
		#endregion 
		
		#region Math operators
		public static BasedNumber operator +(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue + Num2.DecimalValue);
		public static BasedNumber operator -(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue - Num2.DecimalValue);
		public static BasedNumber operator *(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue * Num2.DecimalValue);
		public static BasedNumber operator /(BasedNumber Num1, BasedNumber Num2) => new BasedNumber(Num1.DecimalValue / Num2.DecimalValue);
		#endregion
	}
}
