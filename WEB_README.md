# Test Case Generator - Web Application

## 🌐 Beautiful Web Interface

A modern, responsive web application for generating .NET test cases with an intuitive UI.

## ✨ Features

### Modern UI
- 🎨 **Material Design** - Clean, professional interface using MudBlazor
- 📱 **Responsive** - Works on desktop, tablet, and mobile
- 🌙 **Dark Mode Ready** - Theme support built-in
- ⚡ **Real-time Updates** - Live progress indicators

### User Experience
- 📤 **Drag & Drop Upload** - Easy file selection
- ⚙️ **Visual Configuration** - Dropdowns and switches for all options
- 📊 **Interactive Charts** - Visual coverage metrics
- 💾 **One-Click Download** - Download generated tests instantly
- 📋 **Copy to Clipboard** - Quick code copying

### Analysis & Reporting
- 📈 **Coverage Dashboard** - Visual metrics and scores
- 🎯 **Method Analysis Table** - Detailed breakdown of all methods
- 💡 **Smart Recommendations** - AI-powered suggestions
- 🔍 **Code Preview** - Syntax-highlighted code display

## 🚀 Quick Start

### Option 1: Using Script (Recommended)
```powershell
.\run-web.ps1
```

### Option 2: Manual
```powershell
dotnet run --project TestCaseGenerator.Web
```

### Access the Application
Open your browser and navigate to:
- **HTTPS**: https://localhost:5001
- **HTTP**: http://localhost:5000

## 📖 How to Use

### Step 1: Upload Your C# File
1. Click the **"Choose C# File"** button
2. Select your `.cs` source file
3. See a preview of your code

### Step 2: Configure Options
- **Test Framework**: Choose NUnit, MSTest, or xUnit
- **Mocking Framework**: Select Moq, NSubstitute, or None
- **Test Type**: Pick Unit, Integration, API, or UI tests
- **Namespace**: Set your test namespace
- **Options**: Toggle test data generation and coverage analysis

### Step 3: Generate Tests
1. Click **"Generate Tests"** button
2. Watch real-time progress
3. Review the analysis results

### Step 4: Get Your Tests
- **Download**: Click "Download Test File" to save
- **Copy**: Click "Copy to Clipboard" for quick paste
- **Review**: See generated code with syntax highlighting

## 🎨 UI Components

### Dashboard Cards
- **Methods Count** - Total methods analyzed
- **Test Cases** - Number of generated tests
- **Avg Complexity** - Average cyclomatic complexity
- **Coverage Score** - Overall quality metric

### Analysis Table
- Method names with async indicators
- Return types and parameter counts
- Color-coded complexity badges
- Test case counts per method

### Coverage Report
- Overall coverage percentage
- Method-level breakdown
- Actionable recommendations
- Visual alerts and warnings

### Code Display
- Syntax-highlighted output
- Dark theme code editor
- Scrollable with line numbers
- Professional formatting

## 🎯 Features in Detail

### File Upload
- **Supported**: `.cs` files only
- **Max Size**: 5MB per file
- **Preview**: First 500 characters shown
- **Validation**: Automatic syntax checking

### Configuration Panel
All options from the console app:
- Test framework selection
- Mocking framework choice
- Test type specification
- Custom namespace
- Feature toggles

### Real-time Progress
- **Analyzing code structure** - Parsing phase
- **Extracting methods** - Analysis phase
- **Calculating coverage** - Metrics phase
- **Generating tests** - Generation phase

### Results Display
- **Metrics Cards** - Key statistics at a glance
- **Method Table** - Detailed method information
- **Coverage Analysis** - Quality recommendations
- **Generated Code** - Full test class output

## 🔧 Technical Details

### Technology Stack
- **Framework**: Blazor Server (.NET 6.0)
- **UI Library**: MudBlazor (Material Design)
- **Notifications**: Blazored Toast
- **Backend**: TestCaseGenerator.Core

### Architecture
```
Browser
    ↓ (SignalR)
Blazor Server
    ↓
TestCaseGenerator.Core
    ↓
Generated Tests
```

### Performance
- **Upload**: Instant (client-side)
- **Analysis**: < 1 second
- **Generation**: < 2 seconds
- **Total Time**: 2-3 seconds typical

## 📱 Responsive Design

### Desktop (1920px+)
- Full-width layout
- Side-by-side panels
- Large tables and charts

