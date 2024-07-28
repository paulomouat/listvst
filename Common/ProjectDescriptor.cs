namespace ListVst;

public readonly record struct ProjectDescriptor
{
    private const string SpecialFolderPrefix = "_";
    
    public string Name { get; }
    public string File { get; }
    public string SpecialFolder { get; }
    public string Path { get; }
    
    public string[] Segments { get; }
    public string[] Subsegments { get; }
    
    public ProjectDescriptor(string path)
    {
        Path = path;
        
        var sanitizedPath = Path;
        if (sanitizedPath.StartsWith('/'))
        {
            sanitizedPath = sanitizedPath[1..];
        }

        Segments = Segment(sanitizedPath);

        (Name, SpecialFolder) = ExtractProjectFolder(Segments);
        File = ExtractProjectFile(Segments);
        Subsegments = ExtractSubsegments(Segments, Name, SpecialFolder);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<string>.Default.GetHashCode(Path) * -1521134295;
    }    

    public bool Equals(ProjectDescriptor other)
    {
        return EqualityComparer<string>.Default.Equals(Path, other.Path);
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

    private static string[] ExtractSubsegments(IEnumerable<string> segments, string project, string specialFolder)
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
}