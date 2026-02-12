using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Windows.UI;
using TaskTimerWidget.ViewModels;
using Serilog;
using System.Linq;
using Microsoft.UI.Xaml.Media;

namespace TaskTimerWidget
{
    /// <summary>
    /// Main application window with task management UI.
    /// Handles user interactions for tasks and timer management.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private MainViewModel? _viewModel;
        private AppWindow? _appWindow;
        private TaskViewModel? _editingTask;
        private int _editingTaskIndex = -1;
        private TaskViewModel? _changeTimeTask;
        private int _changeTimeTaskIndex = -1;
        private bool _isCompactMode = false;
        private bool _isMouseOver = false;
        private bool _isWindowActive = false;
        private bool _isTitleBarTransitioning = false;
        private const int NORMAL_HEIGHT = 500;
        private const int TITLEBAR_HEIGHT = 32;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();
            InitializeViewModel();
            SubscribeToWindowEvents();
        }

        /// <summary>
        /// Initializes window properties and settings.
        /// </summary>
        private void InitializeWindow()
        {
            try
            {
                _appWindow = this.AppWindow;
                if (_appWindow != null)
                {
                    // Set window size for widget appearance
                    _appWindow.Resize(new SizeInt32(220, 500));
                    Log.Information("MainWindow resized to 220x500");

                    // Set window to always-on-top (widget behavior)
                    var presenter = _appWindow.Presenter as Microsoft.UI.Windowing.OverlappedPresenter;
                    if (presenter != null)
                    {
                        presenter.IsAlwaysOnTop = true;
                        Log.Information("Window set to always-on-top");
                    }

                    // Set window icon for taskbar
                    try
                    {
                        var iconPath = System.IO.Path.Combine(AppContext.BaseDirectory, "app.ico");
                        if (System.IO.File.Exists(iconPath))
                        {
                            _appWindow.SetIcon(iconPath);
                            Log.Information("Window icon set to: {IconPath}", iconPath);
                        }
                        else
                        {
                            Log.Warning("Icon file not found at: {IconPath}", iconPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Could not set window icon");
                    }

                    // Configure title bar to look like a widget (no minimize/maximize buttons)
                    var titleBar = _appWindow.TitleBar;
                    if (titleBar != null)
                    {
                        // Extend content into title bar area (removes default chrome)
                        titleBar.ExtendsContentIntoTitleBar = true;

                        // Collapse title bar height to hide minimize/maximize buttons
                        // Only close button will remain visible
                        titleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;

                        Log.Information("Window configured as widget (minimize/maximize buttons hidden)");
                    }

                    // Set the custom title bar for dragging support
                    // This uses the native WinUI 3 API for smooth, system-integrated window dragging
                    try
                    {
                        this.ExtendsContentIntoTitleBar = true;
                        this.SetTitleBar(TitleBarGrid);
                        Log.Information("Custom title bar set for dragging");
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Could not set custom title bar");
                    }
                }
                else
                {
                    Log.Warning("AppWindow not available");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing MainWindow");
            }
        }

        /// <summary>
        /// Subscribe to window activation state changes (Sticky Notes style)
        /// </summary>
        private void SubscribeToWindowEvents()
        {
            // Window_Activated event is subscribed in XAML
            Log.Information("Window activation events subscription ready");
        }

        /// <summary>
        /// Handle window activation state changes (Sticky Notes style)
        /// </summary>
        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            try
            {
                _isWindowActive = args.WindowActivationState != WindowActivationState.Deactivated;
                UpdateTitleBarVisibility();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error handling window activation");
            }
        }

        /// <summary>
        /// Handles mouse entering the main grid
        /// </summary>
        private void MainGrid_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _isMouseOver = true;
            UpdateTitleBarVisibility();
        }

        /// <summary>
        /// Handles mouse leaving the main grid
        /// </summary>
        private void MainGrid_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _isMouseOver = false;
            UpdateTitleBarVisibility();
        }

        /// <summary>
        /// Update title bar visibility based on window activation state and mouse position
        /// In compact mode: TitleBar is GONE (Height=0) unless window is active OR mouse is over
        /// In normal mode: TitleBar uses opacity (visible when active, invisible when inactive)
        /// </summary>
        private async void UpdateTitleBarVisibility()
        {
            try
            {
                if (_isCompactMode)
                {
                    // Prevent concurrent transitions that could cause size calculation errors
                    if (_isTitleBarTransitioning)
                    {
                        Log.Information("TitleBar transition already in progress, skipping");
                        return;
                    }

                    // Compact mode: Show TitleBar only if window is active OR mouse is over
                    bool shouldShow = _isWindowActive || _isMouseOver;
                    bool wasTitleBarVisible = TitleBarGrid.Visibility == Visibility.Visible;

                    if (shouldShow && !wasTitleBarVisible)
                    {
                        _isTitleBarTransitioning = true;

                        // Show TitleBar (restore height and visibility)
                        TitleBarGrid.Height = TITLEBAR_HEIGHT;
                        TitleBarGrid.Visibility = Visibility.Visible;
                        TitleBarGrid.Opacity = 1.0;
                        TitleBarGrid.IsHitTestVisible = true;

                        // Increase window height by TITLEBAR_HEIGHT
                        if (_appWindow != null)
                        {
                            var currentSize = _appWindow.Size;
                            _appWindow.Resize(new SizeInt32(currentSize.Width, currentSize.Height + TITLEBAR_HEIGHT));
                        }

                        // Small delay to complete transition
                        await System.Threading.Tasks.Task.Delay(50);
                        _isTitleBarTransitioning = false;

                        Log.Information($"Compact mode - TitleBar shown, window height increased by {TITLEBAR_HEIGHT}px");

                        // After transition, check if state changed and re-evaluate
                        // This handles the case where mouse left quickly during transition
                        bool shouldStillShow = _isWindowActive || _isMouseOver;
                        if (!shouldStillShow && TitleBarGrid.Visibility == Visibility.Visible)
                        {
                            Log.Information("State changed during show transition, hiding TitleBar");
                            UpdateTitleBarVisibility();
                        }
                    }
                    else if (!shouldShow && wasTitleBarVisible)
                    {
                        _isTitleBarTransitioning = true;

                        // First: Decrease window height by TITLEBAR_HEIGHT
                        // This ensures the task remains fully visible during the transition
                        if (_appWindow != null)
                        {
                            var currentSize = _appWindow.Size;
                            _appWindow.Resize(new SizeInt32(currentSize.Width, currentSize.Height - TITLEBAR_HEIGHT));
                        }

                        // Then: Hide TitleBar (GONE - no space taken)
                        // Small delay to ensure resize happens first
                        await System.Threading.Tasks.Task.Delay(10);

                        TitleBarGrid.Height = 0;
                        TitleBarGrid.Visibility = Visibility.Collapsed;
                        TitleBarGrid.IsHitTestVisible = false;

                        // Additional delay to complete transition
                        await System.Threading.Tasks.Task.Delay(40);
                        _isTitleBarTransitioning = false;

                        Log.Information($"Compact mode - TitleBar hidden, window height decreased by {TITLEBAR_HEIGHT}px");

                        // After transition, check if state changed and re-evaluate
                        // This handles the case where mouse entered quickly during transition
                        bool shouldStillHide = !_isWindowActive && !_isMouseOver;
                        if (!shouldStillHide && TitleBarGrid.Visibility == Visibility.Collapsed)
                        {
                            Log.Information("State changed during hide transition, showing TitleBar");
                            UpdateTitleBarVisibility();
                        }
                    }

                    Log.Information($"Compact mode - Title bar visibility: {shouldShow}, isActive={_isWindowActive}, isMouseOver={_isMouseOver}");
                }
                else
                {
                    // Normal mode: Use opacity (keep space reserved)
                    TitleBarGrid.Opacity = _isWindowActive ? 1.0 : 0.0;

                    if (!_isWindowActive)
                    {
                        TitleBarGrid.IsHitTestVisible = false;
                    }
                    else
                    {
                        await System.Threading.Tasks.Task.Delay(100);
                        TitleBarGrid.IsHitTestVisible = true;
                    }

                    Log.Information($"Normal mode - Title bar opacity: {TitleBarGrid.Opacity}, isActive={_isWindowActive}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating title bar visibility");
            }
        }

        /// <summary>
        /// Initializes the ViewModel and binds it to the view.
        /// </summary>
        private async void InitializeViewModel()
        {
            try
            {
                _viewModel = App.GetService<MainViewModel>();

                // Try to set DataContext for binding
                try
                {
                    (this.Content as FrameworkElement)!.DataContext = _viewModel;
                }
                catch
                {
                    Log.Warning("Could not set DataContext via FrameworkElement");
                }

                // Load tasks
                if (_viewModel != null)
                {
                    await _viewModel.InitializeAsync();
                    Log.Information("ViewModel initialized and data loaded");

                    // Update UI
                    UpdateEmptyState();
                    UpdateStatusBar();

                    // Subscribe to property changes
                    _viewModel.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == nameof(_viewModel.Tasks) ||
                            args.PropertyName == nameof(_viewModel.ErrorMessage))
                        {
                            UpdateEmptyState();
                            UpdateStatusBar();
                        }
                    };

                    // Subscribe to collection changes (for ObservableCollection Items)
                    _viewModel.Tasks.CollectionChanged += (sender, args) =>
                    {
                        UpdateEmptyState();
                        UpdateStatusBar();
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing ViewModel");
            }
        }

        /// <summary>
        /// Handles the Close button click event.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Minimize button click event.
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Minimize window to taskbar
            var presenter = _appWindow?.Presenter as Microsoft.UI.Windowing.OverlappedPresenter;
            if (presenter != null)
            {
                presenter.Minimize();
                Log.Information("Window minimized to taskbar");
            }
        }

        /// <summary>
        /// Handles right-click on the Change Time card to close it and show context menu.
        /// </summary>
        private void ChangeTimeBorder_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            CloseChangeTimeCard();
            e.Handled = true;
        }

        /// <summary>
        /// Handles taps on the Change Time card itself to close it.
        /// Button clicks inside are handled separately and don't bubble here.
        /// </summary>
        private void ChangeTimeBorder_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Don't close if clicking on buttons (buttons handle their own events)
            if (!IsInsideElement(e.OriginalSource, ChangeTimeButtonPanel))
            {
                CloseChangeTimeCard();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the Compact Mode toggle button click event.
        /// </summary>
        private async void CompactModeButton_Click(object sender, RoutedEventArgs e)
        {
            // Close any open inline editors first
            if (_changeTimeTask != null)
            {
                CloseChangeTimeCard();
                await System.Threading.Tasks.Task.Delay(50);
            }

            _isCompactMode = !_isCompactMode;

            if (_isCompactMode)
            {
                // Initial TitleBar state (will be controlled by UpdateTitleBarVisibility)
                // Start with it hidden (GONE)
                UpdateTitleBarVisibility();

                // Change Grid row height to Auto (content-based)
                if (MainGrid?.RowDefinitions.Count > 1)
                {
                    MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
                }

                // Reduce padding in compact mode (but keep small bottom padding)
                if (TaskScrollView != null)
                {
                    TaskScrollView.Padding = new Thickness(12, 4, 12, 8);
                    TaskScrollView.VerticalAlignment = VerticalAlignment.Top;
                }

                // Hide UI elements first
                if (NewTaskBorder != null) NewTaskBorder.Visibility = Visibility.Collapsed;
                if (ChangeTimeBorder != null) ChangeTimeBorder.Visibility = Visibility.Collapsed;
                if (AddTaskButton != null) AddTaskButton.Visibility = Visibility.Collapsed;
                if (EmptyStatePanel != null) EmptyStatePanel.Visibility = Visibility.Collapsed;
                if (StatusBar != null) StatusBar.Visibility = Visibility.Collapsed;

                // Hide non-active tasks and find active task container
                FrameworkElement? activeContainer = null;
                if (TasksItemsControl != null)
                {
                    for (int i = 0; i < TasksItemsControl.Items.Count; i++)
                    {
                        if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container)
                        {
                            var border = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(container, 0) as Border;
                            if (border?.Tag is TaskViewModel taskVm)
                            {
                                if (taskVm.IsActive)
                                {
                                    container.Visibility = Visibility.Visible;
                                    activeContainer = container;
                                }
                                else
                                {
                                    container.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                    }
                }

                // Wait for layout to update
                await System.Threading.Tasks.Task.Delay(100);

                // Get current TitleBar height (might be 0 if hidden, or 32 if shown)
                double titleBarHeight = TitleBarGrid?.ActualHeight ?? 0;

                // Get task border with margin
                double taskBorderHeight = 0;
                double taskMargin = 0;
                if (activeContainer != null && Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(activeContainer) > 0)
                {
                    var border = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(activeContainer, 0) as Border;
                    if (border != null)
                    {
                        taskBorderHeight = border.ActualHeight;
                        taskMargin = border.Margin.Top + border.Margin.Bottom;
                    }
                }

                // ScrollView padding (4 top + 8 bottom)
                double scrollViewPaddingVertical = 12;

                // Calculate using actual content (TitleBar may or may not be visible)
                int compactHeight = (int)Math.Ceiling(titleBarHeight + scrollViewPaddingVertical + taskBorderHeight + taskMargin);

                Log.Information($"Compact mode - TitleBar: {titleBarHeight}, TaskBorder: {taskBorderHeight}, " +
                    $"TaskMargin: {taskMargin}, Calculated: {compactHeight}");

                // Switch to compact mode with calculated height
                _appWindow?.Resize(new SizeInt32(220, compactHeight));

                CompactModeButton.Content = "◱";
                Log.Information($"Switched to compact mode (height: {compactHeight}px)");
            }
            else
            {
                // Restore normal mode
                // Restore TitleBar (will be controlled by UpdateTitleBarVisibility)
                UpdateTitleBarVisibility();

                // Restore Grid row height to fill (*)
                if (MainGrid?.RowDefinitions.Count > 1)
                {
                    MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                }

                // Switch back to normal mode
                _appWindow?.Resize(new SizeInt32(220, NORMAL_HEIGHT));

                // Restore normal padding and alignment
                if (TaskScrollView != null)
                {
                    TaskScrollView.Padding = new Thickness(12, 12, 12, 12);
                    TaskScrollView.VerticalAlignment = VerticalAlignment.Stretch;
                }

                // Show all tasks
                if (TasksItemsControl != null)
                {
                    for (int i = 0; i < TasksItemsControl.Items.Count; i++)
                    {
                        if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container)
                        {
                            container.Visibility = Visibility.Visible;
                        }
                    }
                }

                // Show UI elements
                if (AddTaskButton != null) AddTaskButton.Visibility = Visibility.Visible;
                if (StatusBar != null) StatusBar.Visibility = Visibility.Visible;
                UpdateEmptyState();

                CompactModeButton.Content = "◧";
                Log.Information("Switched to normal mode");
            }
        }

        /// <summary>
        /// Handles taps on the main grid to dismiss inline editors when clicking outside.
        /// </summary>
        private void MainGrid_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Close Change Time card if clicking outside it
            if (_changeTimeTask != null && !IsInsideElement(e.OriginalSource, ChangeTimeBorder))
            {
                CloseChangeTimeCard();
            }
        }

        /// <summary>
        /// Checks if a source element is inside the specified parent element.
        /// </summary>
        private bool IsInsideElement(object source, DependencyObject parent)
        {
            var element = source as DependencyObject;
            while (element != null)
            {
                if (element == parent) return true;
                element = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        /// <summary>
        /// Handles the Add Task button click event.
        /// </summary>
        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle new task input card visibility
            if (NewTaskBorder.Visibility == Visibility.Collapsed)
            {
                NewTaskBorder.Visibility = Visibility.Visible;
                NewTaskTextBox.Text = string.Empty;
                NewTaskTextBox.Focus(FocusState.Programmatic);

                // Scroll to bottom to show the input card
                await System.Threading.Tasks.Task.Delay(50);
                ScrollToBottom();
            }
            else
            {
                // If already visible, create the task
                CreateTaskFromInput();
            }
        }

        /// <summary>
        /// Handles key press in the new task textbox.
        /// </summary>
        private void NewTaskTextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                CreateTaskFromInput();
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;
                CancelTaskInput();
            }
        }


        /// <summary>
        /// Handles focus lost on the task textbox to save input (same as pressing Enter).
        /// </summary>
        private void NewTaskTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Only act if the input card is still visible (not already handled by Enter/Escape)
            if (NewTaskBorder.Visibility == Visibility.Visible)
            {
                CreateTaskFromInput();
            }
        }

        /// <summary>
        /// Handles task item click to select and toggle timer.
        /// </summary>
        private void TaskItem_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is Border border && border.Tag is TaskViewModel taskVm)
            {
                if (_viewModel != null)
                {
                    _viewModel.SelectTaskCommand.Execute(taskVm);

                    // Update background colors for all task items
                    UpdateTaskItemColors();

                    // Update status text
                    if (taskVm.IsRunning)
                    {
                        var statusElement = border.FindName("StatusText") as TextBlock;
                        if (statusElement != null)
                        {
                            statusElement.Text = "⏱️ Running...";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update background colors for all task items based on active state.
        /// </summary>
        private void UpdateTaskItemColors()
        {
            if (TasksItemsControl == null)
                return;

            // Iterate through all task items and update their background colors
            for (int i = 0; i < TasksItemsControl.Items.Count; i++)
            {
                if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container)
                {
                    var visual = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(container, 0) as Border;
                    if (visual?.Tag is TaskViewModel taskVm)
                    {
                        // Active (running) = Gold, Inactive (paused) = Dark Gray (#2A2A2A)
                        var color = taskVm.IsActive ? Microsoft.UI.Colors.Gold : new Color { A = 255, R = 0x2A, G = 0x2A, B = 0x2A };
                        visual.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(color);
                    }
                }
            }
        }

        /// <summary>
        /// Handles pointer entering task item for hover effect.
        /// </summary>
        private void TaskItem_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Border border && border.Tag is TaskViewModel taskVm)
            {
                // Hover: Lighter tone (active=lighter gold, inactive=lighter gray)
                Color hoverColor;
                if (taskVm.IsActive)
                {
                    // Lighter gold for active tasks (#FFD700 -> lighter)
                    hoverColor = new Color { A = 255, R = 0xFF, G = 0xE0, B = 0x50 };
                }
                else
                {
                    // Slightly lighter dark gray (#3A3A3A) for inactive tasks
                    hoverColor = new Color { A = 255, R = 0x3A, G = 0x3A, B = 0x3A };
                }
                border.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(hoverColor);
            }
        }

        /// <summary>
        /// Handles pointer exiting task item to restore background.
        /// </summary>
        private void TaskItem_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Border border && border.Tag is TaskViewModel taskVm)
            {
                // Restore: Active = Gold, Inactive = Dark Gray (#2A2A2A)
                var color = taskVm.IsActive ? Microsoft.UI.Colors.Gold : new Color { A = 255, R = 0x2A, G = 0x2A, B = 0x2A };
                border.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(color);
            }
        }

        /// <summary>
        /// Update empty state visibility based on task count.
        /// </summary>
        private void UpdateEmptyState()
        {
            if (EmptyStatePanel != null && _viewModel != null)
            {
                EmptyStatePanel.Visibility = _viewModel.Tasks.Count == 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Update status bar with task count and error messages.
        /// </summary>
        private void UpdateStatusBar()
        {
            if (StatusBar != null && _viewModel != null)
            {
                if (!string.IsNullOrEmpty(_viewModel.ErrorMessage))
                {
                    StatusBar.Text = _viewModel.ErrorMessage;
                    StatusBar.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.Colors.Firebrick);
                }
                else
                {
                    var count = _viewModel.Tasks.Count;
                    StatusBar.Text = $"{count} task{(count != 1 ? "s" : "")}";
                    StatusBar.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.Colors.Gray);
                }
            }
        }

        /// <summary>
        /// Centers window on screen.
        /// </summary>
        private void CenterOnScreen()
        {
            var window = this;
            // Window centering can be done by AppWindow if available
            // This is a placeholder for future implementation
        }

        /// <summary>
        /// Handles right-click on task item to show rename option.
        /// </summary>
        private void TaskItem_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            if (sender is Border border && border.Tag is TaskViewModel taskVm)
            {
                e.Handled = true;

                // Create and show flyout menu
                var flyout = new MenuFlyout();

                // Rename menu item
                var renameItem = new MenuFlyoutItem
                {
                    Text = "Rename",
                    Icon = new SymbolIcon { Symbol = Symbol.Rename }
                };

                renameItem.Click += (s, args) =>
                {
                    // Show rename input using the new task card
                    ShowRenameInput(taskVm);
                };

                flyout.Items.Add(renameItem);

                // Change Time menu item
                var changeTimeItem = new MenuFlyoutItem
                {
                    Text = "Change Time",
                    Icon = new SymbolIcon { Symbol = Symbol.Clock }
                };

                changeTimeItem.Click += (s, args) =>
                {
                    ShowChangeTimeInput(taskVm);
                };

                flyout.Items.Add(changeTimeItem);

                // Show at pointer position
                flyout.ShowAt(border, e.GetPosition(border));
            }
        }

        /// <summary>
        /// Shows the inline Change Time card at the task's position (like Rename).
        /// Replaces the task card with an edit card showing name, time, and adjustment buttons.
        /// </summary>
        private void ShowChangeTimeInput(TaskViewModel taskVm)
        {
            if (_viewModel?.Tasks == null)
                return;

            // If already editing time for a different task, restore it first
            if (_changeTimeTask != null && _changeTimeTaskIndex >= 0)
            {
                _viewModel.Tasks.Insert(_changeTimeTaskIndex, _changeTimeTask);
                HideChangeTimeCard();
            }

            // Find the index of the task
            var index = _viewModel.Tasks.IndexOf(taskVm);
            if (index < 0)
                return;

            // Store the task and its index
            _changeTimeTask = taskVm;
            _changeTimeTaskIndex = index;

            // Remove the task from the list
            _viewModel.Tasks.RemoveAt(index);

            // Move change time card to that position
            MoveChangeTimeCardToPosition(index);

            // Setup the card content
            ChangeTimeTaskName.Text = taskVm.Name;
            ChangeTimeDisplay.Text = taskVm.FormattedTime;

            // Set name color based on active state
            ChangeTimeTaskName.Foreground = new SolidColorBrush(
                taskVm.IsActive ? Microsoft.UI.Colors.Black : Microsoft.UI.Colors.White);
            ChangeTimeDisplay.Foreground = new SolidColorBrush(
                taskVm.IsActive ? Microsoft.UI.Colors.Black : Microsoft.UI.Colors.White);

            // Set card background based on active state (same as task card)
            ChangeTimeBorder.Background = new SolidColorBrush(
                taskVm.IsActive ? Microsoft.UI.Colors.Gold : new Color { A = 255, R = 0x2A, G = 0x2A, B = 0x2A });

            // Build adjustment buttons
            BuildChangeTimeButtons(taskVm);

            // Subscribe to timer updates so the display stays live
            taskVm.PropertyChanged += OnChangeTimeTaskPropertyChanged;
        }

        /// <summary>
        /// Keeps the Change Time display in sync with live timer updates.
        /// </summary>
        private void OnChangeTimeTaskPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskViewModel.FormattedTime) && _changeTimeTask != null)
            {
                ChangeTimeDisplay.Text = _changeTimeTask.FormattedTime;
            }
        }

        /// <summary>
        /// Creates the time adjustment buttons for the Change Time card.
        /// </summary>
        private void BuildChangeTimeButtons(TaskViewModel taskVm)
        {
            ChangeTimeButtonPanel.Children.Clear();

            void AddButton(string label, long deltaSeconds, bool isNegative)
            {
                var btn = new Button
                {
                    Content = label,
                    FontSize = 10,
                    MinWidth = 0,
                    Height = 24,
                    Padding = new Thickness(6, 2, 6, 2),
                    Background = new SolidColorBrush(isNegative
                        ? new Color { A = 255, R = 0x50, G = 0x28, B = 0x28 }
                        : new Color { A = 255, R = 0x28, G = 0x50, B = 0x28 }),
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
                    BorderThickness = new Thickness(0),
                    CornerRadius = new CornerRadius(12)
                };

                btn.Click += (s, args) =>
                {
                    if (_viewModel == null || _changeTimeTask == null) return;
                    var newTotal = _changeTimeTask.ElapsedSeconds + deltaSeconds;
                    _viewModel.SetTaskElapsedTime(_changeTimeTask, newTotal);
                    ChangeTimeDisplay.Text = _changeTimeTask.FormattedTime;
                };

                ChangeTimeButtonPanel.Children.Add(btn);
            }

            AddButton("-1h", -3600, true);
            AddButton("-5m", -300, true);
            AddButton("+5m", 300, false);
            AddButton("+1h", 3600, false);
        }

        /// <summary>
        /// Moves the Change Time card to the specified position in the task list.
        /// </summary>
        private void MoveChangeTimeCardToPosition(int index)
        {
            try
            {
                if (TasksItemsControl?.ItemsPanelRoot is StackPanel itemsPanel)
                {
                    // Remove from current parent
                    if (ChangeTimeBorder.Parent is StackPanel currentParent)
                    {
                        currentParent.Children.Remove(ChangeTimeBorder);
                    }

                    ChangeTimeBorder.Visibility = Visibility.Visible;
                    itemsPanel.Children.Insert(index, ChangeTimeBorder);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error moving change time card to position");
            }
        }

        /// <summary>
        /// Hides the Change Time card and returns it to original parent.
        /// </summary>
        private void HideChangeTimeCard()
        {
            try
            {
                if (TasksItemsControl?.ItemsPanelRoot is StackPanel itemsPanel &&
                    itemsPanel.Children.Contains(ChangeTimeBorder))
                {
                    itemsPanel.Children.Remove(ChangeTimeBorder);
                }

                if (ChangeTimeBorder.Parent == null && TaskScrollView?.Content is StackPanel scrollViewStackPanel)
                {
                    if (!scrollViewStackPanel.Children.Contains(ChangeTimeBorder))
                    {
                        scrollViewStackPanel.Children.Add(ChangeTimeBorder);
                    }
                }

                ChangeTimeBorder.Visibility = Visibility.Collapsed;
                ChangeTimeButtonPanel.Children.Clear();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error hiding change time card");
            }
        }

        /// <summary>
        /// Closes the Change Time card and restores the task to its position.
        /// </summary>
        private void CloseChangeTimeCard()
        {
            if (_changeTimeTask != null && _changeTimeTaskIndex >= 0 && _viewModel?.Tasks != null)
            {
                // Unsubscribe from timer updates
                _changeTimeTask.PropertyChanged -= OnChangeTimeTaskPropertyChanged;

                _viewModel.Tasks.Insert(_changeTimeTaskIndex, _changeTimeTask);

                // Restore active task color after UI updates
                var wasActive = _changeTimeTask.IsActive;
                var restoredTask = _changeTimeTask;

                _changeTimeTask = null;
                _changeTimeTaskIndex = -1;
                HideChangeTimeCard();

                if (wasActive)
                {
                    _ = RestoreActiveTaskColor(restoredTask);
                }
            }
        }

        /// <summary>
        /// Restores the Gold background for an active task after re-inserting it.
        /// </summary>
        private async System.Threading.Tasks.Task RestoreActiveTaskColor(TaskViewModel taskVm)
        {
            await System.Threading.Tasks.Task.Delay(50);
            for (int i = 0; i < TasksItemsControl.Items.Count; i++)
            {
                if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container)
                {
                    var border = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(container, 0) as Border;
                    if (border?.Tag == taskVm)
                    {
                        border.Background = new SolidColorBrush(Microsoft.UI.Colors.Gold);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Shows rename input card at the position of the task being edited.
        /// </summary>
        private void ShowRenameInput(TaskViewModel taskVm)
        {
            if (_viewModel?.Tasks == null)
                return;

            // If already editing a different task, restore it first
            if (_editingTask != null && _editingTaskIndex >= 0)
            {
                _viewModel.Tasks.Insert(_editingTaskIndex, _editingTask);
                HideEditCard();
            }

            // Find the index of the task being renamed
            var index = _viewModel.Tasks.IndexOf(taskVm);
            if (index < 0)
                return;

            // Store the editing task and its index
            _editingTask = taskVm;
            _editingTaskIndex = index;

            // Remove the task from the list
            _viewModel.Tasks.RemoveAt(index);

            // Move input card from StackPanel to ItemsControl's StackPanel at the correct position
            MoveEditCardToPosition(index);

            // Setup the input
            NewTaskTextBox.Text = taskVm.Name;
            NewTaskTextBox.Focus(FocusState.Programmatic);
            NewTaskTextBox.SelectAll();

            // Store reference to the task being renamed
            NewTaskTextBox.Tag = taskVm;
        }

        /// <summary>
        /// Moves the edit card to the specified position in the task list.
        /// </summary>
        private void MoveEditCardToPosition(int index)
        {
            try
            {
                // Get the ItemsControl's StackPanel
                if (TasksItemsControl?.ItemsPanelRoot is StackPanel itemsPanel)
                {
                    // Remove NewTaskBorder from its current parent
                    if (NewTaskBorder.Parent is StackPanel currentParent)
                    {
                        currentParent.Children.Remove(NewTaskBorder);
                    }

                    // Insert at the correct position
                    NewTaskBorder.Visibility = Visibility.Visible;
                    itemsPanel.Children.Insert(index, NewTaskBorder);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error moving edit card to position");
            }
        }

        /// <summary>
        /// Hides and removes the edit card from ItemsControl's panel.
        /// </summary>
        private void HideEditCard()
        {
            try
            {
                // Remove from ItemsControl's StackPanel if it's there
                if (TasksItemsControl?.ItemsPanelRoot is StackPanel itemsPanel &&
                    itemsPanel.Children.Contains(NewTaskBorder))
                {
                    itemsPanel.Children.Remove(NewTaskBorder);
                }

                // Move back to original parent (the main StackPanel in ScrollView)
                if (NewTaskBorder.Parent == null && TaskScrollView?.Content is StackPanel scrollViewStackPanel)
                {
                    if (!scrollViewStackPanel.Children.Contains(NewTaskBorder))
                    {
                        scrollViewStackPanel.Children.Add(NewTaskBorder);
                    }
                }

                NewTaskBorder.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error hiding edit card");
            }
        }

        /// <summary>
        /// Creates a task from input or renames if editing existing task.
        /// </summary>
        private async void CreateTaskFromInput()
        {
            var taskName = NewTaskTextBox.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(taskName) && _viewModel != null)
            {
                // Check if we're renaming an existing task
                if (NewTaskTextBox.Tag is TaskViewModel existingTask)
                {
                    // Save the active state before rename
                    bool wasActive = existingTask.IsActive;

                    existingTask.Name = taskName;

                    // Restore the edited task to its original position
                    if (_editingTaskIndex >= 0 && _editingTask != null)
                    {
                        _viewModel.Tasks.Insert(_editingTaskIndex, _editingTask);
                        _editingTask = null;
                        _editingTaskIndex = -1;
                    }

                    NewTaskTextBox.Tag = null;
                    HideEditCard();

                    // Restore the active state color after UI updates
                    if (wasActive)
                    {
                        await System.Threading.Tasks.Task.Delay(50);

                        // Find and update the task's background color
                        for (int i = 0; i < TasksItemsControl.Items.Count; i++)
                        {
                            if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container)
                            {
                                var border = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(container, 0) as Border;
                                if (border?.Tag == existingTask)
                                {
                                    border.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gold);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Creating new task
                    _viewModel.AddTaskCommand.Execute(taskName);
                    NewTaskBorder.Visibility = Visibility.Collapsed;

                    // Scroll to bottom after task is added
                    await System.Threading.Tasks.Task.Delay(100); // Wait for UI to update
                    ScrollToBottom();
                }

                NewTaskTextBox.Text = string.Empty;
            }
            else if (string.IsNullOrWhiteSpace(taskName))
            {
                CancelTaskInput();
            }
        }

        /// <summary>
        /// Scrolls the task list to the bottom.
        /// </summary>
        private void ScrollToBottom()
        {
            try
            {
                if (TaskScrollView != null)
                {
                    // Scroll to the maximum vertical offset (bottom)
                    TaskScrollView.ScrollTo(0, TaskScrollView.ScrollableHeight);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error scrolling to bottom");
            }
        }

        /// <summary>
        /// Cancels task input and hides the input card.
        /// </summary>
        private void CancelTaskInput()
        {
            // If editing a task, restore it to its original position
            if (_editingTask != null && _editingTaskIndex >= 0 && _viewModel?.Tasks != null)
            {
                _viewModel.Tasks.Insert(_editingTaskIndex, _editingTask);
                _editingTask = null;
                _editingTaskIndex = -1;
                HideEditCard();
            }
            else
            {
                NewTaskBorder.Visibility = Visibility.Collapsed;
            }

            NewTaskTextBox.Text = string.Empty;
            NewTaskTextBox.Tag = null; // Clear rename reference
            AddTaskButton.Focus(FocusState.Programmatic);
        }

        #region Drag and Drop

        // Drag and drop state
        private TaskViewModel? _draggingTask;
        private bool _isDragging = false;
        private Windows.Foundation.Point _dragStartPoint;

        /// <summary>
        /// Handles pointer pressed event to start drag operation.
        /// </summary>
        private void TaskItem_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Border border && border.Tag is TaskViewModel taskVm)
            {
                _dragStartPoint = e.GetCurrentPoint(border).Position;
                _draggingTask = taskVm;
                border.CapturePointer(e.Pointer);
            }
        }

        /// <summary>
        /// Handles pointer moved event to show drop indicator.
        /// </summary>
        private void TaskItem_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_draggingTask == null || sender is not Border border) return;

            var currentPoint = e.GetCurrentPoint(border).Position;
            var distance = Math.Sqrt(
                Math.Pow(currentPoint.X - _dragStartPoint.X, 2) +
                Math.Pow(currentPoint.Y - _dragStartPoint.Y, 2)
            );

            // Start dragging if moved more than 10 pixels
            if (!_isDragging && distance > 10)
            {
                _isDragging = true;
                Log.Information($"Started dragging: {_draggingTask.Name}");
            }

            if (_isDragging)
            {
                // Update drop indicator position
                UpdateDropIndicator(e.GetCurrentPoint(TaskScrollView).Position);
            }
        }

        /// <summary>
        /// Handles pointer released event to complete drop operation.
        /// </summary>
        private void TaskItem_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                border.ReleasePointerCapture(e.Pointer);
            }

            if (_isDragging && _draggingTask != null)
            {
                // Perform drop operation
                PerformDrop();
            }

            // Reset state
            _draggingTask = null;
            _isDragging = false;
            DropIndicatorLine.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Updates the drop indicator line position.
        /// </summary>
        private void UpdateDropIndicator(Windows.Foundation.Point pointerPosition)
        {
            try
            {
                if (_viewModel?.Tasks == null || TasksItemsControl?.ItemsPanelRoot is not StackPanel panel)
                    return;

                DropIndicatorLine.Visibility = Visibility.Visible;

                // Find the closest gap between tasks
                double closestDistance = double.MaxValue;
                double bestY = 0;
                int targetIndex = -1;

                // Check position before first task
                if (TasksItemsControl.Items.Count > 0 &&
                    TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(0) is FrameworkElement firstContainer)
                {
                    var firstPos = firstContainer.TransformToVisual(TaskScrollView).TransformPoint(new Windows.Foundation.Point(0, 0));
                    var topGapY = firstPos.Y;
                    var distance = Math.Abs(pointerPosition.Y - topGapY);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        bestY = topGapY;
                        targetIndex = 0;
                    }
                }

                // Check gaps between consecutive tasks
                for (int i = 0; i < TasksItemsControl.Items.Count - 1; i++)
                {
                    if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement currentContainer &&
                        TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i + 1) is FrameworkElement nextContainer)
                    {
                        var currentPos = currentContainer.TransformToVisual(TaskScrollView).TransformPoint(new Windows.Foundation.Point(0, 0));
                        var nextPos = nextContainer.TransformToVisual(TaskScrollView).TransformPoint(new Windows.Foundation.Point(0, 0));

                        // Gap is between bottom of current and top of next - place line in the middle
                        var gapY = (currentPos.Y + currentContainer.ActualHeight + nextPos.Y) / 2;
                        var distance = Math.Abs(pointerPosition.Y - gapY);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            bestY = gapY;
                            targetIndex = i + 1;
                        }
                    }
                }

                // Check position after last task
                if (TasksItemsControl.Items.Count > 0)
                {
                    var lastIndex = TasksItemsControl.Items.Count - 1;
                    if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(lastIndex) is FrameworkElement lastContainer)
                    {
                        var lastPos = lastContainer.TransformToVisual(TaskScrollView).TransformPoint(new Windows.Foundation.Point(0, 0));
                        var bottomGapY = lastPos.Y + lastContainer.ActualHeight;
                        var distance = Math.Abs(pointerPosition.Y - bottomGapY);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            bestY = bottomGapY;
                            targetIndex = TasksItemsControl.Items.Count;
                        }
                    }
                }

                // Position the line at the best Y coordinate (convert from ScrollView to DropIndicatorLine's parent Grid)
                if (targetIndex >= 0 && DropIndicatorLine.Parent is FrameworkElement parent)
                {
                    var scrollViewPos = TaskScrollView.TransformToVisual(parent).TransformPoint(new Windows.Foundation.Point(0, bestY));
                    DropIndicatorLine.Margin = new Thickness(24, scrollViewPos.Y - 1.5, 24, 0);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating drop indicator");
            }
        }

        /// <summary>
        /// Performs the drop operation to reorder tasks.
        /// </summary>
        private void PerformDrop()
        {
            try
            {
                if (_draggingTask == null || _viewModel?.Tasks == null) return;

                // Parse the indicator line's Y position to determine target index
                var indicatorY = DropIndicatorLine.Margin.Top;
                int targetIndex = 0;

                for (int i = 0; i < TasksItemsControl.Items.Count; i++)
                {
                    if (TasksItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container)
                    {
                        var containerPos = container.TransformToVisual(MainGrid).TransformPoint(new Windows.Foundation.Point(0, 0));

                        if (indicatorY < containerPos.Y)
                        {
                            targetIndex = i;
                            break;
                        }
                        targetIndex = i + 1;
                    }
                }

                int oldIndex = _viewModel.Tasks.IndexOf(_draggingTask);

                if (oldIndex >= 0 && targetIndex != oldIndex && targetIndex != oldIndex + 1)
                {
                    _viewModel.Tasks.RemoveAt(oldIndex);

                    // Adjust target index if we removed an item before it
                    if (targetIndex > oldIndex)
                        targetIndex--;

                    _viewModel.Tasks.Insert(targetIndex, _draggingTask);
                    Log.Information($"Moved task from {oldIndex} to {targetIndex}");

                    // Save the new order to database
                    _ = _viewModel.UpdateTaskOrdersAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error performing drop");
            }
        }

        #endregion

        /// <summary>
        /// Handles window closing event for cleanup.
        /// </summary>
        private void Window_Closed(object sender, WindowEventArgs args)
        {
            _viewModel?.Dispose();
            Log.Information("MainWindow closing");
        }

    }
}
