using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ListVst
{
    class Program
    {
        private ILogger Logger { get; set; }
        private Configuration Configuration { get; set; }

        public void Run()
        {
            Logger.LogInformation("List VSTs");
            Logger.LogInformation($"Source path is {Configuration.SourcePath}");

            var all = Configuration.Processors
                .SelectMany(p => p.Process(Configuration.SourcePath!).Result)
                .ToList();
            
            var allByPath = all
                .ToLookup(e => e.Path, e => e.Vst)
                .OrderBy(v => v.Key);
            Output(allByPath);
            
            var allByVst = all
                .ToLookup(e => e.Vst, e => e.Path)
                .OrderBy(v => v.Key);
            Output(allByVst);
            
            /*var sovsts = await p.ProcessStudioOneProjects(sourcePath);
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

            p.Output(allByVst);*/
        }
        
        public Program(Configuration configuration, ILogger<Program> logger)
        {
            Logger = logger;
            Configuration = configuration;
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
