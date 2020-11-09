namespace SignalRStreaming.DTOs
{
    /// <summary> Input DTO for the test streaming Ad hoc Action. </summary>
    public class InputDTO : StreamingHubRequestDTO
    {
        /// <summary> Gets or sets the number of elements. </summary>
        /// <value> The count. </value>
        public int Count { get; set; }
    }
}