namespace ListVst;

public static class PluginDescriptorExtensions
{
    public static PluginDescriptor ResolveAliases(this PluginDescriptor pluginDescriptor, IPluginRegistry registry)
    {
        // Let's see if we find an alias registered under just the name
        var alias = pluginDescriptor.Name;
        var registered = registry[alias];

        // ...if not, see if one is registered under the full name
        if (registered == PluginDescriptor.NoPlugin)
        {
            alias = pluginDescriptor.FullName;
            registered = registry[alias];
        }

        if (registered != PluginDescriptor.NoPlugin)
        {
            var proposedName = registered.Name;    
            var proposedManufacturer = registered.Manufacturer;    
            if (pluginDescriptor.Name != proposedName || pluginDescriptor.Manufacturer != proposedManufacturer)
            {
                var adjusted = pluginDescriptor with
                {
                    Name = proposedName,
                    Manufacturer = proposedManufacturer
                };
                return adjusted;
            }
        }

        return pluginDescriptor;
    }

    public static IEnumerable<PluginDescriptor> ResolveAliases(this IEnumerable<PluginDescriptor> pluginDescriptors,
        IPluginRegistry registry)
    {
        return pluginDescriptors.Select(pi => pi.ResolveAliases(registry));
    }
}
