namespace SignalRStreaming.Client
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using SignalRStreaming.DTOs;

    /// <summary> Streaming Hub connection manager of the 'StreamingTest' Ad Hoc action. </summary>
    public class StreamingTestStreamingHubConnectionManager : StreamingHubConnectionManager<InputDTO, OutputDTO>, IStreamingTestStreamingHubConnectionManager
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="StreamingTestStreamingHubConnectionManager"/> class.
        /// </summary>
        /// <param name="streamingTestOptions"> Options for controlling the streaming test. </param>
        /// <param name="logger"> The logger. </param>
        /// <param name="authenticationStore"> The authentication store. </param>
        public StreamingTestStreamingHubConnectionManager(
            IOptions<StreamingTestOptions> streamingTestOptions,
            ILogger<StreamingTestStreamingHubConnectionManager> logger,
            AuthenticationStore authenticationStore)
            : base(new Uri(streamingTestOptions?.Value?.ServerUriBase, "/StreamingTestHub"), logger, authenticationStore)
        {
        }
    }
}