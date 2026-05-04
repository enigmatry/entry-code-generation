---
name: csharp-unit-tests
description: Best practices for C# unit and integration testing with NUnit and Shouldly. Use this when writing or reviewing C# tests.
---

# C# Unit Testing

## Stack

- **Test runner**: NUnit 4 (`[Test]` / `[TestCase]` / `[TestCaseSource]`)
- **Assertions**: Shouldly — always prefer over `Assert.*`
- **Categorization**: `[Category("unit")]` or `[Category("smoke")]` — required on every test class to be included in CI

## File and class conventions

- Name test files and classes with the `Fixture` suffix: `Section.cs` → `SectionFixture.cs`
- **Classes named with the `Fixture` suffix do not need `[TestFixture]`** — NUnit discovers them automatically via their `[Test]` methods
- Mirror the production folder structure under the test project root
- Test classes are `internal sealed`

## Naming — PascalCase, no underscores

Test method names use **descriptive PascalCase** — no underscores, no prefixes like `Constructor_` or `ToString_`:

```csharp
// ✅ correct
WhenEndExceedsMaxLengthThrows
ToStringReturnsEndValue
MissingSectionsLineThrows
WithValidRangeCreatesSection

// ❌ avoid
Constructor_WhenEndExceedsMaxLength_ThrowsArgumentOutOfRangeException
Parse_MissingSectionsLine_ThrowsFormatException
```

## Test structure — AAA without comments

Separate Arrange / Act / Assert with a **blank line only** — never write `// Arrange`, `// Act`, or `// Assert` comments:

```csharp
[Test]
public void WhenStartEqualsEndThrows()
{
    var act = () => new Section(1000, 1000);

    Should.Throw<ArgumentException>(() => act());
}
```

```csharp
[Test]
public void ToStringReturnsEndValue()
{
    var section = new Section(0, 2500);

    var result = section.ToString();

    result.ShouldBe("2500");
}
```

## Parameterized tests — [TestCase] and [TestCaseSource]

**Always scan for duplication before writing a new `[Test]` method.** If two or more tests share identical body structure and differ only in one or more literal values (strings, numbers, enum values), they MUST be collapsed into a single `[TestCase]`-parameterized method.

```csharp
// ❌ avoid — identical structure, only the expected string differs
[Test] public void ActiveStatusHasCorrectName()   { ... status.Name.ShouldBe("Active"); }
[Test] public void InactiveStatusHasCorrectName() { ... status.Name.ShouldBe("Inactive"); }

// ✅ correct — collapsed into one parameterized test
[TestCase(UserStatusId.Active,   "Active")]
[TestCase(UserStatusId.Inactive, "Inactive")]
public void StatusHasCorrectName(UserStatusId id, string expected)
{
    var status = UserStatus.FromValue(id.Value);

    status.Name.ShouldBe(expected);
}
```

Use `[TestCase]` whenever the same assertion logic applies to multiple input values:
```csharp
[TestCase("")]
[TestCase("   ")]
[TestCase(null)]
public void CreateUserWithEmptyNameThrows(string? name)
{
    var act = () => new UserBuilder().WithFullName(name!).Build();

    Should.Throw<ArgumentException>(() => act());
}
```

Use `[TestCaseSource]` when test data is more complex or reused across multiple tests:
```csharp
private static readonly object[][] InvalidEmails =
[
    ["not-an-email"],
    ["missing@domain"],
    ["@nodomain.com"],
];

[TestCaseSource(nameof(InvalidEmails))]
public void CreateUserWithInvalidEmailThrows(string email)
{
    var act = () => new UserBuilder().WithEmailAddress(email).Build();

    Should.Throw<ArgumentException>(() => act());
}
```

## Exception assertions

Always use `Should.Throw<T>()` with a lambda — never `Assert.Throws`:
```csharp
Should.Throw<ArgumentException>(() => new Section(500, 100));
```

When validating the exception message or properties, assert on the caught exception:
```csharp
var ex = Should.Throw<ArgumentException>(() => new Section(500, 100));
ex.Message.ShouldContain("start");
```

## Snapshot tests (Angular code generation)

This repository tests Angular code generation by comparing rendered output against reference `.txt` files in `Angular/FilesToBeGenerated/`. Comparison is whitespace-agnostic — all whitespace is stripped before comparing.

When changing Razor templates, update the corresponding `.txt` snapshot files. To verify output, run the test and inspect the assertion failure message which shows actual vs. expected content.

## What NOT to do

- Do not use underscores in test method names.
- Do not add `[TestFixture]` to classes whose names end with `Fixture`.
- Do not write `// Arrange`, `// Act`, `// Assert` comments.
- Do not use `Assert.That` — use Shouldly only.
- Do not leave empty catch blocks.
- Do not write separate `[Test]` methods for cases that differ only in input values — use `[TestCase]` or `[TestCaseSource]` instead. **Always check for this before writing any new `[Test]` method.**
- Do not test implementation details — assert on observable behavior and public API output, not on internal state or private method calls.
