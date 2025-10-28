using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Windows.UI;
using TaskTimerWidget.ViewModels;
using Serilog;

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
                bool isActive = args.WindowActivationState != WindowActivationState.Deactivated;
                UpdateTitleBar(isActive);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error handling window activation");
            }
        }

        /// <summary>
        /// Update title bar visibility based on window activation state
        /// </summary>
        private async void UpdateTitleBar(bool isActive)
        {
            try
            {
                // Show/hide title bar based on window activation
                // Use Opacity to keep space reserved but invisible
                // Use IsHitTestVisible to disable interactions when inactive
                TitleBarGrid.Opacity = isActive ? 1.0 : 0.0;

                // If deactivating, immediately disable hit test
                if (!isActive)
                {
                    TitleBarGrid.IsHitTestVisible = false;
                }
                else
                {
                    // If activating, add a small delay before enabling hit test
                    // This prevents the user's click from immediately hitting the button
                    await System.Threading.Tasks.Task.Delay(100);
                    TitleBarGrid.IsHitTestVisible = true;
                }

                Log.Information($"Title bar updated: isActive={isActive}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating title bar");
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
        /// Handles the Add Task button click event.
        /// </summary>
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle new task input card visibility
            if (NewTaskBorder.Visibility == Visibility.Collapsed)
            {
                NewTaskBorder.Visibility = Visibility.Visible;
                NewTaskTextBox.Text = string.Empty;
                NewTaskTextBox.Focus(FocusState.Programmatic);
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
        /// Creates a task from the textbox input.
        /// </summary>
        private void CreateTaskFromInput()
        {
            var taskName = NewTaskTextBox.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(taskName) && _viewModel != null)
            {
                _viewModel.AddTaskCommand.Execute(taskName);
                NewTaskTextBox.Text = string.Empty;
                NewTaskBorder.Visibility = Visibility.Collapsed;
            }
            else if (string.IsNullOrWhiteSpace(taskName))
            {
                CancelTaskInput();
            }
        }

        /// <summary>
        /// Cancels task input and hides the input card.
        /// </summary>
        private void CancelTaskInput()
        {
            NewTaskTextBox.Text = string.Empty;
            NewTaskBorder.Visibility = Visibility.Collapsed;
            AddTaskButton.Focus(FocusState.Programmatic);
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
        /// Handles window closing event for cleanup.
        /// </summary>
        private void Window_Closed(object sender, WindowEventArgs args)
        {
            _viewModel?.Dispose();
            Log.Information("MainWindow closing");
        }

    }
}
