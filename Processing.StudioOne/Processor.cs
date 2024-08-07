﻿using Microsoft.Extensions.Logging;

namespace ListVst.Processing.StudioOne;

public class Processor(ILogger<Processor> logger) : ProcessorBase(logger)
{
    protected override string ProjectType => "Studio One";
    protected override string FileExtension => "song";

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