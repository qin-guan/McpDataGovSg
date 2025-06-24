using System.ComponentModel;
using McpDataGovSg.HttpServices;
using McpDataGovSg.Services;
using ModelContextProtocol.Server;
using Dataset = McpDataGovSg.Entities.Dataset;

namespace McpDataGovSg.Tools;

[McpServerToolType]
public class DatasetQueryTool(DgsSearchService searchService, DgsHttpService http)
{
    [McpServerTool, Description(
         "Searches for datasets from Data.gov.sg based on a query string. " +
         "Results can be further filtered by format, creation date, last update date, and managing agency."
     )]
    public async Task<IEnumerable<Dataset>> SearchDatasets(
        [Description(
            "The search term to find relevant datasets. Performs full text search on the dataset name and description."
        )]
        string search,
        CancellationToken ct = default
    )
    {
        return await searchService.Search(search, ct);
    }

    [McpServerTool, Description(
         "Retrieves metadata for a specific dataset. The metadata includes basic information about the dataset " +
         "like name, when it was created at, and also more complex information such as the column metadata, which " +
         "describes the columns of the dataset and its corresponding types."
     )]
    public async Task<DatasetMetadata> GetDatasetMetadata(
        [Description(
            "The search term to find relevant datasets. Performs full text search on the dataset name and description."
        )]
        string id,
        CancellationToken ct = default
    )
    {
        return await http.GetDatasetMetadataAsync(id, ct);
    }
}