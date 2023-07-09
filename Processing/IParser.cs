namespace ListVst.Processing;

public interface IParser
{
    public const string NoManufacturer = "(n/a)";

    Task<IEnumerable<PluginDescriptor>> Parse(string xml);
}