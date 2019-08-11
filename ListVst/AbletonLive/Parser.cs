using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ListVst.AbletonLive
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
            var pluginDescElements = document.Descendants("PluginDesc");
            var manufacturerElements = pluginDescElements.Descendants("Manufacturer");
            var nameElements = manufacturerElements.Select(m => m.Parent).Elements("Name");
            var values = nameElements.Attributes("Value").Select(a => a.Value);
            return values;
        }
    }
}
