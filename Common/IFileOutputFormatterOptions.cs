namespace ListVst;

public interface IFileOutputFormatterOptions : IOutputFormatterOptions
{
    string? Path { get; }
}