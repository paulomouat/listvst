namespace ListVst;

public class Configuration
{
    public string? SourcePath { get; set; }
    public IEnumerable<IProcessor> Processors { get; set; }

    public Configuration()
    {
        Processors = Array.Empty<IProcessor>();
    }
}