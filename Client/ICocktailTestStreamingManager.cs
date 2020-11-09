namespace SignalRStreaming.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;
    using SignalRStreaming.DTOs;

    /// <summary> Interface for the manager of the streaming Ad Hoc Actions. </summary>
    public interface ICocktailTestStreamingManager
    {
        /// <summary> Gets or sets the connection. </summary>
        /// <value> The connection. </value>
        HubConnection HubConnection { get; }

        /// <summary> Send the streaming exception test data. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="inputDTO"> The input DTO. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task SendStreamingExceptionTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, InputDTO inputDTO, CancellationToken cancellationToken);

        /// <summary> Stop the 'streaming exception test' stream async. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task StopStreamingExceptionTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, CancellationToken cancellationToken);

        /// <summary> Send the streaming test data. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="inputDTO"> The input DTO. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task SendStreamingTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, InputDTO inputDTO, CancellationToken cancellationToken);

        /// <summary> Stop the "streaming test" stream async. </summary>
        /// <param name="funcEventHandlerAsync">
        ///     The function to call in the client with the server respose.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task StopStreamingTestStreamAsync(Func<OutputDTO, Task> funcEventHandlerAsync, CancellationToken cancellationToken);
    }
}