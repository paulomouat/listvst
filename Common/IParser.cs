namespace ListVst;

public interface IParser
{
    IEnumerable<string> Parse(string xml);
}