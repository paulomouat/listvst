using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using ListVst;
using ListVst.OutputFormatting.HtmlFile;
using Moq;
using Xunit;

namespace OutputFormatting.HtmlFile.Tests;

public class ProjectEntryIndexFixture
{
    [Fact]
    public void RendersAllDescriptors()
    {
        var descriptor = new PluginDescriptor("plugin11", "manufacturer1", "manufacturer1 plugin11");

        var sut = GetSubject();
        var outputElement = new XElement("output");
        sut.AddItemToEntry(descriptor, outputElement);

        outputElement.ToString().Should().Be(
@"<output>
  <a href=""#manufacturer1-plugin11"">manufacturer1 plugin11</a>
</output>"
        );
    }

    [Fact]
    public void AddLookup_RendersAll()
    {
        var projects = new[]
        {
            new ProjectDescriptor("/root/sub1/file1.ext"),
            new ProjectDescriptor("/root/sub1/file2.ext")
        };

        var plugins = new[]
        {
            new PluginDescriptor("plugin11", "manufacturer1", "manufacturer1 plugin11"),
            new PluginDescriptor("plugin12", "manufacturer1", "manufacturer1 plugin12"),
        };

        var mapping = new[]
        {
            new { ProjectDescriptor = projects[0], PluginDescriptor = plugins[0] },
            new { ProjectDescriptor = projects[0], PluginDescriptor = plugins[1] },
            new { ProjectDescriptor = projects[1], PluginDescriptor = plugins[0] },
        };
        
        var lookup = mapping.ToLookup(p => p.ProjectDescriptor, p => p.PluginDescriptor);

        var sut = GetSubject();
        sut.AddFromLookup(lookup);

        sut.ToString().Should().Be(
            @"<div id=""mockId"">
  <div class=""index title"">mockTitle</div>
  <a class=""link-to-top"" href=""#document-title"">top</a>
  <div class=""stats"">Number of entries: 2</div>
  <div class=""item"">
    <a href=""#root-sub1-file1-ext"">/root/sub1/file1.ext</a>
  </div>
  <div class=""item"">
    <a href=""#root-sub1-file2-ext"">/root/sub1/file2.ext</a>
  </div>
</div>");
    }
    
    private ProjectEntryIndex GetSubject()
    {
        var sectionMock = Mock.Of<ISection>();
        var sut = new ProjectEntryIndex("mockId", "mockTitle", sectionMock);
        return sut;
    }
}