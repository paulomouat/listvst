using System.Text;
using System.Xml.Linq;

namespace ListVst.Processing.AbletonLive
{
    public class Parser : IParser
    {
        private XDocument? Document { get; set; }

        public IEnumerable<string> Parse(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return Array.Empty<string>();
            }

            Document = CreateDocument(xml);
            var names = GetDeviceNames(Document);
            return names;
        }

        private static string CleanUp(string xml)
        {
            var builder = new StringBuilder(xml);
            
            builder.Replace("::", "_")
                .Replace("x:", "_");
            
            return builder.ToString();
        }

        private static XDocument CreateDocument(string xml)
        {
            xml = CleanUp(xml);

            //var nsm = new XmlNamespaceManager(new NameTable());
            //nsm.AddNamespace("x", "urn:ignore");

            //var ctx = new XmlParserContext(null, nsm, null, XmlSpace.Preserve);
            //var reader = new XmlTextReader(xml, XmlNodeType.Element, ctx);
            //var parsed = XDocument.Load(reader);
            var parsed = XDocument.Parse(xml);
            return parsed;
        }

        private static IEnumerable<string> GetDeviceNames(XDocument document)
        {
            var values = new HashSet<string>();

            var pluginDescElements = document.Descendants("PluginDesc");
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
}
