using System.Collections.Generic;

namespace ListVst
{
    public class VstList
    {
        public string Path { get; }
        public IEnumerable<string> Vsts { get; }

        public VstList(string path, IEnumerable<string> vsts)
        {
            Path = path;
            Vsts = vsts;
        }
    }
}
