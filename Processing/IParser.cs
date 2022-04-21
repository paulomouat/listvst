namespace ListVst.Processing;

public interface IParser
{
    Task<IEnumerable<PluginInfo>> Parse(string xml);
}