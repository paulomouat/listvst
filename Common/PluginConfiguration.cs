namespace ListVst;

public class PluginConfiguration
{
    public string? Manufacturer { get; set; }
    public NameConfiguration[] Names { get; set; }
    
    public PluginConfiguration()
    {
        Names = Array.Empty<NameConfiguration>();
    }
}