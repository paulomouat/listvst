namespace ListVst;

public record struct PluginInfo(string Name, string Manufacturer, PluginType PluginType)
{
    public static readonly PluginInfo NoPlugin = new ("None", "None", PluginType.Unknown);
}
