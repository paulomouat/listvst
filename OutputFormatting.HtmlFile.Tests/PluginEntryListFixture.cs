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
        var pairs = new[]
        {
            new
            {
              PluginDescriptor = new PluginDescriptor("n1", "m1", PluginType.Vst),
              ProjectDescriptor = new ProjectDescriptor("/root/sub1/file1.ext")
            },
            new
            {
              PluginDescriptor = new PluginDescriptor("n1", "m1", PluginType.Vst),
              ProjectDescriptor = new ProjectDescriptor("/root/sub1/file2.ext")
            }
        };

        var descriptors = pairs
          .ToLookup(p => p.PluginDescriptor, p => p.ProjectDescriptor);
        
        var sut = GetSubject();
        var outputElement = new XElement("output");
        sut.AddItemsToEntry(descriptors, outputElement);

        outputElement.Should().BeEquivalentTo(XElement.Parse(
@"<output>
  <div class=""item-container"">
    <div class=""item-container-title"">root</div>
    <div class=""item"">
      <a href=""#root-sub1-file1-ext"">sub1 / file1.ext</a>
      <span class=""plugintype"">VST</span>
    </div>
    <div class=""item"">
      <a href=""#root-sub1-file2-ext"">sub1 / file2.ext</a>
      <span class=""plugintype"">VST</span>
    </div>
  </div>
</output>"
        ));
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
            new PluginDescriptor("plugin11", "manufacturer1", PluginType.AudioUnit),
            new PluginDescriptor("plugin12", "manufacturer1", PluginType.Vst),
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

        sut.Should().BeEquivalentTo(XElement.Parse(
@"<div id=""mockId"">
  <div id=""manufacturer1-plugin11"" class=""entry"">
    <div>
      <div>manufacturer1</div>
      <div class=""key title"">plugin11</div>
    </div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
    <div class=""item-container"">
      <div class=""item-container-title"">root</div>
      <div class=""item"">
        <a href=""#root-sub1-file1-ext"">sub1 / file1.ext</a>
        <span class=""plugintype"">AU</span>
      </div>
      <div class=""item"">
        <a href=""#root-sub1-file2-ext"">sub1 / file2.ext</a>
        <span class=""plugintype"">AU</span>
      </div>
    </div>
  </div>
  <div id=""manufacturer1-plugin12"" class=""entry"">
    <div>
      <div>manufacturer1</div>
      <div class=""key title"">plugin12</div>
    </div>
    <a class=""link-to-top"" href=""#document-title"">top</a>
    <a class=""link-to-section"" href=""#"">section index</a>
    <div class=""item-container"">
      <div class=""item-container-title"">root</div>
      <div class=""item"">
        <a href=""#root-sub1-file1-ext"">sub1 / file1.ext</a>
        <span class=""plugintype"">VST</span>
      </div>
    </div>
  </div>
</div>"));
    }

    private PluginEntryList GetSubject()
    {
        var sectionMock = Mock.Of<ISection>();
        var sut = new PluginEntryList("mockId", sectionMock);
        return sut;
    }
}