namespace SignalRStreaming.Client
{
    using SignalRStreaming.DTOs;

    /// <summary> Interface for the 'streaming exception test' ad hoc action. </summary>
    public interface IStreamingExceptionTestStreamingHubConnectionManager : IStreamingHubConnectionManager<InputDTO, OutputDTO>
    {
    }
}