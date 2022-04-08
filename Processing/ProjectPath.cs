namespace ListVst.Processing;

public class ProjectPath
{
    private const string SpecialFolderPrefix = "_";
    
    public string Project { get; private set; }
    public string ProjectFile { get; private set; }
    public string SpecialFolder { get; private set; }
    
    public IEnumerable<string> Segments { get; private set; }
    public IEnumerable<string> Subsegments { get; private set; }
    
    public static ProjectPath Parse(string rawPath)
    {
        if (rawPath.StartsWith("/"))
        {
            rawPath = rawPath[1..];
        }

        var segments = Segment(rawPath);

        var (project, specialFolder) = ExtractProjectFolder(segments);
        var projectFile = ExtractProjectFile(segments);
        var subsegments = ExtractSubsegments(segments, project, specialFolder);
        
        var projectPath = new ProjectPath
        {
            Project = project,
            ProjectFile = projectFile,
            SpecialFolder = specialFolder,
            Segments = segments,
            Subsegments = subsegments
        };

        return projectPath;
    }

    private static string[] Segment(string path)
    {
        var segments = path.Split("/");
        return segments;
    }

    private static (string Project, string SpecialFolder) ExtractProjectFolder(string[] segments)
    {
        var first = segments.First();
        var projectFolder = first;
        var specialFolder = string.Empty;
        if (first.StartsWith(SpecialFolderPrefix))
        {
            specialFolder = first;
            projectFolder = segments.Skip(1).First();
        }

        return (projectFolder, specialFolder);
    }
    
    private static string ExtractProjectFile(IEnumerable<string> segments)
    {
        var projectFile = segments.Last();
        return projectFile;
    }

    private static string[] ExtractSubsegments(string[] segments, string project, string specialFolder)
    {
        var skip = 0;
        if (!string.IsNullOrWhiteSpace(project))
        {
            skip++;
        }
        if (!string.IsNullOrWhiteSpace(specialFolder))
        {
            skip++;
        }

        return segments.Skip(skip).ToArray();
    }
    
    private ProjectPath()
    {
        Project = string.Empty;
        ProjectFile = string.Empty;
        SpecialFolder = string.Empty;
        Segments = Array.Empty<string>();
        Subsegments = Array.Empty<string>();
    }
}