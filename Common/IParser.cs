using System.Collections.Generic;

namespace ListVst;

public interface IParser
{
    IEnumerable<string> Parse(string xml);
}