using TaskTimerWidget.Models;
using TaskTimerWidget.Services;
using TaskTimerWidget.Helpers;
using System.Windows.Input;

namespace TaskTimerWidget.ViewModels
{
    /// <summary>
    /// ViewModel for individual task representation and management.
    /// </summary>
    public class TaskViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private TaskItem _model;
        private bool _isActive;

        private ICommand? _startCommand;
        private ICommand? _pauseCommand;
        private ICommand? _deleteCommand;

        public Guid Id => _model.Id;

        public string Name
        {
            get => _model.Name;
            set
            {
                if (_model.Name != value)
                {
                    _model.Name = value;
                    OnPropertyChanged();
                    _ = _taskService.UpdateTaskAsync(_model);
                }
            }
        }

        public long ElapsedSeconds
        {
            get => _model.ElapsedSeconds;
            set
            {
                if (_model.ElapsedSeconds != value)
                {
                    _model.ElapsedSeconds = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedTime));
                    _ = _taskService.UpdateTaskAsync(_model);
                }
            }
        }

        public bool IsRunning
        {
            get => _model.IsRunning;
            set
            {
                if (_model.IsRunning != value)
                {
                    _model.IsRunning = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsActive));
                    _ = _taskService.UpdateTaskAsync(_model);
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }

        public string FormattedTime => _model.GetFormattedTime();

        public DateTime CreatedAt => _model.CreatedAt;

        public ICommand StartCommand =>
            _startCommand ??= new RelayCommand(() => IsRunning = true);

        public ICommand PauseCommand =>
            _pauseCommand ??= new RelayCommand(() => IsRunning = false);

        public ICommand DeleteCommand =>
            _deleteCommand ??= new RelayCommand(async () =>
            {
                await _taskService.DeleteTaskAsync(Id);
                OnTaskDeleted?.Invoke(this, Id);
            });

        public event EventHandler<Guid>? OnTaskDeleted;

        public TaskViewModel(TaskItem model, ITaskService taskService)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _isActive = false;
        }

        /// <summary>
        /// Updates the elapsed time display without changing the model.
        /// Used for live timer updates.
        /// </summary>
        public void UpdateElapsedDisplay(long seconds)
        {
            _model.ElapsedSeconds = seconds;
            OnPropertyChanged(nameof(ElapsedSeconds));
            OnPropertyChanged(nameof(FormattedTime));
        }

        /// <summary>
        /// Gets the underlying task model.
        /// </summary>
        public TaskItem GetModel() => _model;
    }
}
