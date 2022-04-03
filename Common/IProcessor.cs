using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListVst;

public interface IProcessor
{
    Task<IEnumerable<(string Path, string Vst)>> Process(string sourcePath);
}