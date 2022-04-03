using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ListVst
{
    public class FileList
    {
        public string SourcePath { get; }

        public FileList(string sourcePath)
        {
            SourcePath = sourcePath;
        }

        public IEnumerable<string> GetFiles(string extension)
        {
            var files = Directory.EnumerateFiles(SourcePath, $"*.{extension}", SearchOption.AllDirectories);
            files = files.Where(f => !f.Contains("._")).OrderBy(f => f);
            return files;
        }
    }
}
