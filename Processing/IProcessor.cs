namespace ListVst.Processing;

public interface IProcessor
{
    Task<IEnumerable<PluginRawData>> Process(string sourcePath);
}