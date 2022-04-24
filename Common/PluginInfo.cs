namespace ListVst;

public readonly record struct PluginInfo(string Name, string Manufacturer, PluginType PluginType)
{
    public static readonly PluginInfo NoPlugin = new ("", "", PluginType.Unknown);

    public string FullName => (Manufacturer + " " + Name).Trim();
}
