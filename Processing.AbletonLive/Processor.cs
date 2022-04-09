using Microsoft.Extensions.Logging;

namespace ListVst.Processing.AbletonLive;

public class Processor : ProcessorBase
{
    public override string ProjectType => "Ableton Live";
    public override string FileExtension => "als";
    public override Func<string, bool> FileFilter => f => !f.Contains("Backup");
        
    public Processor(ILogger<Processor> logger)
        : base(logger)
    { }

    protected override IProjectFile GetProjectFile(string file)
    {
        return new ProjectFile(file);
    }

    protected override IParser GetParser(ILogger logger)
    {
        return new Parser(logger);
    }
}