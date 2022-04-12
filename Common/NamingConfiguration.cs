namespace ListVst;

public class NamingConfiguration
{
    public const string SectionName = "Naming";
    
    public PluginAliasesConfiguration[] PluginAliases { get; set; }
    public PluginManufacturersConfiguration[] PluginManufacturers { get; set; }
    
    public NamingConfiguration()
    {
        PluginAliases = Array.Empty<PluginAliasesConfiguration>();
        PluginManufacturers = Array.Empty<PluginManufacturersConfiguration>();
    }
}