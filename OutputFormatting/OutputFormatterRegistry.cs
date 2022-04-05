namespace ListVst.OutputFormatting;

public class OutputFormatterRegistry : IOutputFormatterRegistry
{
    private Dictionary<string, IOutputFormatter> Registry { get; }

    public IOutputFormatter? this[string format]
    {
        get
        {
            Registry.TryGetValue(format, out IOutputFormatter outputFormatter);
            return outputFormatter;
        }
    }

    public OutputFormatterRegistry()
        : this(Array.Empty<IOutputFormatter>())
    {
        Registry = new Dictionary<string, IOutputFormatter>();
    }

    public OutputFormatterRegistry(IEnumerable<IOutputFormatter> formatters)
    {
        Registry = new Dictionary<string, IOutputFormatter>();
        foreach (var formatter in formatters)
        {
            Registry[formatter.Format] = formatter;
        }
    }
    
    public void Register(string format, IOutputFormatter outputFormatter)
    {
        Registry[format] = outputFormatter;
    }
}