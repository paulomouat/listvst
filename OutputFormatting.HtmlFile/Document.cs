using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Document : XElement
{
    public string Title { get; }
    public XElement Body { get; }
    
    public static Document Create(string title)
    {
        var container = new Document(title);
        return container;
    }

    public void Add(Section section)
    {
        Body.Add(section);
    }

    private Document(string title)
        : base("html")
    {
        Title = title;

        var head = new XElement("head", new XElement("title", Title), new XElement("style", Style));
        Add(head);
        
        var body = new XElement("body");
        Body = body;
            
        var titleElement = new XElement("p", Title);
        titleElement.SetAttributeValue("class", "list-title");
        Body.Add(titleElement);

        Add(Body);
    }

    private const string Style =
@"
body {
    font-family: 'Helvetica', 'Arial', sans-serif;
    color: #444444;
}
.list-title {
    font-size: 2em;
    font-weight: bold;
}
.main-index-title {
    font-size: 1.5em;
    font-weight: bold;
}
.section-title {
    font-size: 1.5em;
    font-weight: bold;
}
.index-title {
    font-size: 1.1em;
    font-weight: bold;
}
.title {
    font-size: 1.1em;
    font-weight: bold;
}
a {
    text-decoration: none;
    color: #666666;
}
a:hover {
    text-decoration: underline;
    color: #999999;
}
";
}