namespace SignalRStreaming.Server
{
    using System;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using SignalRStreaming.DTOs;

    /// <summary>
    ///     Interface to manage the Streaming Hub that will be implemented by the Application layer.
    /// </summary>
    public interface ITestStreamingHubManager
    {
        /// <summary>
        ///     Event raised when the <see cref="StreamingTestAsync(ChannelReader{InputDTO})"/> is processed.
        /// </summary>
        event Func<OutputDTO, Task> StreamingTestProcessed;

        /// <summary> An Ad hoc Action for testing the streaming communication type. </summary>
        /// <param name="channelReader">
        ///     The channel reader for the <see cref="InputDTO"/> for the streaming Ad hoc Action.
        /// </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task StreamingTestAsync(ChannelReader<InputDTO> channelReader);

        /// <summary> Streaming exception test async. </summary>
        /// <param name="channelReader"> The channel reader. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        Task StreamingExceptionTestAsync(ChannelReader<InputDTO> channelReader);
    }
}