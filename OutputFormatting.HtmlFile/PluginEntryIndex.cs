namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryIndex : EntryIndex<PluginDescriptor, ProjectDescriptor>
{
    public PluginEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }

    protected override string GetKey(PluginDescriptor entry)
    {
        return entry.Name;
    }}