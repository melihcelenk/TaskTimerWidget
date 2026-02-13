# Build & Publish Guide - Microsoft Store

## Prerequisites

- **Visual Studio 2022** (with .NET desktop development & Windows App SDK workloads)
- **Windows 10/11 SDK** (10.0.19041.0+)
- **.NET 8.0 SDK**
- **Microsoft Partner Center account** (for Store publishing)

## 1. Pre-Build Checklist

Before building a release:

- [ ] Update version in `Package.appxmanifest` (`<Identity Version="x.x.x.0" />`)
- [ ] Update version in `TaskTimerWidget.csproj` (`<Version>` and `<InformationalVersion>`)
- [ ] Update `README.md` (version, date, changelog)
- [ ] Update `STORE_LISTING.md` if features changed
- [ ] All changes committed and pushed

## 2. Build MSIX Package

### Option A: Visual Studio (Recommended)

1. Open `TaskTimerWidget.csproj` in Visual Studio 2022
2. Set configuration to **Release | x64**
3. Right-click project > **Publish** > **Create App Packages...**
4. Select **Microsoft Store under a new app name** or **Sideloading**
   - For Store: Sign in with your Partner Center account
   - For Sideloading: Select to create packages for sideloading
5. Select **x64** architecture
6. Click **Create**
7. Output will be in `src/TaskTimerWidget/AppPackages/`

### Option B: Command Line

```bash
# Navigate to project directory
cd src/TaskTimerWidget

# Build Release MSIX package
dotnet publish TaskTimerWidget.csproj -c Release -p:Platform=x64
```

### Option C: MSBuild (for MSIX bundle)

```bash
msbuild TaskTimerWidget.csproj /p:Configuration=Release /p:Platform=x64 /p:GenerateAppxPackageOnBuild=true /p:AppxBundle=Always /p:AppxBundlePlatforms=x64
```

## 3. Store Association (First Time Only)

If not already associated with a Store app:

1. Visual Studio > Right-click project > **Publish** > **Associate App with the Store...**
2. Sign in to Partner Center
3. Select your app (or create new)
4. This generates `Package.StoreAssociation.xml`

## 4. Upload to Microsoft Store

### Partner Center Portal

1. Go to [Partner Center](https://partner.microsoft.com/dashboard)
2. Navigate to **Apps and games** > **Task Timer Widget**
3. Click **Start update** (or create new submission)

### Packages Tab

4. Click **Packages**
5. Upload the `.msixupload` or `.msix` file from `AppPackages/` folder
   - File is typically at: `AppPackages/TaskTimerWidget_x.x.x.0_x64_Test/TaskTimerWidget_x.x.x.0_x64.msix`
6. Wait for upload validation to complete

### Store Listing Tab

7. Update **What's new in this version** with release notes
8. Update screenshots if UI changed
9. Update description if features changed (see `STORE_LISTING.md`)

### Submission

10. Review all tabs (Pricing, Properties, Age ratings, etc.)
11. Click **Submit to the Store**
12. Wait for certification (typically 1-3 business days)

## 5. Create GitHub Release

After Store submission:

```bash
# Create tag
git tag v1.1.0

# Push tag
git push origin v1.1.0

# Create release with MSIX asset
gh release create v1.1.0 \
  --title "Task Timer Widget v1.1.0 - Manual Time Adjustment" \
  --notes-file docs/general/RELEASE_NOTES_v1.1.0.md \
  "AppPackages/TaskTimerWidget_1.1.0.0_x64.msix"
```

## 6. Post-Release

- [ ] Verify Store listing is live
- [ ] Verify GitHub release page
- [ ] Update website if needed
- [ ] Announce on social media / GitHub Discussions

## Troubleshooting

### "ProcessorArchitecture neutral" Error
```bash
# Always specify Platform for MSIX builds
dotnet build TaskTimerWidget.csproj -p:Platform=x64
```

### Package Validation Fails
- Ensure `Package.appxmanifest` version is higher than the current Store version
- Ensure all referenced assets (icons, splash screen) exist in `Assets/`
- Ensure `AppxPackageSigningEnabled` is `False` for Store submissions (Store signs it)

### Store Certification Rejection
- Check for crash-on-launch issues
- Ensure privacy policy URL is accessible
- Ensure app description matches actual functionality
- Review [Store Policies](https://learn.microsoft.com/en-us/windows/uwp/publish/store-policies)

## Key Files

| File | Purpose |
|------|---------|
| `Package.appxmanifest` | App identity, version, capabilities |
| `TaskTimerWidget.csproj` | Build config, SDK versions |
| `Package.StoreAssociation.xml` | Store association (don't commit secrets) |
| `docs/general/STORE_LISTING.md` | Store description templates |
| `docs/general/RELEASE_NOTES_*.md` | Release notes per version |
