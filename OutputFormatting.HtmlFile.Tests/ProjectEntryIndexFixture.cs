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
    public void AddLookup_RendersAll()
    {
        var projects = new[]
        {
            new ProjectDescriptor("/root/sub1/file1.ext"),
            new ProjectDescriptor("/root/sub1/file2.ext")
        };

        var plugins = new[]
        {
            new PluginDescriptor("plugin11", "manufacturer1", PluginType.AudioUnit),
            new PluginDescriptor("plugin12", "manufacturer1", PluginType.Vst3),
        };

        var records = new[]
        {
            new PluginRecord(plugins[0], projects[0]),
            new PluginRecord(plugins[1], projects[0]),
            new PluginRecord(plugins[0], projects[1])
        };
        
        var sut = GetSubject();
        sut.AddPluginRecords(records);

        sut.Should().Be(XElement.Parse(
            @"<div id=""mockId"">
  <div class=""index title"">mockTitle</div>
  <a class=""link-to-top"" href=""#document-title"">top</a>
  <div class=""stats"">Number of entries: 2</div>
  <div class=""index-items"">
    <div class=""item-container"">
      <div class=""item-container-title"">root</div>
      <div class=""item"">
        <a href=""#root-sub1-file1-ext"">sub1 / file1.ext</a>
        <span class=""plugintype"">AU</span>
        <span class=""plugintype"">VST3</span>
      </div>
      <div class=""item"">
        <a href=""#root-sub1-file2-ext"">sub1 / file2.ext</a>
        <span class=""plugintype"">AU</span>
      </div>
    </div>
  </div>
</div>"));
    }
    
    private ProjectEntryIndex GetSubject()
    {
        var sectionMock = Mock.Of<ISection>();
        var sut = new ProjectEntryIndex("mockId", "mockTitle", sectionMock);
        return sut;
    }
}