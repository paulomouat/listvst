﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListVst.StudioOne
{
    public class Processor
    {
        public async Task<IEnumerable<(string Path, string Vst)>> Process(string sourcePath)
        {
            var results = new List<(string Path, string Vst)>();

            var fl = new FileList(sourcePath);
            var files = fl.GetFiles("song").Where(f => !f.Contains("(Autosaved)") &&
                !f.Contains("(Before Autosave)") && !f.Contains("/History/"));
            foreach (var file in files)
            {
                Console.WriteLine($"Processing Studio One project {file}");
                var pf = new ProjectFile(file);
                await pf.Read();
                var c = pf.Contents;
                var p = new Parser();
                var vsts = p.Parse(c);
                var list = vsts.Select(vst => (file, vst)).ToList();
                results.AddRange(list);
            }

            return results;
        }
    }
}
