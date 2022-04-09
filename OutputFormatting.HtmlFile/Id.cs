using System.Text;

namespace ListVst.OutputFormatting.HtmlFile;

public readonly record struct Id(string SourceValue)
{
    public string Value => Escape(SourceValue);
    private string SourceValue { get; } = SourceValue;

    public static implicit operator string(Id id) => id.Value;
    
    private static string Escape(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var escaped = MultiReplace(value, new[] { " ", "/", ".", "'", ";", ":" });
        return escaped;
    }

    private static string MultiReplace(string target, IEnumerable<string> replaced, string replacement = "-")
    {
        var builder = new StringBuilder(target.ToLowerInvariant());

        foreach (var c in replaced)
        {
            builder.Replace(c, replacement);
        }
        
        if (replacement.Length > 0 && builder[0] == replacement[0])
        {
            builder.Remove(0, 1);
        }

        return builder.ToString();
    }
}