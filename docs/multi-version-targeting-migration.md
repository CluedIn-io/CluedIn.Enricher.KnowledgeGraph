# Migrating a Connector/Enricher to Multi-Version Targeting

This document tracks the migration of `CluedIn.Enricher.KnowledgeGraph` from a single-version build
to the multi-version targeting pattern. Modeled on the equivalent docs already merged for
`CluedIn.Connector.Dataverse.V2`, `CluedIn.Enricher.GoogleMaps`, `CluedIn.Enricher.Gleif`,
`CluedIn.Enricher.OpenCorporates`, `CluedIn.Enricher.Permid`, and `CluedIn.Enricher.Brreg`.

Branch: `feature/multi-version-targeting` (off `develop`).

---

## Overview

| CluedIn version | .NET TFM | Package suffix |
|---|---|---|
| 4.7.0 | net6.0 | `.470` |
| 4.8.0 | net6.0 | `.480` |
| 5.0.0-beta.* | net10.0 | `.500` |

Verified directly for this repo's own `NuGet.config` feeds (not assumed from a sibling repo):
`5.0.0-*` resolves to `5.0.0-beta.576`.

4.6.0 excluded — no known 4.6-only dependency in this repo's small `ExternalSearchProvider` API
surface (two projects: `ExternalSearch.Providers.KnowledgeGraph` and
`ExternalSearch.Providers.KnowledgeGraph.Provider`).

No unit test project exists (`test/unit` is scaffold-only, no `.csproj`). One integration test
project exists (`ExternalSearch.KnowledgeGraph.Integration.Tests`), but its only test class is
entirely commented out — no live test code, no `AutoFixture` usage, so no `GlobalUsings.cs` was
needed for the xunit v2/v3 namespace split (unlike GoogleMaps/Gleif/OpenCorporates/Brreg).

---

## Step 1 — Pipeline template (`azure-pipelines.yml`)

Status: **Done**

Replaced the single-version `crawler.build.yml` steps-template with the multi-version
`crawler.build.jobs.yml` jobs-template; dropped the top-level `pool: vmImage: 'windows-latest'` and
the manual `NuGetAuthenticate@0` step (each job in the jobs-template does its own). Kept the
`variables: - group: nuget` variable group reference as-is.

```yaml
jobs:
  - template: crawler.build.jobs.yml@templates
    parameters:
      pool:
        vmImage: 'ubuntu-22.04'
      useGitVersionDotNetTool: true
      multiVersionCluedInTargets:
        - cluedInVersion: '4.7.0'
        - cluedInVersion: '4.8.0'
        - cluedInVersion: '5.0.0-beta.*'
```

`useGitVersionDotNetTool: true` was included from the start this time — Permid's fork found that
omitting it silently falls back to the retired legacy `GitVersionTask@5` marketplace task, which
fails outright on the hosted agent.

---

## Step 2 — `Directory.Build.props`

Status: **Done**

Honours `CluedInMultiVersionTargetFramework`, net10.0 local fallback; derives
`CLUEDIN_V47`/`V48`/`V50` `DefineConstants`; pinned `LangVersion` to `13.0` up front (several sibling
repos hit `CS8936` on net6.0 without this — didn't actually need it here since this repo has no raw
string literals, but cheap to pin defensively and keeps this file consistent with the others).

---

## Step 3 — `Packages.props`

Status: **Done**

Guarded `_CluedIn`. Split the test-package versions that were previously hardcoded to the
xunit.v3/`Microsoft.NET.Test.Sdk` 18.x/`AutoFixture.Xunit3` generation into `CLUEDIN_V50`-conditional
`ItemGroup`s, adding the xunit v2/`Microsoft.NET.Test.Sdk` 17.12.0/`AutoFixture.Xunit2` 4.18.0
fallback for net6.0.

---

## Step 4 — Test project (`test/Directory.Build.props`)

Status: **Done**

Same split applied to the previously-unconditional `PackageReference`s in
`test/Directory.Build.props` (the only place they were declared — no per-project `PackageReference`s
needed changing). No `GlobalUsings.cs` needed (see Overview — the one integration test class is
fully commented out, no `AutoFixture`/`ITestOutputHelper` usage to guard).

---

## Step 5 — `NuGet.config` casing

Status: **Done**

