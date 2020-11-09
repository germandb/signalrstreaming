namespace SignalRStreaming
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Newtonsoft.Json;
    using SignalRStreaming.Client;
    using SignalRStreaming.Fakes;
    using SignalRStreaming.Server;

    /// <summary>
    ///     The class for an ASP.NET Core Application where the services are configured and how the
    ///     requests are handled. It is required.
    /// </summary>
    public class StartupTest
    {
        /// <summary> Initializes a new instance of the <see cref="StartupTest"/> class. </summary>
        /// <param name="configuration"> The configuration. </param>
        public StartupTest(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary> Gets or sets the service to be replaced. </summary>
        /// <value> The service to be replaced. </value>
        public static List<ServiceDescriptor> ServiceDescriptorsToReplace { get; set; }

        /// <summary> Gets or sets the additional registrations action. </summary>
        /// <value> The additional registrations action. </value>
        public static Action<IServiceCollection> AdditionalRegistrations { get; set; }

        /// <summary> Gets the configuration. </summary>
        /// <value> The configuration. </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP
        ///     request pipeline.
        /// </summary>
        /// <param name="app"> The application. </param>
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestStreamingHub>("/StreamingTestHub");
                endpoints.MapHub<TestStreamingHub>("/StreamingExceptionTestHub");
            });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"> The services. </param>
        /// <remarks>
        ///     It is necessary to make this method virtual to let the tests override its configuration.
        /// </remarks>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("Test", options => { });

            services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };
            });

            services.AddScoped<AuthenticationStore>();

            services.AddScoped<ITestStreamingHubManager, TestStreamingHubManager>();
            services.AddScoped<ICocktailTestStreamingManager, TestStreamingProxyOnline>();
            services.AddScoped<ICocktailTestStreamingManager, TestStreamingProxyOnline>();
            services.AddScoped<IStreamingTestStreamingHubConnectionManager, StreamingTestStreamingHubConnectionManager>();
            services.AddScoped<IStreamingExceptionTestStreamingHubConnectionManager, StreamingExceptionTestStreamingHubConnectionManager>();

            AdditionalRegistrations?.Invoke(services);

            // Replace the default implementation of a service with configurations for testing purposes.
            if (ServiceDescriptorsToReplace != null)
            {
                foreach (ServiceDescriptor service in ServiceDescriptorsToReplace)
                {
                    services.Replace(service);
                }
            }

            // Since tests run in parallel, it's possible multiple servers will startup and read
            // files being written by another test. Use a unique directory per server to avoid this collision.
            services.AddDataProtection()
                .PersistKeysToFileSystem(Directory.CreateDirectory(Path.GetRandomFileName()));
        }
    }
}