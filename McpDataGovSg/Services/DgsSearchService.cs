using Lifti;
using McpDataGovSg.Entities;
using McpDataGovSg.HttpServices;

namespace McpDataGovSg.Services;

public class DgsSearchService(DgsHttpService httpService)
{
    private bool _indexInit;

    private readonly FullTextIndex<string> _index = new FullTextIndexBuilder<string>()
        .WithDefaultTokenization(o => o.CaseInsensitive())
        .WithObjectTokenization<Dataset>(options => options
            .WithKey(d => d.DatasetId)
            .WithField("Name", d => d.Name, scoreBoost: 1.5)
            .WithField("ManagedByAgencyName", d => d.ManagedByAgencyName ?? "", scoreBoost: 1.8)
            .WithField("Description", d => d.Description ?? "", o => o.WithStemming())
            .WithScoreBoosting(boost => boost
                .Freshness(d => d.LastUpdatedAt, 2D)
            )
        )
        .WithQueryParser(options => options.AssumeFuzzySearchTerms())
        .Build();

    public async Task<IEnumerable<Dataset>> Search(string searchQuery, CancellationToken ct = default)
    {
        await LoadData(ct);

        var results = _index.Search(searchQuery);
        var datasets = await httpService.ListAllDatasetsAsync(ct);

        return results.Select(r => datasets.First(d => d.DatasetId == r.Key));
    }

    private async Task LoadData(CancellationToken ct = default)
    {
        if (_indexInit) return;

        var datasets = await httpService.ListAllDatasetsAsync(ct);
        await _index.AddRangeAsync(datasets, ct);
        _indexInit = true;
    }
}