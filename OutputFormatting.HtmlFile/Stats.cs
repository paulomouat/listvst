using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Stats : XElement
{
    public Stats(int count)
        : base("div")
    {
        Add(new XAttribute("class", "stats"));
        Value = "Number of entries: " + count;
    }
}