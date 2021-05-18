using System.Collections;

namespace Strict.BackEnd.Evaluator
{
	public static class Predicates
	{
		public static bool IsFalse(object obj) =>
			obj switch
			{
				null => true,
				false => true,
				_ => Numbers.IsFixnum(obj) && obj.Equals(0) || Numbers.IsRealnum(obj) && obj.Equals(0.0) ||
					obj switch
					{
						string s when string.IsNullOrEmpty(s) => true,
						IEnumerable enumerable => !enumerable.GetEnumerator().MoveNext(),
						_ => false
					}
			};
	}
}