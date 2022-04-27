using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryTitle : XElement
{
    public EntryTitle()
        : base("div")
    { }
}