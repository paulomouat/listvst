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
        
        if (target.StartsWith(replacement))
        {
            builder.Remove(0, 1);
        }

        foreach (var c in replaced)
        {
            builder.Replace(c, replacement);
        }

        return builder.ToString();
    }
}