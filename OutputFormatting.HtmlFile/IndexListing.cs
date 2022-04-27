using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class IndexListing : XElement
{
    public IndexListing()
        : base("div")
    {
        Add(new XAttribute("class", "index-items"));
    }
}