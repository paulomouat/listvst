using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using ListVst;
using ListVst.OutputFormatting.HtmlFile;
using Moq;
using Xunit;

namespace OutputFormatting.HtmlFile.Tests;

public class ProjectEntryListFixture
{
    [Fact]
    public void RendersAllDescriptors()
    {
        var descriptors = new[]
        {
            new PluginDescriptor("plugin11", "manufacturer1", "manufacturer1 plugin11"),
            new PluginDescriptor("plugin21", "manufacturer2", "manufacturer2 plugin21"),
        };

        var sut = GetSubject();
        var outputElement = new XElement("output");
        sut.AddItemsToEntry(descriptors, outputElement);

        outputElement.ToString().Should().Be(
@"<output>
  <div class=""item-container"">
    <div class=""item-container-title"">manufacturer1</div>
    <div class=""item"">
      <a href=""#manufacturer1-plugin11"">plugin11</a>
    </div>
  </div>
  <div class=""item-container"">
    <div class=""item-container-title"">manufacturer2</div>
    <div class=""item"">
      <a href=""#manufacturer2-plugin21"">plugin21</a>
    </div>
  </div>
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
  <div id=""root-sub1-file1-ext"" class=""entry"">
    <div class=""key title"">/root/sub1/file1.ext</div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
    <div class=""item-container"">
      <div class=""item-container-title"">manufacturer1</div>
      <div class=""item"">
        <a href=""#manufacturer1-plugin11"">plugin11</a>
      </div>
      <div class=""item"">
        <a href=""#manufacturer1-plugin12"">plugin12</a>
      </div>
    </div>
  </div>
  <div id=""root-sub1-file2-ext"" class=""entry"">
    <div class=""key title"">/root/sub1/file2.ext</div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
    <div class=""item-container"">
      <div class=""item-container-title"">manufacturer1</div>
      <div class=""item"">
        <a href=""#manufacturer1-plugin11"">plugin11</a>
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