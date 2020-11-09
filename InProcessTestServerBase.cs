namespace SignalRStreaming
{
    using System;
    using System.Threading.Tasks;

    /// <summary> In process test server. </summary>
    public abstract class InProcessTestServerBase : IAsyncDisposable
    {
        /// <summary> Gets URL of the web sockets./ </summary>
        /// <value> The web sockets url. </value>
        public abstract string WebSocketsUrl { get; }

        /// <summary> Gets URL of the document. </summary>
        /// <value> The url. </value>
        public abstract string Url { get; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources asynchronously.
        /// </summary>
        /// <returns> A task that represents the asynchronous dispose operation. </returns>
        public abstract ValueTask DisposeAsync();
    }
}