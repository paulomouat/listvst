namespace ListVst.Processing;

public interface IProcessor
{
    Task<IEnumerable<PluginDescriptor>> Process(string sourcePath);
}