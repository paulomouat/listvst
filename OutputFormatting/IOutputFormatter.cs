namespace ListVst.OutputFormatting;

public interface IOutputFormatter
{
    string Format { get; }
    Task Write(IEnumerable<(string Path, string Vst)> details, IOutputFormatterOptions options);
}