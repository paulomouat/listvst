namespace ListVst.Processing;

public interface IParser
{
    IEnumerable<string> Parse(string xml);
}