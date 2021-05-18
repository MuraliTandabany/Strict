using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace Strict.BackEnd.Evaluator.Utilities
{
	public static class ValueUtilities
	{
		public static string AsString(object value)
		{
			if (value == null)
				return null;
			if (value is not string)
				return value.ToString();
			var text = (string)value;
			if (text.IndexOf('\'') < 0)
				return $"'{text}'";
			if (text.IndexOf('"') < 0)
				return $"\"{text}\"";
			text = text.Replace("'", "\\'");
			return $"'{text}'";
		}

		public static string AsPrintString(object value)
		{
			if (value == null)
				return "None";
			if (value is string)
				return (string)value;
			if (value is not IList)
				return AsString(value);
			var isTuple = value is ReadOnlyCollection<object>;
			var sb = new StringBuilder();
			sb.Append(isTuple
				? "("
				: "[");
			var nitems = 0;
			foreach (var item in (IList)value)
			{
				if (nitems > 0)
					sb.Append(", ");
				sb.Append(AsString(item));
				nitems++;
			}
			sb.Append(isTuple
				? ")"
				: "]");
			return sb.ToString();
		}
	}
}