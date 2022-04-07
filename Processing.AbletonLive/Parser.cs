using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing.AbletonLive;

public class Parser : IParser
{
    private ILogger Logger { get; }

    public Parser(ILogger logger)
    {
        Logger = logger;
    }
        
    public async Task<IEnumerable<string>> Parse(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return Array.Empty<string>();
        }
            
        // NOTE: Performance is substantially faster if we slice the xml into
        // the chunks we are interested in via an XmlReader and then creating
        // individual XElement instances for each, than it is if we create a
        // single XDocument with the whole xml string
        var pluginElements = await ExtractPluginElements(xml);
        var xelements = pluginElements.Select(XElement.Parse);
        var names = GetDeviceNames(xelements);
        return names;
    }

    private static async Task<IEnumerable<string>> ExtractPluginElements(string xml)
    {
        var elements = new List<string>();
            
        using var sr = new StringReader(xml);
        using var reader = XmlReader.Create(sr, new XmlReaderSettings{ Async = true });
        while (await reader.ReadAsync())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "PluginDesc")
            {
                var element = await reader.ReadOuterXmlAsync();
                elements.Add(element);
            }
        }

        return elements;
    }

    private static IEnumerable<string> GetDeviceNames(IEnumerable<XElement> pluginDescElements)
    {
        var values = new HashSet<string>();

        foreach(var pluginDescElement in pluginDescElements)
        {
            var manufacturerElements = pluginDescElement.Descendants("Manufacturer");
            var manufacturerElement = manufacturerElements.FirstOrDefault();
            if (manufacturerElement != null)
            {
                var manufacturer = manufacturerElement.Attribute("Value")!.Value;
                var nameElements = manufacturerElement.Parent!.Elements("Name");
                var names = nameElements.Attributes("Value").Select(a => a.Value);

                var pluginDetails = names.Select(n => $"{manufacturer} {n}").ToHashSet();
                foreach (var pd in pluginDetails)
                {
                    values.Add(pd);
                }
                continue;
            }

            var plugNameElements = pluginDescElement.Descendants("PlugName");
            var plugNameElement = plugNameElements.FirstOrDefault();
            if (plugNameElement != null)
            {
                var name = plugNameElement.Attribute("Value")?.Value;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    values.Add(name);
                }
            }
        }

        return values;
    }
}