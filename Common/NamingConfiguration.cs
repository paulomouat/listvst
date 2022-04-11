namespace ListVst;

public class NamingConfiguration
{
    public const string SectionName = "Naming";
    
    public PluginNamingConfiguration[] PluginAliases { get; set; }
    
    public NamingConfiguration()
    {
        PluginAliases = Array.Empty<PluginNamingConfiguration>();
    }
}