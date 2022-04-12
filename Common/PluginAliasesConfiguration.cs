namespace ListVst;

public class PluginAliasesConfiguration
{
    public string? Name { get; set; }
    public string[] Aliases { get; set; }
    
    public PluginAliasesConfiguration()
    {
        Aliases = Array.Empty<string>();
    }
}