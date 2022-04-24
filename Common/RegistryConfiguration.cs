namespace ListVst;

public class RegistryConfiguration
{
    public const string SectionName = "Registry";
    
    public PluginConfiguration[] Plugins { get; set; }
    
    public RegistryConfiguration()
    {
        Plugins = Array.Empty<PluginConfiguration>();
    }
}