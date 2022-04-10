namespace ListVst.Processing;

public interface IProcessor
{
    Task<IEnumerable<PluginProjectPair>> Process(string sourcePath);
}