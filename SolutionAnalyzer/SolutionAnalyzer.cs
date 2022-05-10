using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SolutionAnalyzer
{
    internal class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
    {
        public void Report(ProjectLoadProgress loadProgress)
        {
            var projectDisplay = Path.GetFileName(loadProgress.FilePath);
            if (loadProgress.TargetFramework != null)
            {
                projectDisplay += $" ({loadProgress.TargetFramework})";
            }

            Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
        }
    }

    public class Analyzer
    {
        public static bool RegisterVisualStudioInstance(string msBuildPath)
        {
            // Enumerate valid visual studio instances.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();

            // Find the valid one from user input.
            foreach (var visualStudioInstance in visualStudioInstances)
            {
                if (visualStudioInstance != null &&
                    visualStudioInstance.MSBuildPath.Contains(msBuildPath))
                {
                    MSBuildLocator.RegisterInstance(visualStudioInstance);
                    return true;
                }
            }

            return false;
        }

        public static Task<Solution> AnalyzeSolution(string solutionPath)
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                // Print message for WorkspaceFailed event to help diagnosing project load failures.
                workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

                // Attach progress reporter so we print projects as they are loaded.
                return workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
            }
        }
    }
}
