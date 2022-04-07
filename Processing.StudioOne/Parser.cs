using System.Text;
using System.Xml.Linq;

namespace ListVst.Processing.StudioOne
{
    public class Parser : IParser
    {
        private XDocument? Document { get; set; }

        public async Task<IEnumerable<string>> Parse(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return Array.Empty<string>();
            }
            
            Document = CreateDocument(xml);
            
            var names = GetDeviceNames(Document);

            await Task.CompletedTask;
            
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
            var list = new HashSet<string>();

            var attributesElements = document.Descendants("Attributes");
            //var containingDeviceData = attributesElements.Where(xe => xe.Attribute("_id")?.Value == "deviceData");
            var containingGhostData = attributesElements.Where(xe => xe.Attribute("_id")?.Value == "ghostData");
            foreach (var ghostData in containingGhostData)
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
}
