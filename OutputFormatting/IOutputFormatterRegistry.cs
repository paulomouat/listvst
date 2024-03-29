namespace ListVst.OutputFormatting;

public interface IOutputFormatterRegistry
{
    IOutputFormatter? this[string format] { get; }
    void Register(string format, IOutputFormatter outputFormatter);
}