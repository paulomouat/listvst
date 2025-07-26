namespace ListVst;

public record PluginType : IComparable<PluginType>
{
    public static readonly PluginType Unknown = new("unknown");
    public static readonly PluginType AudioUnit = new("audiounit");
    public static readonly PluginType Vst = new("vst");
    public static readonly PluginType Vst3 = new("vst3");

    public string Value { get; }
    public string Designation => Designations[this];
    
    public int CompareTo(PluginType? other)
    {
        return string.Compare(Value, other?.Value, StringComparison.Ordinal);
    }
    
    public override string ToString()
    {
        return Value;
    }
    
    private PluginType(string value)
    {
        Value = value;
    }
    
    private static IDictionary<PluginType, string> Designations { get; } = new Dictionary<PluginType, string>
    {
        { Unknown, "?" },
        { AudioUnit, "AU" },
        { Vst, "VST" },
        { Vst3, "VST3" }
    };
        
}