using FluentAssertions;
using ListVst;
using Moq;
using Xunit;

namespace Common.Tests;

public class PluginRawDataExtensionsFixture
{
    /*[Fact]
    public void PluginInfo_IsSame_IfAliasNotRegistered()
    {
        var registryMock = new Mock<PluginRegistry> { CallBase = true };
        var registry = registryMock.Object;
        var pluginInfo = new PluginInfo("abc", "def", PluginType.Vst);
        var sut = new PluginRawData(pluginInfo, "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginInfo.Should().Be(pluginInfo);
    }
    
    [Fact]
    public void PluginInfo_IsAdjusted_IfAliasIsRegistered()
    {
        var registry = Mock.Of<IPluginRegistry>(m => m["fn"] == new PluginInfo("abc", "def", PluginType.Unknown));
        var sut = new PluginRawData(new PluginInfo("fn", "", PluginType.Vst), "pp");

        var actual = sut.ResolveAliases(registry);

        actual.PluginInfo.Should().Be(new PluginInfo("abc", "def", PluginType.Vst));
    }
    
    [Fact]
    public void PluginFullNames_AreChanged_IfAliasesAreRegistered()
    {
        var registry = Mock.Of<IPluginRegistry>(m =>
            m["fn1"] == new PluginInfo("abc", "def", PluginType.Unknown) &&
            m["fn2"] == new PluginInfo("def", "def", PluginType.Unknown) &&
            m["fn3"] == PluginInfo.NoPlugin);
        var sut = new[]
        {
            new PluginRawData("fn1", "pp1"),
            new PluginRawData("fn2", "pp2"),
            new PluginRawData("fn3", "pp3"),
        };

        var actual = sut.ResolveAliases(registry);

        actual.Should().BeEquivalentTo(new[]
        {
            new PluginRawData("def abc", "pp1"),
            new PluginRawData("def def", "pp2"),
            new PluginRawData("fn3", "pp3"),
        });
    }

    /*[Fact]
    public void PluginData_HasPluginDescriptor_WithTruncatedName_IfManufacturerFound()
    {
        var registry = Mock.Of<IPluginManufacturersRegistry>(m => m.GetManufacturer("abcdefg") == "ijk");
        var sut = new PluginRawData("abcdefg", "pp");

        var actual = sut.ToPluginData(registry);

        actual.PluginDescriptor.Name.Should().Be("defg");
    }
    
    [Fact]
    public void PluginData_HasPluginDescriptor_WithSameName_IfManufacturerIsEmptyString()
    {
        var registry = Mock.Of<IPluginManufacturersRegistry>(m => m.GetManufacturer("abcdefg") == "");
        var sut = new PluginRawData("abcdefg", "pp");

        var actual = sut.ToPluginData(registry);

        actual.PluginDescriptor.Name.Should().Be("abcdefg");
    }    
    
    [Fact]
    public void PluginData_HasExpectedPluginDescriptor()
    {
        var registry = Mock.Of<IPluginManufacturersRegistry>(m => m.GetManufacturer("abcdefg") == "ijk");
        var sut = new PluginRawData("abcdefg", "pp");

        var actual = sut.ToPluginData(registry);

        actual.PluginDescriptor.Should().BeEquivalentTo(new
        {
            Name = "defg",
            Manufacturer = "ijk",
            FullName = "abcdefg"
        });
    }

    [Fact]
    public void PluginData_HasExpectedProjectDescriptor()
    {
        var registry = Mock.Of<IPluginManufacturersRegistry>(m => m.GetManufacturer("abcdefg") == "ijk");
        var sut = new PluginRawData("abcdefg", "pp");

        var actual = sut.ToPluginData(registry);

        actual.ProjectDescriptor.Should().BeEquivalentTo(new
        {
            Path = "pp"
        });
    }

    [Fact]
    public void PluginRawDataElements_AreConverted_ToPluginDataElements()
    {
        var registry = Mock.Of<IPluginManufacturersRegistry>(m =>
            m.GetManufacturer("abcdefg1") == "ijk1" &&
            m.GetManufacturer("abcdefg2") == "ijk2" &&
            m.GetManufacturer("abcdefg3") == "");
        var sut = new[]
        {
            new PluginRawData("abcdefg1", "pp1"),
            new PluginRawData("abcdefg2", "pp2"),
            new PluginRawData("abcdefg3", "pp3"),
        };

        var actual = sut.ToPluginData(registry);

        actual.Should().BeEquivalentTo(new[]
        {
            new PluginData(new PluginDescriptor("efg1", "ijk1", "abcdefg1"), new ProjectDescriptor("pp1")),
            new PluginData(new PluginDescriptor("efg2", "ijk2", "abcdefg2"), new ProjectDescriptor("pp2")),
            new PluginData(new PluginDescriptor("abcdefg3", "", "abcdefg3"), new ProjectDescriptor("pp3")),
        });
    }*/
}