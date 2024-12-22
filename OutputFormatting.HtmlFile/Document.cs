using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Document : XElement
{
    private string Title { get; }
    private XElement Body { get; }
    
    public static Document Create(string title)
    {
        var container = new Document(title);
        return container;
    }

    public void AddMainIndex(MainIndex mainIndex)
    {
        Body.Add(mainIndex);
    }

    public void AddSection(ISection section)
    {
        Body.Add(section);
    }

    public void AddScriptInvocations()
    {
        var scriptElement = new XElement("script", "updateSelection();");
        Body.Add(scriptElement);
    }

    private Document(string title)
        : base("html")
    {
        Title = title;

        var head = new XElement("head", new XElement("title", Title));
        var meta = new XElement("meta", string.Empty, new XAttribute("charset", "UTF-8"));
        var stylesheet = new XElement("link", string.Empty,
            new XAttribute("rel", "stylesheet"),
            new XAttribute("type", "text/css"),
            new XAttribute("href", "styles.css"));
        var scripts = new XElement("script", string.Empty,
            new XAttribute("src", "selections.js"));
        head.Add([meta, stylesheet, scripts]);
        Add(head);
        
        var body = new XElement("body");
        Body = body;
            
        var titleElement = new XElement("div", Title,
            new XAttribute("id", "document-title"),
            new XAttribute("class", "list title"));
        Body.Add(titleElement);
        
        Add(Body);
    }
}