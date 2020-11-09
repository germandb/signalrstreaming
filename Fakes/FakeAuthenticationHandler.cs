namespace SignalRStreaming.Fakes
{
    using System;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary> Fake authentication handler. </summary>
    /// <remarks>
    ///     This code was extracted for the blog post "Create fake user for ASP.NET Core integration
    ///     tests. https://gunnarpeipman.com/aspnet-core-integration-test-fake-user.
    /// </remarks>
    public class FakeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary> Initializes a new instance of the <see cref="FakeAuthenticationHandler"/> class. </summary>
        /// <param name="options"> Options for controlling the operation. </param>
        /// <param name="logger"> The logger. </param>
        /// <param name="encoder"> The encoder. </param>
        /// <param name="clock"> The clock. </param>
        public FakeAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc/>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, "Test user"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, "Test");

            AuthenticateResult result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}