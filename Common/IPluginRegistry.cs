namespace ListVst;

public interface IPluginRegistry
{
    PluginDescriptor this[string alias] { get; }
    void Register(string name, string manufacturer, string alias);
}