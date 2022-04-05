namespace ListVst.Processing;

public class ProcessingConfiguration
{
    public const string SectionName = "Processing";
    
    public string[] Processors { get; set; }
    
    public ProcessingConfiguration()
    {
        Processors = Array.Empty<string>();
    }
}