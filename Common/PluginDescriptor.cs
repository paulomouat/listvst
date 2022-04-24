namespace ListVst;

public readonly record struct PluginDescriptor(string Name, string Manufacturer, PluginType PluginType)
{
    public static readonly PluginDescriptor NoPlugin = new ("None", "None", PluginType.Unknown);

    public string FullName => (Manufacturer + " " + Name).Trim(); 
}
