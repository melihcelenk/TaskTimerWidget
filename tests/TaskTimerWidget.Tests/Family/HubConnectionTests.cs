using TaskTimerWidget.Family;
using WidgetFamily;

namespace TaskTimerWidget.Tests.Family
{
    /// <summary>The membership driven by a real hub over a pipe (S04 → A).</summary>
    public sealed class HubConnectionTests
    {
        private static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan RETRY = TimeSpan.FromMilliseconds(100);

        [Fact]
        public async Task Window_LeavesTheTaskbarWhileTheHubRuns_AndComesBackWhenItCloses()
        {
            var pipeName = "TaskTimerWidget.Tests." + Guid.NewGuid().ToString("N");
            var window = new FakeFamilyWindow();
            await using var connection = new FamilyConnection(new FamilyMembership(window), pipeName, RETRY, action => action());
            connection.Start();

            var hub = new HubServer(pipeName);
            hub.Start();
            await WaitUntil(() => !window.IsShownInTaskbar);

            await hub.DisposeAsync();
            await WaitUntil(() => window.IsShownInTaskbar);
            Assert.True(window.IsVisible);
        }

        [Fact]
        public async Task HubPlaceMessage_ReachesTheWindow()
        {
            var pipeName = "TaskTimerWidget.Tests." + Guid.NewGuid().ToString("N");
            var window = new FakeFamilyWindow();
            await using var connection = new FamilyConnection(new FamilyMembership(window), pipeName, RETRY, action => action());
            connection.Start();
            await using var hub = new HubServer(pipeName);
            hub.Start();
            await WaitUntil(() => !window.IsShownInTaskbar);

            await hub.SendAsync(FamilyMembership.MEMBER_ID, FamilyMessage.Place(Edge.Bottom, new PixelRect(900, 500, 64, 64)), CancellationToken.None);

            await WaitUntil(() => window.MovedTo is not null);
        }

        private static async Task WaitUntil(Func<bool> condition)
        {
            var deadline = DateTime.UtcNow + TIMEOUT;
            while (!condition())
            {
                Assert.True(DateTime.UtcNow < deadline, "The condition was not met in time.");
                await Task.Delay(20);
            }
        }
    }
}
