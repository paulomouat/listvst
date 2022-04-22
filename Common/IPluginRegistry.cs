namespace ListVst;

public interface IPluginRegistry
{
    PluginInfo this[string alias] { get; }
    void Register(string name, string manufacturer, string alias);
}