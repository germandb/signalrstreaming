namespace SignalRStreaming.Client
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using SignalRStreaming.DTOs;
    using SignalRStreaming.Exceptions;

    /// <summary> Manager for Streaming Hub connections. </summary>
    /// <typeparam name="TRequestDTO"> The specific <see cref="StreamingHubRequestDTO"/>. </typeparam>
    /// <typeparam name="TResponseDTO"> The specific <see cref="StreamingHubResponseDTO"/>. </typeparam>
    public abstract class StreamingHubConnectionManager<TRequestDTO, TResponseDTO> : IStreamingHubConnectionManager<TRequestDTO, TResponseDTO>
        where TResponseDTO : StreamingHubResponseDTO
        where TRequestDTO : StreamingHubRequestDTO
    {
        private readonly ILogger logger;
        private readonly AuthenticationStore authenticationStore;
        private HubConnection connection;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="StreamingHubConnectionManager{TRequestDTO, TResponseDTO}"/> class.
        /// </summary>
        /// <param name="url"> The URL of the Hub. For example: http://localhost:4131/Tasks/AdHocActionStreamingHub. </param>
        /// <param name="logger"> The logger. </param>
        /// <param name="authenticationStore"> The authentication store. </param>
        /// <remarks>
        ///     For each streaming Ad Hoc action a new StreamingHubConnectionManager got created.
        /// </remarks>
        protected StreamingHubConnectionManager(
            Uri url,
            ILogger<StreamingHubConnectionManager<TRequestDTO, TResponseDTO>> logger,
            AuthenticationStore authenticationStore)
        {
            this.Url = url;
            this.logger = logger;
            this.authenticationStore = authenticationStore;
        }

        public HubConnection HubConnection => this.connection;

        /// <inheritdoc/>
        public Uri Url { get; private set; }

        /// <inheritdoc/>
        public bool IsConnectionStarted => this.connection != null && this.connection.State.Equals(HubConnectionState.Connected);

        /// <inheritdoc/>
        public void SendStream(
            Channel<TRequestDTO> channel,
            TRequestDTO streamingHubRequestDTO,
            CancellationToken cancellationToken)
        {
            try
            {
                channel.Writer.WriteAsync(streamingHubRequestDTO, cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);

                throw new CoreException(string.Format(CultureInfo.CurrentCulture, SignalRClientMessages.ErrorSendStream), ex);
            }
        }

        /// <inheritdoc/>
        public async Task StartConnectionAsync(Func<RequestResultDTO<TResponseDTO>, Task> funcEventHandlerAsync)
        {
            this.connection =
                new HubConnectionBuilder()
                    .ConfigureLogging(log =>
                    {
                        log.SetMinimumLevel(LogLevel.Information);
                    })
                    .WithUrl(this.Url, opt =>
                    {
                        opt.Transports = HttpTransportType.WebSockets;
                        opt.SkipNegotiation = true;
                        opt.AccessTokenProvider = () => Task.FromResult(this.authenticationStore.AccessToken);
                    })
                    .AddNewtonsoftJsonProtocol(options =>
                    {
                        options.PayloadSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };
                    })
                    .Build();

            this.SubscribeConnectionToEvents(funcEventHandlerAsync);

            await this.connection.StartAsync().ConfigureAwait(true);

            this.logger.LogInformation(string.Format(CultureInfo.CurrentCulture, SignalRClientMessages.ConnectionStarted, this.connection.ConnectionId));
        }

        /// <inheritdoc/>
        public async Task StopConnectionAsync()
        {
            this.UnsubscribeConnectionFromEvents();

            await this.connection.StopAsync().ConfigureAwait(true);

            this.logger.LogInformation(string.Format(CultureInfo.CurrentCulture, SignalRClientMessages.ConnectionStop, this.connection.ConnectionId));

            this.connection = null;
        }

        /// <inheritdoc/>
        public void CompleteChannel(Channel<TRequestDTO> channel)
        {
            try
            {
                channel.Writer.TryComplete();
            }
            catch (Exception ex)
            {
                channel.Writer.TryComplete(ex);

                this.logger.LogError(ex.Message);

                throw new CoreException(string.Format(CultureInfo.CurrentCulture, SignalRClientMessages.ErrorSendStream), ex);
            }

            this.logger.LogInformation(string.Format(CultureInfo.CurrentCulture, SignalRClientMessages.CompleteChannel, this.connection.ConnectionId));
        }

        /// <inheritdoc/>
        public Channel<TRequestDTO> CreateChannel()
        {
            this.logger.LogInformation(string.Format(CultureInfo.CurrentCulture, SignalRClientMessages.CreateChannel, this.connection.ConnectionId));

            return Channel.CreateUnbounded<TRequestDTO>();
        }

        /// <inheritdoc/>
        public async Task SetupChannelAsync(Channel<TRequestDTO> channel, string clientMethod, CancellationToken cancellationToken)
        {
            // Open channel connection.
            await this.connection.SendAsync(clientMethod, channel.Reader, cancellationToken);
        }

        /// <summary> Subscribes the connection to events. </summary>
        /// <param name="funcEventHandlerAsync"> The method to be invoked when the Hub indicates. </param>
        private void SubscribeConnectionToEvents(Func<RequestResultDTO<TResponseDTO>, Task> funcEventHandlerAsync)
        {
            this.connection.Closed += this.ConnectionClosedEventHandlerAsync;

            this.connection.On<RequestResultDTO<TResponseDTO>>("ReceiveStreamingResponse", async (streamingResult) =>
            {
                await funcEventHandlerAsync(streamingResult).ConfigureAwait(true);
            });
        }

        /// <summary> Unsubscribes the connection from events. </summary>
        private void UnsubscribeConnectionFromEvents() => this.connection.Closed -= this.ConnectionClosedEventHandlerAsync;

        /// <summary> Connection closed event handler. </summary>
        /// <param name="exception"> The exception. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        private Task ConnectionClosedEventHandlerAsync(Exception exception)
        {
            if (exception is null)
            {
                return Task.CompletedTask;
            }

            return Task.Run(() => this.logger.LogError(exception?.Message));
        }
    }
}