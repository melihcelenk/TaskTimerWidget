using TaskTimerWidget.Family;
using WidgetFamily;

namespace TaskTimerWidget.Tests.Family
{
    public sealed class FamilyMembershipTests
    {
        private static readonly PixelRect HUB = new(900, 500, 64, 64);

        private readonly FakeFamilyWindow _window = new();
        private readonly FamilyMembership _membership;

        public FamilyMembershipTests()
        {
            _membership = new FamilyMembership(_window);
        }

        [Fact]
        public void HubArriving_TakesTheWindowOffTheTaskbar()
        {
            _membership.OnHubArrived();

            Assert.False(_window.IsShownInTaskbar);
        }

        [Fact]
        public void HubLeaving_BringsTheWindowBackToTheTaskbar_AndKeepsItOpen()
        {
            // S04 → A: the timer keeps running and stays reachable.
            _membership.OnHubArrived();

            _membership.OnHubLeft();

            Assert.True(_window.IsShownInTaskbar);
            Assert.True(_window.IsVisible);
        }

        [Fact]
        public void HubLeavingWhileTheWindowWasHiddenByIt_ShowsTheWindowAgain()
        {
            _membership.OnHubArrived();
            _membership.OnMessage(FamilyMessage.Hide());

            _membership.OnHubLeft();

            Assert.True(_window.IsVisible);
        }

        [Fact]
        public void PlaceMessage_MovesTheWindowNextToTheHub()
        {
            _membership.OnMessage(FamilyMessage.Place(Edge.Right, HUB));

            var expected = Placement.NextTo(HUB, Edge.Right, _window.Size, FakeFamilyWindow.SCREEN, FamilyMembership.GAP);
            Assert.Equal(expected, _window.MovedTo);
        }

        [Fact]
        public void HideAndShowMessages_HideAndShowTheWindow()
        {
            _membership.OnMessage(FamilyMessage.Hide());
            Assert.False(_window.IsVisible);

            _membership.OnMessage(FamilyMessage.Show());
            Assert.True(_window.IsVisible);
        }

        [Fact]
        public void OpenRequest_PlacesTheWindowAndBringsItToTheFront()
        {
            var handled = _membership.OnActivated(ActivationUri.BuildOpen(FamilyMembership.PROTOCOL, Edge.Left, HUB));

            Assert.True(handled);
            Assert.Equal(
                Placement.NextTo(HUB, Edge.Left, _window.Size, FakeFamilyWindow.SCREEN, FamilyMembership.GAP),
                _window.MovedTo);
            Assert.Equal(1, _window.BroughtToFrontCount);
        }

        [Fact]
        public void OtherActivation_LeavesTheWindowWhereItIs()
        {
            var handled = _membership.OnActivated(new Uri("tasktimer-widget:something-else"));

            Assert.False(handled);
            Assert.Null(_window.MovedTo);
        }

        [Fact]
        public void ProtocolAndMemberId_MatchTheManifest()
        {
            var manifest = File.ReadAllText(Path.Combine(RepoRoot(), "src", "TaskTimerWidget", "Package.appxmanifest"));

            Assert.Contains($"<uap:Protocol Name=\"{FamilyMembership.PROTOCOL}\"", manifest, StringComparison.Ordinal);
            Assert.Contains($"<Protocol>{FamilyMembership.PROTOCOL}</Protocol>", manifest, StringComparison.Ordinal);
            Assert.Contains($"Name=\"{FamilyContract.ExtensionName}\"", manifest, StringComparison.Ordinal);
            Assert.Contains($"Id=\"{FamilyMembership.MEMBER_ID}\"", manifest, StringComparison.Ordinal);
        }

        private static string RepoRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "src", "TaskTimerWidget", "Package.appxmanifest")))
            {
                directory = directory.Parent;
            }

            return directory?.FullName ?? throw new InvalidOperationException("Repository root not found.");
        }
    }
}
