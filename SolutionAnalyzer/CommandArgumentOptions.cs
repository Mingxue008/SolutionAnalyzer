using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionAnalyzer
{
    internal class CommandArgumentOptions
    {
        [Option('b', "ms-build-path", Required = true, HelpText = "Path to a Visual Studio MSBuild toolset.")]
        public string VisualStudioMSBuildPath { get; set; }

        [Option('s', "solution-path", Required = true, HelpText = "Path to the solution to be analyzed.")]
        public string SolutionPath { get; set; }
    }
}
