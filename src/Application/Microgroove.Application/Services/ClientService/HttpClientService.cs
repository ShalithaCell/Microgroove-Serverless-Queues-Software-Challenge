using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly.Retry;
using Polly;

namespace Microgroove.Application.Services.ClientService
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientService> _logger;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public HttpClientService(HttpClient httpClient, ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Define the retry policy
            _retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        // Log each retry attempt
                        _logger.LogWarning($"Retry {retryAttempt} encountered an error: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}. Waiting {timespan} before next retry.");
                    });
        }

        public virtual async  Task<string> GetInitialsAsync(string fullName)
        {
            // Example usage of the retry policy
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"https://tagdiscovery.com/api/get-initials?name={Uri.EscapeDataString(fullName)}"));

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
