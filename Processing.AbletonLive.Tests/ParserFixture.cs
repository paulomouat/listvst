using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using ListVst;
using ListVst.Processing.AbletonLive;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Processing.AbletonLive.Tests
{
    public class ParserFixture
    {
        private const string TestFile = "version-11-1-1.xml";
        
        [Fact]
        public async Task PluginDescriptors_AreExtracted_FromWellFormedFile()
        {
            var sut = GetSubject();

            var xml = GetTestXml().ToString();

            var parsed = await sut.Parse(xml);

            parsed.Should().BeEquivalentTo(new[]
            {
                new PluginDescriptor("Cinematic Rooms", "LiquidSonics", PluginType.AudioUnit),
                new PluginDescriptor("Cinematic Rooms", "", PluginType.Vst),
                new PluginDescriptor("Cinematic Rooms", "", PluginType.Vst3),
                new PluginDescriptor("Satson Buss", "Sonimus", PluginType.AudioUnit)
            });
        }

        private static Parser GetSubject()
        {
            var logger = Mock.Of<ILogger>();
            var parser = new Parser(logger);
            return parser;
        }
        
        private static XDocument GetTestXml()
        {
            var xdoc = XDocument.Load(TestFile);
            return xdoc;
        }
    }
}
