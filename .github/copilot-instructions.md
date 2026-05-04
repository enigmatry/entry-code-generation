# Copilot Instructions

## Build & Test

```bash
# Build
dotnet build Enigmatry.Entry.CodeGeneration.sln

# Run all tests (unit + smoke)
dotnet test Enigmatry.Entry.CodeGeneration.sln --filter "TestCategory=unit|TestCategory=smoke"

# Run a single test project
dotnet test Enigmatry.Entry.CodeGeneration.Tests/Enigmatry.Entry.CodeGeneration.Tests.csproj --filter "TestCategory=unit"

# Run a single test by name
dotnet test --filter "FullyQualifiedName~AngularCodeGeneratorFixture"
```

Tests use **NUnit** with `[Category("unit")]` or `[Category("smoke")]` attributes, and **Shouldly** for assertions.

## Architecture

This is a **.NET dotnet tool** (`enigmatry.codegeneration.console`) that generates **Angular** UI components (list tables and forms) from C# configuration classes. The flow is:

1. **Consumer defines configurations** — classes implementing `IComponentConfiguration<ListComponentBuilder<T>>` or `IComponentConfiguration<FormComponentBuilder<T>>` in a separate assembly.
2. **`ConfigurationScanner`** — reflects over the target assembly at runtime to discover all `IComponentConfiguration<>` implementations and invokes `Configure(builder)` on each.
3. **Builder models** — `ListComponentBuilder<T>` / `FormComponentBuilder<T>` collect the fluent configuration and `.Build()` into `ListComponentModel` / `FormComponentModel`.
4. **`CodeGenerator`** — orchestrates scanning → building → rendering.
5. **Razor templates** (`Templates/Angular/Material/`) — render the component models into Angular TypeScript/HTML files via `Enigmatry.Entry.TemplatingEngine.Razor`.
6. **`ITemplateWriter`** — writes rendered output to disk (or `InMemoryTemplateWriter` in tests).

### Projects

| Project | Role |
|---|---|
| `Enigmatry.Entry.CodeGeneration.Configuration` | Fluent builder API (List, Form, controls, formatters) |
| `Enigmatry.Entry.CodeGeneration` | Rendering engine, Razor templates, Angular-specific generators |
| `Enigmatry.Entry.CodeGeneration.Validation` | FluentValidation-style API that maps to Angular Formly validation rules |
| `Enigmatry.Entry.CodeGeneration.Tools` | CLI entry point (`System.CommandLine`) |
| `*.Tests` / `*.Validation.Tests` / `*.Configuration.Tests` | Test projects |

## Key Conventions

### Implementing a component configuration
Consumers create a class that implements `IComponentConfiguration<TBuilder>` with a single `Configure(TBuilder builder)` method. The scanner instantiates it via reflection — no registration needed.

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

### Form controls
Use the strongly-typed `FormControl(x => x.Prop)` for inferred type, or explicit variants like `InputFormControl`, `SelectFormControl`, `DatepickerFormControl`, etc. Validation rules are wired via a separate `ValidationConfiguration<T>` class using `RuleFor(x => x.Prop)`.

### Column/control ordering
Use `.Component().OrderBy(OrderByType.Model)` to order by model property declaration order, or `OrderByType.Configuration` (default) to use the order of builder calls.

### Central package management
All NuGet versions are declared in `Directory.Packages.props`. Do **not** add `Version` attributes to individual `.csproj` `<PackageReference>` entries.

### Global build settings (`Directory.Build.props`)
- `TreatWarningsAsErrors=true` — all warnings are errors; fix them, don't suppress.
- `Nullable=enable` — nullable reference types are on everywhere.
- `ImplicitUsings=enable` — common namespaces are implicitly available.
- `LangVersion=latest`

### Test snapshots
Angular code generation tests compare output against `.txt` snapshot files in `Angular/FilesToBeGenerated/`. Comparison is whitespace-agnostic (`Uglify` strips all whitespace). When changing templates, update the corresponding `.txt` files.

### Versioning
Releases use **MinVer** (tag-based semantic versioning). NuGet packages are published to nuget.org from `master` or `release/*` branches via Azure Pipelines.
