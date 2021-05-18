using System.Collections;

namespace Strict.Language
{
	public class Types
	{
		private static readonly IType stringType = new StringType("str");

		public static string GetTypeName(object value) =>
			value switch
			{
				null => "NoneType",
				int => "int",
				double => "float",
				string => "str",
				IFunction => "function",
				IList => "list",
				_ => value.GetType().Name
			};

		public static IType GetType(object value)
		{
			if (value is string)
				return stringType;
			var dynamicObject = value as DynamicObject;
			if (dynamicObject != null)
				return dynamicObject.Class;
			return null;
		}
	}
}