namespace ListVst.OutputFormatting;

public record struct MappedFormatter(string Format, string File, IOutputFormatter Formatter);