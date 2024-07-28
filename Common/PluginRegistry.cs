namespace ListVst;

public class PluginRegistry : IPluginRegistry
{
    private Dictionary<string, PluginDescriptor> Registry { get; } = new();

    public PluginDescriptor this[string alias]
    {
        get
        {
            if (!Registry.TryGetValue(alias, out var pluginDescriptor))
            {
                return PluginDescriptor.NoPlugin;
            }
            
            return pluginDescriptor;
        }
    }

    public void Register(string name, string manufacturer, string alias)
    {
        Registry[alias] = new PluginDescriptor(name, manufacturer, PluginType.Unknown);
    }
    
    public void Register(string name, string manufacturer, string[] aliases)
    {
        foreach (var alias in aliases)
        {
            Register(name, manufacturer, alias);
        }
    }
}