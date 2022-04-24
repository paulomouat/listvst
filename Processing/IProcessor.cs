namespace ListVst.Processing;

public interface IProcessor
{
    Task<IEnumerable<ParsedRecord>> Process(string sourcePath);
}