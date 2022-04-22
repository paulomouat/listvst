namespace ListVst;

public static class PluginRawDataExtensions
{
    public static PluginRawData ResolveAliases(this PluginRawData rawData, IPluginRegistry registry)
    {
        var current = rawData.PluginInfo;
        var proposed = current.ResolveAliases(registry);
        
        if (current != proposed)
        {
            var adjusted = rawData with
            {
                PluginInfo = proposed
            };
            return adjusted;
        };
        
        return rawData;
    }

    public static IEnumerable<PluginRawData> ResolveAliases(this IEnumerable<PluginRawData> rawData,
        IPluginRegistry registry)
    {
        return rawData.Select(rd => rd.ResolveAliases(registry));
    }

    public static PluginData ToPluginData(this PluginRawData rawData)
    {
        var projectDescriptor = new ProjectDescriptor(rawData.ProjectPath);

        var name = rawData.PluginInfo.Name;
        var manufacturer = rawData.PluginInfo.Manufacturer;
        var fullName = rawData.PluginInfo.FullName;
        
        var pluginDescriptor = new PluginDescriptor(name, manufacturer, fullName);

        var pluginData = new PluginData(pluginDescriptor, projectDescriptor);
        return pluginData;
    }

    public static IEnumerable<PluginData> ToPluginData(this IEnumerable<PluginRawData> rawData)
    {
        return rawData.Select(rd => rd.ToPluginData());
    }
}
