using System.Xml;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing.AbletonLive;

public class Parser : IParser
{
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

        var pluginInfos = ExtractPluginData(xml);
        return await Task.FromResult(pluginInfos);
    }

    private static IEnumerable<PluginInfo> ExtractPluginData(string xml)
    {
        var pluginInfos = new List<PluginInfo>();
            
        using var sr = new StringReader(xml);
        using var reader = XmlReader.Create(sr, new XmlReaderSettings{ Async = true });
        while(reader.ReadToFollowing("PluginDesc"))
        {
            var pluginInfoReader = reader.ReadSubtree();
            var pluginInfo = ProcessPluginDesc(pluginInfoReader);
            pluginInfos.Add(pluginInfo);
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
            switch (reader.Name)
            {
                case "AuPluginInfo":

                    pluginType = PluginType.AudioUnit;
                    
                    reader.Read();
                    
                    if (reader.ReadToNextSibling("Name"))
                    {
                        name = reader.GetAttribute("Value") ?? string.Empty;
                    }

                    if (reader.ReadToNextSibling("Manufacturer"))
                    {
                        manufacturer = reader.GetAttribute("Value") ?? string.Empty;
                    }

                    break;
                case "VstPluginInfo":
                    
                    pluginType = PluginType.Vst;

                    reader.Read();

                    if (reader.ReadToNextSibling("PlugName"))
                    {
                        name = reader.GetAttribute("Value") ?? string.Empty;
                    }
                    
                    break;
                case "Vst3PluginInfo":
                    
                    pluginType = PluginType.Vst3;
                    
                    reader.Read();

                    if (reader.ReadToNextSibling("Name"))
                    {
                        name = reader.GetAttribute("Value") ?? string.Empty;
                    }

                    break;
            }
        }

        return new PluginInfo(name, manufacturer, pluginType);
    }
}