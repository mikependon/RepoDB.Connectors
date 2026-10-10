---
name: repodb-connector-update-references
description: Guides bumping NuGet package references to their latest stable versions across the RepoDB.Connectors repo (all RepoDb.Connector.* projects and their UnitTests/IntegrationTests siblings, plus the shared src/Directory.Build.props), verifying the bump with a real build, recording it in each affected connector's CHANGELOG.md following that connector's own version scheme, checking the READMEs for anything version-specific that would go stale, and writing a dated summary report into the repo's Markdown/ folder. Use this whenever the user asks to update, bump, upgrade, or refresh NuGet/package references, dependencies, or third-party libraries in this repo, asks to "check for the latest packages", or asks to add a release-notes/changelog entry for a dependency bump. Covers both single-connector bumps and repo-wide sweeps across all six connectors.
---

# RepoDB.Connectors version-bump workflow

A dependency bump in this repo touches more than a version string: the same package is usually referenced
from several places (the connector's own `.csproj`, its `.UnitTests.csproj`, its `.IntegrationTests.csproj`,
and possibly the shared `src/Directory.Build.props`), the bump needs to be proven safe with a real build
before it's trustworthy, and it needs to show up in the connector's own `CHANGELOG.md` using whatever
version scheme that *specific* connector already follows — not a scheme imposed from outside. This skill
captures that end-to-end sequence so none of the steps get skipped or done out of order.

## Workflow

### 1. Inventory every reference, don't bump from memory

Search `src/**/*.csproj` and `src/Directory.Build.props` for `PackageReference` — both the usual
`Include="..."` form and the `Update="..."` form that test projects use to override a version coming from
the `MSTest.Sdk`. Also check each test project's `Sdk="MSTest.Sdk/X.Y.Z"` attribute on the `<Project>`
element — that's a NuGet package version too (the MSTest SDK itself), and it's easy to miss because it
doesn't look like a `PackageReference`.

Group what you find by package, not by project — the same package version (e.g. `Meziantou.Analyzer`,
`Microsoft.Testing.Extensions.CodeCoverage`, the `MSTest.*` family) is usually repeated identically across
every connector's test projects, so one NuGet lookup answers it for all of them at once. Packages specific
to one connector (`AWS.AdvancedDotnetDataProviderWrapper.*`, `Npgsql`, `MySql.Data`, `MySqlConnector`) only
need checking once per connector.

If the user asked for "all connectors," the affected set is every connector whose project tree contains at
least one of the packages that turned out to have a newer stable version — that commonly ends up being all
six (`AuroraDb.Npgsql`, `AuroraDb.MySqlConnector`, `CockroachDb`, `EnterpriseDb`, `MariaDb`,
`MariaDbConnector`) because of the shared test-tooling packages, even when the user was only thinking about
one connector's driver dependency.

### 2. Resolve the latest *stable* version per package — never a pre-release

Query the NuGet flat-container API directly rather than guessing from the version already in the file:

```
curl -s https://api.nuget.org/v3-flatcontainer/<package-id-lowercase>/index.json
```

This returns a `versions` array, oldest first. Filter out any entry containing a `-` (that's how NuGet
marks `-alpha`, `-beta`, `-rc`, `-preview`, etc.) and take the last remaining entry — that's the latest
stable release. Do this even for a version that looks "obviously final"; a package can have a newer-looking
pre-release sitting on top of its real latest stable release (this repo's own
`Microsoft.CodeAnalysis.NetAnalyzers` has an `11.0.100-rc.1` ahead of its `10.0.401` stable — the rc must be
skipped). If the current version already *is* the latest stable one, say so explicitly in your report
instead of silently moving on — "already latest" is a finding, not a non-event.

### 3. Apply the version edits everywhere the package appears

Update every occurrence found in step 1 for that package — the `Include=` in the connector project, every
`Update=` in its test projects, the `Sdk=` attribute if it's `MSTest.Sdk`, and `Directory.Build.props` if
the package is referenced there. A package bumped in only one of its several locations is a half-finished
bump and will mask the stale version in diagnostics later.

### 4. Build-verify before calling it done

