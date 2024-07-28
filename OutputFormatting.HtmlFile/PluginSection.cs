namespace ListVst.OutputFormatting.HtmlFile;

public class PluginSection(string id = "listing-by-plugin", string title = "Listing by plugin")
    : Section(id, title)
{
    protected override IEntryIndex BuildEntryIndex(IEnumerable<PluginRecord> data)
    {
        var index = new PluginEntryIndex(Id + "-index", "All entries", this);
        index.AddPluginRecords(data);
        return index;
    }
    
    protected override IEntryList BuildEntryList(IEnumerable<PluginRecord> data)
    {
        var list = new PluginEntryList(Id + "-entries", this);
        list.AddPluginRecords(data);
        return list;
    }
}