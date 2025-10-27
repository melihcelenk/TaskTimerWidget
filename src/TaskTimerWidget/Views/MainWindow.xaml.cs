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

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();
            InitializeViewModel();
        }

        /// <summary>
        /// Initializes window properties and settings.
        /// </summary>
        private void InitializeWindow()
        {
            try
            {
                var appWindow = this.AppWindow;
                if (appWindow != null)
                {
                    // Set window size for widget appearance
                    appWindow.Resize(new SizeInt32(200, 500));
                    Log.Information("MainWindow resized to 200x500");

                    // Remove window chrome (title bar, min/max/close buttons)
                    var titleBar = appWindow.TitleBar;
                    if (titleBar != null)
                    {
                        // Extend content into title bar area
                        titleBar.ExtendsContentIntoTitleBar = true;

                        // Set drag region for the top of the window (for window dragging)
                        titleBar.SetDragRectangles(new[] { new RectInt32(0, 0, 200, 40) });

                        Log.Information("Window chrome removed, custom title bar enabled");
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
                // Hover: Slightly lighter dark gray (#3A3A3A) for subtle effect
                var hoverColor = new Color { A = 255, R = 0x3A, G = 0x3A, B = 0x3A };
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
