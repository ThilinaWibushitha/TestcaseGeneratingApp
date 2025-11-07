using System.Linq;
using System.Text;
using TestCaseGenerator.Core.Models;

namespace TestCaseGenerator.Core.Templates
{
    public class MSTestTemplate : ITestTemplate
    {
        public string GetTestAttributeName() => "[TestMethod]";
        public string GetTestClassAttributeName() => "[TestClass]";
        public string GetSetupAttributeName() => "[TestInitialize]";
        public string GetTeardownAttributeName() => "[TestCleanup]";

        public string GenerateTestClass(ClassAnalysis classAnalysis, GenerationOptions options)
        {
            var sb = new StringBuilder();
            
            // Using directives
            sb.AppendLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            
            if (options.MockingFramework == MockingFramework.Moq)
            {
                sb.AppendLine("using Moq;");
            }
            else if (options.MockingFramework == MockingFramework.NSubstitute)
            {
                sb.AppendLine("using NSubstitute;");
            }
            
            sb.AppendLine($"using {classAnalysis.Namespace};");
            sb.AppendLine();
            
            // Namespace
            sb.AppendLine($"namespace {options.Namespace}");
            sb.AppendLine("{");
            
            // Test class
            sb.AppendLine($"    {GetTestClassAttributeName()}");
            sb.AppendLine($"    public class {classAnalysis.ClassName}Tests");
            sb.AppendLine("    {");
            
            // Fields
            sb.AppendLine($"        private {classAnalysis.ClassName}? _sut;");
            
            foreach (var dependency in classAnalysis.Methods.SelectMany(m => m.Dependencies).Distinct())
            {
                if (dependency.StartsWith("I") && char.IsUpper(dependency[1]))
                {
                    if (options.MockingFramework == MockingFramework.Moq)
                    {
                        sb.AppendLine($"        private Mock<{dependency}>? _mock{dependency.Substring(1)};");
                    }
                    else if (options.MockingFramework == MockingFramework.NSubstitute)
                    {
                        sb.AppendLine($"        private {dependency}? _mock{dependency.Substring(1)};");
                    }
                }
            }
            
            sb.AppendLine();
            
            // Setup method
            sb.AppendLine($"        {GetSetupAttributeName()}");
            sb.AppendLine("        public void Setup()");
            sb.AppendLine("        {");
            
            foreach (var dependency in classAnalysis.Methods.SelectMany(m => m.Dependencies).Distinct())
            {
                if (dependency.StartsWith("I") && char.IsUpper(dependency[1]))
                {
                    if (options.MockingFramework == MockingFramework.Moq)
                    {
                        sb.AppendLine($"            _mock{dependency.Substring(1)} = new Mock<{dependency}>();");
                    }
                    else if (options.MockingFramework == MockingFramework.NSubstitute)
                    {
                        sb.AppendLine($"            _mock{dependency.Substring(1)} = Substitute.For<{dependency}>();");
                    }
                }
            }
            
            sb.AppendLine("            // _sut = new " + classAnalysis.ClassName + "(/* inject dependencies */);");
            sb.AppendLine("        }");
            sb.AppendLine();
            
            // Generate test methods
            foreach (var method in classAnalysis.Methods.Where(m => m.IsPublic))
            {
                foreach (var testCase in method.SuggestedTestCases)
                {
                    sb.AppendLine(GenerateTestMethod(method, testCase, options));
                    sb.AppendLine();
                }
            }
            
            // Teardown
            sb.AppendLine($"        {GetTeardownAttributeName()}");
            sb.AppendLine("        public void Teardown()");
            sb.AppendLine("        {");
            sb.AppendLine("            // Cleanup");
            sb.AppendLine("        }");
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            return sb.ToString();
        }

        public string GenerateTestMethod(MethodAnalysis methodAnalysis, string testCaseName, GenerationOptions options)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine($"        {GetTestAttributeName()}");
            
            if (methodAnalysis.IsAsync)
            {
                sb.AppendLine($"        public async Task {testCaseName}()");
            }
            else
            {
                sb.AppendLine($"        public void {testCaseName}()");
            }
            
            sb.AppendLine("        {");
            sb.AppendLine("            // Arrange");
            
            if (options.GenerateTestData)
            {
                foreach (var param in methodAnalysis.Parameters)
                {
                    sb.AppendLine($"            var {param.Name} = {GenerateTestData(param.Type)};");
                }
            }
            
            sb.AppendLine("            var expected = /* TODO: Define expected result */;");
            sb.AppendLine();
            sb.AppendLine("            // Act");
            
            var paramList = string.Join(", ", methodAnalysis.Parameters.Select(p => p.Name));
            
            if (methodAnalysis.IsAsync)
            {
                if (methodAnalysis.ReturnType == "Task")
                {
                    sb.AppendLine($"            await _sut!.{methodAnalysis.MethodName}({paramList});");
                }
                else
                {
                    sb.AppendLine($"            var result = await _sut!.{methodAnalysis.MethodName}({paramList});");
                }
            }
            else
            {
                if (methodAnalysis.ReturnType == "void")
                {
                    sb.AppendLine($"            _sut!.{methodAnalysis.MethodName}({paramList});");
                }
                else
                {
                    sb.AppendLine($"            var result = _sut!.{methodAnalysis.MethodName}({paramList});");
                }
            }
            
            sb.AppendLine();
            sb.AppendLine("            // Assert");
            
            if (testCaseName.Contains("ShouldThrow"))
            {
                var exceptionType = ExtractExceptionType(testCaseName);
                sb.AppendLine($"            Assert.ThrowsException<{exceptionType}>(() => {{ /* TODO: Add test logic */ }});");
            }
            else if (methodAnalysis.ReturnType != "void" && methodAnalysis.ReturnType != "Task")
            {
                sb.AppendLine("            Assert.AreEqual(expected, result);");
            }
            else
            {
                sb.AppendLine("            Assert.Inconclusive(\"TODO: Implement test assertions\");");
            }
            
            sb.AppendLine("        }");
            
            return sb.ToString();
        }

        private string GenerateTestData(string type)
        {
            return type switch
            {
                "string" => "\"test\"",
                "int" => "42",
                "long" => "42L",
                "double" => "42.0",
                "decimal" => "42.0m",
                "float" => "42.0f",
                "bool" => "true",
                "DateTime" => "DateTime.Now",
                "Guid" => "Guid.NewGuid()",
                _ when type.Contains("List") => $"new {type}()",
                _ when type.Contains("[]") => $"new {type.Replace("[]", "[0]")}",
                _ => $"new {type}()"
            };
        }

        private string ExtractExceptionType(string testCaseName)
        {
            if (testCaseName.Contains("ArgumentNullException"))
                return "ArgumentNullException";
            if (testCaseName.Contains("InvalidOperationException"))
                return "InvalidOperationException";
            if (testCaseName.Contains("OperationCanceledException"))
                return "OperationCanceledException";
            return "Exception";
        }
    }
}
