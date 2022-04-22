namespace ListVst;

public class PluginConfiguration
{
    public string? Name { get; set; }
    public string? Manufacturer { get; set; }
    public string[] Aliases { get; set; }
    
    public PluginConfiguration()
    {
        Aliases = Array.Empty<string>();
    }
}