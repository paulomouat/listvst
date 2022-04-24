namespace ListVst;

public class NameConfiguration
{
    public string? Name { get; set; }
    public string[] Aliases { get; set; }
    
    public NameConfiguration()
    {
        Aliases = Array.Empty<string>();
    }
}