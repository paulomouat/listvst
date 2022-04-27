using System.Linq;
using FluentAssertions;
using ListVst;
using ListVst.OutputFormatting.HtmlFile;
using Moq;
using Xunit;

namespace OutputFormatting.HtmlFile.Tests;

public class ProjectEntryListFixture
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
  <div id=""root-sub1-file1-ext"" class=""entry"">
    <div>
      <div class=""key title"">root</div>
      <div>sub1 / file1.ext</div>
    </div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
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
  <div id=""root-sub1-file2-ext"" class=""entry"">
    <div>
      <div class=""key title"">root</div>
      <div>sub1 / file2.ext</div>
    </div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
    <div class=""item-container"">
      <div class=""item-container-title"">manufacturer1</div>
      <div class=""item"">
        <a href=""#manufacturer1-plugin11"">plugin11</a>
        <span class=""plugintype"">?</span>
      </div>
    </div>
  </div>
</div>");
    }
    
    private ProjectEntryList GetSubject()
    {
        var sectionMock = Mock.Of<ISection>();
        var sut = new ProjectEntryList("mockId", sectionMock);
        return sut;
    }
}