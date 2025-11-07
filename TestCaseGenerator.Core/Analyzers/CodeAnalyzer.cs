using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestCaseGenerator.Core.Models;

namespace TestCaseGenerator.Core.Analyzers
{
    public class CodeAnalyzer
    {
        public async Task<ClassAnalysis> AnalyzeClassAsync(string filePath)
        {
            var code = await File.ReadAllTextAsync(filePath);
            return AnalyzeClass(code);
        }

        public ClassAnalysis AnalyzeClass(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();

            var classDeclaration = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault();

            if (classDeclaration == null)
            {
                throw new InvalidOperationException("No class found in the provided code.");
            }

            var analysis = new ClassAnalysis
            {
                ClassName = classDeclaration.Identifier.Text,
                Namespace = GetNamespace(classDeclaration),
                IsAbstract = classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword)),
                IsSealed = classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.SealedKeyword))
            };

            // Analyze base class
            if (classDeclaration.BaseList != null)
            {
                var baseTypes = classDeclaration.BaseList.Types;
                foreach (var baseType in baseTypes)
                {
                    var typeName = baseType.Type.ToString();
                    if (typeName.StartsWith("I") && char.IsUpper(typeName[1]))
                    {
                        analysis.Interfaces.Add(typeName);
                    }
                    else
                    {
                        analysis.BaseClass = typeName;
                    }
                }
            }

            // Analyze properties
            var properties = classDeclaration.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>();

            foreach (var property in properties)
            {
                analysis.Properties.Add(new PropertyInfo
                {
                    Name = property.Identifier.Text,
                    Type = property.Type.ToString(),
                    HasGetter = property.AccessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)) ?? false,
                    HasSetter = property.AccessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)) ?? false
                });
            }

            // Analyze methods
            var methods = classDeclaration.DescendantNodes()
                .OfType<MethodDeclarationSyntax>();

            foreach (var method in methods)
            {
                analysis.Methods.Add(AnalyzeMethod(method));
            }

            return analysis;
        }

        private MethodAnalysis AnalyzeMethod(MethodDeclarationSyntax method)
        {
            var analysis = new MethodAnalysis
            {
                MethodName = method.Identifier.Text,
                ReturnType = method.ReturnType.ToString(),
                IsAsync = method.Modifiers.Any(m => m.IsKind(SyntaxKind.AsyncKeyword)),
                IsPublic = method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)),
                IsStatic = method.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword))
            };

            // Analyze parameters
            foreach (var param in method.ParameterList.Parameters)
            {
                analysis.Parameters.Add(new ParameterInfo
                {
                    Name = param.Identifier.Text,
                    Type = param.Type?.ToString() ?? "object",
                    IsNullable = param.Type?.ToString().EndsWith("?") ?? false,
                    HasDefaultValue = param.Default != null,
                    DefaultValue = param.Default?.Value.ToString()
                });
            }

            // Calculate cyclomatic complexity
            analysis.CyclomaticComplexity = CalculateCyclomaticComplexity(method);

            // Analyze dependencies (constructor parameters, method calls)
            var identifiers = method.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .Select(i => i.Identifier.Text)
                .Distinct()
                .Where(i => char.IsUpper(i[0])) // Likely type names
                .ToList();

            analysis.Dependencies.AddRange(identifiers);

            // Generate suggested test cases
            analysis.SuggestedTestCases = GenerateTestCaseSuggestions(analysis);

            return analysis;
        }

        private int CalculateCyclomaticComplexity(MethodDeclarationSyntax method)
        {
            int complexity = 1; // Base complexity

            var decisionPoints = method.DescendantNodes().Where(node =>
                node.IsKind(SyntaxKind.IfStatement) ||
                node.IsKind(SyntaxKind.WhileStatement) ||
                node.IsKind(SyntaxKind.ForStatement) ||
                node.IsKind(SyntaxKind.ForEachStatement) ||
                node.IsKind(SyntaxKind.CaseSwitchLabel) ||
                node.IsKind(SyntaxKind.CatchClause) ||
                node.IsKind(SyntaxKind.LogicalAndExpression) ||
                node.IsKind(SyntaxKind.LogicalOrExpression) ||
                node.IsKind(SyntaxKind.ConditionalExpression)
            );

            complexity += decisionPoints.Count();
            return complexity;
        }

        private List<string> GenerateTestCaseSuggestions(MethodAnalysis method)
        {
            var suggestions = new List<string>();

            // Basic test case
            suggestions.Add($"{method.MethodName}_WithValidInput_ShouldReturnExpectedResult");

            // Null parameter tests
            foreach (var param in method.Parameters.Where(p => !p.Type.EndsWith("?")))
            {
                if (param.Type == "string" || param.Type.Contains("[]") || !IsValueType(param.Type))
                {
                    suggestions.Add($"{method.MethodName}_With{CapitalizeFirst(param.Name)}Null_ShouldThrowArgumentNullException");
                }
            }

            // Edge cases for numeric parameters
            foreach (var param in method.Parameters.Where(p => IsNumericType(p.Type)))
            {
                suggestions.Add($"{method.MethodName}_With{CapitalizeFirst(param.Name)}Zero_ShouldHandleCorrectly");
                suggestions.Add($"{method.MethodName}_With{CapitalizeFirst(param.Name)}Negative_ShouldHandleCorrectly");
            }

            // String parameter edge cases
            foreach (var param in method.Parameters.Where(p => p.Type == "string"))
            {
                suggestions.Add($"{method.MethodName}_With{CapitalizeFirst(param.Name)}Empty_ShouldHandleCorrectly");
            }

            // Collection parameter edge cases
            foreach (var param in method.Parameters.Where(p => p.Type.Contains("List") || p.Type.Contains("[]") || p.Type.Contains("IEnumerable")))
            {
                suggestions.Add($"{method.MethodName}_With{CapitalizeFirst(param.Name)}Empty_ShouldHandleCorrectly");
            }

            // Async method tests
            if (method.IsAsync)
            {
                suggestions.Add($"{method.MethodName}_WhenCancelled_ShouldThrowOperationCanceledException");
            }

            // Complex methods need more test cases
            if (method.CyclomaticComplexity > 5)
            {
                suggestions.Add($"{method.MethodName}_WithComplexScenario_ShouldHandleAllBranches");
            }

            return suggestions;
        }

        private string GetNamespace(ClassDeclarationSyntax classDeclaration)
        {
            var namespaceDeclaration = classDeclaration.Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault();

            if (namespaceDeclaration != null)
            {
                return namespaceDeclaration.Name.ToString();
            }

            var fileScopedNamespace = classDeclaration.Ancestors()
                .OfType<FileScopedNamespaceDeclarationSyntax>()
                .FirstOrDefault();

            return fileScopedNamespace?.Name.ToString() ?? "DefaultNamespace";
        }

        private bool IsValueType(string type)
        {
            var valueTypes = new[] { "int", "long", "short", "byte", "sbyte", "uint", "ulong", "ushort", 
                                    "float", "double", "decimal", "bool", "char", "DateTime", "TimeSpan", "Guid" };
            return valueTypes.Contains(type);
        }

        private bool IsNumericType(string type)
        {
            var numericTypes = new[] { "int", "long", "short", "byte", "sbyte", "uint", "ulong", "ushort", 
                                      "float", "double", "decimal" };
            return numericTypes.Contains(type);
        }

        private string CapitalizeFirst(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
