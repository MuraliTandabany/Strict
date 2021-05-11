namespace Strict.Language
{
	public class Parameter
	{
		public string Name { get; }

		public object DefaultValue { get; }

		public bool IsList { get; }

		public Parameter(string name, object defaultValue, bool isList)
		{
			Name = name;
			DefaultValue = defaultValue;
			IsList = isList;
		}
	}
}