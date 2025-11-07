# Architecture Overview

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         USER INTERFACE                          │
│                                                                 │
│  ┌───────────────────────────────────────────────────────────┐ │
│  │              TestCaseGenerator (Console App)              │ │
│  │                                                           │ │
│  │  • Interactive CLI (Spectre.Console)                     │ │
│  │  • Menu System                                           │ │
│  │  • Progress Indicators                                   │ │
│  │  • Visual Reports                                        │ │
│  └───────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                         CORE LIBRARY                            │
│                   TestCaseGenerator.Core                        │
│                                                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐        │
│  │  Analyzers   │  │  Generators  │  │  Templates   │        │
│  │              │  │              │  │              │        │
│  │ • Code       │  │ • Test       │  │ • NUnit      │        │
│  │   Analyzer   │  │   Generator  │  │ • MSTest     │        │
│  │ • Coverage   │  │              │  │ • xUnit      │        │
│  │   Analyzer   │  │              │  │              │        │
│  └──────────────┘  └──────────────┘  └──────────────┘        │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │                       Models                             │  │
│  │                                                          │  │
│  │  • ClassAnalysis    • MethodAnalysis                    │  │
│  │  • GenerationOptions • TestFramework                    │  │
│  │  • MockingFramework  • TestType                         │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                    EXTERNAL DEPENDENCIES                        │
│                                                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐        │
│  │   Roslyn     │  │   Spectre    │  │  Newtonsoft  │        │
│  │ (CodeAnalysis)│  │  (Console)   │  │    (Json)    │        │
│  └──────────────┘  └──────────────┘  └──────────────┘        │
└─────────────────────────────────────────────────────────────────┘
```

## Component Breakdown

### 1. Console Application Layer
**Responsibility**: User interaction and presentation

```
Program.cs
├── ShowBanner()              # Display welcome screen
├── ShowMainMenu()            # Main menu navigation
├── GenerateTestsFlow()       # Test generation workflow
├── AnalyzeCodeFlow()         # Code analysis workflow
├── BatchGenerateFlow()       # Batch processing workflow
├── GetGenerationOptions()    # Configuration input
├── DisplayClassAnalysis()    # Show analysis results
└── DisplayCoverageReport()   # Show coverage metrics
```

### 2. Core Library Layer
**Responsibility**: Business logic and test generation

#### Analyzers
```
CodeAnalyzer
├── AnalyzeClass()           # Parse and analyze C# class
├── AnalyzeMethod()          # Analyze individual methods
├── CalculateCyclomaticComplexity()
├── GenerateTestCaseSuggestions()
└── GetNamespace()

CoverageAnalyzer
├── AnalyzeCoverage()        # Calculate coverage metrics
├── CalculateCoverageScore() # Score individual methods
└── GenerateRecommendations()# Provide actionable advice
```

#### Generators
```
TestGenerator
├── GenerateTestsAsync()     # Main generation method
├── GenerateTests()          # Synchronous version
├── AnalyzeCode()            # Code analysis
└── GetTemplate()            # Template selection
```

#### Templates
```
ITestTemplate (Interface)
├── GenerateTestClass()
├── GenerateTestMethod()
├── GetTestAttributeName()
├── GetTestClassAttributeName()
├── GetSetupAttributeName()
└── GetTeardownAttributeName()

Implementations:
├── NUnitTemplate
├── MSTestTemplate
└── XUnitTemplate
```

## Data Flow

### Test Generation Flow
```
1. User Input
   ↓
2. File Reading
   ↓
3. Code Parsing (Roslyn)
   ↓
4. Syntax Tree Analysis
   ↓
5. Method/Class Extraction
   ↓
6. Complexity Calculation
   ↓
7. Test Case Suggestion
   ↓
8. Template Selection
   ↓
9. Code Generation
   ↓
10. Coverage Analysis
    ↓
11. File Output
    ↓
12. Report Display
```

### Analysis Flow
```
Source Code
    ↓
Roslyn Parser
    ↓
Syntax Tree
    ↓
┌─────────────────┐
│ Extract:        │
│ • Classes       │
│ • Methods       │
│ • Parameters    │
│ • Dependencies  │
│ • Properties    │
└─────────────────┘
    ↓
┌─────────────────┐
│ Calculate:      │
│ • Complexity    │
│ • Test Cases    │
│ • Coverage      │
└─────────────────┘
    ↓
Analysis Results
```

## Class Relationships

### Core Models
```
ClassAnalysis
├── ClassName: string
├── Namespace: string
├── Methods: List<MethodAnalysis>
├── Properties: List<PropertyInfo>
├── Interfaces: List<string>
├── IsAbstract: bool
├── IsSealed: bool
└── BaseClass: string?

MethodAnalysis
├── MethodName: string
├── ReturnType: string
├── Parameters: List<ParameterInfo>
├── IsAsync: bool
├── IsPublic: bool
├── IsStatic: bool
├── SuggestedTestCases: List<string>
├── Dependencies: List<string>
└── CyclomaticComplexity: int

GenerationOptions
├── TestFramework: TestFramework
├── MockingFramework: MockingFramework
├── TestType: TestType
├── GenerateTestData: bool
├── IncludeCoverageAnalysis: bool
├── OutputDirectory: string
└── Namespace: string
```

## Design Patterns Used

### 1. Strategy Pattern
**Templates**: Different test generation strategies for each framework
```
ITestTemplate
    ↓
