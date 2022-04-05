namespace ListVst.OutputFormatting;

public class OutputFormattingConfiguration
{
    public const string SectionName = "Output";
    
    public string[] Formatters { get; set; }
    
    public OutputFormattingConfiguration()
    {
        Formatters = Array.Empty<string>();
    }
}