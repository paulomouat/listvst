namespace ListVst.OutputFormatting;

public interface IOutputFormatter
{
    string Format { get; }
    Task Write(IEnumerable<PluginData> pairs, IOutputFormatterOptions options);
}