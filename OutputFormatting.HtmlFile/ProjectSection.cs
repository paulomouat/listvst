namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectSection(string id = "listing-by-path", string title = "Listing by project path")
    : Section(id, title)
{
    protected override IEntryIndex BuildEntryIndex(IEnumerable<PluginRecord> data)
    {
        var index = new ProjectEntryIndex(Id + "-index", "All entries", this);
        index.AddPluginRecords(data);
        return index;
    }
    
    protected override IEntryList BuildEntryList(IEnumerable<PluginRecord> data)
    {
        var list = new ProjectEntryList(Id + "-entries", this);
        list.AddPluginRecords(data);
        return list;
    }
}