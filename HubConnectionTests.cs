namespace SignalRStreaming
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using SignalRStreaming.Client;
    using SignalRStreaming.DTOs;

    /// <summary>
    ///     This class contains tests that verify how the <see cref="StreamingHub"/> are invoked
    ///     correctly by the client.
    /// </summary>
    [TestClass]
    public class HubConnectionTests
    {
        /// <summary>
        ///     Streaming test called with token authentication completes invoke successfully.
        /// </summary>
        /// <returns> A task that enables this method to be awaited. </returns>
        [TestMethod]
        public async Task StreamingTest_CalledWithTokenAuthentication_CompletesInvokeSuccessfully()
        {
            // Arrange.
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = loggerFactory.CreateLogger("StreamingTest");

            void AdditionalRegistrations(IServiceCollection s)
            {
                s.Configure<StreamingTestOptions>((options) => options.ServerUriBase = new Uri("http://127.0.0.1"));
            }

            StartupTest.AdditionalRegistrations = AdditionalRegistrations;

            List<OutputDTO> outputDTOs = new List<OutputDTO>();

            // Configure Services.
            await using (InProcessTestServer<StartupTest> testServer = await InProcessTestServer<StartupTest>.StartServer(loggerFactory))
            {
                // Initialize the Proxy.
                ICocktailTestStreamingManager cocktailTestStreamingManager = testServer.Services.GetService<ICocktailTestStreamingManager>();

                // Act.
                try
                {
                    CancellationToken cancellationToken = CancellationToken.None;

                    // Send the data to the Hub.
                    for (int ne = 1; ne <= 10; ne++)
                    {
                        InputDTO inputDTO = new InputDTO() { Count = ne };

                        await cocktailTestStreamingManager.SendStreamingTestStreamAsync(StreamingProcessedAsync, inputDTO, cancellationToken);
                    }

                    // Wait for the Hub to process all the data.
                    TimeSpan startTime = new TimeSpan(DateTime.Now.Ticks);

                    while (outputDTOs.Count < 10)
                    {
                        await Task.Delay(100);
                    }

                    // Assert.
                    List<OutputDTO> expectedOuputDTO = new List<OutputDTO>()
                    {
                        new OutputDTO() { Count = 1 },
                        new OutputDTO() { Count = 2 },
                        new OutputDTO() { Count = 3 },
                        new OutputDTO() { Count = 4 },
                        new OutputDTO() { Count = 5 },
                        new OutputDTO() { Count = 6 },
                        new OutputDTO() { Count = 7 },
                        new OutputDTO() { Count = 8 },
                        new OutputDTO() { Count = 9 },
                        new OutputDTO() { Count = 10 },
                    };

                    outputDTOs.Count.Should().Be(10, because: "10 objects should be streamed into the channel.");

                    expectedOuputDTO.Should().BeEquivalentTo(outputDTOs);

                    await cocktailTestStreamingManager.StopStreamingTestStreamAsync(StreamingProcessedAsync, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{ExceptionType} from test", ex.GetType().FullName);

                    throw;
                }
            }

            // Take the processed result from the Hub and write the output count.
            async Task StreamingProcessedAsync(OutputDTO outputDTO)
            {
                logger.LogInformation("The count is {0}", outputDTO.Count);

                outputDTOs.Add(outputDTO);

                await Task.CompletedTask.ConfigureAwait(false);
            }
        }

        /// <summary> Streaming test called with a client method that returns an exception. </summary>
        /// <returns> A task that enables this method to be awaited. </returns>
        [TestMethod]
        public async Task StreamingExceptionTest_ReturnCocktailHubException()
        {
            // Arrange.
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<StreamingExceptionTestStreamingHubConnectionManager> logger = loggerFactory.CreateLogger<StreamingExceptionTestStreamingHubConnectionManager>();

            void AdditionalRegistrations(IServiceCollection s)
            {
                s.Configure<StreamingTestOptions>((options) => options.ServerUriBase = new Uri("http://127.0.0.1"));
            }

            StartupTest.AdditionalRegistrations = AdditionalRegistrations;
            List<OutputDTO> outputDTOs = new List<OutputDTO>();

            // Configure Services.
            await using (InProcessTestServer<StartupTest> testServer = await InProcessTestServer<StartupTest>.StartServer(loggerFactory))
            {
                // Initialize the Proxy.
                ICocktailTestStreamingManager cocktailTestStreamingManager = testServer.Services.GetService<ICocktailTestStreamingManager>();
                CancellationToken cancellationToken = CancellationToken.None;

                // Act.
                try
                {
                    // Send the data to the Hub.
                    for (int ne = 1; ne <= 10; ne++)
                    {
                        InputDTO inputDTO = new InputDTO() { Count = ne };

                        await cocktailTestStreamingManager.SendStreamingExceptionTestStreamAsync(StreamingExceptionAsync, inputDTO, cancellationToken);
                    }

                    // Assert. Wait for the Hub to process all the data.
                    TimeSpan startTime = new TimeSpan(DateTime.Now.Ticks);

                    while (outputDTOs.Count < 10)
                    {
                        double elapsedTime = new TimeSpan(DateTime.Now.Ticks).Subtract(startTime).TotalMilliseconds;

                        if (elapsedTime > 15000)
                        {
                            throw new TimeoutException("The channel processing time has been more than 10 seconds.");
                        }

                        await Task.Delay(100);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{ExceptionType} from test", ex.GetType().FullName);
                    ex.Should().BeOfType(typeof(HubException));
                }
                finally
                {
                    await cocktailTestStreamingManager.StopStreamingExceptionTestStreamAsync(StreamingExceptionAsync, cancellationToken);
                }
            }

            // Take the processed result from the Hub and write the output count.
            async Task StreamingExceptionAsync(OutputDTO outputDTO)
            {
                logger.LogInformation("The count is {0}", outputDTO.Count);
                outputDTOs.Add(outputDTO);
                await Task.CompletedTask.ConfigureAwait(false);
            }
        }
    }
}