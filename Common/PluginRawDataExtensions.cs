namespace ListVst;

public static class PluginRawDataExtensions
{
    public static PluginRawData ResolveAliases(this PluginRawData rawData, IPluginAliasesRegistry registry)
    {
        var current = rawData.PluginFullName;
        var proposed = registry[current];
        if (!string.IsNullOrWhiteSpace(proposed) && current != proposed)
        {
            var adjusted = rawData with { PluginFullName = proposed };
            return adjusted;
        };

        return rawData;
    }

    public static IEnumerable<PluginRawData> ResolveAliases(this IEnumerable<PluginRawData> rawData,
        IPluginAliasesRegistry registry)
    {
        return rawData.Select(rd => rd.ResolveAliases(registry));
    }

    public static PluginData ToPluginData(this PluginRawData rawData, IPluginManufacturersRegistry registry)
    {
        var projectDescriptor = new ProjectDescriptor(rawData.ProjectPath);

        var fullName = rawData.PluginFullName;
        var name = fullName;
        var manufacturer = registry.GetManufacturer(fullName);
        if (!string.IsNullOrEmpty(manufacturer))
        { 
            name = name[manufacturer.Length..].Trim();
        }

        var pluginDescriptor = new PluginDescriptor(name, manufacturer, fullName);

        var pluginData = new PluginData(pluginDescriptor, projectDescriptor);
        return pluginData;
    }

    public static IEnumerable<PluginData> ToPluginData(this IEnumerable<PluginRawData> rawData,
        IPluginManufacturersRegistry registry)
    {
        return rawData.Select(rd => rd.ToPluginData(registry));
    }
}
