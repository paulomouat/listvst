namespace ListVst.Processing;

public interface IParser
{
    Task<IEnumerable<PluginDescriptor>> Parse(string xml);
}