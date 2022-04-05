namespace ListVst.OutputFormatting;

public interface IFileOutputFormatterOptions : IOutputFormatterOptions
{
    string? Path { get; }
}