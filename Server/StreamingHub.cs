namespace SignalRStreaming.Server
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using SignalRStreaming.DTOs;

    /// <summary> The SignalR Hub for streaming data between the client and the server. </summary>
    /// <typeparam name="TResponseDTO"> The specific <see cref="StreamingHubResponseDTO"/>. </typeparam>
    [Authorize]
    public abstract class StreamingHub<TResponseDTO> : Hub<IStreamingHub<TResponseDTO>>
        where TResponseDTO : StreamingHubResponseDTO
    {
        private readonly ILogger logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StreamingHub{TResponseDTO}"/> class.
        /// </summary>
        /// <param name="logger"> The logger. </param>
        protected StreamingHub(ILogger<StreamingHub<TResponseDTO>> logger)
        {
            this.logger = logger;
        }

        /// <summary> Queries if application instance is already connected. </summary>
        /// <param name="connectionId"> The identifier of the connection. </param>
        /// <returns> True if connected, false if not. </returns>
        public bool IsConnected(string connectionId)
        {
            if (connectionId == null)
            {
                return false;
            }

            if (this.Clients.Client(connectionId) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Executes the connected async action. This method gets called whenever a new
        ///     connection is set.
        /// </summary>
        /// <returns> A task that enables this method to be awaited. </returns>
        public override Task OnConnectedAsync()
        {
            this.logger.LogInformation(string.Format(CultureInfo.CurrentCulture, SignalRServerMessages.ClientConnected, this.Context.ConnectionId));

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Executes the disconnected async action. This method gets called whenever an
        ///     application disconnects.
        /// </summary>
        /// <param name="ex"> The exception. </param>
        /// <returns> A task that enables this method to be awaited. </returns>
        public override Task OnDisconnectedAsync(Exception ex)
        {
            string connectionId = this.Context.ConnectionId;

            if (ex == null)
            {
                this.logger.LogInformation(string.Format(CultureInfo.CurrentCulture, SignalRServerMessages.ClientExplicitlyClosedTheConnection, connectionId));
            }
            else
            {
                this.logger.LogError(string.Format(CultureInfo.CurrentCulture, SignalRServerMessages.ClientTimeOut, connectionId));
            }

            return Task.CompletedTask;
        }

        /// <summary> Returns a processed stream. </summary>
        /// <param name="streamingHubResponseDTO"> The streaming hub response DTO. </param>
        /// <returns> The processed stream. </returns>
        public async Task ReturnProcessedStream(TResponseDTO streamingHubResponseDTO)
        {
            await this.Clients.All.ReceiveStreamingResponse(new RequestResultDTO<TResponseDTO>(streamingHubResponseDTO));
        }
    }
}