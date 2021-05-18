namespace Strict.BackEnd.Evaluator
{
	public static class Strings
	{
		public static object Multiply(object obj1, object obj2)
		{
			var text = (string)obj1;
			var repeat = (int)obj2;
			var result = string.Empty;
			for (var k = 0; k < repeat; k++)
				result += text;
			return result;
		}

		public static bool IsString(object obj) => obj is string;
	}
}