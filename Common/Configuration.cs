namespace ListVst;

public class Configuration
{
    public string? SourcePath { get; set; }
    public IEnumerable<OutputDetails> Outputs { get; set; }

    public Configuration()
    {
        Outputs = Array.Empty<OutputDetails>();
    }
}