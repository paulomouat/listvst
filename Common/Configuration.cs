namespace ListVst;

public class Configuration
{
    public string? SourcePath { get; set; }
    public IEnumerable<IProcessor> Processors { get; set; }
    public IEnumerable<OutputDetails> Outputs { get; set; }

    public Configuration()
    {
        Processors = Array.Empty<IProcessor>();
        Outputs = Array.Empty<OutputDetails>();
    }
}