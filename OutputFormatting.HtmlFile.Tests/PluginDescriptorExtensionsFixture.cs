using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using ListVst;
using ListVst.OutputFormatting.HtmlFile;
using Xunit;

namespace OutputFormatting.HtmlFile.Tests;

public class PluginDescriptorExtensionsFixture
{
    [Fact]
    public void Descriptors_ForTheSamePlugin_WithDifferentTypes_AreConsolidated()
    {
        var pd1 = new PluginDescriptor("n11", "m1", PluginType.Vst);
        var pd2 = new PluginDescriptor("n11", "m1", PluginType.AudioUnit);

        var descriptors = new[] { pd1, pd2 };

        var actual = descriptors.ToXElements();

        actual.Should().BeEquivalentTo(new[]
        {
            XElement.Parse(
                @"<div class=""item-container"">
                    <div class=""item-container-title"">m1</div>
                    <div class=""item"">
                        <a href=""#m1-n11"">n11</a>
                        <span class=""plugintype"">AU</span>
                        <span class=""plugintype"">VST</span>
                    </div>
                </div>")
        });
    }

    [Fact]
    public void Descriptors_ForTheSamePlugin_WithSameType_AreUnique()
    {
        var pd1 = new PluginDescriptor("n11", "m1", PluginType.Vst);
        var pd2 = new PluginDescriptor("n11", "m1", PluginType.Vst);

        var descriptors = new[] { pd1, pd2 };

        var actual = descriptors.ToXElements();

        actual.Should().BeEquivalentTo(new[]
        {
            XElement.Parse(
                @"<div class=""item-container"">
                    <div class=""item-container-title"">m1</div>
                    <div class=""item"">
                        <a href=""#m1-n11"">n11</a>
                        <span class=""plugintype"">VST</span>
                    </div>
                </div>")
        });
    }
    
    [Fact]
    public void Descriptors_ForDifferentPlugins_FromTheSameManufacturer_AreAdded_InTheSameContainer()
    {
        var pd1 = new PluginDescriptor("n11", "m1", PluginType.Vst);
        var pd2 = new PluginDescriptor("n12", "m1", PluginType.AudioUnit);

        var descriptors = new[] { pd1, pd2 };

        var actual = descriptors.ToXElements();

        actual.Should().BeEquivalentTo(new[]
        {
            XElement.Parse(
                @"<div class=""item-container"">
                    <div class=""item-container-title"">m1</div>
                    <div class=""item"">
                        <a href=""#m1-n11"">n11</a>
                        <span class=""plugintype"">VST</span>
                    </div>
                    <div class=""item"">
                        <a href=""#m1-n12"">n12</a>
                        <span class=""plugintype"">AU</span>
                    </div>
                </div>")
        });
    }

    [Fact]
    public void Descriptors_ForPlugins_FromDifferentManufacturer_AreAdded_InSeparateContainers()
    {
        var pd1 = new PluginDescriptor("n11", "m1", PluginType.Vst);
        var pd2 = new PluginDescriptor("n21", "m2", PluginType.AudioUnit);

        var descriptors = new[] { pd1, pd2 };

        var actual = descriptors.ToXElements();

        actual.Should().BeEquivalentTo(new[]
        {
            XElement.Parse(
                @"<div class=""item-container"">
                    <div class=""item-container-title"">m1</div>
                    <div class=""item"">
                        <a href=""#m1-n11"">n11</a>
                        <span class=""plugintype"">VST</span>
                    </div>
                </div>"),
            XElement.Parse(
                @"<div class=""item-container"">
                    <div class=""item-container-title"">m2</div>
                    <div class=""item"">
                        <a href=""#m2-n21"">n21</a>
                        <span class=""plugintype"">AU</span>
                    </div>
                </div>")
        });
    }
}