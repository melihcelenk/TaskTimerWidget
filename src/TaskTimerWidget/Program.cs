using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace TaskTimerWidget
{
    /// <summary>
    /// Entry point. The timer runs as a single instance: a second start (from the Start menu or from the widget family
    /// hub's protocol) is handed to the running instance instead of opening a second window.
    /// </summary>
    public static class Program
    {
        private const string INSTANCE_KEY = "main";

        [STAThread]
        private static int Main(string[] args)
        {
            WinRT.ComWrappersSupport.InitializeComWrappers();
            if (RedirectToRunningInstance())
            {
                return 0;
            }

            Application.Start(initialization =>
            {
                var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                _ = new App();
            });
            return 0;
        }

        /// <summary>Hands this activation to the running instance, if there is one.</summary>
        /// <returns>Whether another instance is running and received the activation.</returns>
        private static bool RedirectToRunningInstance()
        {
            var mainInstance = AppInstance.FindOrRegisterForKey(INSTANCE_KEY);
            if (mainInstance.IsCurrent)
            {
                return false;
            }

            var activation = AppInstance.GetCurrent().GetActivatedEventArgs();

            // Redirecting must not block the STA thread's message pump; wait for it on a worker thread.
            Task.Run(() => mainInstance.RedirectActivationToAsync(activation).AsTask()).GetAwaiter().GetResult();
            return true;
        }
    }
}
