# Migrating from Formly-based code generation to signals-based code generation

This guide describes how to move an application from the **deprecated Formly output** (Angular v17+, `@ngx-formly`-based form components wrapped in NgModules) to the **signals output** (Angular v22+, standalone components built on plain Angular Material and reactive forms, with signal-based inputs/outputs).

The Formly templates remain in the tool but are **frozen and deprecated**: they receive no new features and will be removed in a future major version.

- [What changes at a glance](#what-changes-at-a-glance)
- [Prerequisites](#prerequisites)
- [Enabling signals generation](#enabling-signals-generation)
- [Gradual migration and its limits](#gradual-migration-and-its-limits)
- [Configuration API changes (C#)](#configuration-api-changes-c)
- [Generated component contract changes (TypeScript)](#generated-component-contract-changes-typescript)
- [Validation](#validation)
- [Readonly behavior](#readonly-behavior)
- [Formatting](#formatting)
- [i18n](#i18n)
- [Styling hooks](#styling-hooks)
- [List (table) components](#list-table-components)
- [Known limitations of the signals templates](#known-limitations-of-the-signals-templates)
- [Step-by-step checklist](#step-by-step-checklist)
- [Troubleshooting](#troubleshooting)

## What changes at a glance

| | Formly output (deprecated) | Signals output |
|---|---|---|
| Angular version | v17+ | v22+ |
| Form engine | `@ngx-formly/core` + `FormlyFieldConfig[]` built at runtime | Plain reactive forms: a typed `FormGroup` and Material markup generated at build time |
| Component style | `standalone: false` classes declared in a generated `*-generated.module.ts` | Standalone components, `ChangeDetectionStrategy.OnPush`; **no module files are generated** |
| Inputs/outputs | `@Input()` / `@Output()` | `input()` / `model()` / `output()` signals |
| Runtime dependencies | `@ngx-formly/*`, `@enigmatry/entry-form` (field types, wrappers, `ENTRY_FIELD_TYPE_RESOLVER`) | Angular Material, `@enigmatry/entry-form` (expression dictionary types, `sortOptions`, `SelectConfiguration`, `ENTRY_ASYNC_VALIDATOR_RESOLVER`, `EntryFieldFormatDirective`) |
| Validation messages | Formly global message registry, resolved at runtime | Generated `<mat-error>` blocks with messages resolved at generation time |
| Custom field types | Registered in the app's Formly type registry by name | Rendered as a custom element; the providing component is imported via `.WithImport(...)` |

## Prerequisites

1. **Angular v22 or later** in the consuming application. The generated code uses the built-in control flow (`@if`/`@for`), signal inputs (`input()`, `model()`, `output()`), `toSignal`/`toObservable`, and standalone component imports.
2. **Angular Material** must be installed; the generated components import `MatFormFieldModule`, `MatInputModule`, `MatSelectModule`, etc. as needed. If any form uses a `DateTimePickerFormControl`, `@mat-datetimepicker/core` is required as well.
3. **`@enigmatry/entry-form`** at a version that exports:
   - `IFieldExpressionDictionary<T>`, `IFieldPropertyExpressionDictionary<T>` (also used by the Formly output),
   - `SelectConfiguration`, `sortOptions`,
   - `ENTRY_ASYNC_VALIDATOR_RESOLVER` (injection token; only needed when controls use `.WithValidators(...)`),
   - `EntryFieldFormatDirective` (only needed when controls use `.WithFormat(...)`).
4. The codegen tool version that ships the signals templates (the version this document ships with, or newer).
5. `@ngx-formly/*` packages can be removed from the application **after** the last Formly-generated component is gone. Do not remove them while any component still generates through the deprecated templates.

## Enabling signals generation

Signals generation is opt-in via the CLI flag:

```bash
entry-codegen -sa <your-assembly>.dll -dd <destination> --signals
# short form: -s
```

The `--standalone-components` / `-stc` flag only affects the deprecated Formly module output and is ignored by the signals templates (signals components are always standalone).

### Per-component override

Every component configuration can override the global flag:

```csharp
public void Configure(FormComponentBuilder<UserModel> builder)
{
    builder.WithSignals();        // generate this component with the signals templates
    // builder.WithSignals(false) // ...or pin it to the deprecated Formly templates
}
```

The same method exists on `ListComponentBuilder<T>`. When set, the per-component value takes precedence over the CLI flag.

## Gradual migration and its limits

The per-component override enables migrating one form at a time, but there is one structural caveat: **feature modules**.

While the global `--signals` flag is off, the tool still generates a `<feature>-generated.module.ts` per feature, and that module lists *all* of the feature's generated components in `declarations`. A standalone (signals) component cannot be declared in an NgModule — Angular rejects it at compile time.

Practical consequences:

- Migrate **feature by feature**, not control by control. When you flip the components of a feature to signals, stop importing that feature's `*-generated.module.ts` and import the generated standalone components directly where they are used.
- Once every feature is migrated, run the tool with `--signals` globally, remove the per-component `WithSignals()` calls, and delete all `*-generated.module.ts` files (the signals run no longer generates or overwrites them, so stale copies linger until deleted).

## Configuration API changes (C#)

### Renames (compat shims in place)

The Formly-specific names were renamed. The old names still compile so existing configurations keep working, but new code should use the new names:

| Old | New | Old name status |
|---|---|---|
| `FormlyTypes` (class) | `ControlTypes` | `[Obsolete]` alias |
| `FormControl.FormlyType` | `FormControl.ControlType` | plain alias (kept non-obsolete because the deprecated Razor templates reference it) |
| `IFormlyValidationRule.FormlyRuleName` | `RuleName` | `[Obsolete]` alias |
| `IFormlyValidationRule.FormlyValidationMessage` | `ValidationMessage` | `[Obsolete]` alias |
| `IFormlyValidationRule.FormlyTemplateOptions` | `TemplateOptions` | `[Obsolete]` alias |

The interface names themselves (`IFormlyValidationRule`, etc.) are unchanged to preserve binary compatibility of the published `Enigmatry.Entry.CodeGeneration.Validation` package.

### New required configuration: `WithImport`

Any control that renders a custom element — `CustomFormControl` and `RichTextInputFormControl` — must declare which Angular component provides that element:

```csharp
builder.CustomFormControl(x => x.FileUpload)
    .WithCustomControlType("entry-file-input")
    .WithImport("EntryFileInputComponent", "@enigmatry/entry-file-input");

builder.RichTextInputFormControl(x => x.Description)
    .WithEditor(RichTextEditor.Redactor)
    .WithImport("EntryRedactorComponent", "@enigmatry/entry-redactor");
```

The symbol is added to the generated standalone component's `imports` array and an `import { ... } from '<package>'` statement is emitted. **Generation fails with a descriptive error** if a custom/rich-text control has no import configured — with Formly, the type registry resolved this at runtime; with standalone components it must be known at generation time.

The custom component itself must:
- implement `ControlValueAccessor` (it is bound with `formControlName`),
- expose a `readonly` input (bound to the generated `isDisabled(...)` helper).

### Wrappers are gone

`WithCustomWrapper(...)` / `WithCustomWrappers(...)` are Formly-only and ignored by the signals templates. Migrate as follows:

| Formly wrapper | Signals replacement |
|---|---|
| `form-field` | Automatic — controls render inside `<mat-form-field>` where applicable |
| `tooltip` | `.WithTooltipText(...)` → `matTooltip` on the field |
| custom composition wrappers | A `CustomFormControl` with `.WithImport(...)` that owns its own markup |

### `ENTRY_FIELD_TYPE_RESOLVER` replaced by `WithReadonlyDisplay`

The Formly-era `ENTRY_FIELD_TYPE_RESOLVER` token (runtime swapping of field types when the form is readonly) has no signals equivalent. Instead, opt in per form:

```csharp
builder.WithReadonlyDisplay();
```

See [Readonly behavior](#readonly-behavior).

### New array options

Array controls generate real add/remove buttons; their labels (and translation ids) are configurable:

```csharp
builder.ArrayFormControl(x => x.Addresses)
    .WithAddButtonLabel("Add address")            // optional translation id as 2nd argument
    .WithRemoveButtonLabel("Remove address")
    .WithItemConfiguration(item => { /* ... */ });
```

Defaults are "Add" / "Remove" with ids `<feature>.<component>.<array>.add-item` / `.remove-item`.

## Generated component contract changes (TypeScript)

The selector, file names, and output locations are unchanged. What changes is how the parent interacts with the component.

### Registration

Formly components were declared in the generated feature module. Signals components are standalone — import them directly:

```typescript
@Component({
  imports: [UserEditGeneratedComponent],
  /* ... */
})
export class UserEditPageComponent { }
```

### Model binding

`model` is now a `model()` signal input and supports two-way binding. On submit the component merges the current form value into the model, updates it, and emits `save` — exactly like the Formly version (including the 500 ms submit throttle).

```html
<app-g-user-edit [(model)]="user" (save)="onSave($event)" (cancel)="onCancel()" />
```

One-way `[model]="user"` still works; `save` carries the merged model either way.

### Inputs that kept their names and semantics

`isReadonly`, `saveButtonText`, `cancelButtonText`, `saveButtonDisabled`, `formButtonsTemplate` (an `ng-template` replacing the default action buttons), and the expression dictionaries:

```html
<app-g-user-edit
    [fieldsHideExpressions]="{ lastName: (model) => !model.firstName }"
    [fieldsLabelExpressions]="{ firstName: (model) => model.isCompany ? 'Company name' : 'First name' }"
    ...
```

They are signal inputs now, but the binding syntax from the parent is identical. Note the [array-item limitations](#known-limitations-of-the-signals-templates) below.

### Select options — breaking changes

| Scenario | Formly output | Signals output |
|---|---|---|
| Fixed values (`WithFixedValues`) | Overridable `@Input() <prop>Options: any[]` | **No input.** Options are baked into the component. To supply options at runtime, configure the control with dynamic values instead. |
| Dynamic values (`WithDynamicValues`) | `@Input() <prop>Options: any[]` (array, pushed through an internal `BehaviorSubject`) | `[<prop>Callback]` — an **`Observable<unknown[]>`** input:<br>`<app-g-user-edit [countryCallback]="countries$" />` |
| Custom value/label keys | `@Input() <prop>OptionsConfiguration: SelectConfiguration` | Unchanged (`input<SelectConfiguration>`) |

Selects nested inside array items get members prefixed with the array property: an item-level `country` select inside `addresses` exposes `addressesCountryOptionsConfiguration` (fixed values) — dynamic selects inside array items follow the same `addressesCountryCallback` naming.

### Outputs

`save`, `cancel`, and `buttonClick` (fired with the button's property name) are unchanged.

## Validation

- Rules configured through the validation builder (`IsRequired`, `MaxLength`, `GreaterThan`, patterns, ...) become real `Validators.*` on the generated `FormControl`s, and matching `<mat-error>` blocks are generated with the message text **resolved at generation time** (the Formly `${field?.templateOptions?...}` runtime interpolations are gone).
- Formly's global validation-message registry is not used. If your app configured global messages there, per-rule custom messages (`WithMessage(...)`) are the replacement.
- **Named validators** (`.WithValidators("UniqueName")`) become *async validators* resolved through dependency injection. Provide the resolver once in your app:

  ```typescript
  import { ENTRY_ASYNC_VALIDATOR_RESOLVER } from '@enigmatry/entry-form';

  { provide: ENTRY_ASYNC_VALIDATOR_RESOLVER, useValue: (name: string) => myAsyncValidators[name] }
  ```

  The token is injected optionally: if it is not provided, named validators are skipped. A default error line (`<name> validation failed`, id `validators.<kebab-name>`) is generated for each named validator.
- `fieldsRequiredExpressions` toggles `Validators.required` at runtime; a default "is required" error line is generated for every field that lacks a static required rule so the message has somewhere to appear.

## Readonly behavior

Setting `[isReadonly]="true"`:

- disables the whole `FormGroup` (and re-applies `disabled` to statically-readonly controls when leaving readonly mode),
- inputs additionally get the native `readonly` attribute,
- the action buttons are hidden,
- the `entry-form-readonly` class is applied to the `<form>`.

With `builder.WithReadonlyDisplay()` the component instead renders a plain label/value display (`entry-readonly-field` / `entry-readonly-label` / `entry-readonly-value`) for supported controls while readonly — selects show the option display name, formatted controls render through the equivalent Angular pipe. Password fields stay masked; rich-text and custom controls keep their interactive markup (custom controls receive `readonly` through their input).

## Formatting

`.WithFormat(...)` on a form control renders the `entryFieldFormat` directive (`[entryFieldFormatDef]="{ name: 'currency', ... }"`) on the input, and `EntryFieldFormatDirective` is imported from `@enigmatry/entry-form`. Make sure your `@enigmatry/entry-form` version ships this directive before migrating forms that use formatters.

## i18n

Translation ids are preserved wherever a concept survived the migration — labels, placeholders, hints, tooltips, select option display names, and validation messages keep their ids, so existing translation files keep working. Differences:

- Text is emitted as `i18n` / `i18n-placeholder` / `i18n-matTooltip` attributes on real elements (labels still use `$localize` inside the component), so Angular's standard extraction picks everything up from the generated files.
- New ids introduced by the signals output:
  - array buttons: `<feature>.<component>.<array>.add-item` / `.remove-item`,
  - default async-validator errors: `validators.<kebab-cased-name>`,
  - dynamic "required" errors: `<feature>.<component>.<property>.required`; for array-item children the property segment is prefixed with the array (`...<array>.<child>.required`).

## Styling hooks

CSS hooks are preserved: every field carries `entry-<property>-field entry-<control-type>`, groups render as a `div.entry-field-group` (a `CreateUiSection(...)` type is appended as an extra class), and `WithClassName(..., ApplyWhen.FormIsReadonly)` becomes a `[class.x]="isReadonly()"` binding. New hooks: `entry-array-add-button`, `entry-array-remove-button`, and the readonly-display classes listed above.

## List (table) components

Lists never used Formly, so the migration is mechanical: the signals list component is standalone (imports `EntryTableComponent` from `@enigmatry/entry-components/table`), uses `input()`/`output()` signals and `OnPush`. All input and output names (`data`, `loading`, `pageChange`, `sortChange`, `rowClick`, ...) are unchanged, so parent templates typically migrate without edits — only the module registration changes (import the component directly).

## Known limitations of the signals templates

These are validated at generation time where possible — the tool throws a descriptive error instead of emitting broken TypeScript:

| Limitation | Behavior |
|---|---|
| Nested arrays (an `ArrayFormControl` inside another array's item) | Generation error |
| `AutocompleteFormControl` inside an array item | Generation error (per-row filter state cannot be generated) |
| Custom/rich-text control without `.WithImport(...)` | Generation error |
| `fieldsHideExpressions` / `fieldsLabelExpressions` for array-item children | Supported with `'arrayProperty.childProperty'` keys |
| `fieldsPropertyExpressions` / `fieldsDisableExpressions` / `fieldsRequiredExpressions` for array-item children | Not supported (root-level controls only) |
| Overriding fixed select options via an input | Not supported — use dynamic values |
| Formly wrappers | Ignored (see the wrapper migration table) |

## Step-by-step checklist

1. Upgrade the application to Angular v22+ and update `@enigmatry/entry-form` / `@enigmatry/entry-components` to matching versions.
2. Update the codegen tool: `dotnet tool update enigmatry.codegeneration.console -g`.
3. In the configuration assembly:
   - add `.WithImport(...)` to every `CustomFormControl` and `RichTextInputFormControl`;
   - replace wrapper usage per the table above;
   - add `.WithReadonlyDisplay()` to forms that relied on `ENTRY_FIELD_TYPE_RESOLVER`;
   - optionally adopt the new names (`ControlTypes`, `RuleName`, ...).
4. Provide `ENTRY_ASYNC_VALIDATOR_RESOLVER` if any control uses `.WithValidators(...)`.
5. Pick a feature, add `builder.WithSignals()` to its components, regenerate, and:
   - stop importing the feature's `*-generated.module.ts`; import the generated standalone components directly;
   - change dynamic select bindings from `[xOptions]="array"` to `[xCallback]="observable$"`;
   - remove any code that provided `ENTRY_FIELD_TYPE_RESOLVER` or registered Formly types/wrappers for this feature.
6. Repeat per feature. When all features are migrated:
   - switch the pipeline to `entry-codegen ... --signals` and delete the per-component `WithSignals()` calls;
   - delete all `*-generated.module.ts` files;
   - remove `@ngx-formly/*` from `package.json` and the Formly setup (root module config, global message registry, custom field types/wrappers).
7. Regenerate everything once more and run your e2e/UI tests — the DOM structure inside forms changes (Material markup instead of `<formly-form>`), so selectors in tests may need updating.

## Troubleshooting

| Symptom | Cause / fix |
|---|---|
| Generation fails: *"renders a custom element, but no import is configured"* | Add `.WithImport("<Symbol>", "<package>")` to the named control. |
| Generation fails: *"Nested arrays are not supported"* / *"Autocomplete controls are not supported inside array items"* | Restructure the form — see [limitations](#known-limitations-of-the-signals-templates). |
| Angular compile error: *"Component X is standalone, and cannot be declared in an NgModule"* | A still-generated feature module declares a migrated component — migrate the whole feature and stop using its generated module. |
| Dynamic select renders no options | The binding is now `[<prop>Callback]` and expects an `Observable<unknown[]>`, not an array. |
| Async validator never runs | `ENTRY_ASYNC_VALIDATOR_RESOLVER` is not provided; the component skips async validators when the token is absent. |
| Custom control ignores readonly mode | The custom component must expose a `readonly` input; the disabled state also propagates through its `ControlValueAccessor`. |
| Formatter directive not applied / unknown attribute `entryFieldFormat` | Update `@enigmatry/entry-form` to a version that ships `EntryFieldFormatDirective`. |
