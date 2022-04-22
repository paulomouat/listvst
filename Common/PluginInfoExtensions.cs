namespace ListVst;

public static class PluginInfoExtensions
{
    public static PluginInfo ResolveAliases(this PluginInfo pluginInfo, IPluginRegistry registry)
    {
        // Let's see if we find an alias registered under just the name
        var alias = pluginInfo.Name;
        var registered = registry[alias];

        // ...if not, see if one is registered under the full name
        if (registered == PluginInfo.NoPlugin)
        {
            alias = pluginInfo.FullName;
            registered = registry[alias];
        }

        if (registered != PluginInfo.NoPlugin)
        {
            var proposedName = registered.Name;    
            var proposedManufacturer = registered.Manufacturer;    
            if (pluginInfo.Name != proposedName || pluginInfo.Manufacturer != proposedManufacturer)
            {
                var adjusted = pluginInfo with
                {
                    Name = proposedName,
                    Manufacturer = proposedManufacturer
                };
                return adjusted;
            };
        }
        
        return pluginInfo;
    }

    public static IEnumerable<PluginInfo> ResolveAliases(this IEnumerable<PluginInfo> pluginInfos,
        IPluginRegistry registry)
    {
        return pluginInfos.Select(pi => pi.ResolveAliases(registry));
    }
}
