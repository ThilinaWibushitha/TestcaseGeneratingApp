# Test Case Generator - Usage Guide

## Quick Start Guide

### Step 1: Build the Project

Open terminal in the project directory and run:

```bash
dotnet restore
dotnet build
```

### Step 2: Run the Application

```bash
dotnet run --project TestCaseGenerator
```

### Step 3: Try the Examples

The tool comes with example files in the `Examples/` directory:

1. **Calculator.cs** - Simple class with mathematical operations
2. **UserService.cs** - Service class with dependencies and async methods

## Detailed Walkthrough

### Example 1: Generate Tests for Calculator

1. Run the application
2. Select **"Generate Tests"**
3. Enter path: `Examples/Calculator.cs`
4. Choose test framework: **NUnit** (or your preference)
5. Choose mocking framework: **None** (Calculator has no dependencies)
6. Select test type: **Unit**
7. Enter output directory: `./GeneratedTests`
8. Enter namespace: `Examples.Tests`
9. Confirm test data generation: **Yes**
10. Confirm coverage analysis: **Yes**

**Result**: You'll see:
- Code analysis results
- Generated test cases for each method
- Coverage analysis with recommendations
- Test file saved to `./GeneratedTests/CalculatorTests.cs`

### Example 2: Generate Tests for UserService

1. Run the application
2. Select **"Generate Tests"**
3. Enter path: `Examples/UserService.cs`
4. Choose test framework: **NUnit**
5. Choose mocking framework: **Moq** (UserService has dependencies)
6. Select test type: **Unit**
7. Enter output directory: `./GeneratedTests`
8. Enter namespace: `Examples.Tests`
9. Confirm test data generation: **Yes**
10. Confirm coverage analysis: **Yes**

**Result**: You'll see:
- Automatic detection of `IUserRepository` and `IEmailService` dependencies
- Mock setup code generated
- Test cases for all public methods
- Edge cases for null checks, invalid inputs
- Async test patterns
- Coverage analysis showing complexity metrics

## Understanding the Output

### Generated Test Structure

```csharp
[TestFixture]
public class CalculatorTests
{
    private Calculator? _sut;  // System Under Test
    
    [SetUp]
    public void Setup()
    {
        _sut = new Calculator();
    }
    
    [Test]
    public void Add_WithValidInput_ShouldReturnExpectedResult()
    {
        // Arrange - Setup test data
        var a = 42;
        var b = 42;
        var expected = 84;
        
        // Act - Execute the method
        var result = _sut!.Add(a, b);
        
        // Assert - Verify the result
        Assert.That(result, Is.EqualTo(expected));
    }
}
```

### Coverage Analysis Explained

**Coverage Score**: 0-100% indicating test quality
- **90-100%**: Excellent coverage
- **70-89%**: Good coverage
- **50-69%**: Moderate coverage
- **Below 50%**: Needs improvement

**Cyclomatic Complexity**: Measures code complexity
- **1-5**: Simple, easy to test
- **6-10**: Moderate complexity
- **11+**: High complexity, needs more tests

**Recommendations**: Actionable suggestions
- Low coverage warnings
- High complexity alerts
- Missing test scenarios
- Best practices

## Advanced Usage

### Batch Processing

Generate tests for an entire directory:

1. Select **"Batch Generate"**
2. Enter directory path: `./Examples`
3. Configure options (same as single file)
4. Watch as tests are generated for all `.cs` files

### Code Analysis Only

Analyze code without generating tests:

1. Select **"Analyze Code"**
2. Enter file path
3. Review:
   - Class structure
   - Method signatures
   - Complexity metrics
   - Suggested test cases
   - Coverage analysis

## Customizing Generated Tests

### Step 1: Review Generated Code

Generated tests are templates. Always review and customize:

```csharp
// Generated (needs customization)
var expected = /* TODO: Define expected result */;

// Customized
var expected = 84;
```

### Step 2: Update Mock Setups

```csharp
// Generated (needs setup)
_mockUserRepository = new Mock<IUserRepository>();

// Customized with behavior
_mockUserRepository = new Mock<IUserRepository>();
_mockUserRepository
    .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
    .ReturnsAsync(new User { Id = 1, Name = "Test" });
```

