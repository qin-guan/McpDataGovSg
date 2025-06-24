# MCP for data.gov.sg

Basic tool connector to allow querying the data.gov.sg API.

## Features

- Full text search across datasets
- Fetch dataset metadata
- (soon) Fetch / search dataset data

## Get started

To test it out, add the following to your project in VScode.

`.vscode/mcp.json`
```json
{
  "inputs": [],
  "servers": {
    "ProdMcpDataGovSg": {
      "command": "docker",
      "args": [
        "run",
        "-i",
        "--rm",
        "ghcr.io/qin-guan/mcpdatagovsg"
      ],
      "env": {}
    },
    "DevMcpDataGovSg": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "McpDataGovSg/McpDataGovSg.csproj",
      ]
    }
  }
}
```

Then, start GitHub Copilot in Agent mode, and you can ask it information about any dataset on data.gov.sg.

## Development

```
dotnet run --project McpDataGovSg/McpDataGovSg.csproj
```

Then, connect to the DevMcpDataGovSg option in VScode.
