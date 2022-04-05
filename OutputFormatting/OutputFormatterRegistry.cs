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
    {
        Registry = new Dictionary<string, IOutputFormatter>();
    }

    public void Register(string format, IOutputFormatter outputFormatter)
    {
        Registry[format] = outputFormatter;
    }
}