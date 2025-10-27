using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using TaskTimerWidget.Services;
using TaskTimerWidget.ViewModels;

namespace TaskTimerWidget
{
    /// <summary>
    /// Application entry point and configuration.
    /// Handles dependency injection, logging, and app lifecycle.
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        public App()
        {
            InitializeComponent();
            InitializeLogging();
            InitializeServices();
        }

        /// <summary>
        /// Configure Serilog for application logging.
        /// </summary>
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "TaskTimerWidget",
                        "Logs",
                        "app-.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("TaskTimerWidget Application Starting...");
        }

        /// <summary>
        /// Configure dependency injection and services.
        /// </summary>
        private async void InitializeServices()
        {
            var services = new ServiceCollection();

            // Register Services
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton<ITaskService, TaskService>();

            // Register ViewModels
            services.AddSingleton<MainViewModel>();

            // Create service provider
            _serviceProvider = services.BuildServiceProvider();

            // Initialize TaskService
            var taskService = _serviceProvider.GetRequiredService<ITaskService>();
            if (taskService is TaskService taskServiceImpl)
            {
                await taskServiceImpl.InitializeAsync();
            }
        }

        /// <summary>
        /// Gets a service instance from the dependency injection container.
        /// </summary>
        public static T GetService<T>() where T : class
        {
            return (Current as App)?._serviceProvider?.GetRequiredService<T>()
                ?? throw new InvalidOperationException($"Service {typeof(T).Name} not found");
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window? m_window;
    }
}
