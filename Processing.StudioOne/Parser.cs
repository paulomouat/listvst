using System.Xml;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing.StudioOne;

public class Parser : IParser
{
    private static readonly IDictionary<string, string> XmlNamespaces = new Dictionary<string, string>
    {
        { "x", "urn:presonus.com/studioone" }
    };

    private ILogger Logger { get; }

    public Parser(ILogger logger)
    {
        Logger = logger;
    }

    public async Task<IEnumerable<PluginInfo>> Parse(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return Array.Empty<PluginInfo>();
        }
            
        var pluginInfos = await ExtractPluginData(xml);
        return pluginInfos;
    }

    private static async Task<IEnumerable<PluginInfo>> ExtractPluginData(string xml)
    {
        var pluginInfos = new List<PluginInfo>();
        
        using var sr = new StringReader(xml);
        using var reader = CreateXmlReader(sr);
        while (await reader.ReadAsync())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes" &&
                reader.GetAttribute("id", XmlNamespaces["x"]) == "ghostData")
            {
                var pluginInfoReader = reader.ReadSubtree();
                var pluginInfo = ProcessPluginDesc(pluginInfoReader);
                pluginInfos.Add(pluginInfo);
            }
        }

        return pluginInfos;
    }

    private static PluginInfo ProcessPluginDesc(XmlReader reader)
    {
        var manufacturer = string.Empty;
        var name = string.Empty;
        var pluginType = PluginType.Unknown;

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes" &&
                reader.GetAttribute("id", XmlNamespaces["x"]) == "classInfo")
            {
                name = reader.GetAttribute("name") ?? string.Empty;
                var subCategory = reader.GetAttribute("subCategory") ?? string.Empty;

                switch (subCategory)
                {
                    case "AudioUnit":
                        pluginType = PluginType.AudioUnit;
                        break;
                    case "VST2":
                        pluginType = PluginType.Vst;
                        break;
                    default:
                        if (subCategory.StartsWith("VST3"))
                        {
                            pluginType = PluginType.Vst3;
                        }

                        break;
                }
            }
        }

        return new PluginInfo(name, manufacturer, pluginType);
    }

    private static XmlReader CreateXmlReader(TextReader textReader)
    {
        var settings = new XmlReaderSettings { NameTable = new NameTable(), Async = true };
        var xmlns = new XmlNamespaceManager(settings.NameTable);
        foreach (var nskvp in XmlNamespaces)
        {
            xmlns.AddNamespace(nskvp.Key, nskvp.Value);
        }

        var context = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
        var reader = XmlReader.Create(textReader, settings, context);
        return reader;
    }
}