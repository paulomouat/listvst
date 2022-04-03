namespace ListVst;

public interface IProjectFile
{
    string Name { get; }
    string Path { get; }
    string? Contents { get; }

    Task Read();
}