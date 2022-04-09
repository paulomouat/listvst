using Microsoft.Extensions.Logging;

namespace ListVst.Processing.StudioOne;

public class Processor : ProcessorBase
{
    public override string ProjectType => "Studio One";
    public override string FileExtension => "song";
    public override Func<string, bool> FileFilter => f => !f.Contains("(Autosaved)") &&
                                                          !f.Contains("(Before Autosave)") &&
                                                          !f.Contains("/History/");
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