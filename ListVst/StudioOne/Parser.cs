using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ListVst.StudioOne
{
    public class Parser
    {
        private XDocument Document { get; set; }

        public IEnumerable<string> Parse(string xml)
        {
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
            var attributesElements = document.Descendants("Attributes");
            var containingDeviceData = attributesElements.Where(xe => xe.Attribute("_id")?.Value == "deviceData");
            var withNameAttribute = containingDeviceData.Attributes().Where(a => a.Name == "name");
            var values = withNameAttribute.Select(a => a.Value);
            return values;
        }
    }
}
