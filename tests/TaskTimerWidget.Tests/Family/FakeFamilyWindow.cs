using TaskTimerWidget.Family;
using WidgetFamily;

namespace TaskTimerWidget.Tests.Family
{
    /// <summary>Records what the family logic asks of the window.</summary>
    internal sealed class FakeFamilyWindow : IFamilyWindow
    {
        public static readonly PixelRect SCREEN = new(0, 0, 1920, 1040);

        public bool IsShownInTaskbar { get; private set; } = true;

        public bool IsVisible { get; private set; } = true;

        public PixelRect? MovedTo { get; private set; }

        public int BroughtToFrontCount { get; private set; }

        public PixelSize Size { get; set; } = new(220, 500);

        public PixelRect WorkAreaAround(PixelRect anchor) => SCREEN;

        public void SetShownInTaskbar(bool shown) => IsShownInTaskbar = shown;

        public void MoveTo(PixelRect bounds) => MovedTo = bounds;

        public void Show() => IsVisible = true;

        public void Hide() => IsVisible = false;

        public void BringToFront()
        {
            IsVisible = true;
            BroughtToFrontCount++;
        }
    }
}
