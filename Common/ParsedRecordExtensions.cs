namespace ListVst;

public static class ParsedRecordExtensions
{
    public static ParsedRecord ResolveAliases(this ParsedRecord parsedRecord, IPluginRegistry registry)
    {
        var current = parsedRecord.PluginInfo;
        var proposed = current.ResolveAliases(registry);
        
        if (current != proposed && proposed != PluginInfo.NoPlugin)
        {
            var adjusted = parsedRecord with
            {
                PluginInfo = proposed
            };
            return adjusted;
        };
        
        return parsedRecord;
    }

    public static IEnumerable<ParsedRecord> ResolveAliases(this IEnumerable<ParsedRecord> parsedRecords,
        IPluginRegistry registry)
    {
        return parsedRecords.Select(rd => rd.ResolveAliases(registry));
    }

    public static PluginRecord ToPluginRecord(this ParsedRecord parsedRecord)
    {
        var projectDescriptor = new ProjectDescriptor(parsedRecord.ProjectPath);

        var name = parsedRecord.PluginInfo.Name;
        var manufacturer = parsedRecord.PluginInfo.Manufacturer;
        var fullName = parsedRecord.PluginInfo.FullName;
        
        var pluginDescriptor = new PluginDescriptor(name, manufacturer, fullName);

        var pluginRecord = new PluginRecord(pluginDescriptor, projectDescriptor);
        return pluginRecord;
    }

    public static IEnumerable<PluginRecord> ToPluginRecord(this IEnumerable<ParsedRecord> parsedRecords)
    {
        return parsedRecords.Select(rd => rd.ToPluginRecord());
    }
}
