namespace SignalRStreaming.Client
{
    using SignalRStreaming.DTOs;

    /// <summary> Interface for the 'streaming test' ad hoc action. </summary>
    public interface IStreamingTestStreamingHubConnectionManager : IStreamingHubConnectionManager<InputDTO, OutputDTO>
    {
    }
}