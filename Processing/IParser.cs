namespace ListVst.Processing;

public interface IParser
{
    Task<IEnumerable<string>> Parse(string xml);
}