using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TaskTimerWidget.Models;
using TaskTimerWidget.Services;
using TaskTimerWidget.Helpers;
using Serilog;

namespace TaskTimerWidget.ViewModels
{
    /// <summary>
    /// Main ViewModel managing application state and task operations.
    /// Handles timer coordination and task management.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private TaskViewModel? _activeTask;
        private bool _isLoading;
        private string? _errorMessage;
        private DispatcherTimer? _timerUpdate;
        private TaskViewModel? _selectedTask;

        private ICommand? _addTaskCommand;
        private ICommand? _selectTaskCommand;

        public ObservableCollection<TaskViewModel> Tasks { get; }

        public TaskViewModel? ActiveTask
        {
            get => _activeTask;
            private set => SetProperty(ref _activeTask, value);
        }

        public TaskViewModel? SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand AddTaskCommand =>
            _addTaskCommand ??= new RelayCommand<string>(async (taskName) =>
            {
                if (!string.IsNullOrWhiteSpace(taskName))
                {
                    await AddTaskAsync(taskName);
                }
            });

        public ICommand SelectTaskCommand =>
            _selectTaskCommand ??= new RelayCommand<TaskViewModel>(async (taskVm) =>
            {
                if (taskVm != null)
                {
                    await SelectTaskAsync(taskVm);
                }
            });

        public MainViewModel(ITaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            Tasks = new ObservableCollection<TaskViewModel>();

            // Initialize timer for updating UI
            InitializeTimer();
        }

        /// <summary>
        /// Initializes the ViewModel and loads existing tasks.
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                // Load tasks from service
                var tasks = await _taskService.GetAllTasksAsync();
                foreach (var task in tasks)
                {
                    var viewModel = new TaskViewModel(task, _taskService);
                    viewModel.OnTaskDeleted += (sender, id) => OnTaskDeleted(id);
                    Tasks.Add(viewModel);
                }

                Log.Information($"Loaded {Tasks.Count} tasks into UI");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load tasks";
                Log.Error(ex, "Error initializing MainViewModel");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Adds a new task with the given name.
        /// </summary>
        private async Task AddTaskAsync(string taskName)
        {
            try
            {
                var newTask = await _taskService.CreateTaskAsync(taskName);
                var viewModel = new TaskViewModel(newTask, _taskService);
                viewModel.OnTaskDeleted += (sender, id) => OnTaskDeleted(id);
                Tasks.Add(viewModel);

                // Don't auto-select new task, let user click it
                Log.Information($"Task added: {newTask.Name}");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to add task";
                Log.Error(ex, "Error adding task");
            }
        }

        /// <summary>
        /// Selects a task and starts/pauses its timer appropriately.
        /// </summary>
        private Task SelectTaskAsync(TaskViewModel taskVm)
        {
            try
            {
                // If same task is clicked, toggle its active state (pause/resume)
                if (ActiveTask == taskVm)
                {
                    if (taskVm.IsRunning)
                    {
                        // Pause the task
                        taskVm.IsRunning = false;
                        taskVm.IsActive = false;
                    }
                    else
                    {
                        // Resume the task
                        taskVm.IsRunning = true;
                        taskVm.IsActive = true;
                    }
                }
                else
                {
                    // Different task is clicked
                    // Pause previous active task
                    if (ActiveTask != null)
                    {
                        ActiveTask.IsRunning = false;
                        ActiveTask.IsActive = false;
                    }

                    // Set new active task
                    SelectedTask = taskVm;
                    ActiveTask = taskVm;
                    ActiveTask.IsActive = true;
                    ActiveTask.IsRunning = true;
                }

                Log.Debug($"Task selected: {taskVm.Name}");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to select task";
                Log.Error(ex, "Error selecting task");
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Removes a task from the collection when it's deleted.
        /// </summary>
        private void OnTaskDeleted(Guid taskId)
        {
            var taskVm = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskVm != null)
            {
                Tasks.Remove(taskVm);
                if (ActiveTask?.Id == taskId)
                {
                    ActiveTask = null;
                }
                Log.Information($"Task removed from UI: {taskId}");
            }
        }

        /// <summary>
        /// Initializes the UI update timer (1 second interval).
        /// </summary>
        private void InitializeTimer()
        {
            _timerUpdate = new DispatcherTimer();
            _timerUpdate.Interval = TimeSpan.FromSeconds(1);
            _timerUpdate.Tick += (sender, e) => UpdateActiveTaskTimer();
            _timerUpdate.Start();
        }

        /// <summary>
        /// Updates the active task's elapsed time display.
        /// </summary>
        private void UpdateActiveTaskTimer()
        {
            if (ActiveTask?.IsRunning == true)
            {
                var currentElapsed = ActiveTask.ElapsedSeconds + 1;
                ActiveTask.UpdateElapsedDisplay(currentElapsed);

                // Save the updated model to storage
                _ = _taskService.UpdateTaskAsync(ActiveTask.GetModel());
            }
        }

        /// <summary>
        /// Cleanup method for disposal.
        /// </summary>
        public void Dispose()
        {
            _timerUpdate?.Stop();
            _timerUpdate = null;
        }
    }
}
