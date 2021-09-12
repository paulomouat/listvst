using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListVst
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("List VSTs");

            //var sourcePath = Environment.GetEnvironmentVariable("HOME") + "/Documents/projects/music";
            var sourcePath = "/Volumes/projects/music";

            if (args.Length == 1)
            {
                sourcePath = args[0];
            }

            var p = new Program();

            Console.WriteLine($"Source path is {sourcePath}");

            var sovsts = await p.ProcessStudioOneProjects(sourcePath);
            var alvsts = await p.ProcessAbletonLiveProjects(sourcePath);

            Console.WriteLine();
            Console.WriteLine("List of VSTs (by project):");
            Console.WriteLine();

            var all = alvsts.Concat(sovsts);

            var allByPath = all.ToLookup(e => e.Path, e => e.Vst).OrderBy(v => v.Key);

            p.Output(allByPath);

            Console.WriteLine("List of VSTs (by VST):");
            Console.WriteLine();

            var allByVst = all.ToLookup(e => e.Vst, e => e.Path).OrderBy(v => v.Key);

            p.Output(allByVst);

            Console.WriteLine("Done.");
        }

        private async Task<IEnumerable<(string Path, string Vst)>> ProcessStudioOneProjects(string sourcePath)
        {
            var processor = new StudioOne.Processor();
            return await processor.Process(sourcePath);
        }

        private async Task<IEnumerable<(string Path, string Vst)>> ProcessAbletonLiveProjects(string sourcePath)
        {
            var processor = new AbletonLive.Processor();
            return await processor.Process(sourcePath);
        }

        private void Output(IOrderedEnumerable<IGrouping<string, string>> lookup)
        {
            foreach(var group in lookup)
            {
                Console.WriteLine(group.Key);
                foreach(var element in group)
                {
                    Console.WriteLine(element);
                }

                Console.WriteLine();
            }
        }
    }
}
