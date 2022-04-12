namespace ListVst;

public class PluginManufacturersRegistry : IPluginManufacturersRegistry
{
    private HashSet<string> Registry { get; }

    public string GetManufacturer(string pluginName)
    {
        // TODO: This is a naive implementation
        var match = Registry.FirstOrDefault(pluginName.StartsWith);

        if (!string.IsNullOrEmpty(match))
        {
            return match;
        }
        
        return string.Empty;
    }

    public PluginManufacturersRegistry()
    {
        Registry = new HashSet<string>();
    }

    public void Register(string name)
    {
        Registry.Add(name);
    }
}