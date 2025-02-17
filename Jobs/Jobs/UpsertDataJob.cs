using Jobs.Models;
using Shared.Interfaces;
using Shared.Models;
using System.Text.Json;

namespace Jobs.Jobs
{
    public class UpsertDataJob(IRepository<DogFact> repository, IHttpClientFactory httpClientFactory)
    {
        private readonly IRepository<DogFact> _repository = repository;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task Execute()
        {
            try
            {
                // HTTP client with IHttpClientFactory
                var client = _httpClientFactory.CreateClient();

                // Get DogFacts from Public API (limited to 5 per call)
                var response = await client.GetAsync("https://dogapi.dog/api/v2/facts?limit=5");
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var dogFactResponse = JsonSerializer.Deserialize<DogFactResponse>(jsonString, options);

                    // Prepare and perform batch upsert
                    if (dogFactResponse?.Data != null)
                    {
                        var factsToUpsert = new List<DogFact>();
                        foreach (var factData in dogFactResponse.Data)
                        {
                            factsToUpsert.Add(new DogFact
                            {
                                Id = factData.Id,
                                Body = factData.Attributes.Body
                            });
                        }

                        await _repository.UpsertRangeAsync(factsToUpsert);
                    }
                }
                else
                {
                    Console.Write($"Error fetching DogFacts. HTTP Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error processing DogFacts upsert: {ex}");
            }
        }
    }
}
