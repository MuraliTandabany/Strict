using System.Collections.Generic;
using System.Linq;
using Strict.Context;

namespace Strict.BackEnd.Evaluator
{
	public class Module : IContext
	{
		private IDictionary<string, object> Values { get; } = new Dictionary<string, object>();
		public IContext GlobalContext { get; }

		public Module(IContext globalContext) => GlobalContext = globalContext;

		public object GetValue(string name) =>
			Values.ContainsKey(name)
				? Values[name]
				: GlobalContext?.GetValue(name);

		public void SetValue(string name, object value) => Values[name] = value;

		public bool HasValue(string name) =>
			Values.ContainsKey(name) || (GlobalContext?.HasValue(name) ?? false);

		public ICollection<string> GetNames() => Values.Keys.ToList();
	}
}