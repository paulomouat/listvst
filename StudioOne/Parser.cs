using System.Xml.Linq;

namespace ListVst.StudioOne
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
            return names.Distinct().OrderBy(n => n);
        }

        private string CleanUp(string xml)
        {
            xml = xml.Replace("::", "_");
            xml = xml.Replace("x:", "_");
            return xml;
        }

        private XDocument CreateDocument(string xml)
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

        private IEnumerable<string> GetDeviceNames(XDocument document)
        {
            var list = new List<string>();

            var attributesElements = document.Descendants("Attributes");
            //var containingDeviceData = attributesElements.Where(xe => xe.Attribute("_id")?.Value == "deviceData");
            var containingGhostData = attributesElements.Where(xe => xe.Attribute("_id")?.Value == "ghostData");
            foreach (var ghostData in containingGhostData)
            {
                var classInfoAttributes = ghostData.Descendants("Attributes").Where(xe => xe.Attribute("_id")?.Value == "classInfo");
                var withNameAttribute = classInfoAttributes.Attributes().Where(a => a.Name == "name");
                var values = withNameAttribute.Select(a => a.Value);
                list.AddRange(values);
            }            

            return list;
        }
    }
}
