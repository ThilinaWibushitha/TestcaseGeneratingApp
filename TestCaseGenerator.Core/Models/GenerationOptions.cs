namespace TestCaseGenerator.Core.Models
{
    public class GenerationOptions
    {
        public TestFramework TestFramework { get; set; } = TestFramework.NUnit;
        public MockingFramework MockingFramework { get; set; } = MockingFramework.Moq;
        public TestType TestType { get; set; } = TestType.Unit;
        public bool GenerateTestData { get; set; } = true;
        public bool IncludeCoverageAnalysis { get; set; } = true;
        public bool UseTemplates { get; set; } = true;
        public string OutputDirectory { get; set; } = string.Empty;
        public string Namespace { get; set; } = "Tests";
    }
}
