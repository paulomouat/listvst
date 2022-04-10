namespace ListVst.OutputFormatting.HtmlFile;

public class Formatter : IOutputFormatter
{
    public string Format => "html";

    protected virtual async Task Write(IEnumerable<PluginProjectPair> pairs, IFileOutputFormatterOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.Path))
        {
            throw new ArgumentException(nameof(options.Path));
        }

        var document = Document.Create("List of VSTs");

        var detailsList = pairs.ToList();
        
        var projectSection = new ProjectSection();
        projectSection.Add(detailsList);

        var pluginSection = new PluginSection();
        pluginSection.Add(detailsList);

        var mainIndex = new MainIndex("main-index", "Main index");
        mainIndex.Add(new ISection[] { projectSection, pluginSection });
       
        document.Add(mainIndex);
        document.Add(projectSection);
        document.Add(pluginSection);

        document.Save(options.Path);
        await Task.CompletedTask;
    }

    Task IOutputFormatter.Write(IEnumerable<PluginProjectPair> pairs, IOutputFormatterOptions options)
    {
        if (options is not IFileOutputFormatterOptions formatterOptions)
        {
            throw new ArgumentException("This formatter requires an options type of IFileOutputFormatterOptions");
        }

        return Write(pairs, formatterOptions);
    }
}