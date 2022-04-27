using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class LinkToSection : XElement
{
    public LinkToSection(string sectionId)
        : base("a")
    {
        Add(new XAttribute("class", "link-to-section"),
            new XAttribute("href", "#" + sectionId));
        Value = "section index";
    }
}