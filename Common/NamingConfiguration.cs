namespace ListVst;

public class NamingConfiguration
{
    public const string SectionName = "Naming";
    
    public PluginNamingConfiguration[] Plugins { get; set; }
    
    public NamingConfiguration()
    {
        Plugins = Array.Empty<PluginNamingConfiguration>();
    }
}