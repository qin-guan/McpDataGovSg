using System;
using MemoryPack;

namespace McpDataGovSg.Entities;

[MemoryPackable]
public partial record Dataset(
    string DatasetId,
    DateTime CreatedAt,
    string Name,
    string Status,
    string Format,
    DateTime LastUpdatedAt,
    string? ManagedByAgencyName,
    string? Description,
    DateTime? CoverageStart,
    DateTime? CoverageEnd
);
