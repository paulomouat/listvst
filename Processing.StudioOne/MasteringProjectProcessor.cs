using Microsoft.Extensions.Logging;

namespace ListVst.Processing.StudioOne;

public class MasteringProjectProcessor(ILogger<MasteringProjectProcessor> logger) : ProcessorBase(logger)
{
    protected override ProjectType ProjectType => ProjectType.StudioOneMasteringProject;
    protected override string FileExtension => "project";

    protected override Func<string, bool> FileFilter => f => !f.Contains("(Autosaved)") &&
                                                             !f.Contains("(Before Autosave)") &&
                                                             !f.Contains("/History/");

    protected override IProjectFile GetProjectFile(string file)
    {
        return new ProjectFile(file);
    }

    protected override IParser GetParser(ILogger logger)
    {
        return new Parser(logger);
    }

}