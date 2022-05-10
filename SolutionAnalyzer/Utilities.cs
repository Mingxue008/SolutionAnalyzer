using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;
using System.IO;

namespace SolutionAnalyzer
{
    internal class Utilities
    {
        public static bool IsSolutionFileValid(string solutionPath)
        {
            return (!string.IsNullOrEmpty(solutionPath) && File.Exists(solutionPath));
        }

        public static void RestorePackages(string solutionPath)
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = @"nuget.exe",
                    Arguments = $"restore {solutionPath} -Verbosity quiet",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();
                process.WaitForExit();
            }
        }

        public static void PrintDependencies(Project project)
        {
            if (project != null)
            {
                Console.WriteLine();
                Console.WriteLine($"{project.Name}:");
                var dependencies = project?.MetadataReferences;

                foreach (var dependency in dependencies)
                {
                    Console.WriteLine(dependency.Display);
                }
            }
        }
    }
}
