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
            Console.WriteLine("List of VSTs:");
            Console.WriteLine();

            var list = alvsts.Concat(sovsts).OrderBy(v => v.Path).ToList();

            p.Output(list);

            Console.WriteLine("Done.");
        }

        private async Task<IEnumerable<VstList>> ProcessStudioOneProjects(string sourcePath)
        {
            var processor = new StudioOne.Processor();
            return await processor.Process(sourcePath);
        }

        private async Task<IEnumerable<VstList>> ProcessAbletonLiveProjects(string sourcePath)
        {
            var processor = new AbletonLive.Processor();
            return await processor.Process(sourcePath);
        }

        private void Output(IEnumerable<VstList> vsts)
        {
            foreach(var vstList in vsts)
            {
                Console.WriteLine(vstList.Path);
                foreach(var vst in vstList.Vsts)
                {
                    Console.WriteLine(vst);
                }

                Console.WriteLine();
            }
        }
    }
}
