using McpDataGovSg.Entities;

namespace McpDataGovSg.HttpServices;

public record DgsGetDatasetsResponse(
    int Code,
    string ErrorMsg,
    GetDatasetsResponseData Data 
);

public record GetDatasetsResponseData(
    IReadOnlyList<Dataset> Datasets,
    int Pages,
    int RowCount,
    int TotalRowCount
);