┌───────────┬───────────┬───────────┐
│   NUnit   │  MSTest   │   xUnit   │
└───────────┴───────────┴───────────┘
```

### 2. Template Method Pattern
**Test Generation**: Common structure with framework-specific details
```
GenerateTestClass()
    ↓
├── Add Using Statements
├── Create Namespace
├── Create Test Class
├── Add Fields
├── Add Setup Method
├── Generate Test Methods
└── Add Teardown Method
```

### 3. Builder Pattern
**Test Code Construction**: Building test code incrementally
```
StringBuilder
    ↓
Add Using Statements
    ↓
Add Namespace
    ↓
Add Class Declaration
    ↓
Add Methods
    ↓
Build Final String
```

### 4. Factory Pattern
**Template Selection**: Creating appropriate template based on framework
```
GetTemplate(framework)
    ↓
switch (framework)
    ↓
├── MSTest → new MSTestTemplate()
├── NUnit  → new NUnitTemplate()
└── XUnit  → new XUnitTemplate()
```

## Extension Points

### Adding New Test Framework
```csharp
// 1. Add to enum
public enum TestFramework
{
    MSTest,
    NUnit,
    XUnit,
    NewFramework  // ← Add here
}

// 2. Create template
public class NewFrameworkTemplate : ITestTemplate
{
    // Implement interface methods
}

// 3. Update factory
private ITestTemplate GetTemplate(TestFramework framework)
{
    return framework switch
    {
        // ... existing cases
        TestFramework.NewFramework => new NewFrameworkTemplate(),
        _ => throw new ArgumentException()
    };
}
```

### Adding New Mocking Framework
```csharp
// 1. Add to enum
public enum MockingFramework
{
    Moq,
    NSubstitute,
    NewMocking  // ← Add here
}

// 2. Update templates to handle new framework
// in GenerateTestClass() method
```

### Adding Custom Test Patterns
```csharp
// In CodeAnalyzer.GenerateTestCaseSuggestions()
private List<string> GenerateTestCaseSuggestions(MethodAnalysis method)
{
    var suggestions = new List<string>();
    
    // Add your custom patterns here
    suggestions.Add($"{method.MethodName}_YourCustomPattern");
    
    return suggestions;
}
```

## Performance Considerations

### Optimization Strategies
1. **Lazy Loading**: Parse only when needed
2. **Caching**: Cache parsed syntax trees
3. **Parallel Processing**: Batch operations can be parallelized
4. **Memory Management**: Dispose of large objects promptly

### Scalability
- **Small Files**: < 1 second per file
- **Medium Files**: 1-3 seconds per file
- **Large Files**: 3-10 seconds per file
- **Batch Processing**: Linear scaling with file count

## Security Considerations

### Input Validation
- File path validation
- Code syntax validation
- Output directory permissions

### Safe Code Generation
- No code execution
- Read-only file analysis
- Controlled output generation

## Testing Strategy

### Unit Tests (Recommended)
```
TestCaseGenerator.Core.Tests/
├── Analyzers/
│   ├── CodeAnalyzerTests.cs
│   └── CoverageAnalyzerTests.cs
├── Generators/
│   └── TestGeneratorTests.cs
└── Templates/
    ├── NUnitTemplateTests.cs
    ├── MSTestTemplateTests.cs
    └── XUnitTemplateTests.cs
```

### Integration Tests
- End-to-end test generation
- File I/O operations
- Template rendering

## Deployment

### Build Configuration
```
Debug:   Development and testing
Release: Production deployment
```

### Distribution Options
1. **Source Code**: Clone and build
2. **Published Binary**: Self-contained executable
3. **NuGet Package**: Core library as package
4. **VS Extension**: Future enhancement

## Dependencies Graph

```
TestCaseGenerator (Console)
    ↓ depends on
TestCaseGenerator.Core (Library)
    ↓ depends on
┌────────────────────────────────┐
│ Microsoft.CodeAnalysis.CSharp  │
│ Newtonsoft.Json                │
└────────────────────────────────┘

TestCaseGenerator (Console)
    ↓ also depends on
┌────────────────────────────────┐
│ Spectre.Console                │
└────────────────────────────────┘
```

## Configuration Flow

```
User Input
    ↓
GenerationOptions
    ↓
┌─────────────────────────┐
│ • TestFramework         │
│ • MockingFramework      │
│ • TestType              │
│ • OutputDirectory       │
│ • Namespace             │
│ • GenerateTestData      │
│ • IncludeCoverageAnalysis│
└─────────────────────────┘
    ↓
TestGenerator
    ↓
Generated Tests
```

## Error Handling Strategy

```
Try-Catch Blocks
    ↓
┌─────────────────────────┐
│ File Not Found          │
│ Invalid Syntax          │
│ Parsing Errors          │
│ Generation Errors       │
│ I/O Errors              │
└─────────────────────────┘
    ↓
User-Friendly Messages
    ↓
Graceful Degradation
```

## Future Architecture Enhancements

### Planned Improvements
1. **Plugin System**: Load custom templates dynamically
2. **Configuration Files**: JSON/YAML configuration support
3. **API Layer**: RESTful API for remote generation
4. **Web Interface**: Browser-based UI
5. **VS Extension**: IDE integration
6. **CI/CD Integration**: GitHub Actions, Azure DevOps

### Extensibility Points
- Custom template loaders
- Additional analyzers
- Report formatters
- Output writers
- Configuration providers

---

This architecture provides a **solid foundation** for test generation while remaining **flexible and extensible** for future enhancements.
