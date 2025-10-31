# Development Guide - Task Timer Widget

## 🛠️ Setup Instructions

### Prerequisites
- **Visual Studio 2022** (Community Edition or higher)
- **.NET 8.0 SDK** (Download from https://dotnet.microsoft.com/download)
- **Windows App SDK Tools** (Installed via Visual Studio)
- **Windows 10/11** Build 17763 or later

### Step 1: Install Required Components

1. **Download and Install Visual Studio 2022**
   - Download from: https://visualstudio.microsoft.com/downloads/
   - During installation, select these workloads:
     - "Desktop development with C++"
     - ".NET desktop development"
   - Also install the Windows App SDK component

2. **Install .NET 8.0 SDK**
   ```bash
   # Via https://dotnet.microsoft.com/download/dotnet/8.0
   # OR via winget:
   winget install Microsoft.DotNet.SDK.8
   ```

3. **Verify Installation**
   ```bash
   dotnet --version
   ```

### Step 2: Clone/Open the Project

```bash
cd C:\Kodlar\Desktop\TaskTimerWidget
```

### Step 3: Open in Visual Studio

1. Open Visual Studio 2022
2. Click "Open a project or solution"
3. Navigate to: `C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget\TaskTimerWidget.csproj`
4. Click "Open"

### Step 4: Restore Dependencies

```bash
dotnet restore src/TaskTimerWidget/TaskTimerWidget.csproj
```

Or in Visual Studio:
- Build > Clean Solution
- Build > Build Solution

## 🚀 Running the Application

### Option 1: Visual Studio
1. Press **F5** to start debugging
2. Application will launch in debug mode

### Option 2: Command Line
```bash
dotnet run --project src/TaskTimerWidget/TaskTimerWidget.csproj
```

### Option 3: Release Build
```bash
dotnet build -c Release src/TaskTimerWidget/TaskTimerWidget.csproj
```

## 📁 Project Structure Explanation

```
src/TaskTimerWidget/
├── App.xaml(.cs)              # Application entry point and initialization
├── MainWindow.xaml(.cs)       # Main UI window
├── Models/
│   └── Task.cs               # Task data model
├── ViewModels/
│   ├── ViewModelBase.cs      # Base class with INotifyPropertyChanged
│   ├── MainViewModel.cs      # Main application logic and state
│   └── TaskViewModel.cs      # Individual task UI logic
├── Services/
│   ├── ITaskService.cs       # Task management interface
│   ├── TaskService.cs        # Task management implementation
│   ├── IStorageService.cs    # Data persistence interface
│   └── StorageService.cs     # JSON file storage implementation
├── Helpers/
│   ├── RelayCommand.cs       # MVVM command implementation
│   └── ValueConverters.cs    # XAML value converters
├── Views/
│   └── MainWindow.xaml       # (Moved from root)
├── Assets/                    # Images, icons, etc.
└── TaskTimerWidget.csproj    # Project file
```

## 🔄 Development Workflow

### 1. Creating a New Feature

1. **Add Model if needed**
   ```csharp
   // Models/NewFeature.cs
   public class NewFeature { }
   ```

2. **Add Service if business logic needed**
   ```csharp
   // Services/INewService.cs
   public interface INewService { }

   // Services/NewService.cs
   public class NewService : INewService { }
   ```

3. **Create ViewModel**
   ```csharp
   // ViewModels/FeatureViewModel.cs
   public class FeatureViewModel : ViewModelBase { }
   ```

4. **Update MainViewModel if UI integration needed**

5. **Create XAML View if UI needed**
   ```xaml
   <!-- Views/FeatureView.xaml -->
   ```

6. **Register in DI Container (App.xaml.cs)**
   ```csharp
   services.AddSingleton<INewService, NewService>();
   services.AddSingleton<FeatureViewModel>();
   ```

### 2. Binding Data in XAML

```xaml
<!-- Binding to ViewModel property -->
<TextBlock Text="{Binding PropertyName}" />

<!-- Binding with converter -->
<TextBlock Text="{Binding IsRunning, Converter={StaticResource IsRunningConverter}}" />

<!-- Command binding -->
<Button Command="{Binding MyCommand}" />
```

### 3. Adding Event Handlers

In code-behind (MainWindow.xaml.cs):
```csharp
private void Button_Click(object sender, RoutedEventArgs e)
{
    // Handle event
}
```

## 🧪 Testing

### Unit Tests (Setup for future)

```bash
# Create test project
dotnet new xunit -n TaskTimerWidget.Tests

# Run tests
dotnet test
```

### Manual Testing Checklist

- [ ] Application starts without errors
- [ ] Can add a new task
- [ ] Task appears in list
- [ ] Clicking task changes color to yellow
- [ ] Timer counts up when task is active
- [ ] Switching tasks pauses previous one
- [ ] Can delete tasks
- [ ] Application closes cleanly
- [ ] Tasks persist after restart

## 📝 Code Style Guide

Follow CLAUDE.md for:
- Naming conventions
- Code formatting
- MVVM patterns
- Exception handling
- Documentation

### Quick Reference
- **Classes**: `PascalCase` (e.g., `TaskViewModel`)
- **Methods**: `PascalCase` (e.g., `GetTaskAsync()`)
- **Properties**: `PascalCase` (e.g., `IsRunning`)
- **Private fields**: `_camelCase` (e.g., `_taskService`)
- **Local variables**: `camelCase` (e.g., `elapsedTime`)

## 🐛 Debugging

### Visual Studio Debugging

1. Set breakpoint by clicking on line number
2. Press **F5** to start debugging
3. Use **Debug** menu for step commands
4. Use **Watch** window to inspect variables
5. Use **Immediate Window** (Ctrl+Alt+I) for live commands

### Log Files

Logs are saved to:
```
%LOCALAPPDATA%\TaskTimerWidget\Logs\
```

View logs:
```powershell
# PowerShell
Get-Content "$env:LOCALAPPDATA\TaskTimerWidget\Logs\app-*.txt" -Tail 50
```

## 🔗 Important Links

- **WinUI 3 Documentation**: https://docs.microsoft.com/en-us/windows/apps/winui/winui3/
- **.NET 8.0 Docs**: https://docs.microsoft.com/en-us/dotnet/
- **MVVM Pattern**: https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/x-bind-markup-extension
- **Serilog Logging**: https://serilog.net/

## 📚 Additional Resources

- [Visual Studio Documentation](https://docs.microsoft.com/en-us/visualstudio/)
- [C# Language Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [XAML Overview](https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/xaml-overview)
- [Async/Await Tutorial](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)

## ⚠️ Common Issues and Solutions

### Issue: "NuGet package not found"
**Solution**: Run `dotnet restore` to download packages

### Issue: "Window doesn't show up"
**Solution**: Check App.xaml.cs MainWindow creation code

### Issue: "Binding not working"
**Solution**:
- Verify DataContext is set correctly
- Check property names match exactly
- Ensure INotifyPropertyChanged is implemented

### Issue: "Application crashes on startup"
**Solution**:
- Check logs in %LOCALAPPDATA%\TaskTimerWidget\Logs\
- Use Debug > Windows > Exception Settings to break on exceptions
- Check InitializeViewModel() in MainWindow.xaml.cs

## 🚀 Performance Optimization

### Tips
1. Use virtualization for large lists (VirtualizingStackPanel)
2. Avoid heavy operations on UI thread (use async/await)
3. Cache objects when possible
4. Monitor with Windows Task Manager
5. Profile with Visual Studio Profiler

### Memory Leak Prevention
- Unsubscribe from events
- Dispose timers and resources
- Don't hold references to disposed objects

## 📋 Committing Code

Before committing:
1. Follow code standards (CLAUDE.md)
2. Write clear commit messages
3. Test your changes
4. Remove debug code/logging
5. Update documentation if needed

Example commit:
```
git commit -m "feat: Add task timer functionality

- Implemented DispatcherTimer for task timing
- Added timer display update logic
- Fixed pause/resume behavior

Closes #1"
```

## 🔄 Git Workflow

```bash
# Create feature branch
git checkout -b feature/my-feature

# Make changes and commit
git add .
git commit -m "feat: description"

# Push to remote
git push origin feature/my-feature

# Create pull request on GitHub
```

## 🛠️ Development Commands (CLI Shortcuts)

### Build and Run

```bash
# Build Debug
cd C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget
dotnet build --configuration Debug

# Build Release
dotnet build --configuration Release

# Run application
dotnet run --configuration Debug
```

### Application Control

```powershell
# Kill running app instance (PowerShell)
powershell -NoProfile -Command "Get-Process TaskTimerWidget -ErrorAction Ignore | Stop-Process -Force -ErrorAction Ignore; Start-Sleep -Milliseconds 500"

# Launch app directly
cd "C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget\bin\Debug\net8.0-windows10.0.19041.0"
start TaskTimerWidget.exe
```

### Quick Build + Test Cycle

```bash
# From project root, after code changes:
cd C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget
dotnet build --configuration Debug 2>&1 | tail -5
```

### Git Operations

```bash
# Check status
git status

# Stage all changes
git add -A

# View diff
git diff src/TaskTimerWidget/Views/MainWindow.xaml

# Commit with message
git commit -m "Faz X.X: Description"

# View log
git log --oneline | head -5
```

### Troubleshooting Build Errors

**File locked error** (app still running):
```powershell
# Kill app and wait before rebuilding
powershell -NoProfile -Command "Get-Process TaskTimerWidget -ErrorAction Ignore | Stop-Process -Force -ErrorAction Ignore; Start-Sleep -Milliseconds 500"
```

**Clean rebuild** (if stuck):
```bash
cd C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget
dotnet clean
dotnet build --configuration Debug
```

---

## 🎯 WinUI 3 Resources and Guides

### Window Customization & Dragging

**Guide**: [WinUI 3 Custom Window Dragging](./docs/WINUI3_WINDOW_DRAGGING.md)

Quick reference for implementing smooth window dragging in WinUI 3 applications:
- Using `SetTitleBar()` native API (recommended)
- Complete examples and code snippets
- Troubleshooting guide
- Applicable to all C# WinUI 3 projects

**Use Case**: Widget-style applications, custom title bars, frameless windows

### Other WinUI 3 Common Tasks

- **Window Sizing & Positioning**: Check AppWindow in Microsoft docs
- **Theme Support**: ExtendsContentIntoTitleBar with BackgroundColor
- **DPI Awareness**: WinUI 3 handles automatically with SetTitleBar
- **System Integration**: Double-click maximize, right-click menu (automatic with SetTitleBar)

---

**Last Updated**: October 28, 2025
**Maintainer**: Development Team
**Status**: Active Development
