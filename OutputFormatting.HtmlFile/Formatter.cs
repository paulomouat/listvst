namespace ListVst.OutputFormatting.HtmlFile;

public class Formatter : IOutputFormatter
{
    public string Format => "html";

    protected virtual async Task Write(IEnumerable<PluginRecord> data, IFileOutputFormatterOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Path))
        {
            throw new ArgumentException(nameof(options.Path));
        }

        var document = Document.Create("List of VSTs");

        var detailsList = data.ToList();
        
        var projectSection = new ProjectSection();
        projectSection.Add(detailsList);

        var pluginSection = new PluginSection();
        pluginSection.Add(detailsList);

        var mainIndex = new MainIndex("main-index", "Main index");
        mainIndex.Add([projectSection, pluginSection]);
       
        document.AddMainIndex(mainIndex);
        document.AddSection(projectSection);
        document.AddSection(pluginSection);
        document.AddScriptInvocations();
        
        document.Save(options.Path);
        await Task.CompletedTask;
    }

    Task IOutputFormatter.Write(IEnumerable<PluginRecord> data, IOutputFormatterOptions options)
    {
        if (options is not IFileOutputFormatterOptions formatterOptions)
        {
            throw new ArgumentException("This formatter requires an options type of IFileOutputFormatterOptions");
        }

        return Write(data, formatterOptions);
    }
}