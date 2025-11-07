# .NET Test Case Generator

A comprehensive, intelligent test case generation tool for .NET Framework that supports multiple testing frameworks, mocking libraries, and provides code analysis with coverage insights.

## 🚀 Features

### Core Capabilities
- ✅ **Multiple Test Frameworks**: MSTest, NUnit, xUnit
- ✅ **Mocking Framework Support**: Moq, NSubstitute
- ✅ **Code Analysis**: Automatic code analysis with cyclomatic complexity calculation
- ✅ **Smart Test Suggestions**: AI-powered test case suggestions based on code structure
- ✅ **Template-Based Generation**: Customizable templates for different test types
- ✅ **Coverage Analysis**: Built-in coverage analysis and recommendations
- ✅ **Test Data Generation**: Automatic test data generation for different types
- ✅ **Batch Processing**: Generate tests for multiple files at once

### Test Types Supported
- Unit Tests
- Integration Tests
- API Tests
- UI Tests

## 📋 Requirements

- .NET 6.0 SDK or later
- Visual Studio 2022 or Visual Studio Code
- Windows, macOS, or Linux

## 🛠️ Installation

### Option 1: Build from Source

1. Clone or download this repository
2. Open terminal in the project directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Build the solution:
   ```bash
   dotnet build
   ```
5. Run the application:
   ```bash
   dotnet run --project TestCaseGenerator
   ```

### Option 2: Use Pre-built Binary

1. Build the release version:
   ```bash
   dotnet publish TestCaseGenerator -c Release -o ./publish
   ```
2. Navigate to the `publish` folder and run:
   ```bash
   ./TestCaseGenerator
   ```

## 📖 Usage

### Interactive Mode

Run the application without arguments to enter interactive mode:

```bash
dotnet run --project TestCaseGenerator
```

The interactive menu provides the following options:

1. **Generate Tests**: Generate test cases for a single C# file
2. **Analyze Code**: Analyze code without generating tests
3. **Batch Generate**: Generate tests for all C# files in a directory
4. **Exit**: Exit the application

### Command-Line Usage

You can also use the tool programmatically in your own projects:

```csharp
using TestCaseGenerator.Core.Generators;
using TestCaseGenerator.Core.Models;

var generator = new TestGenerator();
var options = new GenerationOptions
{
    TestFramework = TestFramework.NUnit,
    MockingFramework = MockingFramework.Moq,
    TestType = TestType.Unit,
    OutputDirectory = "./Tests",
    Namespace = "MyProject.Tests",
    GenerateTestData = true,
    IncludeCoverageAnalysis = true
};

var testCode = await generator.GenerateTestsAsync("path/to/source.cs", options);
```

## 🎯 Examples

### Example 1: Simple Calculator Class

**Source Code** (`Calculator.cs`):
```csharp
namespace MyApp
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Divide(int a, int b)
        {
            if (b == 0)
                throw new ArgumentException("Cannot divide by zero");
            return a / b;
        }
    }
}
```

**Generated Test** (NUnit with Moq):
```csharp
using NUnit.Framework;
using System;
using MyApp;

namespace Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        private Calculator? _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Calculator();
        }

        [Test]
        public void Add_WithValidInput_ShouldReturnExpectedResult()
        {
            // Arrange
            var a = 42;
            var b = 42;
            var expected = 84;

            // Act
            var result = _sut!.Add(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Divide_WithBZero_ShouldHandleCorrectly()
        {
            // Arrange
            var a = 42;
            var b = 0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _sut!.Divide(a, b));
        }
    }
}
```

### Example 2: Service with Dependencies

**Source Code** (`UserService.cs`):
```csharp
namespace MyApp.Services
{
    public interface IUserRepository
    {
        User GetById(int id);
    }

    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public User GetUser(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid user ID");
            
            return _repository.GetById(id);
        }
    }
}
```

The tool will automatically:
- Detect the `IUserRepository` dependency
- Generate mock setup code
- Create test cases for valid and invalid inputs
- Include null checks and edge cases

## 🔧 Configuration

### Generation Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `TestFramework` | Enum | NUnit | Test framework to use (MSTest, NUnit, XUnit) |
| `MockingFramework` | Enum | Moq | Mocking framework (Moq, NSubstitute, None) |
| `TestType` | Enum | Unit | Type of tests to generate |
| `OutputDirectory` | String | "" | Where to save generated tests |
| `Namespace` | String | "Tests" | Namespace for test classes |
| `GenerateTestData` | Boolean | true | Auto-generate test data |
| `IncludeCoverageAnalysis` | Boolean | true | Include coverage analysis |

## 📊 Coverage Analysis

The tool provides comprehensive coverage analysis including:

