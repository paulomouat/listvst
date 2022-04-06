using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Document : XElement
{
    public string Title { get; }
    public XElement Body { get; }

    public Document(string title)
        : base("html")
    {
        Title = title;

        var head = new XElement("head", new XElement("title", Title));
        Add(head);
        
        var body = new XElement("body");
        Body = body;
            
        var titleElement = new XElement("p", Title);
        titleElement.SetAttributeValue("class", "title");
        Body.Add(titleElement);

        Add(Body);
    }
    
    public static Document Create(string title)
    {
        var container = new Document(title);
        return container;
    }

    public void Add(Section section)
    {
        Body.Add(section);
    }
}