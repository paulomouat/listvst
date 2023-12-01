using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using ListVst;
using ListVst.Processing.StudioOne;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Processing.StudioOne.Tests
{
    public class ParserFixture
    {
        private const string TestFile = "version-5-5.xml";

        [Fact]
        public async Task PluginDescriptors_AreExtracted_FromWellFormedFile()
        {
            var sut = GetSubject();

            var xml = GetTestXml();
            
            var parsed = await sut.Parse(xml);

            parsed.Should().BeEquivalentTo(new[]
            {
                new PluginDescriptor("Cinematic Rooms", "(n/a)", PluginType.AudioUnit),
                new PluginDescriptor("2C-Vector", "(n/a)", PluginType.Vst),
                new PluginDescriptor("Cinematic Rooms", "(n/a)", PluginType.Vst3),
            });
        }

        private static Parser GetSubject()
        {
            var logger = Mock.Of<ILogger>();
            var parser = new Parser(logger);
            return parser;
        }
        
        private static string GetTestXml()
        {
            var xml = File.ReadAllText(TestFile);
            return xml;
        }
    }
}
