namespace SignalRStreaming.Server
{
    using System;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using SignalRStreaming.DTOs;

    /// <summary> Hub for testing the streaming with SignalR. </summary>
    public class TestStreamingHub : StreamingHub<OutputDTO>
    {
        private readonly ILogger<TestStreamingHub> logger;
        private readonly ITestStreamingHubManager testStreamingHubManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestStreamingHub"/> class.
        /// </summary>
        /// <param name="logger"> The logger. </param>
        /// <param name="testStreamingHubManager"> Streaming Hub manager logic. </param>
        public TestStreamingHub(ILogger<TestStreamingHub> logger, ITestStreamingHubManager testStreamingHubManager)
            : base(logger)
        {
            this.logger = logger;
            this.testStreamingHubManager = testStreamingHubManager;
        }

        /// <summary> An Ad hoc Action for testing the streaming communication type. </summary>
        /// <param name="channelReader">
        ///     The channel reader for the Input DTO for the streaming Ad hoc Action.
        /// </param>
        /// <returns> A task that enables this method to be awaited. </returns>
        public async Task StreamingTestAsync(ChannelReader<InputDTO> channelReader)
        {
            this.testStreamingHubManager.StreamingTestProcessed += this.TestStreamingHubManager_StreamingTestProcessed;

            await this.testStreamingHubManager.StreamingTestAsync(channelReader).ConfigureAwait(false);

            this.testStreamingHubManager.StreamingTestProcessed -= this.TestStreamingHubManager_StreamingTestProcessed;
        }

        /// <summary> Streaming exception test async. </summary>
        /// <param name="channelReader">
        ///     The channel reader for the Input DTO for the streaming Ad hoc Action.
        /// </param>
        /// <returns> A task that enables this method to be awaited. </returns>
        public async Task StreamingExceptionTestAsync(ChannelReader<InputDTO> channelReader)
        {
            this.testStreamingHubManager.StreamingTestProcessed += this.TestStreamingHubManager_StreamingTestProcessed;

            try
            {
                await this.testStreamingHubManager.StreamingExceptionTestAsync(channelReader).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                RequestResultDTO<OutputDTO> requestResultDTO = new RequestResultDTO<OutputDTO>(ex, correlationToken: Guid.NewGuid());

                await this.SendRequestResultAsync(requestResultDTO);
            }

            this.testStreamingHubManager.StreamingTestProcessed -= this.TestStreamingHubManager_StreamingTestProcessed;
        }

        /// <summary>
        ///     Event handler. Raised by the <see cref="ITestStreamingHubManager"/> when the
        ///     response is processed.
        /// </summary>
        /// <param name="outputDTO"> Output DTO for the streaming Ad hoc Action. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        private async Task TestStreamingHubManager_StreamingTestProcessed(OutputDTO outputDTO)
        {
            await this.SendRequestResultAsync(new RequestResultDTO<OutputDTO>(outputDTO));
        }

        /// <summary> Send request result async./ </summary>
        /// <param name="requestResultDTO"> The request result DTO. </param>
        /// <returns>
        ///     A task that doesn't include a result and enables this method to be awaited.
        /// </returns>
        private async Task SendRequestResultAsync(RequestResultDTO<OutputDTO> requestResultDTO)
        {
            await this.Clients.All.ReceiveStreamingResponse(requestResultDTO).ConfigureAwait(false);
        }
    }
}