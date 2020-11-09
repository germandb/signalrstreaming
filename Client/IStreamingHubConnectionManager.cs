namespace SignalRStreaming.Client
{
    using System;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;
    using SignalRStreaming.DTOs;

    /// <summary> Interface that defines a manager for streaming Hub connections. </summary>
    /// <typeparam name="TRequestDTO"> The specific <see cref="StreamingHubRequestDTO"/>. </typeparam>
    /// <typeparam name="TResponseDTO"> The specific <see cref="StreamingHubResponseDTO"/>. </typeparam>
    public interface IStreamingHubConnectionManager<TRequestDTO, TResponseDTO>
        where TResponseDTO : StreamingHubResponseDTO
        where TRequestDTO : StreamingHubRequestDTO
    {
        /// <summary> Gets the URL of the Hub. </summary>
        /// <value> The URL of the Hub. </value>
        Uri Url { get; }

        /// <summary> Gets a value indicating whether the connection is started. </summary>
        /// <value> True if the connection is started, false if not. </value>
        bool IsConnectionStarted { get; }

        /// <summary> Gets or sets the connection. </summary>
        /// <value> The connection. </value>
        HubConnection HubConnection { get; }

        /// <summary> Stops the connection to the Hub. </summary>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task StopConnectionAsync();

        /// <summary> Starts the connection to the Hub. </summary>
        /// <param name="funcEventHandlerAsync"> The method to be invoked by the Hub. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task StartConnectionAsync(Func<RequestResultDTO<TResponseDTO>, Task> funcEventHandlerAsync);

        /// <summary> Sets up the channel. </summary>
        /// <param name="channel"> The channel. </param>
        /// <param name="clientMethod"> The client method. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task SetupChannelAsync(Channel<TRequestDTO> channel, string clientMethod, CancellationToken cancellationToken);

        /// <summary> Creates a channel. </summary>
        /// <returns>
        ///     A Channel that enable to read and write the DTO used for the requests to the SignalR
        ///     streaming hub.
        /// </returns>
        Channel<TRequestDTO> CreateChannel();

        /// <summary> Complete channel. </summary>
        /// <param name="channel"> The channel. </param>
        void CompleteChannel(Channel<TRequestDTO> channel);

        /// <summary> Send stream. </summary>
        /// <param name="channel"> The channel. </param>
        /// <param name="streamingHubRequestDTO"> The streaming hub request DTO. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        void SendStream(Channel<TRequestDTO> channel, TRequestDTO streamingHubRequestDTO, CancellationToken cancellationToken);
    }
}