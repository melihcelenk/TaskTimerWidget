using TaskTimerWidget.Models;
using Serilog;

namespace TaskTimerWidget.Services
{
    /// <summary>
    /// Implementation of task management service.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly IStorageService _storageService;
        private readonly List<TaskItem> _tasks;
        private readonly object _lockObject = new();

        public TaskService(IStorageService storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _tasks = new List<TaskItem>();
        }

        /// <summary>
        /// Initializes the service by loading stored tasks.
        /// </summary>
        public async System.Threading.Tasks.Task InitializeAsync()
        {
            try
            {
                var storedTasks = await _storageService.LoadTasksAsync();
                lock (_lockObject)
                {
                    _tasks.Clear();
                    _tasks.AddRange(storedTasks);
                }
                Log.Information($"Loaded {_tasks.Count} tasks from storage");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing TaskService");
            }
        }

        public System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            lock (_lockObject)
            {
                return System.Threading.Tasks.Task.FromResult(_tasks.AsEnumerable());
            }
        }

        public System.Threading.Tasks.Task<TaskItem?> GetTaskAsync(Guid id)
        {
            lock (_lockObject)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                return System.Threading.Tasks.Task.FromResult(task);
            }
        }

        public async System.Threading.Tasks.Task<TaskItem> CreateTaskAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Task name cannot be empty", nameof(name));
            }

            var task = new TaskItem(name);
            lock (_lockObject)
            {
                _tasks.Add(task);
            }

            await _storageService.SaveTasksAsync(_tasks);
            Log.Information($"Task created: {task.Id} - {name}");
            return task;
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(TaskItem task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            lock (_lockObject)
            {
                var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
                if (existingTask != null)
                {
                    existingTask.Name = task.Name;
                    existingTask.ElapsedSeconds = task.ElapsedSeconds;
                    existingTask.IsRunning = task.IsRunning;
                    existingTask.ModifiedAt = DateTime.UtcNow;
                }
            }

            await _storageService.SaveTasksAsync(_tasks);
            Log.Debug($"Task updated: {task.Id}");
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(Guid id)
        {
            lock (_lockObject)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    _tasks.Remove(task);
                    Log.Information($"Task deleted: {id}");
                }
            }

            await _storageService.SaveTasksAsync(_tasks);
        }

        public async System.Threading.Tasks.Task StartTaskAsync(Guid id)
        {
            var task = await GetTaskAsync(id);
            if (task != null && !task.IsRunning)
            {
                task.IsRunning = true;
                await UpdateTaskAsync(task);
                Log.Debug($"Task started: {id}");
            }
        }

        public async System.Threading.Tasks.Task PauseTaskAsync(Guid id)
        {
            var task = await GetTaskAsync(id);
            if (task != null && task.IsRunning)
            {
                task.IsRunning = false;
                await UpdateTaskAsync(task);
                Log.Debug($"Task paused: {id}");
            }
        }

        public async System.Threading.Tasks.Task AddElapsedTimeAsync(Guid id, long seconds)
        {
            var task = await GetTaskAsync(id);
            if (task != null)
            {
                task.AddSeconds(seconds);
                await UpdateTaskAsync(task);
            }
        }

        public async System.Threading.Tasks.Task ResetTaskAsync(Guid id)
        {
            var task = await GetTaskAsync(id);
            if (task != null)
            {
                task.Reset();
                await UpdateTaskAsync(task);
                Log.Debug($"Task reset: {id}");
            }
        }
    }
}
