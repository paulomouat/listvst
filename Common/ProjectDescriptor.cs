namespace ListVst;

public class ProjectDescriptor
{
    private const string SpecialFolderPrefix = "_";
    
    public string Name { get; private set; }
    public string File { get; private set; }
    public string SpecialFolder { get; private set; }
    public string Path { get; private set; }
    
    public IEnumerable<string> Segments { get; private set; }
    public IEnumerable<string> Subsegments { get; private set; }
    
    public static ProjectDescriptor Parse(string rawPath)
    {
        var sanitizedPath = rawPath;
        
        if (sanitizedPath.StartsWith("/"))
        {
            sanitizedPath = sanitizedPath[1..];
        }

        var segments = Segment(sanitizedPath);

        var (project, specialFolder) = ExtractProjectFolder(segments);
        var projectFile = ExtractProjectFile(segments);
        var subsegments = ExtractSubsegments(segments, project, specialFolder);
        
        var projectDescriptor = new ProjectDescriptor
        {
            Name = project,
            File = projectFile,
            SpecialFolder = specialFolder,
            Path = rawPath,
            Segments = segments,
            Subsegments = subsegments
        };

        return projectDescriptor;
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
    
    private ProjectDescriptor()
    {
        Name = string.Empty;
        File = string.Empty;
        SpecialFolder = string.Empty;
        Path = string.Empty;
        Segments = Array.Empty<string>();
        Subsegments = Array.Empty<string>();
    }
}