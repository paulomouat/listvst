using System.Linq;
using FluentAssertions;
using ListVst;
using ListVst.OutputFormatting.HtmlFile;
using Moq;
using Xunit;

namespace OutputFormatting.HtmlFile.Tests;

public class PluginEntryIndexFixture
{
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
            new { PluginDescriptor = plugins[0], ProjectDescriptor = projects[0] },
            new { PluginDescriptor = plugins[1], ProjectDescriptor = projects[0] },
            new { PluginDescriptor = plugins[0], ProjectDescriptor = projects[1] },
        };
        
        var lookup = mapping.ToLookup(p => p.PluginDescriptor, p => p.ProjectDescriptor);

        var sut = GetSubject();
        sut.AddFromLookup(lookup);

        sut.ToString().Should().Be(
@"<div id=""mockId"">
  <div class=""index title"">mockTitle</div>
  <a class=""link-to-top"" href=""#document-title"">top</a>
  <div class=""stats"">Number of entries: 2</div>
  <div class=""item-container"">
    <div class=""item-container-title"">manufacturer1</div>
    <div class=""item"">
      <a href=""#manufacturer1-plugin11"">plugin11</a>
    </div>
    <div class=""item"">
      <a href=""#manufacturer1-plugin12"">plugin12</a>
    </div>
  </div>
</div>");
    }

    private PluginEntryIndex GetSubject()
    {
        var sectionMock = Mock.Of<ISection>();
        var sut = new PluginEntryIndex("mockId", "mockTitle", sectionMock);
        return sut;
    }
}