### BasedNumber
This lib allows you to create value-strings for any numbers in any bases in range of [2; 36]. 
BasedNumber class allows you to create instance implicited from double and than use it with math/compare operators.
Use namespace LuminoDiodeBasedNumber to access 'BasedNumber' and 'BasedNumberStatic' classes.
The main methods are:
1. BasedNumberStatic.FromDecimalToNewBase which returns a value-string in passed base for passed decimal value, for example "3G" if you converts 100 (10) to (28) base.
2. BasedNumberStatic.ToDecimal which returns decimal value of passed string in passed base, for example 46217 if you converts it from ZNT with base 36.
3. BasedNumberStatic.ToNewBase which returns value-string in new base, e.g. returns "22QH" if you converts ZNT(36) to the base of 28.
