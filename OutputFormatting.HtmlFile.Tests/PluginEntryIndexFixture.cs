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
            new PluginDescriptor("plugin11", "manufacturer1", PluginType.Unknown),
            new PluginDescriptor("plugin12", "manufacturer1", PluginType.Unknown),
        };

        var records = new[]
        {
            new PluginRecord(plugins[0], projects[0]),
            new PluginRecord(plugins[1], projects[0]),
            new PluginRecord(plugins[0], projects[1])
        };
        
        var sut = GetSubject();
        sut.AddPluginRecords(records);

        sut.ToString().Should().Be(
@"<div id=""mockId"">
  <div class=""index title"">mockTitle</div>
  <a class=""link-to-top"" href=""#document-title"">top</a>
  <div class=""stats"">Number of entries: 2</div>
  <div class=""index-items"">
    <div class=""item-container"">
      <div class=""item-container-title"">manufacturer1</div>
      <div class=""item"">
        <a href=""#manufacturer1-plugin11"">plugin11</a>
        <span class=""plugintype"">?</span>
      </div>
      <div class=""item"">
        <a href=""#manufacturer1-plugin12"">plugin12</a>
        <span class=""plugintype"">?</span>
      </div>
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