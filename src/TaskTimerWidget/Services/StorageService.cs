using Newtonsoft.Json;
using TaskTimerWidget.Models;
using Serilog;

namespace TaskTimerWidget.Services
{
    /// <summary>
    /// Implementation of storage service using local JSON files.
    /// </summary>
    public class StorageService : IStorageService
    {
        private readonly string _storageDirectory;
        private readonly string _tasksFilePath;
        private readonly object _lockObject = new();

        public StorageService()
        {
            // Initialize storage directory
            _storageDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "TaskTimerWidget",
                "Data");

            _tasksFilePath = Path.Combine(_storageDirectory, "tasks.json");

            // Ensure directory exists
            EnsureStorageDirectoryExists();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskItem>> LoadTasksAsync()
        {
            try
            {
                lock (_lockObject)
                {
                    if (!File.Exists(_tasksFilePath))
                    {
                        Log.Information("No existing tasks file found, returning empty collection");
                        return Enumerable.Empty<TaskItem>();
                    }

                    var json = File.ReadAllText(_tasksFilePath);
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        return Enumerable.Empty<TaskItem>();
                    }

                    var tasks = JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
                    Log.Information($"Loaded {tasks.Count} tasks from {_tasksFilePath}");
                    return tasks;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading tasks from storage");
                return Enumerable.Empty<TaskItem>();
            }
        }

        public async System.Threading.Tasks.Task SaveTasksAsync(IEnumerable<TaskItem> tasks)
        {
            try
            {
                lock (_lockObject)
                {
                    var json = JsonConvert.SerializeObject(tasks.ToList(), Formatting.Indented);
                    File.WriteAllText(_tasksFilePath, json);
                    Log.Debug($"Saved {tasks.Count()} tasks to {_tasksFilePath}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving tasks to storage");
                throw;
            }
        }

        public async System.Threading.Tasks.Task ClearAsync()
        {
            try
            {
                lock (_lockObject)
                {
                    if (File.Exists(_tasksFilePath))
                    {
                        File.Delete(_tasksFilePath);
                        Log.Information("Tasks storage cleared");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error clearing tasks storage");
                throw;
            }
        }

        /// <summary>
        /// Ensures the storage directory exists.
        /// </summary>
        private void EnsureStorageDirectoryExists()
        {
            try
            {
                if (!Directory.Exists(_storageDirectory))
                {
                    Directory.CreateDirectory(_storageDirectory);
                    Log.Information($"Created storage directory: {_storageDirectory}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error creating storage directory: {_storageDirectory}");
                throw;
            }
        }
    }
}
