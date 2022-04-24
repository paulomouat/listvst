using FluentAssertions;
using ListVst;
using Xunit;

namespace Common.Tests;

public class PluginRegistryFixture
{
    [Fact]
    public void GetByAlias_WithExistingAlias_ReturnsCorrespondingPluginDescriptor()
    {
        var sut = new PluginRegistry();
        
        sut.Register("n1", "m1", "a1");

        var actual = sut["a1"];
        
        actual.Should().Be(new PluginDescriptor("n1", "m1", PluginType.Unknown));
    }

    [Fact]
    public void GetByAlias_WithNonExistingAlias_ReturnsCorrespondingPluginDescriptor()
    {
        var sut = new PluginRegistry();
        
        sut.Register("n1", "m1", "a1");

        var actual = sut["a2"];
        
        actual.Should().Be(PluginDescriptor.NoPlugin);
    }

    [Fact]
    public void RegisteringMultipleAliases_ReturnsCorrespondingPluginDescriptors()
    {
        var sut = new PluginRegistry();
        
        sut.Register("n1", "m1", new[] { "a1", "a2", "a3" });

        var actual1 = sut["a1"];
        var actual2 = sut["a2"];
        var actual3 = sut["a3"];
        
        actual1.Should().Be(new PluginDescriptor("n1", "m1", PluginType.Unknown));
        actual2.Should().Be(new PluginDescriptor("n1", "m1", PluginType.Unknown));
        actual3.Should().Be(new PluginDescriptor("n1", "m1", PluginType.Unknown));
    }
}