namespace ListVst.Processing;

public class ProcessorRegistry : IProcessorRegistry
{
    public IEnumerable<IProcessor> Processors => ProcessorsInternal;

    private List<IProcessor> ProcessorsInternal { get; }

    public ProcessorRegistry()
        : this(Array.Empty<IProcessor>())
    { }

    public ProcessorRegistry(IEnumerable<IProcessor> processors)
    {
        ProcessorsInternal = new List<IProcessor>();
        ProcessorsInternal.AddRange(processors);
    }

    public void Add(IProcessor processor)
    {
        ProcessorsInternal.Add(processor);
    }
}