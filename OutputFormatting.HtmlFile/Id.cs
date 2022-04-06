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
        var result = string.Empty;
            
        if (!string.IsNullOrWhiteSpace(value))
        {
            result = value.Replace(" ", "-");
            result = result.Replace("/", "-");
            result = result.Replace(".", "-");
            result = result.ToLowerInvariant();
        }

        return result;
    }
}