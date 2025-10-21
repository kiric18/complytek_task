using EmployeeManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Services
{
    public class RandomStringGenerator : IRandomStringGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RandomStringGenerator> _logger;

        public RandomStringGenerator(HttpClient httpClient, ILogger<RandomStringGenerator> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GenerateRandomStringAsync(int length = 8)
        {
            try
            {
                var requestBody = new
                {
                    codesToGenerate = 1,
                    onlyUniques = true,
                    prefix = "PRJ-",
                    suffix = $"-{DateTime.Now.Year}",
                    charactersSets = new[] { "\\w", "\\w", "\\w", "\\w", "\\w", "\\w"}
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Random.org API returned status code: {StatusCode}", response.StatusCode);
                    return GenerateFallbackString(length);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseContent);

                if (jsonDoc.RootElement.TryGetProperty("result", out var result) &&
                    result.TryGetProperty("random", out var random) &&
                    random.TryGetProperty("data", out var data) &&
                    data.GetArrayLength() > 0)
                {
                    return data[0].GetString() ?? GenerateFallbackString(length);
                }

                _logger.LogWarning("Unexpected response format from Random.org API");
                return GenerateFallbackString(length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Random.org API");
                return GenerateFallbackString(length);
            }
        }

        private string GenerateFallbackString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
