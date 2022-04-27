using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Entry : XElement
{
    public Entry(Id id)
        : base("div")
    {
        Add(new XAttribute("id", id.Value),
            new XAttribute("class", "entry"));
    }
}