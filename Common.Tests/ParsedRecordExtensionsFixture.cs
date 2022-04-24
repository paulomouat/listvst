using FluentAssertions;
using ListVst;
using Moq;
using Xunit;

namespace Common.Tests;

public class ParsedRecordExtensionsFixture
{
    [Fact]
    public void PluginDescriptor_IsSame_IfAliasNotRegistered()
    {
        var registryMock = new Mock<PluginRegistry> { CallBase = true };
        var registry = registryMock.Object;
        var pluginDescriptor = new PluginDescriptor("abc", "def", PluginType.Vst);
        var sut = new ParsedRecord(pluginDescriptor, "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginDescriptor.Should().Be(pluginDescriptor);
    }

    [Fact]
    public void PluginDescriptor_IsSame_IfRegistryReturns_NoPlugin()
    {
        var registryMock = GetRegistryMock();
        var registry = registryMock.Object;
        var pluginDescriptor = new PluginDescriptor("abc", "def", PluginType.Vst);
        var sut = new ParsedRecord(pluginDescriptor, "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginDescriptor.Should().Be(pluginDescriptor);
    }

    [Fact]
    public void PluginDescriptor_IsAdjusted_IfAliasIsRegistered_AndMatchesName()
    {
        var registryMock = GetRegistryMock();
        registryMock.SetupGet(m => m["fn"]).Returns(new PluginDescriptor("abc", "def", PluginType.Unknown));
        var registry = registryMock.Object;
        var sut = new ParsedRecord(new PluginDescriptor("fn", "", PluginType.Vst), "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginDescriptor.Should().Be(new PluginDescriptor("abc", "def", PluginType.Vst));
    }

    [Fact]
    public void PluginDescriptor_IsAdjusted_IfAliasIsRegistered_AndMatchesManufacturerAndName()
    {
        var registryMock = GetRegistryMock();
        registryMock.SetupGet(m => m["mn fn"]).Returns(new PluginDescriptor("abc", "def", PluginType.Unknown));
        var registry = registryMock.Object;
       
        var sut = new ParsedRecord(new PluginDescriptor("fn", "mn", PluginType.Vst), "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginDescriptor.Should().Be(new PluginDescriptor("abc", "def", PluginType.Vst));
    }
    
    [Fact]
    public void PluginDescriptors_AreChanged_IfAliasesAreRegistered()
    {
        var registryMock = GetRegistryMock();
        registryMock.SetupGet(m => m["fn1"]).Returns(new PluginDescriptor("abc", "def", PluginType.Unknown));
        registryMock.SetupGet(m => m["fn2"]).Returns(new PluginDescriptor("def", "def", PluginType.Unknown));
        var registry = registryMock.Object;
        var sut = new[]
        {
            new ParsedRecord(new PluginDescriptor("fn1", "ghi", PluginType.Vst), "pp1"),
            new ParsedRecord(new PluginDescriptor("fn2", "jkl", PluginType.Vst3), "pp2"),
            new ParsedRecord(new PluginDescriptor("fn3", "mno", PluginType.AudioUnit), "pp3"),
        };

        var actual = sut.ResolveAliases(registry);

        actual.Should().BeEquivalentTo(new[]
        {
            new ParsedRecord(new PluginDescriptor("abc", "def", PluginType.Vst), "pp1"),
            new ParsedRecord(new PluginDescriptor("def", "def", PluginType.Vst3), "pp2"),
            new ParsedRecord(new PluginDescriptor("fn3", "mno", PluginType.AudioUnit), "pp3"),
        });
    }
    
    [Fact]
    public void PluginRecord_HasExpectedProjectDescriptor()
    {
        var sut = new ParsedRecord(new PluginDescriptor("abc", "def", PluginType.Unknown), "pp");

        var actual = sut.ToPluginRecord();

        actual.ProjectDescriptor.Should().BeEquivalentTo(new
        {
            Path = "pp"
        });
    }

    [Fact]
    public void ParsedRecords_AreConverted_ToPluginRecordElements()
    {
        var sut = new[]
        {
            new ParsedRecord(new PluginDescriptor("abc1","def1", PluginType.Unknown), "pp1"),
            new ParsedRecord(new PluginDescriptor("abc2","def2", PluginType.Unknown), "pp2"),
            new ParsedRecord(new PluginDescriptor("abc3","def3", PluginType.Unknown), "pp3"),
        };

        var actual = sut.ToPluginRecord();

        actual.Should().BeEquivalentTo(new[]
        {
            //new PluginRecord(new PluginDescriptor("abc1", "def1", "def1 abc1"), new ProjectDescriptor("pp1")),
            //new PluginRecord(new PluginDescriptor("abc2", "def2", "def2 abc2"), new ProjectDescriptor("pp2")),
            //new PluginRecord(new PluginDescriptor("abc3", "def3", "def3 abc3"), new ProjectDescriptor("pp3")),
            new PluginRecord(new PluginDescriptor("abc1", "def1", PluginType.Unknown), new ProjectDescriptor("pp1")),
            new PluginRecord(new PluginDescriptor("abc2", "def2", PluginType.Unknown), new ProjectDescriptor("pp2")),
            new PluginRecord(new PluginDescriptor("abc3", "def3", PluginType.Unknown), new ProjectDescriptor("pp3")),
        });
    }

    private static Mock<IPluginRegistry> GetRegistryMock()
    {
        var registryMock = new Mock<IPluginRegistry>();
        registryMock.SetupGet(m => m[It.IsAny<string>()]).Returns(PluginDescriptor.NoPlugin);
        return registryMock;
    }
}