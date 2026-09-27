---
name: repodb-new-adonet-connector
description: Guides building a dedicated .NET ADO.NET data provider ("connector") for a specific database engine - <Provider>Connection, <Provider>Command, <Provider>Parameter, <Provider>ParameterCollection, <Provider>DataReader, <Provider>Transaction, <Provider>Exception, <Provider>ConnectionStringBuilder, <Provider>Factory, a <Provider>Type enum mirroring the underlying driver's native type system, and (if the engine supports native bulk-loading) a <Provider>BulkCopy implementation. Use this whenever the user wants to create a dedicated System.Data.Common-based connector/provider for RepoDB (or standalone use) that wraps an existing ADO.NET driver or another wrapper library under provider-prefixed types, mentions building a RepoDb.Connector.<Provider> package, wants to avoid class-name collisions between multiple providers that share the same underlying driver (e.g. several Postgres-wire-compatible engines all built on Npgsql), or wants to expose extra capability (failover, IAM auth, cluster awareness) through a standard ADO.NET surface. This is the foundational layer beneath a RepoDB provider - use the repodb-new-db-provider skill afterward (or the repodb-new-db-provider-bulk-operations skill for bulk support) to build the actual RepoDB integration on top of the connector this skill produces.
---

# Building a dedicated ADO.NET connector

A connector (`RepoDb.Connector.<Provider>`) is a plain `System.Data.Common` data provider — not RepoDB-
specific — that wraps an existing driver (or another wrapper library) under a dedicated, provider-prefixed
set of types (`<Provider>Connection`, `<Provider>Command`, ...). It exists for one of two reasons: several
RepoDB providers might share the same underlying driver and need distinct types so RepoDB's mapper classes
can tell them apart, or the underlying driver needs an intermediate wrapper (for failover, IAM auth,
cluster topology awareness, etc.) that's worth surfacing through a clean ADO.NET surface. Confirm one of
these actually applies before building one — see `references/architecture.md`'s "Why build one" section.

Almost everything in a connector is one-line delegation to whatever's underneath — the actual engineering
effort concentrates in a handful of specific decisions (which exception types to normalize vs. pass
through, how to design the `<Provider>Type` enum, and — if the engine has a native bulk-load protocol —
implementing `<Provider>BulkCopy`, which is genuine protocol work rather than delegation). Getting the
delegation boilerplate right is mostly mechanical once you've seen one real example; getting those specific
decisions right is where the actual design judgment goes.

## Workflow

### 1. Confirm the shape and decide what to wrap

`references/architecture.md` covers the two wrapping shapes — direct driver wrap (the common case) vs.
wrapping an intermediate capability-adding library — and when a dedicated connector is warranted at all.
`references/checklist.md`'s first section has the concrete questions to answer up front: which shape,
whether a public "unwrap to the raw driver" escape hatch is needed (often is, for a bulk-load protocol the
intermediate wrapper doesn't expose), and how exception normalization should work.

### 2. Build the standard object set

`references/architecture.md` lists the full `System.Data.Common` class set every connector implements and
what each is responsible for. `references/worked-example.md` walks through a complete, real, recently-built
connector (`RepoDb.Connector.AuroraDb.Npgsql`, wrapping the AWS Advanced .NET Data Provider Wrapper over
Npgsql — shape B) class by class with real code, including the exception-wrapping pattern in
`<Provider>Command`, the completed-transaction guard in `<Provider>Transaction`, and the
`<Provider>Type`/`<Provider>TypeConverter` mapping pattern. `references/checklist.md`'s implementation
order starts with `<Provider>Connection` and builds outward — get a working connection and a parameterized
query round-trip working before anything else.

### 3. Design the type system

Enumerate the driver's own native-type enum, map every genuinely scalar value to a `<Provider>Type` member,
and deliberately exclude composite/structural values rather than forcing a lossy equivalent — converting
one should throw `NotSupportedException`, not silently guess. `references/checklist.md` has the specific
questions; `references/worked-example.md` has a complete real converter to pattern-match against.

### 4. Implement bulk copy, if the engine supports native bulk-loading

This is the one part of the connector that's genuine implementation rather than delegation — building
`<Provider>BulkCopy` on top of whatever lower-level primitive the driver exposes (a binary import protocol,
a purpose-built bulk-copy class, or nothing at all). `references/worked-example.md` has a complete real
implementation built on Npgsql's binary `COPY` protocol, including column-mapping resolution and the
"unwrap to the raw driver for this one protocol-specific operation" pattern. `references/checklist.md`
covers the design questions (source-shape coverage, null representation, connection-ownership) regardless
of which primitive you're building on. This `<Provider>BulkCopy` class is what a sibling
`RepoDb.<Provider>.BulkOperations` package (the `repodb-new-db-provider-bulk-operations` skill) will later
orchestrate — build it to be usable standalone, independent of RepoDB.

### 5. Scaffold the project and test

`references/project-layout.md` covers the real multi-connector repo structure, `.csproj` shape, and README
conventions. `references/checklist.md`'s test section covers the unit/integration split every connector
uses — unit tests for pure delegation behavior with no live database, integration tests including a
**dedicated round-trip test per mapped type value**, which is the single most effective test for catching a
wrong entry in the type converter.

## Reference files

- `references/architecture.md` — why a dedicated connector exists, the standard object set and their
  responsibilities, the two wrapping shapes, and the bulk-copy exception to the delegation pattern.
- `references/worked-example.md` — a complete, real, recently-built connector walked through class by
  class with real code.
- `references/project-layout.md` — the multi-connector repo structure, `.csproj` shape, and README
  conventions.
- `references/checklist.md` — the design questions to answer before coding, implementation order, and the
  unit/integration test structure.

## Companion skills

This connector is the foundation for a RepoDB provider — see, in this same skills repo:
`repodb-new-db-provider` (build the RepoDB provider on top of this connector),
`repodb-new-db-provider-bulk-operations` (add bulk operations, orchestrating this connector's
`<Provider>BulkCopy`), and `repodb-document-provider`/`repodb-document-bulk-operations` (document either
afterward).
