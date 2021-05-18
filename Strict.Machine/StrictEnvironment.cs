using System.Collections.Generic;
using System.Linq;
using Strict.Context;

namespace Strict.Machine
{
	public class StrictEnvironment : IContext
	{
		private IContext Parent { get; }
		private Dictionary<string, object> Values { get; } = new();
		private object ReturnValue { get; set; }
		private bool HasReturnValue { get; set; }

		public StrictEnvironment() { }

		public StrictEnvironment(IContext parent) => Parent = parent;

		public IContext GlobalContext =>
			Parent == null
				? this
				: Parent.GlobalContext;

		public bool WasContinue { get; set; }

		public bool WasBreak { get; set; }

		public void SetReturnValue(object value)
		{
			ReturnValue = value;
			HasReturnValue = true;
		}

		public object GetValue(string name) =>
			!Values.ContainsKey(name)
				? Parent?.GetValue(name)
				: Values[name];

		public void SetValue(string name, object value) => Values[name] = value;

		public bool HasValue(string name) => Values.ContainsKey(name);

		public ICollection<string> GetNames() => Values.Keys.ToList();
	}
}