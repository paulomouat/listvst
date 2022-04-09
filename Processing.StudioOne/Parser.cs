using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing.StudioOne;

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
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes" && reader.GetAttribute("_id") == "ghostData")
            {
                var element = await reader.ReadOuterXmlAsync();
                elements.Add(element);
            }
        }

        return elements;
    }
        
    private static IEnumerable<string> GetDeviceNames(IEnumerable<XElement> pluginElements)
    {
        var list = new HashSet<string>();

        foreach (var ghostData in pluginElements)
        {
            var classInfoAttributes = ghostData.Descendants("Attributes").Where(xe => xe.Attribute("_id")?.Value == "classInfo");
            var withNameAttribute = classInfoAttributes.Attributes().Where(a => a.Name == "name");
            var values = withNameAttribute.Select(a => a.Value).ToHashSet();
            foreach (var value in values)
            {
                list.Add(value);
            }
        }            

        return list;
    }        
}