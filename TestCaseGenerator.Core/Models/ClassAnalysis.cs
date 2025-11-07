using System.Collections.Generic;

namespace TestCaseGenerator.Core.Models
{
    public class ClassAnalysis
    {
        public string ClassName { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public List<MethodAnalysis> Methods { get; set; } = new();
        public List<string> Interfaces { get; set; } = new();
        public List<PropertyInfo> Properties { get; set; } = new();
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public string? BaseClass { get; set; }
    }

    public class PropertyInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool HasGetter { get; set; }
        public bool HasSetter { get; set; }
    }
}
