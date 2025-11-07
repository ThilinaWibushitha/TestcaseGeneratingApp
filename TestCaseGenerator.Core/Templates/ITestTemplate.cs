using TestCaseGenerator.Core.Models;

namespace TestCaseGenerator.Core.Templates
{
    public interface ITestTemplate
    {
        string GenerateTestClass(ClassAnalysis classAnalysis, GenerationOptions options);
        string GenerateTestMethod(MethodAnalysis methodAnalysis, string testCaseName, GenerationOptions options);
        string GetTestAttributeName();
        string GetTestClassAttributeName();
        string GetSetupAttributeName();
        string GetTeardownAttributeName();
    }
}
