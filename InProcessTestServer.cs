namespace SignalRStreaming
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Testing;

    /// <summary> In process test server. </summary>
    /// <typeparam name="TStartup"> The startup class. </typeparam>
    public class InProcessTestServer<TStartup> : InProcessTestServerBase
        where TStartup : class
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger logger;
        private readonly IDisposable logToken;
        private readonly IDisposable extraDisposable;
        private IHost host;
        private IHostApplicationLifetime lifetime;
        private string url;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InProcessTestServer{TStartup}"/> class.
        /// </summary>
        private InProcessTestServer()
            : this(loggerFactory: null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InProcessTestServer{TStartup}"/> class.
        /// </summary>
        /// <param name="loggerFactory"> The logger factory. </param>
        /// <param name="disposable"> The disposable. </param>
        private InProcessTestServer(ILoggerFactory loggerFactory, IDisposable disposable)
        {
            this.extraDisposable = disposable;

            if (loggerFactory == null)
            {
                AssemblyTestLog testLog = AssemblyTestLog.ForAssembly(typeof(TStartup).Assembly);
                this.logToken = testLog.StartTestLog(
                    output: null,
                    className: $"{nameof(InProcessTestServer<TStartup>)}{typeof(TStartup).Name}",
                    out this.loggerFactory,
                    testName: nameof(InProcessTestServerBase));
            }
            else
            {
                this.loggerFactory = loggerFactory;
            }

            this.logger = loggerFactory.CreateLogger<InProcessTestServer<TStartup>>();
        }

        /// <summary> Gets URL of the web sockets. </summary>
        /// <value> The web sockets url. </value>
        public override string WebSocketsUrl => this.Url.Replace("http", "ws");

        /// <summary> Gets URL of the document. </summary>
        /// <value> The url. </value>
        public override string Url => this.url;

        /// <summary> Gets the services. </summary>
        /// <value> The services. </value>
        public IServiceProvider Services => this.host.Services;

        /// <summary> Starts a server. </summary>
        /// <param name="loggerFactory"> The logger factory. </param>
        /// <param name="disposable"> (optional) the disposable. </param>
        /// <returns> The <see cref="InProcessTestServer{TStartup}"/> started. </returns>
        public static async Task<InProcessTestServer<TStartup>> StartServer(ILoggerFactory loggerFactory, IDisposable disposable = null)
        {
            InProcessTestServer<TStartup> server = new InProcessTestServer<TStartup>(loggerFactory, disposable);
            await server.StartServerInner();
            return server;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources asynchronously.
        /// </summary>
        /// <returns> A task that represents the asynchronous dispose operation. </returns>
        public override async ValueTask DisposeAsync()
        {
            try
            {
                this.extraDisposable?.Dispose();
                this.logger.LogInformation("Start shutting down test server");
            }
            finally
            {
                await this.host.StopAsync();
                this.host.Dispose();
                this.loggerFactory.Dispose();
            }
        }

        /// <summary> Starts the server. </summary>
        /// <exception cref="TimeoutException"> Thrown when timeout. </exception>
        /// <returns> A task that enables this method to be awaited. </returns>
        private async Task StartServerInner()
        {
            // The use of 127.0.0.1 instead of localhost ensures the use of IPV4 across different OSes.
            string baseUrl = "http://127.0.0.1";

            this.host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder
                    .ConfigureLogging(builder =>
                        builder.SetMinimumLevel(LogLevel.Trace)
                               .AddProvider(new ForwardingLoggerProvider(this.loggerFactory)))
                    .UseStartup(typeof(TStartup))
                    .UseEnvironment("Testing")
                    .UseKestrel()
                    .UseUrls(baseUrl)
                    .UseContentRoot(Directory.GetCurrentDirectory());
                }).Build();

            this.logger.LogInformation("Starting test server...");

            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            try
            {
                await this.host.StartAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException($"Timed out waiting for application to start.");
            }

            this.logger.LogInformation("Test Server started");

            // Get the URL from the server
            this.url = this.host.Services.GetService<IServer>().Features.Get<IServerAddressesFeature>().Addresses.Single();

            this.lifetime = this.host.Services.GetRequiredService<IHostApplicationLifetime>();
            this.lifetime.ApplicationStopped.Register(() =>
            {
                this.logger.LogInformation("Test server shut down");
                this.logToken?.Dispose();
            });
        }

        /// <summary> Creates the Forwarding logger provider. </summary>
        private class ForwardingLoggerProvider : ILoggerProvider
        {
            private readonly ILoggerFactory loggerFactory;

            public ForwardingLoggerProvider(ILoggerFactory loggerFactory)
            {
                this.loggerFactory = loggerFactory;
            }

            public void Dispose()
            {
                this.loggerFactory.Dispose();
            }

            public ILogger CreateLogger(string categoryName)
            {
                return this.loggerFactory.CreateLogger(categoryName);
            }
        }
    }
}