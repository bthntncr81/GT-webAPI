using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

public class CustomAuthProvider : IAuthenticationProvider
{
    private readonly IConfidentialClientApplication _clientApplication;

    // Constructor to initialize the confidential client
    public CustomAuthProvider(string clientId, string tenantId, string clientSecret)
    {
        _clientApplication = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithTenantId(tenantId)
            .WithClientSecret(clientSecret)
            .Build();
    }

    // This method authenticates HttpRequestMessage
    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    {
        // Acquire an access token for Microsoft Graph
        var authResult = await _clientApplication
            .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
            .ExecuteAsync();

        // Attach the token to the request
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
    }

    // This method authenticates RequestInformation (New requirement in latest Graph SDK)
    public async Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext, CancellationToken cancellationToken)
    {
        // Acquire an access token for Microsoft Graph
        var authResult = await _clientApplication
            .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
            .ExecuteAsync(cancellationToken);

        // Attach the token to the request information
        request.Headers.Add("Authorization", $"Bearer {authResult.AccessToken}");
    }
}