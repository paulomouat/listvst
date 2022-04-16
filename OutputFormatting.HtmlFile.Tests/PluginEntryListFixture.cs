using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using ListVst;
using ListVst.OutputFormatting.HtmlFile;
using Moq;
using Xunit;

namespace OutputFormatting.HtmlFile.Tests;

public class PluginEntryListFixture
{
    [Fact]
    public void AddDescriptors_RendersAll()
    {
        var descriptors = new[]
        {
            new ProjectDescriptor("/root/sub1/file1.ext"),
            new ProjectDescriptor("/root/sub1/file2.ext")
        };

        var sut = GetSubject();
        var outputElement = new XElement("output");
        sut.AddItemsToEntry(descriptors, outputElement);

        outputElement.ToString().Should().Be(
@"<output>
  <div class=""item-container"">
    <div>root</div>
    <div class=""item"">
      <a href=""#root-sub1-file1-ext"">sub1 / file1.ext</a>
    </div>
    <div class=""item"">
      <a href=""#root-sub1-file2-ext"">sub1 / file2.ext</a>
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
            new { PluginDescriptor = plugins[0], ProjectDescriptor = projects[0] },
            new { PluginDescriptor = plugins[1], ProjectDescriptor = projects[0] },
            new { PluginDescriptor = plugins[0], ProjectDescriptor = projects[1] },
        };
        
        var lookup = mapping.ToLookup(p => p.PluginDescriptor, p => p.ProjectDescriptor);

        var sut = GetSubject();
        sut.AddFromLookup(lookup);

        sut.ToString().Should().Be(
@"<div id=""mockId"">
  <div id=""manufacturer1-plugin11"" class=""entry"">
    <div class=""key title"">manufacturer1 plugin11</div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
    <div class=""item-container"">
      <div>root</div>
      <div class=""item"">
        <a href=""#root-sub1-file1-ext"">sub1 / file1.ext</a>
      </div>
      <div class=""item"">
        <a href=""#root-sub1-file2-ext"">sub1 / file2.ext</a>
      </div>
    </div>
  </div>
  <div id=""manufacturer1-plugin12"" class=""entry"">
    <div class=""key title"">manufacturer1 plugin12</div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
    <div class=""item-container"">
      <div>root</div>
      <div class=""item"">
        <a href=""#root-sub1-file1-ext"">sub1 / file1.ext</a>
      </div>
    </div>
  </div>
</div>");
    }

    private PluginEntryList GetSubject()
    {
        var sectionMock = Mock.Of<ISection>();
        var sut = new PluginEntryList("mockId", sectionMock);
        return sut;
    }
}