- **Cyclomatic Complexity**: Measures code complexity
- **Coverage Score**: Estimates test coverage quality
- **Recommendations**: Actionable suggestions for improvement
- **Method-Level Insights**: Detailed analysis per method

### Coverage Score Calculation

- Base score: 50%
- +10% per suggested test case (max +40%)
- -5% for complexity 5-10
- -10% for complexity >10

### Recommendations Include

- Low coverage warnings
- High complexity alerts
- Missing test case identification
- Best practice suggestions

## 🎨 Supported Test Patterns

### Test Case Naming Conventions

The tool follows industry-standard naming:
```
MethodName_Scenario_ExpectedBehavior
```

Examples:
- `Add_WithValidInput_ShouldReturnSum`
- `GetUser_WithNullId_ShouldThrowArgumentNullException`
- `ProcessAsync_WhenCancelled_ShouldThrowOperationCanceledException`

### Generated Test Cases Include

1. **Happy Path**: Valid input scenarios
2. **Null Checks**: Null parameter validation
3. **Edge Cases**: Boundary conditions
4. **Exception Handling**: Error scenarios
5. **Async Patterns**: Cancellation and timeout handling
6. **Collection Tests**: Empty, null, and populated collections

## 🏗️ Architecture

```
TestCaseGenerator/
├── TestCaseGenerator.Core/          # Core library
│   ├── Analyzers/                   # Code analysis
│   │   ├── CodeAnalyzer.cs         # Roslyn-based analyzer
│   │   └── CoverageAnalyzer.cs     # Coverage analysis
│   ├── Generators/                  # Test generation
│   │   └── TestGenerator.cs        # Main generator
│   ├── Models/                      # Data models
│   │   ├── ClassAnalysis.cs
│   │   ├── MethodAnalysis.cs
│   │   └── GenerationOptions.cs
│   └── Templates/                   # Test templates
│       ├── ITestTemplate.cs
│       ├── NUnitTemplate.cs
│       ├── MSTestTemplate.cs
│       └── XUnitTemplate.cs
└── TestCaseGenerator/               # Console application
    └── Program.cs                   # CLI interface
```

## 🔍 How It Works

1. **Code Analysis**: Uses Roslyn to parse C# source code
2. **Pattern Detection**: Identifies methods, parameters, dependencies
3. **Complexity Calculation**: Computes cyclomatic complexity
4. **Test Suggestion**: Generates intelligent test case suggestions
5. **Template Application**: Applies framework-specific templates
6. **Code Generation**: Produces ready-to-use test code
7. **Coverage Analysis**: Evaluates test coverage quality

## 🤝 Contributing

Contributions are welcome! Areas for improvement:

- Additional test frameworks
- More mocking frameworks
- Custom template support
- Configuration file support
- IDE integration
- More test patterns

## 📝 License

This project is provided as-is for educational and commercial use.

## 🐛 Known Limitations

- Requires valid C# syntax
- One class per file recommended
- Complex generic types may need manual adjustment
- Some edge cases in async/await patterns

## 💡 Tips

1. **Start Small**: Test on simple classes first
2. **Review Generated Tests**: Always review and customize generated tests
3. **Iterative Improvement**: Use coverage analysis to identify gaps
4. **Customize Templates**: Modify templates for your coding standards
5. **Batch Processing**: Use batch mode for large codebases

## 📞 Support

For issues, questions, or suggestions:
- Review the examples in this README
- Check the generated coverage reports
- Examine the suggested test cases

## 🎓 Best Practices

1. **Don't Rely Solely on Generated Tests**: Use as a starting point
2. **Add Business Logic Tests**: Generated tests cover structure, not business rules
3. **Review Mock Setups**: Ensure mocks match real behavior
4. **Customize Test Data**: Replace generic test data with meaningful values
5. **Maintain Tests**: Update tests when code changes

## 🚦 Quick Start

```bash
# Clone the repository
git clone <repository-url>

# Navigate to the directory
cd "testcase generating tools"

# Restore and build
dotnet restore
dotnet build

# Run the tool
dotnet run --project TestCaseGenerator

# Follow the interactive prompts
# 1. Select "Generate Tests"
# 2. Enter path to your C# file
# 3. Choose test framework (NUnit/MSTest/XUnit)
# 4. Choose mocking framework (Moq/NSubstitute)
# 5. Select test type (Unit/Integration/API/UI)
# 6. Specify output directory
# 7. Review generated tests!
```

## 🎉 Success!

You now have a powerful test generation tool that will:
- Save hours of manual test writing
- Ensure consistent test patterns
- Identify coverage gaps
- Suggest edge cases you might miss
- Generate production-ready test scaffolding

Happy Testing! 🧪✨
