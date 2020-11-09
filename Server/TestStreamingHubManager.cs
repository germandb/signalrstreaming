namespace SignalRStreaming.Server
{
    using System;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using SignalRStreaming.DTOs;

    /// <inheritdoc/>
    public partial class TestStreamingHubManager : ITestStreamingHubManager
    {
        /// <inheritdoc/>
        public event Func<OutputDTO, Task> StreamingTestProcessed;

        /// <inheritdoc/>
        public async Task StreamingTestAsync(ChannelReader<InputDTO> channelReader)
        {
            while (await channelReader.WaitToReadAsync().ConfigureAwait(false))
            {
                channelReader.TryRead(out InputDTO inputDTO);

                await this.StreamingTestProcessed?.Invoke(new OutputDTO { Count = inputDTO.Count });
            }
        }

        /// <inheritdoc/>
        public async Task StreamingExceptionTestAsync(ChannelReader<InputDTO> channelReader)
        {
            while (await channelReader.WaitToReadAsync().ConfigureAwait(false))
            {
                channelReader.TryRead(out InputDTO inputDTO);

                OutputDTO outputDTO = new OutputDTO();
                outputDTO.Count = inputDTO.Count;

                if (outputDTO.Count >= 5)
                {
                    throw new InvalidOperationException("This is a specific technical message that must be registered in telemetry. It won't be shown to the end user.");
                }

                await this.StreamingTestProcessed?.Invoke(outputDTO);
            }
        }
    }
}