Compiling is the cheapest real signal that a bump didn't break anything — don't skip it even for a
patch/minor bump. Each connector has its own `.slnx` (there's no repo-wide solution), covering the
connector, its `UnitTests`, and its `IntegrationTests` projects across all three target frameworks
(`net8.0`/`net9.0`/`net10.0`):

```
dotnet build <connector-dir>/<Connector>.slnx -c Release
```

Build every solution that contains a bumped package. `0 Error(s)` is the bar. Pre-existing analyzer
warnings (Meziantou `MA00xx` rules, nullable-reference warnings in older test files) are not your concern
here — compare warning *counts* before/after only if you suspect the bump introduced new ones; don't treat
inherited warnings as a problem you need to fix as part of a version bump.

**Major-version bumps need an explicit risk callout.** A patch/minor bump compiling clean is a strong signal
it's safe. A major bump (e.g. this repo's `AWS.AdvancedDotnetDataProviderWrapper.*` 2.x → 3.x) compiling
clean is weaker evidence — the public surface this repo's code happens to touch may be unchanged while other
parts of the package's contract moved. Say plainly in your report that compilation was verified but runtime
behavior was not (integration tests need a live database and aren't part of this flow by default), and
recommend running the integration suite before the bump ships.

### 5. Record it in each affected connector's CHANGELOG.md

Each connector's `CHANGELOG.md` already has an established format (`Keep a Changelog` style, newest entry
first, `#### Changed` section, bullets phrased as "Bumped [Package](nuget-link) from `old` to `new`, the
latest version."). Match that existing phrasing exactly rather than inventing new wording — read the
connector's own `CHANGELOG.md` first to confirm the header level and bullet style currently in use (don't
assume they're identical repo-wide; it's worth a quick check every time).

**Pick the next version number from that connector's own scheme — don't impose one:**

- A connector still versioned `0.0.1-alphaN` (i.e. it hasn't reached `1.0.0` yet) gets its alpha counter
  incremented: `0.0.1-alpha2`, `0.0.1-alpha3`, etc.
- A connector already on stable SemVer (`1.x.y`, no pre-release suffix) gets an ordinary patch bump
  (`1.1.1` → `1.1.2`) — it does **not** get an alpha suffix bolted back on. If a request's wording (e.g.
  "bump the next alpha version") would literally mean forcing a pre-release tag onto an already-stable
  connector, that's a real fork in the road for the project's public versioning — ask the user which they
  want rather than silently picking one; it's happened before in this repo that three of the six connectors
  had already graduated past alpha while the other three hadn't, and the two needed different treatment.

**Date is always `Date: TBA`** for a new, not-yet-released entry — this repo consistently uses `TBA` until
the connector is actually published, never today's date for an in-progress bump.

Insert the new entry **above** the current top entry (newest-first ordering) — don't append to the bottom.

### 6. Check the README for anything the bump makes stale

Grep each affected connector's `README.md` for the bumped package names and for literal version numbers.
In this repo's current READMEs, dependencies are referenced by name and NuGet link only (no pinned version
numbers or version badges), so this step is usually a no-op — but it must still be *performed and reported
as checked*, not silently assumed, because a future README could easily add a "requires NpgsqlX.Y+" note or
a version badge that this step is the only thing standing between it and going stale. If a major-version
bump changed observable behavior worth documenting (a new required connection-string property, a changed
default, etc.), that's also the place to flag it — propose README wording to the user rather than guessing
silently, since README changes are user-facing documentation.

### 7. Write the summary report

Create a dated Markdown file in the repo's `Markdown/` folder (gitignored — see
`.gitignore`'s `/Markdown/` entry), named `Markdown/CONNECTOR_UPDATE_REFERENCES_REPORT-<YYYY-MM-DD>.md`
(`<YYYY-MM-DD>` is the date the bump was performed — use the same name even on a day where a second report
would collide, but check first: if a report for today's date already exists and this is a materially
different bump, say so to the user rather than silently overwriting it). Include:

- A table of every package touched: old version → new version, and whether it was already-latest (no-op).
- The full list of files changed.
- Per-solution build results (`✅`/`❌`, error count, and warning count only if it changed).
- A "Risk notes" section for any major-version bump, per step 4.
- What was *not* changed and why (packages already at latest, README sections left alone because nothing
  was version-specific).

### 8. Don't commit unless asked

Leave the changes as working-tree edits and say so explicitly. This repo's changelog/report trail is meant
to be reviewed before it's committed — don't stage or commit on the user's behalf unless they ask for it.

## Quick reference: where things live

| What | Where |
|---|---|
| Shared analyzer package versions | `src/Directory.Build.props` |
| Connector's own driver dependency | `src/<Connector>/<Connector>/<Connector>.csproj` |
| Test-tooling package versions (MSTest family, coverage, trx) | Every `*.UnitTests.csproj` / `*.IntegrationTests.csproj`, via `PackageReference Update=` and the `Sdk="MSTest.Sdk/X.Y.Z"` attribute |
| Per-connector solution to build | `src/<Connector>/<Connector>.slnx` |
| Release notes | `src/<Connector>/CHANGELOG.md` |
| User-facing docs that might reference a dependency | `src/<Connector>/README.md` |
| Where the summary report goes | `Markdown/CONNECTOR_UPDATE_REFERENCES_REPORT-<YYYY-MM-DD>.md` (repo root, gitignored) |
