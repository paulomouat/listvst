namespace ListVst.OutputFormatting;

public interface IOutputFormatter
{
    string Format { get; }
    Task Write(IEnumerable<PluginRecord> pairs, IOutputFormatterOptions options);
}