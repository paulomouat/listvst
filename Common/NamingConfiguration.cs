namespace ListVst;

public class NamingConfiguration
{
    public const string SectionName = "Naming";
    
    public PluginConfiguration[] Plugins { get; set; }
    public PluginManufacturersConfiguration[] PluginManufacturers { get; set; }
    
    public NamingConfiguration()
    {
        Plugins = Array.Empty<PluginConfiguration>();
        PluginManufacturers = Array.Empty<PluginManufacturersConfiguration>();
    }
}