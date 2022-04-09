namespace ListVst
{
    public class FileList
    {
        public string SourcePath { get; }

        public FileList(string sourcePath)
        {
            SourcePath = sourcePath;
        }

        public IEnumerable<string> GetFiles(string extension, Func<string, bool> filter)
        {
            var files = Directory.EnumerateFiles(SourcePath, $"*.{extension}", SearchOption.AllDirectories);
            files = files.Where(f => !f.Contains("._"))
                .Where(filter)
                .OrderBy(f => f);
            return files;
        }
    }
}
