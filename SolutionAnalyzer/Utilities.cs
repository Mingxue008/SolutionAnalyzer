using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionAnalyzer
{
    internal class Utilities
    {
        public static bool RegisterVisualStudioInstance(CommandArgumentParser parser)
        {
            // Enumerate valid visual studio instances.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            
            // Find the valid one from user input.
            foreach (var visualStudioInstance in visualStudioInstances)
            {
                if (visualStudioInstance != null && 
                    visualStudioInstance.MSBuildPath.Contains(parser.VisualStudioMSBuildPath))
                {
                    MSBuildLocator.RegisterInstance(visualStudioInstance);
                    return true;
                }
            }

            return false;
        }

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
