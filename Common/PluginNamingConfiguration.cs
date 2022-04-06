namespace ListVst;

public class PluginNamingConfiguration
{
    public string? Main { get; set; }
    public string[] Aliases { get; set; }
    
    public PluginNamingConfiguration()
    {
        Aliases = Array.Empty<string>();
    }
}