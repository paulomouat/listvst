namespace ListVst.OutputFormatting;

public interface IOutputFormatter
{
    string Format { get; }
    Task Write(IEnumerable<PluginDescriptor> details, IOutputFormatterOptions options);
}