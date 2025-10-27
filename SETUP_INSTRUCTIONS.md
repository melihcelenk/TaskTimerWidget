# Setup Instructions - Task Timer Widget

## ⚠️ Important: Before Opening in Visual Studio

Bu bölüm kurulumdan önce yapılması gereken adımları anlatır.

### 1. Assets Klasörü Kontrol Et

```bash
# Windows PowerShell'de çalıştır:
cd C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget
ls Assets
```

Eğer Assets klasörü boşsa, aşağıdaki dummy placeholder dosyalarını oluştur:

### 2. Required Visual Studio Components

Visual Studio 2022'yi açta şu workload'ları yükle:
- ✅ Desktop development with C++
- ✅ .NET desktop development
- ✅ Windows App SDK templates

### 3. NuGet Package Restore

```bash
# Project klasöründe çalıştır:
cd C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget
dotnet restore TaskTimerWidget.csproj
```

### 4. Build Test

```bash
# Debug build:
dotnet build -c Debug TaskTimerWidget.csproj

# Release build:
dotnet build -c Release TaskTimerWidget.csproj
```

## 🐛 Known Issues and Fixes

### Issue 1: Assets Not Found
**Error**: `Package.appxmanifest` assets missing
**Fix**: Run this to create placeholder assets:

```powershell
# PowerShell'de çalıştır:
$assetsPath = "C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget\Assets"
mkdir $assetsPath -Force

# Create placeholder images (1x1 transparent PNG)
$emptyPng = @(137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 1, 0, 0, 0, 1, 8, 6, 0, 0, 0, 31, 21, 196, 137, 0, 0, 0, 10, 73, 68, 65, 84, 8, 153, 1, 0, 1, 0, 0, 255, 255, 255, 0, 1, 0, 1, 38, 5, 165, 47, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130)
[byte[]]$bytes = $emptyPng

# Placeholder dosyaları oluştur
$bytes | Set-Content "$assetsPath\StoreLogo.png" -Encoding Byte -Force
$bytes | Set-Content "$assetsPath\Square150x150Logo.png" -Encoding Byte -Force
$bytes | Set-Content "$assetsPath\Square44x44Logo.png" -Encoding Byte -Force
$bytes | Set-Content "$assetsPath\SplashScreen.png" -Encoding Byte -Force

Write-Host "Assets created successfully!"
```

### Issue 2: WinUI Not Found
**Error**: `Microsoft.WindowsAppSDK not found`
**Fix**:
1. Visual Studio → Tools → Manage NuGet Packages
2. Search: "Windows App SDK"
3. Install latest version
4. Or run: `dotnet add package Microsoft.WindowsAppSDK`

### Issue 3: Build Fails on Manifest
**Error**: `Package.appxmanifest validation errors`
**Fix**: Manifest temporarily simplified for development:
- For Store submission, update Package.appxmanifest with real values
- See DEVELOPMENT.md for Store-specific setup

## ✅ Verification Steps

Kurulumdan sonra test et:

```bash
# 1. Restore packages
dotnet restore

# 2. Build debug
dotnet build

# 3. Run application
dotnet run
```

## 📋 If Build Still Fails

### Check 1: .NET Version
```bash
dotnet --version
# Should be 8.0.x or higher
```

### Check 2: Project File
```bash
# Validate .csproj syntax:
type TaskTimerWidget.csproj
# Ensure no XML syntax errors
```

### Check 3: Dependencies
```bash
# List all package versions:
dotnet list package
```

### Check 4: Clean Build
```bash
# Full clean and rebuild:
dotnet clean
dotnet restore
dotnet build -c Debug -v detailed
```

## 🆘 Still Having Issues?

1. **Clear all cache**:
   ```bash
   rm -r bin obj
   dotnet clean
   dotnet restore
   ```

2. **Update VS Tools**:
   - Visual Studio → Help → Check for Updates
   - Install all available updates

3. **Check Logs**:
   - Build output: View → Output
   - Detailed logs: Add `-v detailed` flag

4. **Reference DEVELOPMENT.md**:
   - See "Common Issues and Solutions" section

## 🚀 Next Steps

Once build succeeds:
1. Open in Visual Studio: File → Open → TaskTimerWidget.csproj
2. Press F5 to run
3. Test application functionality
4. Follow TODO.md for development phases

---

**Created**: October 27, 2025
**For Version**: 0.1.0
**Status**: Development Phase
