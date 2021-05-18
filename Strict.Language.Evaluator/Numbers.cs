using System;
using Microsoft.VisualBasic.CompilerServices;
using Strict.Exceptions;

namespace Strict.Evaluator
{
	public static class Numbers
	{
		public static object Add(object obj1, object obj2) =>
			IsNumber(obj1) && IsNumber(obj2)
				? Operators.AddObject(obj1, obj2)
				: Operators.ConcatenateObject(obj1, obj2);

		public static object Subtract(object obj1, object obj2) => Operators.SubtractObject(obj1, obj2);

		public static object Multiply(object obj1, object obj2) => Operators.MultiplyObject(obj1, obj2);

		public static object Divide(object obj1, object obj2) =>
			IsFixnum(obj1) && IsFixnum(obj2) && (int)Operators.ModObject(obj1, obj2) == 0
				? Operators.IntDivideObject(obj1, obj2)
				: Operators.DivideObject(obj1, obj2);

		public static object Remainder(object obj1, object obj2) =>
			!IsFixnum(obj1) || !IsFixnum(obj2)
				? throw new InvalidOperationException("Remainder requires integer values")
				: Operators.ModObject(obj1, obj2);

		public static long GreatestCommonDivisor(long n, long m)
		{
			var a = Math.Min(n, m);
			var b = Math.Max(n, m);
			var rest = b % a;
			while (rest != 0)
			{
				b = a;
				a = rest;
				rest = b % a;
			}
			return Math.Abs(a);
		}

		public static object Abs(object obj) =>
			(bool)Operators.CompareObjectLess(obj, 0, false)
				? Operators.NegateObject(obj)
				: obj;

		public static bool IsFixnum(object obj) => obj is short or int or long;

		public static bool IsRealnum(object obj) => obj is float or double;

		public static bool IsNumber(object obj) => IsFixnum(obj) || IsRealnum(obj);

		public static object Negate(object obj) => Operators.NegateObject(obj);

		public static int ToInteger(object obj) =>
			!IsFixnum(obj)
				? throw new TypeErrorException(
					$"'{Types.GetTypeName(obj)}' object cannot be interpreted as an integer")
				: (int)obj;
	}
}