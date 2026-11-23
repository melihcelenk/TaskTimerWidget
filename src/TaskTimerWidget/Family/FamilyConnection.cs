using WidgetFamily;

namespace TaskTimerWidget.Family
{
    /// <summary>
    /// Connects the membership to the hub's pipe. Pipe events arrive on background threads; they are handed to the
    /// membership through <c>dispatch</c>, which runs them on the UI thread.
    /// </summary>
    public sealed class FamilyConnection : IAsyncDisposable
    {
        /// <summary>How long to wait for the hub before looking again.</summary>
        public static readonly TimeSpan RETRY_INTERVAL = TimeSpan.FromSeconds(2);

        private readonly HubClient _client;

        /// <summary>Creates the connection.</summary>
        /// <param name="membership">The membership to drive.</param>
        /// <param name="pipeName">The hub's pipe name.</param>
        /// <param name="retryInterval">How long to wait for the hub before looking again.</param>
        /// <param name="dispatch">Runs an action on the UI thread.</param>
        public FamilyConnection(FamilyMembership membership, string pipeName, TimeSpan retryInterval, Action<Action> dispatch)
        {
            ArgumentNullException.ThrowIfNull(membership);
            ArgumentNullException.ThrowIfNull(dispatch);
            _client = new HubClient(pipeName, FamilyMembership.MEMBER_ID, retryInterval);
            _client.HubArrived += (_, _) => dispatch(membership.OnHubArrived);
            _client.HubLeft += (_, _) => dispatch(membership.OnHubLeft);
            _client.MessageReceived += (_, message) => dispatch(() => membership.OnMessage(message));
        }

        /// <summary>Starts looking for the hub.</summary>
        public void Start() => _client.Start();

        /// <summary>Stops looking for the hub.</summary>
        /// <returns>A task that completes when the connection is closed.</returns>
        public ValueTask DisposeAsync() => _client.DisposeAsync();
    }
}
