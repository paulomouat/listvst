using System.Runtime.InteropServices.ComTypes;
using FluentAssertions;
using ListVst;
using Moq;
using Xunit;

namespace Common.Tests;

public class PluginInfoExtensionsFixture
{
    [Fact]
    public void PluginInfo_IsSame_IfAliasNotRegistered()
    {
        var registryMock = new Mock<PluginRegistry> { CallBase = true };
        var registry = registryMock.Object;
        var sut = new PluginInfo("abc", "def", PluginType.Vst);

        var actual = sut.ResolveAliases(registry);

        var expected = new PluginInfo("abc", "def", PluginType.Vst);
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void Name_IsAdjusted_IfAliasIsRegistered()
    {
        var registry = Mock.Of<IPluginRegistry>(m => m["fn"] == new PluginInfo("abc", "def", PluginType.Unknown));
        var sut = new PluginInfo("fn", "", PluginType.Vst);

        var pi = sut.ResolveAliases(registry);
        var actual = pi.Name;

        actual.Should().Be("abc");
    }
    
    [Fact]
    public void Manufacturer_IsAdjusted_IfAliasIsRegistered()
    {
        var registry = Mock.Of<IPluginRegistry>(m => m["fn"] == new PluginInfo("abc", "def", PluginType.Unknown));
        var sut = new PluginInfo("fn", "", PluginType.Vst);

        var pi = sut.ResolveAliases(registry);
        var actual = pi.Manufacturer;

        actual.Should().Be("def");
    }
    
    [Fact]
    public void PluginType_IsTaken_FromCandidateInstance()
    {
        var expected = PluginType.Vst;
        
        var registry = Mock.Of<IPluginRegistry>(m => m["fn"] == new PluginInfo("abc", "def", PluginType.Unknown));
        var sut = new PluginInfo("fn", "", expected);

        var pi = sut.ResolveAliases(registry);

        var actual = pi.PluginType;
        
        actual.Should().Be(expected);
    }
    
    [Fact]
    public void PluginInfos_AreAdjusted_IfAliasesAreRegistered()
    {
        var p1 = new PluginInfo("abc", "def", PluginType.Unknown);
        var p2 = new PluginInfo("def", "def", PluginType.Unknown);
        var registry = Mock.Of<IPluginRegistry>(m =>
            m["fn1"] == p1 &&
            m["fn2"] == p2 &&
            m["fn3"] == PluginInfo.NoPlugin);
        var sut = new[]
        {
            new PluginInfo("fn1", "", PluginType.Vst),
            new PluginInfo("fn2", "", PluginType.Vst3),
            new PluginInfo("fn3", "", PluginType.AudioUnit),
        };

        var actual = sut.ResolveAliases(registry);

        actual.Should().BeEquivalentTo(new[]
        {
            new PluginInfo("abc", "def", PluginType.Vst),
            new PluginInfo("def","def", PluginType.Vst3),
            new PluginInfo("fn3", "", PluginType.AudioUnit),
        });
    }

    [Fact]
    public void ResolvingAlias_WhenCandidateInstance_HasManufacturerInName()
    {
        var p1 = new PluginInfo("abc", "def", PluginType.Unknown);
        var registry = Mock.Of<IPluginRegistry>(m =>
            m["def abc"] == p1);
        var sut = new PluginInfo("def abc", "", PluginType.Vst);

        var actual = sut.ResolveAliases(registry);

        actual.Should().Be(new PluginInfo("abc", "def", PluginType.Vst));
    }
}