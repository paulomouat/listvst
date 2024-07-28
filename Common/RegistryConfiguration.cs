namespace ListVst;

public class RegistryConfiguration
{
    public const string SectionName = "Registry";

    public PluginConfiguration[] Plugins { get; set; } = [];
}