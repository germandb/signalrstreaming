namespace SignalRStreaming.Server
{
    using System.Threading.Tasks;
    using SignalRStreaming.DTOs;

    /// <summary> Interface for the actions of the streaming Hub. </summary>
    /// <typeparam name="TResponseDTO"> The specific <see cref="StreamingHubResponseDTO"/>. </typeparam>
    public interface IStreamingHub<TResponseDTO>
        where TResponseDTO : StreamingHubResponseDTO
    {
        /// <summary> Receive the streaming response. </summary>
        /// <param name="streamingHubResponseDTO"> The streaming hub response DTO. </param>
        /// <returns> A task that enables this method to be awaited. </returns>
        Task ReceiveStreamingResponse(RequestResultDTO<TResponseDTO> streamingHubResponseDTO);
    }
}