using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryList : XElement
{
    public string Id { get; }

    public Section ParentSection { get; }
    
    public EntryList(string id, Section parentSection)
        : base("div")
    {
        Id = id;
        ParentSection = parentSection;
        
        SetAttributeValue("id", id);
    }
    
    public virtual void Add(ILookup<string, PluginDescriptor> lookup)
    {
        foreach(var group in lookup)
        {
            var key = group.Key;
            var entryId = new Id(key).Value;
            
            var entry = new XElement("div",
                new XAttribute("id", entryId),
                new XAttribute("class", "entry"));
            
            var entryTitle = new XElement("div",
                new XAttribute("class", "key title"),
                key);
            entry.Add(entryTitle);
            var linkToTop = new XElement("a",
                new XAttribute("class", "link-to-top"),
                new XAttribute("href", "#document-title"),
                "top");
            entry.Add(linkToTop);
            var linkToSection = new XElement("a",
                new XAttribute("class", "link-to-section"),
                new XAttribute("href", "#" + ParentSection.Id),
                "section index");
            entry.Add(linkToSection);
            
            foreach(var pluginDescriptor in group)
            {
                Add(pluginDescriptor, entry);
            }
            
            Add(entry);
        }
    }

    public virtual void Add(PluginDescriptor pluginDescriptor, XElement entry)
    {
        var item = pluginDescriptor.Name;
        var anchor = new XElement("a",
            new XAttribute("href", "#" + new Id(item)),
            item);
        var element = new XElement("div",
            new XAttribute("class", "item"),
            anchor);
        entry.Add(element);
    }
}