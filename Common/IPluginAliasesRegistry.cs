namespace ListVst;

public interface IPluginAliasesRegistry
{
    string? this[string alias] { get; }
    void Register(string name, string alias);
}