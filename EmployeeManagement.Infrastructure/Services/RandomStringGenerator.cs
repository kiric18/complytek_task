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
            var prefix = "PRJ-";
            var suffix = $"-{DateTime.Now.Year}";
            try
            {
                var requestBody = new
                {
                    codesToGenerate = 1,
                    onlyUniques = true,
                    prefix = prefix,
                    suffix = suffix,
                    charactersSets = new[] { 
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"  }
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("codito.io API returned status code: {StatusCode}", response.StatusCode);
                    return GenerateFallbackString(length, prefix, suffix);
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var deserializedList = JsonSerializer.Deserialize<List<string>>(responseContent);
                if (deserializedList != null && deserializedList.Any())
                {
                    return deserializedList.FirstOrDefault()!;
                }

                _logger.LogWarning("Unexpected response format from codito.io API");
                return GenerateFallbackString(length, prefix, suffix);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Random.org API");
                return GenerateFallbackString(length, prefix, suffix);
            }
        }

        private string GenerateFallbackString(int length, string prefix, string suffix)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                    .Select(s => s[random.Next(s.Length)])
                                    .ToArray());

            return $"{prefix}{randomString}{suffix}";
        }
    }
}
