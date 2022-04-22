namespace ListVst;

public class PluginRegistry : IPluginRegistry
{
    private Dictionary<string, PluginInfo> Registry { get; }

    public PluginInfo this[string alias]
    {
        get
        {
            if (!Registry.TryGetValue(alias, out var pluginInfo))
            {
                return PluginInfo.NoPlugin;
            }
            
            return pluginInfo;
        }
    }

    public PluginRegistry()
    {
        Registry = new Dictionary<string, PluginInfo>();
    }

    public void Register(string name, string manufacturer, string alias)
    {
        Registry[alias] = new PluginInfo(name, manufacturer, PluginType.Unknown);
    }
    
    public void Register(string name, string manufacturer, string[] aliases)
    {
        foreach (var alias in aliases)
        {
            Register(name, manufacturer, alias);
        }
    }
}