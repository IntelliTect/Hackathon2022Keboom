using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Keboom.Server.Hubs;

public class PlayerAuthenticationSchemeOptions : AuthenticationSchemeOptions { }

public class PlayerAuthenticationHandler
    : AuthenticationHandler<PlayerAuthenticationSchemeOptions>
{
    public PlayerAuthenticationHandler(
        IOptionsMonitor<PlayerAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock)
    {
    }

    const string PlayerName = "playerName";
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(PlayerName))
        {
            return AuthenticateResult.NoResult();
        }
      
            var claims = new[]
            {
                // By default SignalR will set UserIdentifier to the users ClaimType.NameIdentifier
                new Claim(ClaimTypes.NameIdentifier, Request.Headers[PlayerName]!),
            };

            // generate claimsIdentity on the name of the class
            var claimsIdentity = new ClaimsIdentity(
                claims,
                nameof(PlayerAuthenticationHandler)
            );

            // generate AuthenticationTicket from the Identity
            // and current authentication scheme
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(claimsIdentity),
                this.Scheme.Name
            );

            // pass on the ticket to the middleware
            return AuthenticateResult.Success(ticket);
       
    }
}
