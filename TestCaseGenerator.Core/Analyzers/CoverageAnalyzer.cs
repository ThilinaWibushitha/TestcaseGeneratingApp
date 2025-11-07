using System;
using System.Collections.Generic;
using System.Linq;
using TestCaseGenerator.Core.Models;

namespace TestCaseGenerator.Core.Analyzers
{
    public class CoverageAnalyzer
    {
        public CoverageReport AnalyzeCoverage(ClassAnalysis classAnalysis)
        {
            var report = new CoverageReport
            {
                ClassName = classAnalysis.ClassName,
                TotalMethods = classAnalysis.Methods.Count,
                PublicMethods = classAnalysis.Methods.Count(m => m.IsPublic)
            };

            foreach (var method in classAnalysis.Methods)
            {
                var methodCoverage = new MethodCoverage
                {
                    MethodName = method.MethodName,
                    CyclomaticComplexity = method.CyclomaticComplexity,
                    SuggestedTestCount = method.SuggestedTestCases.Count,
                    TestCases = method.SuggestedTestCases
                };

                // Calculate coverage score based on complexity and test cases
                methodCoverage.CoverageScore = CalculateCoverageScore(method);
                
                report.MethodCoverages.Add(methodCoverage);
            }

            // Calculate overall coverage
            report.OverallCoverageScore = report.MethodCoverages.Any() 
                ? report.MethodCoverages.Average(m => m.CoverageScore) 
                : 0;

            // Generate recommendations
            report.Recommendations = GenerateRecommendations(report);

            return report;
        }

        private double CalculateCoverageScore(MethodAnalysis method)
        {
            // Base score
            double score = 50.0;

            // Add points for test cases
            score += Math.Min(method.SuggestedTestCases.Count * 10, 40);

            // Adjust for complexity
            if (method.CyclomaticComplexity > 10)
            {
                score -= 10;
            }
            else if (method.CyclomaticComplexity > 5)
            {
                score -= 5;
            }

            // Ensure score is between 0 and 100
            return Math.Max(0, Math.Min(100, score));
        }

        private List<string> GenerateRecommendations(CoverageReport report)
        {
            var recommendations = new List<string>();

            // Check for low coverage methods
            var lowCoverageMethods = report.MethodCoverages
                .Where(m => m.CoverageScore < 60)
                .ToList();

            if (lowCoverageMethods.Any())
            {
                recommendations.Add($"⚠️ {lowCoverageMethods.Count} method(s) have low test coverage. Consider adding more test cases.");
            }

            // Check for complex methods
            var complexMethods = report.MethodCoverages
                .Where(m => m.CyclomaticComplexity > 10)
                .ToList();

            if (complexMethods.Any())
            {
                recommendations.Add($"🔍 {complexMethods.Count} method(s) have high cyclomatic complexity (>10). Consider refactoring or adding comprehensive tests.");
                foreach (var method in complexMethods.Take(3))
                {
                    recommendations.Add($"   - {method.MethodName} (Complexity: {method.CyclomaticComplexity})");
                }
            }

            // Check overall coverage
            if (report.OverallCoverageScore < 70)
            {
                recommendations.Add("📊 Overall coverage score is below 70%. Aim for at least 80% coverage.");
            }
            else if (report.OverallCoverageScore >= 90)
            {
                recommendations.Add("✅ Excellent coverage! Your test suite looks comprehensive.");
            }

            // Check for methods without tests
            var methodsWithoutTests = report.MethodCoverages
                .Where(m => m.SuggestedTestCount == 0)
                .ToList();

            if (methodsWithoutTests.Any())
            {
                recommendations.Add($"❌ {methodsWithoutTests.Count} method(s) have no suggested test cases.");
            }

            return recommendations;
        }
    }

    public class CoverageReport
    {
        public string ClassName { get; set; } = string.Empty;
        public int TotalMethods { get; set; }
        public int PublicMethods { get; set; }
        public double OverallCoverageScore { get; set; }
        public List<MethodCoverage> MethodCoverages { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }

    public class MethodCoverage
    {
        public string MethodName { get; set; } = string.Empty;
        public int CyclomaticComplexity { get; set; }
        public int SuggestedTestCount { get; set; }
        public double CoverageScore { get; set; }
        public List<string> TestCases { get; set; } = new();
    }
}
