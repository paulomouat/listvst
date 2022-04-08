using FluentAssertions;
using ListVst.Processing;
using Xunit;

namespace Processing.Tests
{
    public class ProjectPathFixture
    {
        private const string RegularPath = "/main project/some subfolder/another subfolder/main file.ext";
        private const string NoLeadingSlashPath = "main project/some subfolder/another subfolder/main file.ext";
        private const string SpecialFolderPath = "/_special folder/main project/some subfolder/another subfolder/main file.ext";
        private const string SpecialFolderNoLeadingSlashPath = "_special folder/main project/some subfolder/another subfolder/main file.ext";
        private const string ShallowPath = "/main project/main file.ext";
        
        [Theory]
        [InlineData(RegularPath)]
        [InlineData(NoLeadingSlashPath)]
        public void RegularPath_AssignsProject(string rawPath)
        {
            var sut = ProjectPath.Parse(rawPath);

            sut.Project.Should().Be("main project");
        }
        
        [Theory]
        [InlineData(RegularPath)]
        [InlineData(NoLeadingSlashPath)]
        public void RegularPath_AssignsProjectFile(string rawPath)
        {
            var sut = ProjectPath.Parse(rawPath);

            sut.ProjectFile.Should().Be("main file.ext");
        }
        
        [Theory]
        [InlineData(RegularPath)]
        [InlineData(NoLeadingSlashPath)]
        public void RegularPath_DoesNotAssignSpecialFolder(string rawPath)
        {
            var sut = ProjectPath.Parse(rawPath);

            sut.SpecialFolder.Should().BeEmpty();
        }
        
        [Theory]
        [InlineData(SpecialFolderPath)]
        [InlineData(SpecialFolderNoLeadingSlashPath)]
        public void SpecialFolderPath_AssignsProject(string rawPath)
        {
            var sut = ProjectPath.Parse(rawPath);

            sut.Project.Should().Be("main project");
        }
        
        [Theory]
        [InlineData(SpecialFolderPath)]
        [InlineData(SpecialFolderNoLeadingSlashPath)]
        public void SpecialFolderPath_AssignsProjectFile(string rawPath)
        {
            var sut = ProjectPath.Parse(rawPath);

            sut.ProjectFile.Should().Be("main file.ext");
        }
        
        [Theory]
        [InlineData(SpecialFolderPath)]
        [InlineData(SpecialFolderNoLeadingSlashPath)]
        public void SpecialFolderPath_AssignsSpecialFolder(string rawPath)
        {
            var sut = ProjectPath.Parse(rawPath);

            sut.SpecialFolder.Should().Be("_special folder");
        }
        
        [Fact]
        public void FullPath_AssignsSegments()
        {
            var sut = ProjectPath.Parse(SpecialFolderPath);

            sut.Segments.Should().BeEquivalentTo(new[]
            {
                "_special folder",
                "main project",
                "some subfolder",
                "another subfolder",
                "main file.ext"
            });
        }
        
        [Theory]
        [InlineData(RegularPath)]
        [InlineData(SpecialFolderPath)]
        public void FullPath_AssignsSubsegments(string rawPath)
        {
            var sut = ProjectPath.Parse(rawPath);

            sut.Subsegments.Should().BeEquivalentTo(new[]
            {
                "some subfolder",
                "another subfolder",
                "main file.ext"
            });
        }
        
        [Fact]
        public void ShallowPath_HasNoSubsegments()
        {
            var sut = ProjectPath.Parse(ShallowPath);

            sut.Subsegments.Should().BeEquivalentTo(new[]
            {
                "main file.ext"
            });
        }
    }
}
