// See https://aka.ms/new-console-template for more information

using Codelyzer.Analysis;
using Microsoft.Extensions.Logging;

namespace Codelyzer.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var solutionFiles = Directory.GetFiles(args[0], "*.sln", SearchOption.AllDirectories);

            int i = 0;
            foreach(var solutionFile in solutionFiles)
            {
                Console.WriteLine();
                Console.WriteLine($"{i} - {solutionFile}");
                try
                {
                    var projectCount = AnalyzeSolutionFile(solutionFile);
                    Console.WriteLine($"\tSucceeded with {projectCount} projects.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{i} - Failed {ex}");
                }
                i++;
            }
        }

        private static int AnalyzeSolutionFile(string solutionFile)
        {
            /* 2. Create Configuration settings */
            var solutionFolder = Path.GetDirectoryName(solutionFile);
            if (solutionFolder == null)
            {
                return 0;
            }

            AnalyzerConfiguration configuration = new AnalyzerConfiguration(LanguageOptions.CSharp)
            {
                Language = LanguageOptions.CSharp,
                AnalyzeFailedProjects = false,

                ExportSettings =
                {
                    GenerateJsonOutput = true,
                    OutputPath = solutionFolder,
                },

                MetaDataSettings =
                {
                    LiteralExpressions = true,
                    MethodInvocations = true,
                    DeclarationNodes = true,
                    ReferenceData = true,
                },

                BuildSettings =
                {
                    BuildArguments =
                    {
                        "/p:RestorePackagesConfig=true",
                        "/restore"
                    }
                }
            };

            var loggerFactory = LoggerFactory.Create(
                builder => builder.SetMinimumLevel(LogLevel.Trace));

            CodeAnalyzer analyzer = CodeAnalyzerFactory.GetAnalyzer(
                configuration,
                loggerFactory.CreateLogger("Analyzer"));

            var results = analyzer.AnalyzeSolution(solutionFile).Result;
            foreach(var result in results)
            {
                if (result.OutputJsonFilePath != null)
                {
                    Console.WriteLine("Exported to : " + result.OutputJsonFilePath);
                }
            }

            return results.Count;
        }
    }
}
