using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryTitle : XElement
{
    protected EntryTitle()
        : base("div")
    { }
}