namespace ListVst;

public class NamingConfiguration
{
    public const string SectionName = "Naming";
    
    public PluginConfiguration[] Plugins { get; set; }
    
    public NamingConfiguration()
    {
        Plugins = Array.Empty<PluginConfiguration>();
    }
}