using System.Collections.Generic;

namespace TestCaseGenerator.Core.Models
{
    public class MethodAnalysis
    {
        public string MethodName { get; set; } = string.Empty;
        public string ReturnType { get; set; } = string.Empty;
        public List<ParameterInfo> Parameters { get; set; } = new();
        public bool IsAsync { get; set; }
        public bool IsPublic { get; set; }
        public bool IsStatic { get; set; }
        public List<string> SuggestedTestCases { get; set; } = new();
        public List<string> Dependencies { get; set; } = new();
        public int CyclomaticComplexity { get; set; }
    }

    public class ParameterInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public bool HasDefaultValue { get; set; }
        public string? DefaultValue { get; set; }
    }
}
