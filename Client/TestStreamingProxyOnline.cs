namespace SignalRStreaming.Client
{
    using System;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using SignalRStreaming.DTOs;
    using SignalRStreaming.Exceptions;

    /// <summary> Streaming Proxy for all the streaming Ad Hoc Actions. </summary>
    public class TestStreamingProxyOnline : ICocktailTestStreamingManager
    {
        private readonly IOptions<StreamingTestOptions> streamingTestOptions;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<TestStreamingProxyOnline> logger;
        private readonly AuthenticationStore authenticationStore;
        private readonly IStreamingTestStreamingHubConnectionManager streamingTestStreamingHubConnectionManager;
        private readonly IStreamingExceptionTestStreamingHubConnectionManager streamingTestExceptionStreamingHubConnectionManager;
        private Channel<InputDTO> streamingTestChannel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestStreamingProxyOnline"/> class.
        /// </summary>
        /// <param name="streamingTestOptions"> Options for controlling the streaming test. </param>
        /// <param name="loggerFactory"> The logger. </param>
        /// <param name="authenticationStore"> The authentication store. </param>
        /// <param name="streamingTestStreamingHubConnectionManager">
        ///     The streaming test hub connection manager.
        /// </param>
        /// <param name="streamingTestExceptionStreamingHubConnectionManager">
        ///     The streaming test exception hub connection manager.
        /// </param>
        public TestStreamingProxyOnline(
            IOptions<StreamingTestOptions> streamingTestOptions,
            ILoggerFactory loggerFactory,
            AuthenticationStore authenticationStore,
            IStreamingTestStreamingHubConnectionManager streamingTestStreamingHubConnectionManager,
            IStreamingExceptionTestStreamingHubConnectionManager streamingTestExceptionStreamingHubConnectionManager)
        {
            this.streamingTestOptions = streamingTestOptions;
            this.loggerFactory = loggerFactory;
            this.authenticationStore = authenticationStore;
            this.streamingTestStreamingHubConnectionManager = streamingTestStreamingHubConnectionManager;
            this.streamingTestExceptionStreamingHubConnectionManager = streamingTestExceptionStreamingHubConnectionManager;
            this.logger = loggerFactory.CreateLogger<TestStreamingProxyOnline>();
        }

        public HubConnection HubConnection => this.streamingTestStreamingHubConnectionManager.HubConnection;

        /// <inheritdoc/>
        public async Task SendStreamingTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, InputDTO inputDTO, CancellationToken cancellationToken)
        {
            Channel<InputDTO> channel = await this.GetStreamingTestChannelAsync((requestResultDTO) => funcEventHandlerAsync.Invoke(requestResultDTO.Result), cancellationToken).ConfigureAwait(false);

            if (this.streamingTestStreamingHubConnectionManager.IsConnectionStarted)
            {
                this.streamingTestStreamingHubConnectionManager.SendStream(channel, inputDTO, cancellationToken);
            }
            else
            {
                this.logger.LogError("The call to the SendStreamingTestStreamAsync failed becouse the connection is not started yet.");

                throw new CoreException(ErrorCode.UnknownError);
            }
        }

        /// <inheritdoc/>
        public async Task StopStreamingTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, CancellationToken cancellationToken)
        {
            Channel<InputDTO> channel = await this.GetStreamingTestChannelAsync((requestResultDTO) => funcEventHandlerAsync.Invoke(requestResultDTO.Result), cancellationToken).ConfigureAwait(false);

            this.streamingTestStreamingHubConnectionManager.CompleteChannel(channel);

            await this.streamingTestStreamingHubConnectionManager.StopConnectionAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task SendStreamingExceptionTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, InputDTO inputDTO, CancellationToken cancellationToken)
        {
            Channel<InputDTO> channel = await this.GetStreamingExceptionTestChannelAsync((requestResultDTO) => funcEventHandlerAsync.Invoke(requestResultDTO.Result), cancellationToken).ConfigureAwait(false);

            if (this.streamingTestExceptionStreamingHubConnectionManager.IsConnectionStarted)
            {
                this.streamingTestExceptionStreamingHubConnectionManager.SendStream(channel, inputDTO, cancellationToken);
            }
            else
            {
                throw new CoreException(ErrorCode.UnknownError);
            }
        }

        /// <inheritdoc/>
        public async Task StopStreamingExceptionTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, CancellationToken cancellationToken)
        {
            Channel<InputDTO> channel = await this.GetStreamingExceptionTestChannelAsync((requestResultDTO) => funcEventHandlerAsync.Invoke(requestResultDTO.Result), cancellationToken).ConfigureAwait(false);

            this.streamingTestExceptionStreamingHubConnectionManager.CompleteChannel(channel);

            await this.streamingTestExceptionStreamingHubConnectionManager.StopConnectionAsync().ConfigureAwait(false);
        }

        /// <summary> Gets a streaming test channel async. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> A Task that contains the streaming test channel. </returns>
        private async Task<Channel<InputDTO>> GetStreamingTestChannelAsync(Func<RequestResultDTO<OutputDTO>, Task> funcEventHandlerAsync, CancellationToken cancellationToken)
        {
            if (this.streamingTestChannel is null)
            {
                await this.StartStreamingTestChannelAsync(funcEventHandlerAsync, cancellationToken);
            }

            return this.streamingTestChannel;
        }

        /// <summary> Starts a streaming test channel. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        private async Task StartStreamingTestChannelAsync(Func<RequestResultDTO<OutputDTO>, Task> funcEventHandlerAsync, CancellationToken cancellationToken)
        {
            // Initialize the connection and configure the channel.
            await this.streamingTestStreamingHubConnectionManager.StartConnectionAsync(funcEventHandlerAsync).ConfigureAwait(false);

            this.streamingTestChannel = this.streamingTestStreamingHubConnectionManager.CreateChannel();

            await this.streamingTestStreamingHubConnectionManager.SetupChannelAsync(this.streamingTestChannel, "StreamingTestAsync", cancellationToken);
        }

        /// <summary> Starts a streaming test exception channel. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        private async Task StartStreamingTestExceptionChannelAsync(Func<RequestResultDTO<OutputDTO>, Task> funcEventHandlerAsync, CancellationToken cancellationToken)
        {
            // Initialize the connection and configure the channel.
            await this.streamingTestExceptionStreamingHubConnectionManager.StartConnectionAsync(funcEventHandlerAsync).ConfigureAwait(false);

            this.streamingTestChannel = this.streamingTestExceptionStreamingHubConnectionManager.CreateChannel();

            await this.streamingTestExceptionStreamingHubConnectionManager.SetupChannelAsync(this.streamingTestChannel, "StreamingExceptionTestAsync", cancellationToken);
        }

        /// <summary> Gets a streaming exception test channel async. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> A Task that conatains the streaming exception test channel. </returns>
        private async Task<Channel<InputDTO>> GetStreamingExceptionTestChannelAsync(Func<RequestResultDTO<OutputDTO>, Task> funcEventHandlerAsync, CancellationToken cancellationToken)
        {
            if (this.streamingTestChannel is null)
            {
                await this.StartStreamingTestExceptionChannelAsync(funcEventHandlerAsync, cancellationToken);
            }

            return this.streamingTestChannel;
        }
    }
}