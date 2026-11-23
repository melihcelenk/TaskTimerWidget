using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Microsoft.Windows.AppLifecycle;
using TaskTimerWidget.Family;
using TaskTimerWidget.Services;
using TaskTimerWidget.ViewModels;
using WidgetFamily;
using IProtocolActivatedEventArgs = Windows.ApplicationModel.Activation.IProtocolActivatedEventArgs;

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
            try
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
            catch
            {
                // Logging failed, continue anyway
            }
        }

        /// <summary>
        /// Configure dependency injection and services.
        /// </summary>
        private void InitializeServices()
        {
            var services = new ServiceCollection();

            // Register Services
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton<ITaskService, TaskService>();

            // Register ViewModels
            services.AddSingleton<MainViewModel>();

            // Create service provider
            _serviceProvider = services.BuildServiceProvider();

            // Note: TaskService initialization moved to OnLaunched to avoid blocking app startup
        }

        /// <summary>
        /// Gets a service instance from the dependency injection container.
        /// </summary>
        public static T GetService<T>() where T : class
        {
            return (Current as App)?._serviceProvider?.GetRequiredService<T>()
                ?? throw new InvalidOperationException($"Service {typeof(T).Name} not found");
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                // Initialize TaskService asynchronously
                var taskService = _serviceProvider?.GetRequiredService<ITaskService>();
                if (taskService is TaskService taskServiceImpl)
                {
                    await taskServiceImpl.InitializeAsync();
                }

                m_window = new MainWindow();
                m_window.Activate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to launch application");
                // Show error and still try to open window
                m_window = new MainWindow();
                m_window.Activate();
            }

            JoinWidgetFamily(m_window);
        }

        /// <summary>
        /// Joins the widget family (MEW-5): follows the hub over its pipe, and handles the hub's open requests and
        /// starts redirected from a second instance.
        /// </summary>
        private void JoinWidgetFamily(MainWindow window)
        {
            try
            {
                _membership = new FamilyMembership(window);
                void Dispatch(Action action) => window.DispatcherQueue.TryEnqueue(() => action());

                AppInstance.GetCurrent().Activated += (_, activation) => Dispatch(() => OnRedirectedActivation(window, activation));
                HandleProtocolActivation(AppInstance.GetCurrent().GetActivatedEventArgs());

                _family = new FamilyConnection(_membership, FamilyContract.HubPipeNameForThisSession(), FamilyConnection.RETRY_INTERVAL, Dispatch);
                _family.Start();
                window.Closed += (_, _) => _ = _family.DisposeAsync().AsTask();
                Log.Information("Joined the widget family");
            }
            catch (Exception ex)
            {
                // The timer works on its own without the family.
                Log.Error(ex, "Could not join the widget family");
            }
        }

        /// <summary>A second start was handed to this instance: open next to the hub, or come to the front.</summary>
        private void OnRedirectedActivation(MainWindow window, AppActivationArguments activation)
        {
            if (!HandleProtocolActivation(activation))
            {
                ((IFamilyWindow)window).BringToFront();
            }
        }

        private bool HandleProtocolActivation(AppActivationArguments activation)
        {
            if (activation.Kind != ExtendedActivationKind.Protocol
                || activation.Data is not IProtocolActivatedEventArgs protocol
                || _membership is null)
            {
                return false;
            }

            var handled = _membership.OnActivated(protocol.Uri);
            Log.Information("Protocol activation {Uri} handled: {Handled}", protocol.Uri, handled);
            return handled;
        }

        private MainWindow? m_window;
        private FamilyMembership? _membership;
        private FamilyConnection? _family;
    }
}
