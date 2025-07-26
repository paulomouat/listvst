namespace ListVst;

public record ProjectType : IComparable<ProjectType>
{
    public static readonly ProjectType Unknown = new("unknown");
    public static readonly ProjectType AbletonLive = new("abletonlive");
    public static readonly ProjectType StudioOneSong = new("studioonesong");
    public static readonly ProjectType StudioOneMasteringProject = new("studiooneproject");

    public string Value { get; }
    public string Designation => Designations[this];

    public int CompareTo(ProjectType? other)
    {
        return string.Compare(Value, other?.Value, StringComparison.Ordinal);
    }

    public override string ToString()
    {
        return Value;
    }
    
    private ProjectType(string value)
    {
        Value = value;
    }
    
    private static IDictionary<ProjectType, string> Designations { get; } = new Dictionary<ProjectType, string>
    {
        { Unknown, "?" },
        { AbletonLive, "Ableton Live" },
        { StudioOneSong, "Studio One Song" },
        { StudioOneMasteringProject, "Studio One Mastering Project" }
    };
}