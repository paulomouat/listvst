using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class LinkToTop : XElement
{
    public LinkToTop()
        : base("a")
    {
        Add(new XAttribute("class", "link-to-top"),
            new XAttribute("href", "#document-title"));
        Value = "top";
    }
}