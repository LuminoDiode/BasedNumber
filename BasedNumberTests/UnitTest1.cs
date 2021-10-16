using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using LuminoDiodeRandomDataGenerators;
using static LuminoDiodeBasedNumber.BasedNumberStatic;

namespace LuminoDiodeBasedNumber
{
	public class Tests
	{
		public class BasedNumberStaticTests
		{
			private static readonly char[] AllChars =
				Enumerable.Range(char.MinValue, char.MinValue - char.MinValue).Select(x => (char)x).ToArray();

			private static readonly char[] AllValidChars =
				string.Concat("0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',')).ToCharArray();

			private static readonly int[] AllValidCharValues =
				Enumerable.Range(0, AllValidChars.Length).ToArray();

			private static readonly int[] AllSBytes =
				Enumerable.Range(-128, byte.MaxValue).ToArray();

			private static readonly char[] AllInvalidCharValues =
				AllSBytes.Except(AllValidCharValues).Select(x => (char)x).ToArray();

			#region SelfTest
			[Fact]
			public static void SelfTest_AllValidCharsLength()
			{
				Assert.Equal(10 + 26, AllValidChars.Length);
			}
			#endregion

			#region CharIsValid
			[Fact]
			public static void CharIsValidTest1()
			{
				Assert.True(AllValidChars.All(x => BasedNumberStatic.CharIsValid(x)));
			}
			[Fact]
			public static void CharIsValidTest2()
			{
				var AllChars = new List<Char>();
				for (int i = char.MinValue; i <= char.MaxValue; i++) AllChars.Add((char)i);
				var AllInvalidChars = AllChars.Except(AllValidChars);

				Assert.True(AllInvalidChars.All(x => !BasedNumberStatic.CharIsValid(x)));
			}
			#endregion

			#region CharValueIsValid
			[Fact]
			public static void CharValueIsValidTest1()
			{
				Assert.True(Enumerable.Range(0, AllValidChars.Length).Select(x => (Char)x).All(x => BasedNumberStatic.CharValueIsValid(x)));
			}

			[Fact]
			public static void CharValueIsValidTest2()
			{
				Assert.True(AllInvalidCharValues.All(x => !BasedNumberStatic.CharValueIsValid(x)));
			}
			#endregion

			#region StringIsValid
			[Fact]
			public static void StringIsValidTest()
			{
				const int NumOfIters = 1000;
				for (int i = 0; i < NumOfIters; i++)
				{
					var testStr = RandomDataGenerator.String(AllValidChars);

					Assert.True(BasedNumberStatic.StringIsValid(testStr));

					for (int r = 0; r < testStr.Length; r++)
					{
						var testStrWithDot = testStr.Insert(r, ".");

						Assert.True(BasedNumberStatic.StringIsValid(testStrWithDot));
					}
				}
			}
			#endregion

			#region BaseIsValid
			[Fact]
			public static void BaseIsValidTest()
			{
				for (int b = sbyte.MinValue; b <= sbyte.MaxValue; b++)
				{
					if (b >= 2 && b <= 36) Assert.True(BasedNumberStatic.BaseIsValid(b));
					else Assert.False(BasedNumberStatic.BaseIsValid(b));
				}
			}
			#endregion

			#region CharBaseIsValid
			[Fact]
			public static void CharBaseIsValidTest()
			{
				for (int i = 2; i <= 36; i++)
				{
					Assert.True(BasedNumberStatic.CharBaseIsValid(AllValidChars[i - 2], i));
					Assert.True(BasedNumberStatic.CharBaseIsValid(AllValidChars[i - 1], i));
				}
			}
			#endregion

			#region StringBaseIsValid
			[Fact]
			public static void StringBaseIsValidTest()
			{
				const int NumOfIters = 1000;
				for (int i = 0; i < NumOfIters; i++)
				{
					double CurrDouble = RandomDataGenerator.DoubleValue(short.MinValue, short.MaxValue);

					var currStr = CurrDouble.ToString();

					Assert.True(BasedNumberStatic.StringBaseIsValid(currStr, 10));

					foreach (int Base in new int[] { 2, 8, 16 })
					{
						var CurrStr2 = (Convert.ToString((int)CurrDouble, Base) + '.' + new string(Convert.ToString((int)CurrDouble, Base).Reverse().ToArray())).ToUpper();

						Assert.True(BasedNumberStatic.StringBaseIsValid(CurrStr2, Base));
					}
				}
			}
			#endregion

			#region GetCharForDecimalValue
			[Fact]
			public static void GetCharForDecimalValueTest()
			{
				for (int i = 0; i < 10; i++)
				{
					Assert.Equal(i.ToString().ToCharArray().First(), BasedNumberStatic.GetCharForDecimalValue(i));
				}
				for (char i = 'A'; i <= 'Z'; i++)
				{
					Assert.Equal(i, BasedNumberStatic.GetCharForDecimalValue(i - 'A' + 10));
				}
			}
			#endregion

			#region GetDecimalValueForChar
			[Fact]
			public static void GetDecimalValueForCharTest()
			{
				for (int i = 0; i < 10; i++)
				{
					Assert.Equal(i, BasedNumberStatic.GetDecimalValueForChar(i.ToString().First()));
				}
				for (char i = 'A'; i <= 'Z'; i++)
				{
					Assert.Equal(i - 'A' + 10, BasedNumberStatic.GetDecimalValueForChar(i));
				}
			}
			#endregion

			#region ToDecimal
			[Fact]
			public static void ToDecimal()
			{
				string TestBinVal, TestOctVal, TestDecVal, TestHexVal;

				const int NumOfIters = 1000;
				for (int i = 0; i < NumOfIters; i++)
				{
					TestBinVal = RandomDataGenerator.String("01".ToCharArray());
					Assert.Equal(Convert.ToInt32(TestBinVal, 2), (int)BasedNumberStatic.ToDecimal(TestBinVal, 2));

					TestOctVal = RandomDataGenerator.String("01234567".ToCharArray());
					Assert.Equal(Convert.ToInt32(TestOctVal, 8), (int)BasedNumberStatic.ToDecimal(TestOctVal, 8));

					TestDecVal = RandomDataGenerator.String("0123456789".ToCharArray());
					Assert.Equal(Convert.ToInt32(TestDecVal, 10), (int)BasedNumberStatic.ToDecimal(TestDecVal, 10));

					TestHexVal = RandomDataGenerator.String("0123456789ABCDEF".ToCharArray(), 3, 6);
					Assert.Equal(Convert.ToInt32(TestHexVal, 16), (int)BasedNumberStatic.ToDecimal(TestHexVal, 16));
				}
			}
			#endregion

			[Fact]
			public static void MultipleBaseChangeTest()
			{
				const int NumOfIters = 1000;
				for (int r = 0; r < NumOfIters; r++)
				{
					Random rnd = new Random();
					int CurrentBase = 10;

					double d = RandomDataGenerator.DoubleValue(short.MinValue, short.MaxValue);

					String val = d.ToString();

					for (int i = 0; i < byte.MaxValue; i++)
					{
						int NewBase = rnd.Next(2, 37);
						val = ToNewBase(val, CurrentBase, NewBase);
						CurrentBase = NewBase;
					}
					val = ToNewBase(val, CurrentBase, 10);

					Assert.Equal(Math.Round(d, 2), Math.Round(double.Parse(val), 2));
				}
			}
		}
	}
}