`git mv`'d `Nuget.config` → `NuGet.config` (two-step rename). Feeds (`nuget.org`, `develop`,
`release`, `AzurePipelines`) already sufficient — confirmed `CluedIn.Core` restores at `4.7.0`,
`4.8.0`, and `5.0.0-*` without any feed changes.

---

## Step 6 — API compatibility audit across 4.7.0 / 4.8.0 / 5.0.0-beta.*

Status: **Done**

Built both `src/` projects and the integration test project for real
(`dotnet build -p:_CluedIn=<v> -p:CluedInMultiVersionTargetFramework=<tfm>`) against all three
targets — 0 errors everywhere after one fix.

**Finding: RestSharp 106-vs-114 break, one call site.**
`KnowledgeGraphExternalSearchProvider.cs` uses `RestSharp` directly (`RestClient`, `RestRequest`,
`client.ExecuteAsync<T>()`). The two `Query`/`ExternalSearchQuery` call sites use a `var`-inferred
response local, so they compile fine either way (matches the GoogleMaps doc's finding — only
explicitly-typed locals break). The one place that broke: `VerifyConnection` passes its response
into `private ConnectionVerificationResult ConstructVerifyConnectionResponse(RestResponse response)`
— on 4.7.0/4.8.0, `ExecuteAsync<T>()` returns `IRestResponse<T>`, not assignable to the
non-generic `RestResponse` (114+) parameter type. Fixed with a guarded method signature (both
versions of the method only use `.StatusCode`/`.StatusDescription`/`.ErrorException`, present on
both types):

```csharp
#if CLUEDIN_V50
private ConnectionVerificationResult ConstructVerifyConnectionResponse(RestResponse response)
#else
private ConnectionVerificationResult ConstructVerifyConnectionResponse(IRestResponse response)
#endif
```

No other API breaks found — `CluedIn.Core`/`CluedIn.ExternalSearch` themselves have no version-gated
call in this repo's small surface.

---

## Step 7 — Reset the semantic version (`GitVersion.yml`)

Status: **Done**

```yaml
next-version: 1.0
ignore:
  commits-before: 2026-06-20T00:00:00
```

Highest pre-existing tag: `4.6.2` at `2026-06-17T17:25:56+10:00`. Padded to
`2026-06-20T00:00:00` — well over 2 days past — per Gleif's finding that `GitVersion.Tool 5.9.0`
appears to parse `commits-before` using local machine time, not UTC, and a same-day/1-day margin can
silently fail to exclude the old tag (no error — it just keeps incrementing off `4.x`).

**Verified with the actual pinned tool** (`GitVersion.Tool 5.9.0`, installed to a scratch tool-path,
not the globally-installed version): `MajorMinorPatch: "1.0.0"`, confirmed before opening the PR.

---

## Step 8 — Push and confirm CI

Status: **Done**

**CI fully green on the first push** (PR #47, build 151973): all three `Multi-version build+test`
legs (4.7.0, 4.8.0, 5.0.0-beta.*) plus `Multi-version: publish` passed. No integration-test legs run
(no active test project).

---

## Checklist

- [x] `azure-pipelines.yml` — switched to `crawler.build.jobs.yml` with `multiVersionCluedInTargets` (4.7.0, 4.8.0, 5.0.0-beta.*); `useGitVersionDotNetTool: true` included from the start
- [x] `Directory.Build.props` — honours `CluedInMultiVersionTargetFramework` with net10.0 local fallback; `DefineConstants` derived; `LangVersion` pinned to 13.0
- [x] `Packages.props` — `_CluedIn` guarded; test package versions split by `CLUEDIN_V50`
- [x] `test/Directory.Build.props` — test package references split by `CLUEDIN_V50`; no `GlobalUsings.cs` needed (no live AutoFixture usage)
- [x] `NuGet.config` — renamed from `Nuget.config`; feeds confirmed sufficient as-is
- [x] Source — one RestSharp `IRestResponse`/`RestResponse` guard in `KnowledgeGraphExternalSearchProvider.cs`; both src projects + the integration test project build clean (0 errors) on all three legs
- [x] `GitVersion.yml` — `next-version: 1.0`; `ignore.commits-before: 2026-06-20T00:00:00`; verified `MajorMinorPatch: 1.0.0` with the pinned GitVersion.Tool 5.9.0
- [x] Pushed branch and confirmed the Azure DevOps pipeline is green end-to-end — PR #47, build 151973: all three legs + `Multi-version: publish` passed on the first run
