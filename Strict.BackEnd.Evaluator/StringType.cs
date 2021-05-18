using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Strict.Exceptions;
using Strict.Language;

namespace Strict.BackEnd.Evaluator
{
	public class StringType : IType
	{
		private Dictionary<string, IFunction> methods { get; } = new();
		public string Name { get; }

		public StringType(string name) => Name = name;

		public IFunction GetMethod(string name) =>
			methods.ContainsKey(name)
				? methods[name]
				: null;

		public bool HasMethod(string name) => methods.ContainsKey(name);

		public void SetValue(string name, object value) => throw new NotImplementedException();

		public bool HasValue(string name) => throw new NotImplementedException();

		public ICollection<string> GetNames() => throw new NotImplementedException();

		public object GetValue(string name) => throw new NotImplementedException();

		private static int Find(string text, string argument) => text.IndexOf(argument);

		private static string Replace(string text, string toReplace, string newText) =>
			text.Replace(toReplace, newText);

		private static string Join(string sep, IList objects)
		{
			var result = string.Empty;
			var numberObjects = 0;
			foreach (var obj in objects)
			{
				if (numberObjects > 0)
					result += sep;
				result += obj;
				numberObjects++;
			}
			return result;
		}

		private static string[] Split(string text, string separator)
		{
			if (separator == null)
				return new[] { text };
			if (string.IsNullOrEmpty(separator))
				throw new ValueErrorException("empty separator");
			IList<string> result = new List<string>();
			for (var position = text.IndexOf(separator); position >= 0;
				position = text.IndexOf(separator))
			{
				result.Add(text.Substring(0, position));
				text = text.Substring(position + separator.Length);
			}
			result.Add(text);
			return result.ToArray();
		}

		private static object FindMethod(IList<object> arguments) =>
			Find((string)arguments[0], (string)arguments[1]);

		private static object ReplaceMethod(IList<object> arguments) =>
			Replace((string)arguments[0], (string)arguments[1], (string)arguments[2]);

		private static object SplitMethod(IList<object> arguments) =>
			Split((string)arguments[0], (string)arguments[1]);

		private static object JoinMethod(IList<object> arguments) =>
			Join((string)arguments[0], (IList)arguments[1]);
	}
}