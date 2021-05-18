namespace Strict.Context
{
	public interface IContext : IValues
	{
		IContext GlobalContext { get; }
	}
}