namespace ListVst;

public interface IOutputFormatter
{
    string Format { get; }
    Task Write(IEnumerable<(string Path, string Vst)> details);
}