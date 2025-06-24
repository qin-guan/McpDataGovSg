namespace McpDataGovSg.HttpServices;

public record DgsGetDatasetMetadataResponse(
    int Code,
    DatasetMetadata Data,
    string ErrorMsg
);
