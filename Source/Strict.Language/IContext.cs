namespace Strict.Language
{
    public interface IContext : IValues
    {
        IContext GlobalContext { get; }
    }
}