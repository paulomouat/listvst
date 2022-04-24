using FluentAssertions;
using ListVst;
using Moq;
using Xunit;

namespace Common.Tests;

public class PluginDescriptorExtensionsFixture
{
    [Fact]
    public void PluginDescriptor_IsSame_IfAliasNotRegistered()
    {
        var registryMock = new Mock<PluginRegistry> { CallBase = true };
        var registry = registryMock.Object;
        var sut = new PluginDescriptor("abc", "def", PluginType.Vst);

        var actual = sut.ResolveAliases(registry);

        var expected = new PluginDescriptor("abc", "def", PluginType.Vst);
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void Name_IsAdjusted_IfAliasIsRegistered()
    {
        var registry = Mock.Of<IPluginRegistry>(m => m["fn"] == new PluginDescriptor("abc", "def", PluginType.Unknown));
        var sut = new PluginDescriptor("fn", "", PluginType.Vst);

        var pi = sut.ResolveAliases(registry);
        var actual = pi.Name;

        actual.Should().Be("abc");
    }
    
    [Fact]
    public void Manufacturer_IsAdjusted_IfAliasIsRegistered()
    {
        var registry = Mock.Of<IPluginRegistry>(m => m["fn"] == new PluginDescriptor("abc", "def", PluginType.Unknown));
        var sut = new PluginDescriptor("fn", "", PluginType.Vst);

        var pi = sut.ResolveAliases(registry);
        var actual = pi.Manufacturer;

        actual.Should().Be("def");
    }
    
    [Fact]
    public void PluginType_IsTaken_FromCandidateInstance()
    {
        var expected = PluginType.Vst;
        
        var registry = Mock.Of<IPluginRegistry>(m => m["fn"] == new PluginDescriptor("abc", "def", PluginType.Unknown));
        var sut = new PluginDescriptor("fn", "", expected);

        var pi = sut.ResolveAliases(registry);

        var actual = pi.PluginType;
        
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void PluginDdescriptors_AreAdjusted_IfAliasesAreRegistered()
    {
        var p1 = new PluginDescriptor("abc", "def", PluginType.Unknown);
        var p2 = new PluginDescriptor("def", "def", PluginType.Unknown);
        var registry = Mock.Of<IPluginRegistry>(m =>
            m["fn1"] == p1 &&
            m["fn2"] == p2 &&
            m["fn3"] == PluginDescriptor.NoPlugin);
        var sut = new[]
        {
            new PluginDescriptor("fn1", "", PluginType.Vst),
            new PluginDescriptor("fn2", "", PluginType.Vst3),
            new PluginDescriptor("fn3", "", PluginType.AudioUnit),
        };

        var actual = sut.ResolveAliases(registry);

        actual.Should().BeEquivalentTo(new[]
        {
            new PluginDescriptor("abc", "def", PluginType.Vst),
            new PluginDescriptor("def","def", PluginType.Vst3),
            new PluginDescriptor("fn3", "", PluginType.AudioUnit),
        });
    }

    [Fact]
    public void ResolvingAlias_WhenCandidateInstance_HasManufacturerInName()
    {
        var p1 = new PluginDescriptor("abc", "def", PluginType.Unknown);
        var registry = Mock.Of<IPluginRegistry>(m =>
            m["def abc"] == p1);
        var sut = new PluginDescriptor("def abc", "", PluginType.Vst);

        var actual = sut.ResolveAliases(registry);

        actual.Should().Be(new PluginDescriptor("abc", "def", PluginType.Vst));
    }
}