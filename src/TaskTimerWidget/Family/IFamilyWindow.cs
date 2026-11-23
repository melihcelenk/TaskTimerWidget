using WidgetFamily;

namespace TaskTimerWidget.Family
{
    /// <summary>
    /// What the widget family needs from the timer's window. The WinUI window implements it; tests use a fake.
    /// All members are called on the UI thread.
    /// </summary>
    public interface IFamilyWindow
    {
        /// <summary>The window's current size in physical pixels.</summary>
        PixelSize Size { get; }

        /// <summary>The work area of the monitor that holds <paramref name="anchor"/>.</summary>
        /// <param name="anchor">The hub's window bounds.</param>
        /// <returns>The work area in physical pixels.</returns>
        PixelRect WorkAreaAround(PixelRect anchor);

        /// <summary>Shows the window on the taskbar and in Alt+Tab, or takes it off them.</summary>
        /// <param name="shown">Whether the window is listed.</param>
        void SetShownInTaskbar(bool shown);

        /// <summary>Moves the window.</summary>
        /// <param name="bounds">The new bounds; only the position is used.</param>
        void MoveTo(PixelRect bounds);

        /// <summary>Shows the window without activating it.</summary>
        void Show();

        /// <summary>Hides the window.</summary>
        void Hide();

        /// <summary>Shows and activates the window.</summary>
        void BringToFront();
    }
}
