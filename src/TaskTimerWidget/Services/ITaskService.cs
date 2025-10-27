using TaskTimerWidget.Models;

namespace TaskTimerWidget.Services
{
    /// <summary>
    /// Service for managing tasks and their lifecycle.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Gets all tasks.
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetAllTasksAsync();

        /// <summary>
        /// Gets a specific task by ID.
        /// </summary>
        System.Threading.Tasks.Task<TaskItem?> GetTaskAsync(Guid id);

        /// <summary>
        /// Creates a new task with the given name.
        /// </summary>
        System.Threading.Tasks.Task<TaskItem> CreateTaskAsync(string name);

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        System.Threading.Tasks.Task UpdateTaskAsync(TaskItem task);

        /// <summary>
        /// Deletes a task by ID.
        /// </summary>
        System.Threading.Tasks.Task DeleteTaskAsync(Guid id);

        /// <summary>
        /// Starts the timer for a task.
        /// </summary>
        System.Threading.Tasks.Task StartTaskAsync(Guid id);

        /// <summary>
        /// Pauses the timer for a task.
        /// </summary>
        System.Threading.Tasks.Task PauseTaskAsync(Guid id);

        /// <summary>
        /// Adds elapsed seconds to a task.
        /// </summary>
        System.Threading.Tasks.Task AddElapsedTimeAsync(Guid id, long seconds);

        /// <summary>
        /// Resets task timer to zero.
        /// </summary>
        System.Threading.Tasks.Task ResetTaskAsync(Guid id);
    }
}
