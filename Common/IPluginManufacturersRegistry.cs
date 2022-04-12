namespace ListVst;

public interface IPluginManufacturersRegistry
{
    string GetManufacturer(string pluginName);
    void Register(string manufacturer);
}