using FluentAssertions;
using ListVst.OutputFormatting.HtmlFile;
using Xunit;

namespace OutputFormatting.HtmlFile.Tests;

public class IdFixture
{
    [Theory]
    [InlineData("", "")]
    [InlineData("abc", "abc")]
    [InlineData("-abc", "abc")]
    [InlineData("abc-def", "abc-def")]
    [InlineData("abc def", "abc-def")]
    [InlineData("abc/def", "abc-def")]
    [InlineData("abc.def", "abc-def")]
    [InlineData("abc'def", "abc-def")]
    [InlineData("abc;def", "abc-def")]
    [InlineData("abc:def", "abc-def")]
    [InlineData("ab cd/ef.gh'ij;kl:mn", "ab-cd-ef-gh-ij-kl-mn")]
    public void Init_EscapesSpecialCharacters(string sourceValue, string expected)
    {
        var id = new Id(sourceValue);

        var actual = id.Value;

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void ElidesDashInFirstPosition()
    {
        var id = new Id("/some-value");

        var actual = id.Value;

        actual.Should().Be("some-value");
    }
    
    [Fact]
    public void SupportsImplicitConversionToString()
    {
        var id = new Id("some-value");

        var actual = "" + id;

        actual.Should().Be("some-value");
    }
}