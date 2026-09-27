# Project layout

Real connectors live in their own dedicated repository (`mikependon/RepoDB.Connectors`) separate from the
main `mikependon/RepoDB` repo — the connector is a general-purpose ADO.NET provider, not RepoDB-specific,
so it's versioned and released independently.

```
RepoDB.Connectors/
├── docker-compose.yml          # one service per connector's local-dev/integration-test database
├── icon.png, logo.png          # shared branding assets referenced by each connector's .csproj
├── global.json
└── src/
    └── RepoDb.Connector.<Provider>/
        ├── RepoDb.Connector.<Provider>/
        │   ├── <Provider>Connection.cs
        │   ├── <Provider>Command.cs
        │   ├── <Provider>Parameter.cs
        │   ├── <Provider>ParameterCollection.cs
        │   ├── <Provider>DataReader.cs
        │   ├── <Provider>Transaction.cs
        │   ├── <Provider>Exception.cs
        │   ├── <Provider>ConnectionStringBuilder.cs
        │   ├── <Provider>Factory.cs
        │   ├── <Provider>Type.cs
        │   ├── <Provider>TypeConverter.cs
        │   ├── Bulk/
        │   │   ├── <Provider>BulkCopy.cs
        │   │   ├── <Provider>BulkColumnMapping.cs
        │   │   └── <Provider>BulkCopyColumnMappingCollection.cs
        │   └── RepoDb.Connector.<Provider>.csproj
        ├── RepoDb.Connector.<Provider>.UnitTests/
        │   ├── <Provider>ConnectionTest.cs         (behavior assertions with no live DB - e.g. DataSource/
        │   │                                         Database parsing from a connection string)
        │   ├── <Provider>CommandTest.cs
        │   ├── <Provider>CommandConnectionTest.cs
        │   ├── <Provider>ConnectionStringBuilderTest.cs
        │   ├── <Provider>ParameterTest.cs / <Provider>ParameterCollectionTest.cs
        │   ├── <Provider>ParameterTypeTest.cs
        │   ├── <Provider>TypeConverterTest.cs
        │   ├── <Provider>ExceptionTest.cs
        │   ├── <Provider>FactoryTest.cs
        │   └── Bulk/
        │       ├── <Provider>BulkColumnMappingTest.cs
        │       ├── <Provider>BulkCopyColumnMappingCollectionTest.cs
        │       └── <Provider>BulkCopyTest.cs
        ├── RepoDb.Connector.<Provider>.IntegrationTests/
        │   ├── Models/
        │   ├── Operations/
        │   │   ├── ConnectionTest.cs
        │   │   ├── ExecuteNonQueryTest.cs / ExecuteReaderTest.cs / ExecuteScalarTest.cs
        │   │   ├── TransactionTest.cs
        │   │   ├── DataTableTest.cs
        │   │   ├── WriteToServerTest.cs
        │   │   ├── <Provider>TypeTest.cs           (round-trips every mapped type through a real column)
        │   │   └── <Provider>NativeTypeTest.cs      (round-trips via the driver's own native type, to
        │   │                                         confirm the <Provider>Type mapping matches reality)
        │   ├── Setup/Database.cs
        │   └── MSTestSettings.cs
        ├── README.md
        └── CHANGELOG.md
```

## `.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <Title>RepoDb.Connector.YourProvider</Title>
    <AssemblyName>RepoDb.Connector.YourProvider</AssemblyName>
    <TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
    <LangVersion>preview</LangVersion>
    <Nullable>annotations</Nullable>
    <Description>A connector for YourEngine used by RepoDB.</Description>
    <PackageTags>yourengine ado.net data provider database</PackageTags>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageIcon>icon.png</PackageIcon>
    <PackageProjectUrl>https://github.com/mikependon/RepoDB.Connectors</PackageProjectUrl>
    <RepositoryUrl>https://github.com/mikependon/RepoDB.Connectors</RepositoryUrl>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="<the underlying driver, and/or intermediate wrapper library>" Version="x.y.z" />
  </ItemGroup>
  <ItemGroup>
    <!-- lets unit tests construct/inspect internal members (InnerConnection, InnerParameter, ...) -->
    <InternalsVisibleTo Include="RepoDb.Connector.YourProvider.UnitTests" />
  </ItemGroup>
  <ItemGroup>
    <None Include="../README.md" Pack="true" PackagePath="\" />
    <None Include="../icon.png" Pack="true" PackagePath="\" />
  </ItemGroup>
</Project>
```

No `ProjectReference` to `RepoDb.Core` or any RepoDB package at all — the connector doesn't depend on
RepoDB; RepoDB (via `RepoDb.<Provider>`) depends on it.

## README conventions

Every connector's `README.md` follows the same shape (mirror it closely — it's also what
`repodb-document-provider`'s quickstart page and this connector's own NuGet listing draw from):

1. Header/badges, a one-line description, and a disclaimer if the connector is independent/unofficial
   with respect to whatever vendor's engine it targets.
2. "Why does this exist?" — the collision-avoidance and/or extra-capability rationale from
   `references/architecture.md`.
3. "Core ADO.NET Objects" — a table of every `<Provider>Xxx` class, its base class, and its purpose (copy
   the table shape from `references/architecture.md`'s own table).
4. An architecture diagram (plain text box-drawing is fine) showing the layering, especially for
   wrapping-shape-B connectors where there's a real intermediate library worth naming.
5. "Basic Usage" — a plain ADO.NET code sample: open a connection, run a parameterized query, read
   results.
6. One section per notable class (`<Provider>Connection`, `<Provider>Command`, `<Provider>Parameter`,
   the `<Provider>Type` table, `Transactions`, `Connection String Builder`, `Provider Factory`, `Bulk
   Operations`) with its own short usage snippet.
7. "Local Development" — how to stand up a local instance (`docker compose up -d <provider>`) and which
   environment variables the integration tests read, with fallback values for the local instance.
8. "Roadmap" — what's implemented now (numbered) vs. planned next.
9. Changelog pointer, license, contribution notes.

## `docker-compose.yml` entry

Add one service for the connector's own local dev / integration-test database, following whatever
neighboring services in the file already do (image, port mapping, environment, any extension needed —
e.g. `postgis/postgis` instead of plain `postgres` if a spatial type test needs the PostGIS extension).
