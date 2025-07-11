﻿using System.Xml;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing.StudioOne;

public class Parser(ILogger logger) : IParser
{
    private static readonly IDictionary<string, string> XmlNamespaces = new Dictionary<string, string>
    {
        { "x", "urn:presonus.com/studioone" }
    };

    private ILogger Logger { get; } = logger;

    public async Task<IEnumerable<PluginDescriptor>> Parse(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return [];
        }
            
        var pluginDescriptors = await ExtractPluginDescriptors(xml);
        return pluginDescriptors;
    }

    private static async Task<IEnumerable<PluginDescriptor>> ExtractPluginDescriptors(string xml)
    {
        var pluginDescriptors = new List<PluginDescriptor>();
        
        using var sr = new StringReader(xml);
        using var reader = CreateXmlReader(sr);
        while (await reader.ReadAsync())
        {
            if (reader is {NodeType: XmlNodeType.Element, Name: "Attributes"} &&
                reader.GetAttribute("id", XmlNamespaces["x"]) == "ghostData")
            {
                var pluginDescriptorReader = reader.ReadSubtree();
                var pluginDescriptor = ProcessPluginDesc(pluginDescriptorReader);
                pluginDescriptors.Add(pluginDescriptor);
            }
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
            if (reader is {NodeType: XmlNodeType.Element, Name: "Attributes"} &&
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
                        } else if (subCategory.StartsWith("VST2"))
                        {
                            pluginType = PluginType.Vst;
                        } else if (subCategory.StartsWith("(Native)"))
                        {
                            pluginType = PluginType.AudioUnit;
                        }
                        break;
                }
            }
        }

        return new PluginDescriptor(name, manufacturer, pluginType);
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