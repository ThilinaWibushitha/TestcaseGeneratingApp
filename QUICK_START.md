# Quick Start - Test Case Generator

## 🚀 Get Started in 3 Minutes

### Step 1: Build (30 seconds)
```powershell
.\build.ps1
```
Or manually:
```powershell
dotnet restore
dotnet build
```

### Step 2: Run (10 seconds)
```powershell
.\run.ps1
```
Or manually:
```powershell
dotnet run --project TestCaseGenerator
```

### Step 3: Generate Your First Test (2 minutes)

1. **Select**: Choose "Generate Tests"
2. **Path**: Enter `Examples/Calculator.cs`
3. **Framework**: Choose `NUnit` (or your preference)
4. **Mocking**: Choose `None` (Calculator has no dependencies)
5. **Type**: Choose `Unit`
6. **Output**: Enter `./GeneratedTests`
7. **Namespace**: Enter `Examples.Tests`
8. **Confirm**: Yes to test data and coverage analysis

**Done!** Check `./GeneratedTests/CalculatorTests.cs`

## 📋 What You Get

### Project Structure
```
testcase generating tools/
├── TestCaseGenerator/          # Console app
├── TestCaseGenerator.Core/     # Core engine
├── Examples/                   # Sample files
├── GeneratedTests/             # Your tests appear here
└── Documentation files
```

### Key Features
- ✅ **3 Test Frameworks**: MSTest, NUnit, xUnit
- ✅ **2 Mocking Frameworks**: Moq, NSubstitute
- ✅ **Code Analysis**: Complexity & coverage metrics
- ✅ **Smart Suggestions**: Automatic test case generation
- ✅ **Batch Processing**: Multiple files at once

## 🎯 Try These Examples

### Example 1: Simple Class (No Dependencies)
```powershell
# File: Examples/Calculator.cs
# Framework: NUnit
# Mocking: None
# Result: ~8 test methods generated
```

### Example 2: Service with Dependencies
```powershell
# File: Examples/UserService.cs
# Framework: NUnit
# Mocking: Moq
# Result: ~15+ test methods with mock setup
```

## 💡 Common Commands

### Build
```powershell
dotnet build
```

### Run
```powershell
dotnet run --project TestCaseGenerator
```

### Clean
```powershell
dotnet clean
```

### Publish
```powershell
dotnet publish TestCaseGenerator -c Release -o ./publish
```

## 📚 Next Steps

1. ✅ **Try the examples** - Use provided Calculator.cs and UserService.cs
2. ✅ **Review generated tests** - See what gets created
3. ✅ **Test your own code** - Point to your C# files
4. ✅ **Customize output** - Modify generated tests as needed
5. ✅ **Read full docs** - Check README.md and USAGE_GUIDE.md

## 🔧 Troubleshooting

### Build Fails?
```powershell
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Can't Find Examples?
```powershell
# Check you're in the right directory
cd "c:\Users\ASNIT-PC\Desktop\testcase generating tools"
```

### Generated Tests Don't Compile?
- Add missing using statements
- Install test framework NuGet packages
- Configure mock behaviors

## 📖 Documentation Files

- **README.md** - Complete overview and features
- **USAGE_GUIDE.md** - Detailed usage instructions
- **FEATURES.md** - Full feature list
- **QUICK_START.md** - This file

## 🎉 You're Ready!

Run `.\run.ps1` and start generating tests!

---

**Need Help?** Check the full documentation in README.md
