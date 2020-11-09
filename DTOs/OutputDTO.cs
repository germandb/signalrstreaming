namespace SignalRStreaming.DTOs
{
    /// <summary> Output DTO for the streaming Ad hoc Action. </summary>
    public class OutputDTO : StreamingHubResponseDTO
    {
        /// <summary> Gets or sets the number of elements. </summary>
        /// <value> The count. </value>
        public int Count { get; set; }
    }
}