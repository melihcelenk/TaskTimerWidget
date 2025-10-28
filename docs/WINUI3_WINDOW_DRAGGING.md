# WinUI 3 Custom Window Dragging Guide

## Problem
Dragging windows in WinUI 3 using Win32 P/Invoke is unstable, complex, and requires ~80 lines of code.

## Solution: Use `SetTitleBar()` - Native WinUI 3 API

**The correct and simplest approach for custom window dragging in WinUI 3.**

---

## Quick Implementation

### 1. XAML - Define Title Bar UI
```xaml
<!-- Your custom title bar element -->
<Grid x:Name="CustomTitleBar" Height="32" Background="#B8B8B8">
    <!-- Your title bar content -->
</Grid>
```

### 2. C# - Enable Dragging
```csharp
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        // Enable custom title bar for dragging
        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(CustomTitleBar);
    }
}
```

**That's it!** Just 2 lines of code. Window dragging is now handled by the system.

---

## How It Works

| Property | Purpose |
|----------|---------|
| `ExtendsContentIntoTitleBar = true` | Allows content to extend into system title bar area |
| `SetTitleBar(UIElement)` | Registers the UI element as the draggable title bar |

When you call `SetTitleBar()`:
- ✅ Window becomes draggable via that element
- ✅ System handles all drag logic smoothly
- ✅ Double-click-to-maximize works automatically
- ✅ Right-click context menu works
- ✅ DPI scaling handled correctly
- ✅ No manual pointer tracking needed

---

## Full Example: Widget with Custom Title Bar

```csharp
public sealed partial class WidgetWindow : Window
{
    public WidgetWindow()
    {
        this.InitializeComponent();
        InitializeCustomTitleBar();
    }

    private void InitializeCustomTitleBar()
    {
        // Enable dragging from custom title bar
        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(CustomTitleBar);

        // Configure AppWindow for widget appearance
        var appWindow = this.AppWindow;
        appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;

        // Set size
        appWindow.Resize(new SizeInt32(300, 500));
    }
}
```

```xaml
<Window x:Class="MyApp.WidgetWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <Grid RowSpacing="0">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>    <!-- Title Bar -->
            <RowDefinition Height="*"/>        <!-- Content -->
        </Grid.RowDefinitions>

        <!-- Custom Title Bar - Makes window draggable -->
        <Grid Grid.Row="0" x:Name="CustomTitleBar" Height="32"
              Background="#B8B8B8" Padding="12,4,4,4">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="Auto"/>
            </Grid.ColumnDefinitions>

            <TextBlock Grid.Column="0" Text="My Widget" VerticalAlignment="Center"/>
            <Button Grid.Column="1" Content="×" Width="24" Height="24"
                    Click="CloseButton_Click"/>
        </Grid>

        <!-- Content Area -->
        <StackPanel Grid.Row="1" Padding="12">
            <!-- Your widget content here -->
        </StackPanel>
    </Grid>
</Window>
```

---

## Comparison: Before vs After

### ❌ Wrong Way (Win32 P/Invoke)
```csharp
// ~80 lines of code needed
[DllImport("user32.dll")]
private static extern IntPtr GetActiveWindow();

[DllImport("user32.dll")]
private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

private bool _isDragging = false;
private Point _dragStartPoint = new();
private Point _lastPointerPos = new();

private void TitleBar_PointerPressed(object sender, PointerRoutedEventArgs e)
{
    // ... complex drag start logic ...
}

private void TitleBar_PointerMoved(object sender, PointerRoutedEventArgs e)
{
    // ... complex position calculation ...
}

private void TitleBar_PointerReleased(object sender, PointerRoutedEventArgs e)
{
    // ... complex cleanup ...
}
```

**Problems:**
- ❌ 80+ lines of code
- ❌ Unstable/jittery movement
- ❌ Complex pointer tracking
- ❌ Doesn't support double-click-to-maximize
- ❌ Manual DPI handling needed

### ✅ Right Way (SetTitleBar)
```csharp
// 2 lines of code
this.ExtendsContentIntoTitleBar = true;
this.SetTitleBar(CustomTitleBar);
```

**Benefits:**
- ✅ Just 2 lines
- ✅ Smooth, system-integrated dragging
- ✅ Double-click-to-maximize works
- ✅ Right-click context menu works
- ✅ Automatic DPI scaling
- ✅ Microsoft-recommended approach

---

## Important Notes

### What SetTitleBar Does
- Makes the specified UIElement draggable to move the window
- Does NOT remove that element from the layout
- The element remains interactive (buttons, text, etc. still work)

### IsHitTestVisible Behavior
If you want part of the title bar non-interactive (like just a drag area):
```csharp
// Only the background Grid is draggable, not the buttons
this.SetTitleBar(TitleBarGrid); // Grid contains button
// Buttons inside remain clickable due to their IsHitTestVisible = true
```

### With Opacity/Hidden Elements
When using `Opacity="0"` or `Visibility="Hidden"`:
- `SetTitleBar()` still works - the invisible element can still be dragged
- Perfect for widgets with hidden title bars that appear on hover

---

## Troubleshooting

### Window doesn't drag?
```csharp
// Make sure you set BOTH properties
this.ExtendsContentIntoTitleBar = true;  // Required
this.SetTitleBar(YourElement);           // Required
```

### Double-click doesn't maximize?
- This is system-integrated, should work automatically
- Check that `ExtendsContentIntoTitleBar = true`

### Element appears twice or not at all?
- `SetTitleBar()` doesn't remove the element from layout
- If showing twice, you have it both in XAML and SetTitleBar
- Just use in `SetTitleBar()`, not as separate UI

### Performance issues?
- SetTitleBar is optimized by the system
- If slow, check other UI performance issues, not dragging

---

## Official References

- [Microsoft: Title Bar Customization](https://learn.microsoft.com/en-us/windows/apps/develop/title-bar)
- [WinUI 3 Window Customization](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/windowing/windowing-overview)
- [ExtendsContentIntoTitleBar Property](https://learn.microsoft.com/en-us/windows/apps/api/microsoft.ui.xaml.window.extendscontentintotitlebar)

---

## Summary

| Need | Solution |
|------|----------|
| Custom window dragging | Use `SetTitleBar(element)` |
| Smooth system-integrated dragging | SetTitleBar automatically handles it |
| Double-click to maximize | Works by default with SetTitleBar |
| Code complexity | Just 2 lines |
| Recommended by Microsoft | ✅ Yes |

**Use `SetTitleBar()` for ALL custom window dragging in WinUI 3. It's the right way.**
