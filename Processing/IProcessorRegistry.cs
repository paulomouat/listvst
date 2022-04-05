namespace ListVst.Processing;

public interface IProcessorRegistry
{
    IEnumerable<IProcessor> Processors { get; }

    void Add(IProcessor processor);
}