namespace ListVst;

public class PluginAliasesRegistry : IPluginAliasesRegistry
{
    private Dictionary<string, string> Registry { get; }

    public string this[string alias]
    {
        get
        {
            if (!Registry.TryGetValue(alias, out var name))
            {
                return alias;
            }
            
            return name;
        }
    }

    public PluginAliasesRegistry()
    {
        Registry = new Dictionary<string, string>();
    }

    public void Register(string name, string alias)
    {
        Registry[alias] = name;
    }
    
    public void Register(string name, string[] aliases)
    {
        foreach (var alias in aliases)
        {
            Registry[alias] = name;
        }
    }
}