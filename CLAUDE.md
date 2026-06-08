# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build Enigmatry.Entry.CodeGeneration.sln

# Run CI test suite (matches pipeline filter)
dotnet test Enigmatry.Entry.CodeGeneration.sln --filter "TestCategory=unit|TestCategory=smoke"

# Run all tests including uncategorized
dotnet test Enigmatry.Entry.CodeGeneration.sln

# Run a single test project
dotnet test Enigmatry.Entry.CodeGeneration.Tests/Enigmatry.Entry.CodeGeneration.Tests.csproj --filter "TestCategory=unit"

# Run a single test by name
dotnet test --filter "FullyQualifiedName~AngularCodeGeneratorFixture"
```

`TreatWarningsAsErrors=true` is set globally — all compiler warnings are build errors and must be fixed, not suppressed.

## Architecture

This is a **dotnet global tool** (`entry-codegen`) that generates Angular UI components (list tables and forms) from C# fluent builder configurations.

**Flow:**

1. **Consumer defines configurations** — classes in a separate assembly implementing `IComponentConfiguration<ListComponentBuilder<T>>` or `IComponentConfiguration<FormComponentBuilder<T>>`.
2. **`ConfigurationScanner`** — reflects over the target assembly, discovers all `IComponentConfiguration<>` implementations, and calls `Configure(builder)` on each.
3. **Builders** — `ListComponentBuilder<T>` / `FormComponentBuilder<T>` collect configuration and `.Build()` into `ListComponentModel` / `FormComponentModel`.
4. **`CodeGenerator`** — orchestrates scanning → building → rendering.
5. **Razor templates** (`Templates/Angular/Material/`) — render component models into Angular TypeScript/HTML via `Enigmatry.Entry.TemplatingEngine.Razor`.
6. **`ITemplateWriter`** — writes to disk (`InMemoryTemplateWriter` is used in tests).

**Projects:**

| Project | Role |
|---|---|
| `Enigmatry.Entry.CodeGeneration.Configuration` | Fluent builder API (List, Form, controls, formatters) |
| `Enigmatry.Entry.CodeGeneration` | Rendering engine, Razor templates, Angular-specific generators |
| `Enigmatry.Entry.CodeGeneration.Validation` | API that maps validation rules to Angular validation |
| `Enigmatry.Entry.CodeGeneration.Tools` | CLI entry point (`System.CommandLine` + Autofac DI) |

## Key Conventions

### Implementing a configuration (consumer pattern)
```csharp
public class UserListConfiguration : IComponentConfiguration<ListComponentBuilder<UserModel>>
{
    public void Configure(ListComponentBuilder<UserModel> builder)
    {
        builder.Component().HasName("UserList").BelongsToFeature("Users");
        builder.Column(x => x.Email).WithHeaderName("Email address");
        builder.Pagination().ShowFirstLastPageButtons(false);
    }
}
```
The scanner instantiates these via reflection — no registration needed.

### Naming
Use full, descriptive names — no abbreviations in C# identifiers (variables, parameters, methods, properties). For example: `propertyName` not `propName`, `initialValue` not `initVal`, `asyncValidatorParts` not `asyncParts`.

### Package management
All NuGet versions are in `Directory.Packages.props`. Do **not** add `Version` attributes to `<PackageReference>` elements in individual `.csproj` files.

### Test snapshots
Code generation tests compare rendered Angular output against `.txt` snapshot files in `Angular/FilesToBeGenerated/`. Comparison is whitespace-agnostic (`Uglify` strips whitespace). When changing templates, update the corresponding `.txt` snapshot files.

### Versioning
Uses **MinVer** (tag-based). Packages publish to nuget.org from `master` or `release/*` branches via Azure Pipelines.
