using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Stats : XElement
{
    public Stats(string section, int count)
        : base("div")
    {
        Add(new XAttribute("class", "stats"));
        Add("Number of entries: ");
        var span = new XElement("span", string.Empty, new XAttribute("id", $"stats-{section}"));
        Add(span);
        Add(" out of " + count);
    }
}