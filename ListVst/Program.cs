using System;
using System.Collections.Generic;
using System.Linq;
using ListVst.OutputFormatters;
using Microsoft.Extensions.Logging;

namespace ListVst
{
    class Program
    {
        private ILogger Logger { get; }
        private Configuration Configuration { get; }
        
        public Program(Configuration configuration, ILogger<Program> logger)
        {
            Logger = logger;
            Configuration = configuration;
        }

        public void Run(string? format, string? file, string? sourcePath)
        {
            format = Configuration.Outputs.First().Format;
            file = Configuration.Outputs.First().Path!;
            sourcePath = Configuration.SourcePath;
            
            Logger.LogInformation("List VSTs");
            Logger.LogInformation($"Source path is {sourcePath}");
            Logger.LogInformation($"Output is in format {format} into file {file}");

            // TODO: Instantiate through output formatter registry
            var outputFormatter = new TxtFile(file);
            
            var all = Configuration.Processors
                .SelectMany(p => p.Process(Configuration.SourcePath!).Result)
                .ToList();

            outputFormatter.Write(all);
            
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

        /*private void Output(IOrderedEnumerable<IGrouping<string, string>> lookup)
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
        }*/
    }
}
