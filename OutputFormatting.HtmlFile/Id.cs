using System.Text;

namespace ListVst.OutputFormatting.HtmlFile;

public class Id
{
    public string Value { get; set; }

    public Id(string value)
    {
        Value = Escape(value);
    }

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