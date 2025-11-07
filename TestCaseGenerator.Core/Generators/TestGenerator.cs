using System;
using System.IO;
using System.Threading.Tasks;
using TestCaseGenerator.Core.Analyzers;
using TestCaseGenerator.Core.Models;
using TestCaseGenerator.Core.Templates;

namespace TestCaseGenerator.Core.Generators
{
    public class TestGenerator
    {
        private readonly CodeAnalyzer _analyzer;

        public TestGenerator()
        {
            _analyzer = new CodeAnalyzer();
        }

        public async Task<string> GenerateTestsAsync(string sourceFilePath, GenerationOptions options)
        {
            // Analyze the source code
            var classAnalysis = await _analyzer.AnalyzeClassAsync(sourceFilePath);

            // Select appropriate template
            var template = GetTemplate(options.TestFramework);

            // Generate test code
            var testCode = template.GenerateTestClass(classAnalysis, options);

            // Save to output directory if specified
            if (!string.IsNullOrEmpty(options.OutputDirectory))
            {
                var outputFileName = $"{classAnalysis.ClassName}Tests.cs";
                var outputPath = Path.Combine(options.OutputDirectory, outputFileName);
                
                Directory.CreateDirectory(options.OutputDirectory);
                await File.WriteAllTextAsync(outputPath, testCode);
                
                Console.WriteLine($"Test file generated: {outputPath}");
            }

            return testCode;
        }

        public string GenerateTests(string sourceCode, GenerationOptions options)
        {
            // Analyze the source code
            var classAnalysis = _analyzer.AnalyzeClass(sourceCode);

            // Select appropriate template
            var template = GetTemplate(options.TestFramework);

            // Generate test code
            return template.GenerateTestClass(classAnalysis, options);
        }

        public ClassAnalysis AnalyzeCode(string sourceCode)
        {
            return _analyzer.AnalyzeClass(sourceCode);
        }

        public async Task<ClassAnalysis> AnalyzeCodeAsync(string sourceFilePath)
        {
            return await _analyzer.AnalyzeClassAsync(sourceFilePath);
        }

        private ITestTemplate GetTemplate(TestFramework framework)
        {
            return framework switch
            {
                TestFramework.MSTest => new MSTestTemplate(),
                TestFramework.NUnit => new NUnitTemplate(),
                TestFramework.XUnit => new XUnitTemplate(),
                _ => throw new ArgumentException($"Unsupported test framework: {framework}")
            };
        }
    }
}
