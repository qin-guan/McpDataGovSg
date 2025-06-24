using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using McpDataGovSg.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace McpDataGovSg.HttpServices;

public class DgsHttpService
{
    private readonly HttpClient _httpClient;
    private readonly IFusionCache _cache;

    public DgsHttpService(IHttpClientFactory factory, IFusionCache cache)
    {
        _httpClient = factory.CreateClient();
        _cache = cache;
    }

    public async Task<DatasetMetadata> GetDatasetMetadataAsync(string id, CancellationToken ct = default)
    {
        return await _cache.GetOrSetAsync(
            $"Datasets-{id}-MetaData",
            async ct2 => (await _httpClient.GetFromJsonAsync<DgsGetDatasetMetadataResponse>(
                $"https://api-production.data.gov.sg/v2/public/api/datasets/{id}/metadata",
                cancellationToken: ct2
            ))?.Data ?? throw new InvalidOperationException(),
            token: ct
        );
    }

    public async Task<IEnumerable<Dataset>> ListAllDatasetsAsync(CancellationToken ct = default)
    {
        return await _cache.GetOrSetAsync(
            "Datasets",
            async ct2 =>
            {
                var datasets = new ConcurrentBag<Dataset>();

                var first = await GetDatasetPage(ct: ct2);
                foreach (var dataDataset in first.Data.Datasets)
                {
                    datasets.Add(dataDataset);
                }

                await Parallel.ForAsync(2, first.Data.Pages, ct2, async (page, ct3) =>
                {
                    var data = await GetDatasetPage(page, ct3);

                    foreach (var d in data.Data.Datasets)
                    {
                        datasets.Add(d);
                    }
                });

                return datasets;
            },
            TimeSpan.FromDays(1),
            token: ct
        );
    }

    private async Task<DgsGetDatasetsResponse> GetDatasetPage(
        int page = 1,
        CancellationToken ct = default
    )
    {
        return await _httpClient.GetFromJsonAsync<DgsGetDatasetsResponse>(
            $"https://api-production.data.gov.sg/v2/public/api/datasets?page={page}",
            cancellationToken: ct
        ) ?? throw new InvalidOperationException();
    }
}

[JsonSerializable(typeof(DgsGetDatasetsResponse))]
internal sealed partial class DgsHttpServiceContext : JsonSerializerContext;