### Step 3: Add Business Logic Tests

Generated tests cover structure. Add tests for:
- Business rules
- Integration scenarios
- Performance requirements
- Security constraints

## Test Framework Differences

### NUnit
```csharp
[TestFixture]
public class MyTests
{
    [SetUp] public void Setup() { }
    [Test] public void MyTest() { }
    [TearDown] public void Teardown() { }
}
```

### MSTest
```csharp
[TestClass]
public class MyTests
{
    [TestInitialize] public void Setup() { }
    [TestMethod] public void MyTest() { }
    [TestCleanup] public void Teardown() { }
}
```

### xUnit
```csharp
public class MyTests : IDisposable
{
    public MyTests() { } // Constructor = Setup
    [Fact] public void MyTest() { }
    public void Dispose() { } // Dispose = Teardown
}
```

## Mocking Framework Differences

### Moq
```csharp
var mock = new Mock<IUserRepository>();
mock.Setup(x => x.GetById(1)).Returns(user);
var result = mock.Object.GetById(1);
mock.Verify(x => x.GetById(1), Times.Once);
```

### NSubstitute
```csharp
var mock = Substitute.For<IUserRepository>();
mock.GetById(1).Returns(user);
var result = mock.GetById(1);
mock.Received(1).GetById(1);
```

## Common Scenarios

### Scenario 1: Testing Async Methods

Generated code handles async automatically:

```csharp
[Test]
public async Task GetUserAsync_WithValidId_ShouldReturnUser()
{
    // Arrange
    var id = 1;
    
    // Act
    var result = await _sut!.GetUserAsync(id);
    
    // Assert
    Assert.That(result, Is.Not.Null);
}
```

### Scenario 2: Testing Exceptions

```csharp
[Test]
public void Divide_WithZeroDivisor_ShouldThrowArgumentException()
{
    // Arrange
    var a = 10;
    var b = 0;
    
    // Act & Assert
    Assert.Throws<ArgumentException>(() => _sut!.Divide(a, b));
}
```

### Scenario 3: Testing with Mocks

```csharp
[Test]
public async Task CreateUser_WithValidData_ShouldCallRepository()
{
    // Arrange
    var name = "John Doe";
    var email = "john@example.com";
    
    _mockUserRepository
        .Setup(x => x.CreateAsync(It.IsAny<User>()))
        .ReturnsAsync(new User { Id = 1, Name = name, Email = email });
    
    // Act
    var result = await _sut!.CreateUserAsync(name, email);
    
    // Assert
    Assert.That(result.Name, Is.EqualTo(name));
    _mockUserRepository.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
}
```

## Tips for Best Results

### 1. Code Quality Matters
- Well-structured code generates better tests
- Clear method names improve test names
- Good separation of concerns = easier mocking

### 2. Review and Refine
- Generated tests are starting points
- Add domain-specific test cases
- Customize test data to match real scenarios

### 3. Use Coverage Analysis
- Identify untested code paths
- Focus on high-complexity methods
- Follow recommendations

### 4. Maintain Tests
- Update tests when code changes
- Remove obsolete tests
- Keep test data relevant

### 5. Combine with Manual Testing
- Use generated tests for structure
- Add manual tests for business logic
- Include integration tests

## Troubleshooting

### Issue: "No class found in the provided code"
**Solution**: Ensure the file contains a valid C# class definition

### Issue: Generated tests don't compile
**Solution**: 
- Check namespace references
- Ensure dependencies are installed
- Review mock setups

### Issue: Coverage score is low
**Solution**:
- Add more test cases manually
- Test edge cases
- Cover all code paths

### Issue: Mock setup is incomplete
**Solution**:
- Review dependency interfaces
- Add specific mock behaviors
- Configure return values

## Next Steps

1. **Generate tests** for your own code
2. **Review** the generated tests
3. **Customize** test data and assertions
4. **Run** the tests in your test runner
5. **Iterate** based on coverage reports

## Support

For questions or issues:
1. Check this guide
2. Review the README.md
3. Examine the example files
4. Study the generated code

Happy Testing! 🎉
