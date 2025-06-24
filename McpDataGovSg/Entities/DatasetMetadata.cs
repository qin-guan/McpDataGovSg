using MemoryPack;

namespace McpDataGovSg.HttpServices;

[MemoryPackable]
public partial record DatasetMetadata(
    string DatasetId,
    string CreatedAt,
    string Name,
    string Format,
    string LastUpdatedAt,
    string ManagedBy,
    string CoverageStart,
    string CoverageEnd,
    ColumnMetadata ColumnMetadata
);

[MemoryPackable]
public partial record ColumnMetadata(
    string[] Order,
    Dictionary<string, string> Map,
    Dictionary<string, MetaMapping> MetaMapping
);

[MemoryPackable]
public partial record MetaMapping(
    string Name,
    string ColumnTitle,
    string DataType,
    string Index
);
