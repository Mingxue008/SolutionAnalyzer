using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SolutionAnalyzer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CommandArgumentParser argumentParser = new CommandArgumentParser();
            if (!argumentParser.HandleCommand(args))
            {
                return;
            }

            if (!Utilities.RegisterVisualStudioInstance(argumentParser))
            {
                Console.WriteLine($"MSBuild path {argumentParser.VisualStudioMSBuildPath} is invalid.");
                return;
            }

            if (!Utilities.IsSolutionFileValid(argumentParser.SolutionPath))
            {
                Console.WriteLine($"{argumentParser.SolutionPath} is invalid.");
                return;
            }

            // Restore nuget packages.
            Utilities.RestorePackages(argumentParser.SolutionPath);

            Solution analyzeResult = await SolutionAnalyzer.AnalyzeSolution(argumentParser.SolutionPath);
            if (analyzeResult != null)
            {
                foreach(var project in analyzeResult.Projects)
                {
                    Utilities.PrintDependencies(project);
                }
            }
        }
    }
}
