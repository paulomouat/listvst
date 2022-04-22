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
}
