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
        
    public async Task<IEnumerable<PluginDescriptor>> Parse(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return Array.Empty<PluginDescriptor>();
        }

        var pluginDescriptors = ExtractPluginDescriptors(xml);
        return await Task.FromResult(pluginDescriptors);
    }

    private static IEnumerable<PluginDescriptor> ExtractPluginDescriptors(string xml)
    {
        var pluginDescriptors = new List<PluginDescriptor>();
            
        using var sr = new StringReader(xml);
        using var reader = XmlReader.Create(sr, new XmlReaderSettings{ Async = true });
        while(reader.ReadToFollowing("PluginDesc"))
        {
            var pluginDescriptorReader = reader.ReadSubtree();
            var pluginDescriptor = ProcessPluginDesc(pluginDescriptorReader);
            pluginDescriptors.Add(pluginDescriptor);
        }

        return pluginDescriptors;
    }

    private static PluginDescriptor ProcessPluginDesc(XmlReader reader)
    {
        var manufacturer = IParser.NoManufacturer;
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
                        manufacturer = reader.GetAttribute("Value") ?? IParser.NoManufacturer;
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

        return new PluginDescriptor(name, manufacturer, pluginType);
    }
}