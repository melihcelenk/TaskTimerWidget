using TaskTimerWidget.Models;

namespace TaskTimerWidget.Services
{
    /// <summary>
    /// Service for persisting and loading tasks from storage.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Loads all tasks from storage.
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<TaskItem>> LoadTasksAsync();

        /// <summary>
        /// Saves all tasks to storage.
        /// </summary>
        System.Threading.Tasks.Task SaveTasksAsync(IEnumerable<TaskItem> tasks);

        /// <summary>
        /// Clears all tasks from storage.
        /// </summary>
        System.Threading.Tasks.Task ClearAsync();
    }
}
