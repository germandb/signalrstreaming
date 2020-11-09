namespace SignalRStreaming
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using IdentityModel;
    using IdentityModel.Client;

    /// <summary>
    ///     Authentication store. This class stores the access token and the application user
    ///     information related to the current action. It will be passed between microservices and
    ///     proxies as needed.
    /// </summary>
    public class AuthenticationStore
    {
        private string accessToken;

        /// <summary> Gets or sets the access token. </summary>
        /// <value> The access token. </value>
        public string AccessToken
        {
            get
            {
                return this.accessToken;
            }

            set
            {
                this.accessToken = value;

                if (!string.IsNullOrWhiteSpace(this.accessToken))
                {
                    this.UpdateAuthenticationStoreFromToken();
                }
            }
        }

        /// <summary> Gets or sets the identifier of the application user. </summary>
        /// <value> The identifier of the application user. </value>
        public Guid ApplicationUserId { get; set; }

        /// <summary> Gets or sets the application user name. </summary>
        /// <value> The name of the application user. </value>
        public string ApplicationUserName { get; set; }

        /// <summary>
        ///     Updates the authentication store from the JWT token claims. It decodes the JWT
        ///     token, and reads the claims for updating the information.
        /// </summary>
        public void UpdateAuthenticationStoreFromToken()
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (jwtSecurityTokenHandler.CanReadToken(this.AccessToken))
            {
                JwtSecurityToken token = jwtSecurityTokenHandler.ReadJwtToken(this.AccessToken);

                this.UpdateApplicationUserName(token.Claims);

                this.UpdateApplicationUserId(token.Claims);
            }
        }

        /// <summary>
        ///     Updates the authentication store from the <see cref="UserInfoResponse"/> claims.
        /// </summary>
        /// <param name="userInfoResponse"> The user information request response. </param>
        public void UpdateAuthenticationStoreFromUserInfo(UserInfoResponse userInfoResponse)
        {
            this.UpdateApplicationUserName(userInfoResponse.Claims);

            this.UpdateApplicationUserId(userInfoResponse.Claims);
        }

        /// <summary> Updates the application user identifier in the authentication store. </summary>
        /// <param name="claims"> The claims to read the data from. </param>
        private void UpdateApplicationUserId(IEnumerable<Claim> claims)
        {
            Claim applicationUserIdClaim = claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);

            if (applicationUserIdClaim == null)
            {
                return;
            }

            this.ApplicationUserId = Guid.Parse(applicationUserIdClaim.Value);
        }

        /// <summary> Updates the application user name in the authentication store. </summary>
        /// <param name="claims"> The claims to read the data from. </param>
        private void UpdateApplicationUserName(IEnumerable<Claim> claims)
        {
            Claim applicationUserNameClaim = claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);

            if (applicationUserNameClaim == null)
            {
                return;
            }

            this.ApplicationUserName = applicationUserNameClaim.Value;
        }
    }
}