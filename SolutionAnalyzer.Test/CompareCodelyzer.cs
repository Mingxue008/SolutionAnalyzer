using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NUnit.Framework;
using System.IO;

namespace SolutionAnalyzer
{
    [TestClass]
    public class CompareCodelyzer
    {
        private string targetSolutionFolder = @"D:\work\TestProjects";

        [TestInitialize]
        public void SetUp()
        {
            Analyzer.RegisterVisualStudioInstance(@"C:\Program Files\Microsoft Visual Studio\2022\Community");
        }

        [TestMethod]
        public void Roslyn_Analyze_Against_Codelyzer()
        {
            var targetSolutionFiles = Directory.EnumerateFiles(targetSolutionFolder, "*.sln", SearchOption.AllDirectories);

            foreach(var targetSolutionFile in targetSolutionFiles)
            {
                var solutionResult = Analyzer.AnalyzeSolution(targetSolutionFile).Result;
            }
        }
    }
}
