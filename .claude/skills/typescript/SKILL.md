---
name: typescript
description: TypeScript 5.x / ES2022 coding conventions — type safety, async patterns, class method style, and architecture rules. Use this when writing or reviewing TypeScript files or Razor templates (.cshtml) that generate TypeScript code.
---

# TypeScript Development

> Targets TypeScript 5.x compiling to ES2022. Use native features; avoid polyfills.

## Core Intent

- Prefer readable, explicit solutions over clever shortcuts.
- Extend existing abstractions before creating new ones.
- Pure ES modules only — never emit `require`, `module.exports`, or CommonJS helpers.

## Naming & Formatting

- PascalCase for classes, interfaces, enums, and type aliases; camelCase for everything else.
- No `I` prefix on interfaces — use descriptive names.
- No abbreviations in identifiers — use full words (`propertyName` not `propName`, `initialValue` not `initVal`).
- Keep functions focused; extract helpers when branches grow.
- Favor immutable data and pure functions.

## Type System

- No `any` (implicit or explicit) — use `unknown` plus narrowing.
- When a framework API forces a type mismatch (e.g. `FormGroup.patchValue()` with a typed model), use a double cast through `unknown`: `value as unknown as TargetType`. Never use `as any`.
- Use discriminated unions for state machines and realtime events.
- Centralize shared type contracts; do not duplicate shapes.
- Express intent with utility types (`Readonly`, `Partial`, `Record`, `NonNullable`, etc.).

## Async & Error Handling

- Use `async/await`. Wrap every `await` in `try/catch` with meaningful handling — **never an empty catch**.
- Always `await` at the call site — **never discard a Promise with `void`**.
- Surface user-facing errors via the app's notification pattern.
- Guard edge cases early to avoid deep nesting.

## Class Method Style

All class methods must use the **readonly arrow-function property** syntax:

```ts
// ✅ correct
protected readonly onSubmit = (): void => { ... };
protected readonly isHidden = (name: string, visible: boolean): boolean => { ... };
private readonly buildFormGroup = (): FormGroup => { ... };

// ❌ avoid
protected onSubmit(): void { ... }
protected isHidden(name: string, visible: boolean): boolean { ... }
```

**Exception — Angular lifecycle hooks** (`ngOnInit`, `ngOnDestroy`, `ngOnChanges`, etc.) must be plain methods because the framework calls them through an interface:

```ts
// ✅ correct for lifecycle hooks
ngOnInit(): void { ... }
ngOnDestroy(): void { ... }
```

**Note**: `constructor()` is also always a plain method (ES requirement); `effect()` registrations and `inject()` calls belong there.

## Architecture

- Follow the repository's DI / composition pattern; modules are single-purpose.
- Decouple transport, domain, and presentation layers with clear interfaces.
- Supply `initialize` / `dispose` lifecycle hooks when adding services; write targeted tests alongside.

## Security

- Validate and sanitize all external input with schema validators or type guards.
- Avoid dynamic code execution (`eval`, `new Function`, untrusted template rendering).
- Encode untrusted content before rendering HTML; rely on framework escaping.
- Never hardcode secrets — load from secure environment sources.

## What NOT to do

- Do not use `any` — not even for framework API mismatches; use `as unknown as TargetType` instead.
- Do not use inline branching — `if (x) return y;` must always use braces: `if (x) { return y; }`. Follow with an empty line before the next statement (guard-clause style).
- Do not use `void` to discard a Promise.
- Do not leave empty catch blocks.
- Do not use regular method syntax for class methods (use readonly arrow properties).
- Do not use constructor injection in Angular — use `inject()`.
