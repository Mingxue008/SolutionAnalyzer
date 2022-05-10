using Microsoft.CodeAnalysis;
using System;
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

            if (!Utilities.IsSolutionFileValid(argumentParser.SolutionPath))
            {
                Console.WriteLine($"{argumentParser.SolutionPath} is invalid.");
                return;
            }

            // Restore nuget packages.
            Utilities.RestorePackages(argumentParser.SolutionPath);

            if (!Analyzer.RegisterVisualStudioInstance(argumentParser.VisualStudioMSBuildPath))
            {
                Console.WriteLine($"MSBuild path {argumentParser.VisualStudioMSBuildPath} is invalid.");
                return;
            }

            Solution analyzeResult = await Analyzer.AnalyzeSolution(argumentParser.SolutionPath);
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
