using Microsoft.Extensions.Logging;

namespace ListVst.Processing.AbletonLive;

public class Processor(ILogger<Processor> logger) : ProcessorBase(logger)
{
    protected override string ProjectType => "Ableton Live";
    protected override string FileExtension => "als";
    protected override Func<string, bool> FileFilter => f => !f.Contains("Backup");

    protected override IProjectFile GetProjectFile(string file)
    {
        return new ProjectFile(file);
    }

    protected override IParser GetParser(ILogger logger)
    {
        return new Parser(logger);
    }
}