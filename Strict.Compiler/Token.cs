namespace Strict.Compiler
{
	public class Token
	{
		public string Value { get; set; }
		public TokenType TokenType { get; set; }
		public int Row { get; set; }

		private int _column;
		public int Column
		{
			get =>
				(Value != null
					? _column - Value.Length
					: 0) + 1;
			set => _column = value;
		}
	}
}