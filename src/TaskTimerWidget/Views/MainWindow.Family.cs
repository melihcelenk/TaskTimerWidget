using Microsoft.UI.Windowing;
using TaskTimerWidget.Family;
using Windows.Graphics;
using WidgetFamily;

namespace TaskTimerWidget
{
    /// <summary>The window's side of the widget family membership (MEW-5); window-level state only.</summary>
    public sealed partial class MainWindow : IFamilyWindow
    {
        PixelSize IFamilyWindow.Size =>
            _appWindow is null ? default : new PixelSize(_appWindow.Size.Width, _appWindow.Size.Height);

        PixelRect IFamilyWindow.WorkAreaAround(PixelRect anchor)
        {
            var area = DisplayArea.GetFromRect(
                new RectInt32(anchor.Left, anchor.Top, anchor.Width, anchor.Height),
                DisplayAreaFallback.Nearest).WorkArea;
            return new PixelRect(area.X, area.Y, area.Width, area.Height);
        }

        void IFamilyWindow.SetShownInTaskbar(bool shown)
        {
            if (_appWindow is not null)
            {
                _appWindow.IsShownInSwitchers = shown;
            }
        }

        void IFamilyWindow.MoveTo(PixelRect bounds) => _appWindow?.Move(new PointInt32(bounds.Left, bounds.Top));

        void IFamilyWindow.Show() => _appWindow?.Show(false);

        void IFamilyWindow.Hide() => _appWindow?.Hide();

        void IFamilyWindow.BringToFront()
        {
            _appWindow?.Show(true);
            Activate();
        }
    }
}
