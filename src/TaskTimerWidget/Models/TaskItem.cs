using Newtonsoft.Json;

namespace TaskTimerWidget.Models
{
    /// <summary>
    /// Represents a task with timing information.
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// Unique identifier for the task.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Task name/title.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Total elapsed seconds for this task.
        /// </summary>
        [JsonProperty("elapsedSeconds")]
        public long ElapsedSeconds { get; set; }

        /// <summary>
        /// Indicates if the timer is currently running.
        /// </summary>
        [JsonProperty("isRunning")]
        public bool IsRunning { get; set; }

        /// <summary>
        /// Task creation timestamp.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last modification timestamp.
        /// </summary>
        [JsonProperty("modifiedAt")]
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// Display order (lower values appear first).
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Constructor with default values.
        /// </summary>
        public TaskItem()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            ModifiedAt = DateTime.UtcNow;
            Order = 0;
        }

        /// <summary>
        /// Constructor with name.
        /// </summary>
        public TaskItem(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Resets elapsed time to zero and stops the timer.
        /// </summary>
        public void Reset()
        {
            ElapsedSeconds = 0;
            IsRunning = false;
            ModifiedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds seconds to elapsed time.
        /// </summary>
        public void AddSeconds(long seconds)
        {
            ElapsedSeconds += seconds;
            ModifiedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Returns a formatted time string (e.g., "1h 30m 5s", "23m 12s", "35s").
        /// </summary>
        public string GetFormattedTime()
        {
            var timeSpan = TimeSpan.FromSeconds(ElapsedSeconds);
            var parts = new List<string>();

            if (timeSpan.Hours > 0)
                parts.Add($"{timeSpan.Hours}h");

            if (timeSpan.Minutes > 0)
                parts.Add($"{timeSpan.Minutes}m");

            if (timeSpan.Seconds > 0 || parts.Count == 0)
                parts.Add($"{timeSpan.Seconds}s");

            return string.Join(" ", parts);
        }

        public override string ToString()
        {
            return $"{Name} - {GetFormattedTime()} ({(IsRunning ? "Running" : "Paused")})";
        }

        public override bool Equals(object? obj)
        {
            return obj is TaskItem item && item.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
