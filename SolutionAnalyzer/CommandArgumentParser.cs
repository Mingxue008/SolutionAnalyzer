
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionAnalyzer
{
    internal class CommandArgumentParser
    {
        public string VisualStudioMSBuildPath { get; set; }
        public string SolutionPath { get; set; }

        public bool HandleCommand(string[] args)
        {
            var result = Parser.Default
                .ParseArguments<CommandArgumentOptions>(args)
                .WithParsed<CommandArgumentOptions>(o =>
                {
                    VisualStudioMSBuildPath = o.VisualStudioMSBuildPath;
                    
                    if (!string.IsNullOrEmpty(o.SolutionPath) &&
                        File.Exists(o.SolutionPath))
                    {
                        SolutionPath = o.SolutionPath;
                    }
                });

            return (result.Tag != ParserResultType.NotParsed);
        }
    }
}
