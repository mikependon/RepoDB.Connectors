<div align="center">
    <image src="logo.png" style="width:256px;" />
</div>

-----

This repository is the home of the official [RepoDB](https://github.com/mikependon/RepoDB) database connectors — dedicated, provider-specific ADO.NET implementations that RepoDB relies on for data access and bulk operations.

> **Status:** Early development. The API and implementation are subject to change.

## Why does this exist?

As [RepoDB](https://www.nuget.org/packages/RepoDb) expands its support for data movement across various database providers, dedicated ADO.NET objects are required for each provider to avoid class collisions and to expose provider-specific data types, behaviors, and capabilities where applicable.

Rather than bundling every provider into a single library, each database provider gets its own connector project, with its own NuGet package, its own release cadence, and its own documentation, while still following a shared, consistent design across the ADO.NET `System.Data.Common` abstractions.

This repository will progressively host those connectors as they are built, starting with MariaDB.

## Supported Connectors

| Connector | Database Provider | NuGet | Build Status |
| ------- | ------------------ | ----- | ------------ |
| [RepoDb.Connector.MariaDb](src/RepoDb.Connector.MariaDb) | MariaDB | [![NuGet](https://img.shields.io/nuget/v/RepoDb.Connector.MariaDb.svg)](https://www.nuget.org/packages/RepoDb.Connector.MariaDb) | [![Build](https://img.shields.io/github/actions/workflow/status/mikependon/RepoDB.Connectors/build-mariadb.yml?branch=main&label=build)](https://github.com/mikependon/RepoDB.Connectors/actions/workflows/build-mariadb.yml) |
| [RepoDb.Connector.MariaDbConnector](src/RepoDb.Connector.MariaDbConnector) | MariaDB (via MySqlConnector) | [![NuGet](https://img.shields.io/nuget/v/RepoDb.Connector.MariaDbConnector.svg)](https://www.nuget.org/packages/RepoDb.Connector.MariaDbConnector) | [![Build](https://img.shields.io/github/actions/workflow/status/mikependon/RepoDB.Connectors/build-mariadbconnector.yml?branch=main&label=build)](https://github.com/mikependon/RepoDB.Connectors/actions/workflows/build-mariadbconnector.yml) |
| [RepoDb.Connector.EnterpriseDb](src/RepoDb.Connector.EnterpriseDb) | EnterpriseDB | [![NuGet](https://img.shields.io/nuget/v/RepoDb.Connector.EnterpriseDb.svg)](https://www.nuget.org/packages/RepoDb.Connector.EnterpriseDb) | [![Build](https://img.shields.io/github/actions/workflow/status/mikependon/RepoDB.Connectors/build-enterprisedb.yml?branch=main&label=build)](https://github.com/mikependon/RepoDB.Connectors/actions/workflows/build-enterprisedb.yml) |

Each connector lives in its own directory under [`src/`](src) and ships as its own NuGet package. See the connector's own README for its goals, architecture, usage examples, and roadmap.

## Contributing

Contributions are welcome, whether that means improving an existing connector or proposing support for a new database provider. Please open an issue to discuss significant changes before submitting a pull request.

## License

[Apache License 2.0](LICENSE) — Copyright © 2026 [Michael Camara Pendon](https://x.com/mike_pendon)
