namespace ListVst.OutputFormatting;

public interface IOutputFormatter
{
    string Format { get; }
    Task Write(IEnumerable<PluginProjectPair> pairs, IOutputFormatterOptions options);
}