### Tablet (768px - 1919px)
- Stacked panels
- Optimized tables
- Touch-friendly buttons

### Mobile (< 768px)
- Single column layout
- Collapsible sections
- Mobile-optimized controls

## 🎨 Customization

### Theme Colors
Edit `MainLayout.razor` to customize:
```csharp
Primary = "#1976d2"    // Blue
Secondary = "#424242"  // Gray
Success = "#4caf50"    // Green
Error = "#f44336"      // Red
Warning = "#ff9800"    // Orange
Info = "#2196f3"       // Light Blue
```

### Styling
Modify `wwwroot/css/site.css` for custom styles.

## 🔒 Security

### Input Validation
- File type checking
- Size limitations
- Content validation

### Safe Processing
- No code execution
- Sandboxed analysis
- Read-only operations

## 🐛 Troubleshooting

### Port Already in Use
```powershell
# Change port in launchSettings.json
# Or kill the process using the port
netstat -ano | findstr :5000
taskkill /PID <process_id> /F
```

### Build Errors
```powershell
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Browser Not Opening
Manually navigate to:
- https://localhost:5001
- http://localhost:5000

## 📊 Comparison: Console vs Web

| Feature | Console App | Web App |
|---------|------------|---------|
| **Interface** | Terminal CLI | Browser GUI |
| **File Selection** | Type path | Click & upload |
| **Configuration** | Interactive prompts | Visual forms |
| **Results** | Text tables | Interactive charts |
| **Download** | Auto-save | One-click download |
| **Accessibility** | Local only | Network accessible |
| **Multi-user** | No | Yes (with hosting) |

## 🚀 Deployment

### Local Development
```powershell
dotnet run --project TestCaseGenerator.Web
```

### Production Build
```powershell
dotnet publish TestCaseGenerator.Web -c Release -o ./publish
```

### IIS Deployment
1. Publish the application
2. Copy to IIS wwwroot
3. Configure application pool (.NET CLR Version: No Managed Code)
4. Set permissions

### Azure Deployment
```powershell
# Using Azure CLI
az webapp up --name testgenerator --resource-group myResourceGroup
```

## 💡 Tips

### Best Practices
1. **Upload clean code** - Well-formatted source files
2. **Review results** - Always check generated tests
3. **Customize output** - Modify namespace and options
4. **Save locally** - Download tests for version control

### Keyboard Shortcuts
- **Ctrl+V** - Paste code (in supported browsers)
- **Ctrl+C** - Copy selected text
- **F5** - Refresh page
- **Ctrl+Shift+I** - Open developer tools

## 🎓 Learning Resources

### Included Examples
Use the example files to test:
1. Upload `Examples/Calculator.cs`
2. Generate tests
3. Review the output
4. Try `Examples/UserService.cs` next

### Documentation
- **README.md** - Main documentation
- **USAGE_GUIDE.md** - Detailed instructions
- **WEB_README.md** - This file

## 🌟 Advantages of Web UI

### User-Friendly
- No command-line knowledge needed
- Visual feedback at every step
- Intuitive interface

### Accessible
- Works from any device with a browser
- Can be hosted for team access
- No installation required (when hosted)

### Professional
- Modern, polished appearance
- Suitable for demos and presentations
- Client-ready interface

## 🔮 Future Enhancements

### Planned Features
- [ ] Batch file upload
- [ ] Project-level analysis
- [ ] Export reports to PDF
- [ ] Test execution integration
- [ ] User accounts and history
- [ ] API endpoint for CI/CD

## 📞 Support

### Getting Help
1. Check this documentation
2. Review browser console for errors
3. Check network tab for API issues
4. Verify file format and size

### Common Issues
- **Upload fails**: Check file size (max 5MB)
- **Generation errors**: Verify C# syntax
- **Display issues**: Try different browser
- **Performance**: Close other tabs

## 🎉 Summary

The web application provides a **modern, user-friendly interface** for test generation with:
- ✅ Beautiful Material Design UI
- ✅ Real-time progress updates
- ✅ Interactive results display
- ✅ One-click downloads
- ✅ Responsive design
- ✅ Professional appearance

**Perfect for users who prefer a graphical interface over command-line tools!**

---

**Start the web app**: `.\run-web.ps1`
**Access**: https://localhost:5001

Happy Testing! 🧪✨
