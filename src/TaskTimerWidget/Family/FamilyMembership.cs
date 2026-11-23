using WidgetFamily;

namespace TaskTimerWidget.Family
{
    /// <summary>
    /// The timer's behaviour as a widget family member (MEW-5): leaves the taskbar while the hub runs, comes back when
    /// it closes or crashes, and opens next to the hub when asked. Contract: external/WidgetFamily/docs/CONTRACT.md.
    /// </summary>
    public sealed class FamilyMembership
    {
        /// <summary>The member id; the same as the app extension id in Package.appxmanifest.</summary>
        public const string MEMBER_ID = "tasktimer";

        /// <summary>The activation protocol; the same as in Package.appxmanifest.</summary>
        public const string PROTOCOL = "tasktimer-widget";

        /// <summary>Space between the hub and the timer, in physical pixels.</summary>
        public const int GAP = 8;

        private readonly IFamilyWindow _window;
        private bool _hiddenByHub;

        /// <summary>Creates the membership for a window.</summary>
        /// <param name="window">The timer's window.</param>
        public FamilyMembership(IFamilyWindow window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        /// <summary>The hub is present: leave the taskbar.</summary>
        public void OnHubArrived() => _window.SetShownInTaskbar(false);

        /// <summary>The hub was closed or crashed: come back to the taskbar and stay open (S04 → A).</summary>
        public void OnHubLeft()
        {
            _window.SetShownInTaskbar(true);
            if (_hiddenByHub)
            {
                _hiddenByHub = false;
                _window.Show();
            }
        }

        /// <summary>Handles a message from the hub.</summary>
        /// <param name="message">The message.</param>
        public void OnMessage(FamilyMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);
            switch (message.Type)
            {
                case MessageTypes.Place when message.Edge is { } edge && message.Anchor is { } anchor:
                    PlaceNextTo(anchor, edge);
                    break;
                case MessageTypes.Hide:
                    _hiddenByHub = true;
                    _window.Hide();
                    break;
                case MessageTypes.Show:
                    _hiddenByHub = false;
                    _window.Show();
                    break;
            }
        }

        /// <summary>Handles a protocol activation; an open request places the timer next to the hub.</summary>
        /// <param name="uri">The activation URI.</param>
        /// <returns>Whether the URI was an open request.</returns>
        public bool OnActivated(Uri uri)
        {
            if (!ActivationUri.TryParseOpen(uri, out var request) || request is null)
            {
                return false;
            }

            PlaceNextTo(request.Anchor, request.Edge);
            _hiddenByHub = false;
            _window.BringToFront();
            return true;
        }

        private void PlaceNextTo(PixelRect anchor, Edge edge) =>
            _window.MoveTo(Placement.NextTo(anchor, edge, _window.Size, _window.WorkAreaAround(anchor), GAP));
    }
}
