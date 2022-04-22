using FluentAssertions;
using ListVst;
using Moq;
using Xunit;

namespace Common.Tests;

public class PluginRawDataExtensionsFixture
{
    [Fact]
    public void PluginFullName_IsSame_IfAliasNotRegistered()
    {
        var registry = Mock.Of<IPluginAliasesRegistry>(m => m["dummy"] == "abc");
        var sut = new PluginRawData("fn", "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginFullName.Should().Be("fn");
    }

    [Fact]
    public void PluginFullName_IsSame_IfAliasRegistryReturnsNull()
    {
        var registry = Mock.Of<IPluginAliasesRegistry>(m => m["fn"] == null);
        var sut = new PluginRawData("fn", "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginFullName.Should().Be("fn");
    }

    [Fact]
    public void PluginFullName_IsSame_IfAliasRegistryReturnsEmptyString()
    {
        var registry = Mock.Of<IPluginAliasesRegistry>(m => m["fn"] == "");
        var sut = new PluginRawData("fn", "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginFullName.Should().Be("fn");
    }
    
    [Fact]
    public void PluginFullName_IsChanged_IfAliasIsRegistered()
    {
        var registry = Mock.Of<IPluginAliasesRegistry>(m => m["fn"] == "abc");
        var sut = new PluginRawData("fn", "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginFullName.Should().Be("abc");
    }
    
    [Fact]
    public void PluginFullNames_AreChanged_IfAliasesAreRegistered()
    {
        var registry = Mock.Of<IPluginAliasesRegistry>(m => m["fn1"] == "abc" && m["fn2"] == "def");
        var sut = new[]
        {
            new PluginRawData("fn1", "pp1"),
            new PluginRawData("fn2", "pp2"),
            new PluginRawData("fn3", "pp3"),
        };

        var actual = sut.ResolveAliases(registry);

        actual.Should().BeEquivalentTo(new[]
        {
            new PluginRawData("abc", "pp1"),
            new PluginRawData("def", "pp2"),
            new PluginRawData("fn3", "pp3"),
        });
    }
}