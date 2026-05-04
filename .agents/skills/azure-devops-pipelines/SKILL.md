---
name: azure-devops-pipelines
description: Best practices for Azure DevOps Pipeline YAML files in the Enigmatry Entry CodeGeneration project. Use this when creating, editing, or reviewing Azure DevOps CI/CD pipeline YAML.
---

# Azure DevOps Pipelines

## Pipeline files

Pipeline YAML files live at the **repository root**:

| File | Purpose |
|------|---------|
| `azure-pipelines.yml` | Main CI/CD pipeline — build, test, pack, publish to NuGet |
| `code-analysis.yml` | Static code analysis |

Shared pipeline templates are consumed from the `enigmatry-azure-pipelines-templates` repository via a `resources.repositories` reference — do not inline template logic that already exists there.

## Key variables

```yaml
variables:
  artifactName: 'enigmatry-entry-code-generation'
  buildConfiguration: 'Release'
  projectNamePrefix: Enigmatry.Entry.CodeGeneration
  dotnetVersion: '10.0.x'
  NUGET_PACKAGES: $(Pipeline.Workspace)/.nuget/packages
```

## Triggers

Use `batch: true` to avoid redundant builds for rapid pushes. For this repo, builds run on all branches by default:

```yaml
trigger:
  batch: true
  branches:
    include:
      - '*'
```

For PR validation triggers:
```yaml
pr:
  branches:
    include:
      - master
```

## Build stage

The build stage sets the .NET SDK version, retrieves the MinVer version, restores, builds, runs categorized tests, packs, and publishes artifacts.

Tests run with category filters — ensure all test classes have a `[Category]` attribute:
```yaml
- task: DotNetCoreCLI@2
  displayName: Run Unit Tests
  inputs:
    command: 'test'
    projects: $(projectNamePrefix)**/*.Tests.csproj
    arguments: '--filter TestCategory=unit|TestCategory=smoke --configuration $(buildConfiguration)'
```

NuGet packages are cached between runs:
```yaml
- task: Cache@2
  displayName: 'NuGet Cache'
  inputs:
    key: 'nuget | "$(Agent.OS)" | **/packages.lock.json,!**/bin/**,!**/obj/**'
    restoreKeys: |
      nuget | "$(Agent.OS)"
      nuget
    path: $(NUGET_PACKAGES)
    cacheHitVar: 'NUGET_CACHE_RESTORED'
```

## Publish stage

Packages are published to NuGet.org from `master` or `release/*` branches only:

```yaml
- stage: publish_nuget
  dependsOn: ci_build
  condition: and(succeeded(), or(eq(variables['Build.SourceBranch'], 'refs/heads/master'), startsWith(variables['Build.SourceBranch'], 'refs/heads/release/')))
  jobs:
  - deployment:
    environment: nuget
    strategy:
      runOnce:
        deploy:
          steps:
          - task: NuGetCommand@2
            inputs:
              command: 'push'
              packagesToPush: '$(System.ArtifactsDirectory)/**/*.nupkg;!$(System.ArtifactsDirectory)/**/*.symbols.nupkg'
              nuGetFeedType: 'external'
              publishFeedCredentials: 'nuget_org'
```

## Rules

- Never hardcode secrets or connection strings in YAML — use variable groups or Azure Key Vault references.
- Run tests with category filters (`TestCategory=unit|TestCategory=smoke`) — ensure all test classes have a `[Category]` attribute.
- Versioning is managed by MinVer (tag-based) — do not manually set version numbers in pipeline YAML.
- When adding new test projects, ensure they match the `$(projectNamePrefix)**/*.Tests.csproj` glob or update the test task accordingly.
- Use `batch: true` on triggers to avoid redundant builds for rapid pushes.

