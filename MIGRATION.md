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
| Component style | `standalone: false` classes declared in a generated `*-generated.module.ts` | Standalone components, **no module files are generated** |
| Change detection | `ChangeDetectionStrategy.Default` (check-always), stated explicitly so v22's new default does not silently change behavior | `ChangeDetectionStrategy.OnPush` |
| Inputs/outputs | `@Input()` / `@Output()` | `input()` / `model()` / `output()` signals |
| Runtime dependencies | `@ngx-formly/*`, `@enigmatry/entry-form` (field types, wrappers, `ENTRY_FIELD_TYPE_RESOLVER`) | Angular Material, `@enigmatry/entry-form` (expression dictionary types, `sortOptions`, `SelectConfiguration`, `ENTRY_ASYNC_VALIDATOR_RESOLVER`, `EntryFieldFormatDirective`) |
| Validation messages | Formly global message registry, resolved at runtime | Generated `<mat-error>` blocks with messages resolved at generation time |
| Custom field types | Registered in the app's Formly type registry by name | Rendered as a custom element; the providing component is imported via `.WithImport(...)` |

### Change detection on Angular v22

Angular v22 changed the meaning of an omitted `changeDetection`: a component that does not set it is now `OnPush` rather than check-always. The deprecated Formly form template therefore declares `changeDetection: ChangeDetectionStrategy.Default` explicitly, so upgrading a consuming app to v22 does not silently move its generated forms onto `OnPush`.

`Default` is deprecated in v22 in favour of the new `Eager`, and the two are the same enum value. `Eager` does not exist before v22, so the Formly output keeps using `Default` to stay compilable on the v17–v21 range it supports. Do not "modernise" it to `Eager` — that would drop support for those versions. The signals output is unaffected: it has always declared `OnPush`.

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
# short form: -sig
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
| `IFormlyValidationRule.FormlyRuleName` | `GetRuleName()` extension / `ValidationRule<T>.RuleName` | unchanged, still the implementation surface |
| `IFormlyValidationRule.FormlyValidationMessage` | `GetValidationMessage()` extension / `ValidationRule<T>.ValidationMessage` | unchanged, still the implementation surface |
| `IFormlyValidationRule.FormlyTemplateOptions` | `GetTemplateOptions()` extension / `ValidationRule<T>.TemplateOptions` | unchanged, still the implementation surface |

The validation-rule contract is **fully source- and binary-compatible**: `IFormlyValidationRule` and the abstract members of `ValidationRule<TRule>` keep their original Formly-era names, so custom rule implementations and subclasses compile unchanged (and nothing is marked `[Obsolete]`, so warnings-as-errors builds are unaffected). The framework-neutral names are additive reads only: `GetRuleName()` / `GetValidationMessage()` / `GetTemplateOptions()` extension methods on the interface, and plain `RuleName` / `ValidationMessage` / `TemplateOptions` properties on `ValidationRule<TRule>`. The Formly-named members remain the abstract surface until the next major version.

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
- expose a `readonly` input (bound to the generated `isDisabled(...)` helper),
- **forward accessible naming to its internal interactive element.** The generated markup renders a `<label>` with an id and puts `aria-labelledby` on the *host* element; ARIA naming does not cross into a nested `<input>`, `<textarea>` or `contenteditable`, so without forwarding, the control a screen reader focuses stays unnamed. The generator emits a plain `aria-labelledby` **attribute**, so an `@Input() ariaLabelledby` will not receive it — the input has to be aliased to the attribute name (or the value read off the host):

```typescript
// alias the input to the attribute the generated markup emits...
@Input('aria-labelledby') ariaLabelledby?: string;
// ...or read it off the host element once (not private — the template binds it):
protected readonly ariaLabelledby = inject(ElementRef).nativeElement.getAttribute('aria-labelledby');
```

then re-bind it on the inner control (`<input [attr.aria-labelledby]="ariaLabelledby">`). Alternatively, accept the label text as an input and render your own associated label. The same obligation applies to rich-text editor components (`entry-redactor` / `entry-ckeditor`).

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

`model` is now a `model()` signal input and supports two-way binding. On submit the component merges the current form value into the model, updates it, and emits `save` — exactly like the Formly version (including the 500 ms submit throttle). A submit attempt while the form is invalid marks all controls as touched so every validation message becomes visible.

```html
<app-g-user-edit [(model)]="user" (save)="onSave($event)" (cancel)="onCancel()" />
```

One-way `[model]="user"` still works; `save` carries the merged model either way.

Array properties are merged **row by row**: the component tracks each row's original model object (kept aligned with the form rows on add and remove), so unconfigured properties of an existing row survive the round trip, rows added in the UI are appended as new objects, and removing any row — including a middle one — drops exactly that row's data.

### Inputs that kept their names and semantics

`isReadonly`, `saveButtonText`, `cancelButtonText`, `saveButtonDisabled`, `formButtonsTemplate` (an `ng-template` replacing the default action buttons), and the expression dictionaries:

```html
<app-g-user-edit
    [fieldsHideExpressions]="{ lastName: (model) => !model.firstName }"
    [fieldsLabelExpressions]="{ firstName: (model) => model.isCompany ? 'Company name' : 'First name' }"
    ...
```

They are signal inputs now, but the binding syntax from the parent is identical. Note the [array-item limitations](#known-limitations-of-the-signals-templates) below.

Behavioral notes:

- **`fieldsPropertyExpressions` results propagate.** When a calculated property changes a form value, dependent expressions (chained calculations, hide/label/disable/required expressions reading the calculated value) re-evaluate immediately. The change check uses `Object.is`, so `NaN` results are stable; expressions must still return primitives or stable references — an expression returning a fresh object/array on every call never satisfies the change check and re-runs indefinitely (the Formly path had the same assign-per-cycle characteristic).
- **Array-item children get the row as the expression argument.** For `'arrayProperty.childProperty'` keys, hide/label/disable expressions are called with the *current row item* — the original model row merged with the row's current form values, so unconfigured row properties are visible too — matching Formly, where a nested field's expression received the nested model. Cast inside the lambda: `{ 'addresses.city': (model) => !(model as IAddress).verified }`.
- **A hidden control is disabled.** Whether hidden statically (`IsVisible(false)`) or via `fieldsHideExpressions`, the control is disabled while hidden — this covers array-item children per row as well — so it cannot keep an otherwise-valid form unsubmittable. Unlike Formly's default `resetOnHide`, the value is *not* cleared — it stays in the model and in `getRawValue()`.

### Select options — breaking changes

| Scenario | Formly output | Signals output |
|---|---|---|
| Fixed values (`WithFixedValues`) | Overridable `@Input() <prop>Options: any[]` | **No input.** Options are baked into the component. To supply options at runtime, configure the control with dynamic values instead. |
| Dynamic values (`WithDynamicValues`) | `@Input() <prop>Options: any[]` (array, pushed through an internal `BehaviorSubject`) | `[<prop>Callback]` — an **`Observable<unknown[]>`** input:<br>`<app-g-user-edit [countryCallback]="countries$" />` |
| Custom value/label keys | `@Input() <prop>OptionsConfiguration: SelectConfiguration` | Unchanged (`input<SelectConfiguration>`) |

**Option groups** (`WithGroupKey(...)`, `[SelectOptionGroup]` on enum members, or `SelectOption.Group`) are supported: `groupProperty` is added to the generated `SelectConfiguration`, passed to `sortOptions(...)`, and each option carries its `group`. Selects, multi-selects and autocompletes render `<mat-optgroup>` wrappers per group (options without a group render bare, in place); an additional `<prop>OptionGroups` (or `<prop>FilteredOptionGroups` for autocomplete) computed is generated for them. Radio groups and multi-checkboxes keep the `group` value on each option but have no optgroup equivalent, so they still render flat. Note one difference from the Formly output: a **dynamic** select configured with `WithGroupKey(...)` but no custom value/display keys receives `{ groupProperty: '<key>' }` in its `SelectConfiguration` on the signals path (the Formly path emits an empty configuration there, so its group lookup never finds the key).

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
- **Custom rule shapes**: a rule that is not `required` and carries no exact `"<ruleName>: <value>"` template option cannot be turned into a generation-time validator. The tool now logs a **warning** naming the rule and field instead of dropping it silently; its `<mat-error>` markup is still generated and shows if a validator producing that error key is attached at runtime (typically `.WithValidators("<name>")` plus the resolver above). A named validator that shares its error key with a configured rule renders only the rule's configured message — no duplicate default error line.
- **Error visibility**: errors rendered outside a `<mat-form-field>` (checkbox, radio, multi-checkbox, rich-text, custom controls) appear only after the control was touched — matching Material's behavior inside form fields — and a failed submit marks everything touched.

## Readonly behavior

Enabled/disabled state is managed centrally per control from four inputs combined with **OR semantics**: the global `isReadonly` flag, static readonly (`.IsReadonly(true)` — including readonly configured on an enclosing `FormControlGroup`, which propagates to its children, buttons, and manually-bound controls alike), `fieldsDisableExpressions`, and hidden state (hidden controls are disabled, see above). A control is enabled only when none of them applies — a disable expression returning `false` cannot re-enable a statically readonly control. Setting `[isReadonly]="true"`:

- disables every control (statically-readonly controls stay disabled when readonly mode is left again),
- inputs additionally get the native `readonly` attribute,
- the action buttons are hidden,
- array add/remove buttons disappear whenever the array control is disabled — readonly mode, `.IsReadonly(true)` on the array, or a disable expression for the array key,
- the `entry-form-readonly` class is applied to the `<form>`.

With `builder.WithReadonlyDisplay()` the component instead renders a plain label/value display (`entry-readonly-field` / `entry-readonly-label` / `entry-readonly-value`) for supported controls while readonly — selects show the option display name, formatted controls render through the equivalent Angular pipe, and boolean-formatted controls (checkboxes by default) display "Yes"/"No" instead of "true"/"false". Password fields stay masked; rich-text and custom controls keep their interactive markup (custom controls receive `readonly` through their input). `PercentPropertyFormatter.WithMultiplier(...)` is not reflected in the readonly display (the value renders through Angular's plain `percent` pipe).

## Formatting

`.WithFormat(...)` on a form control renders the `entryFieldFormat` directive (`[entryFieldFormatDef]="{ name: 'currency', ... }"`) on the input, and `EntryFieldFormatDirective` is imported from `@enigmatry/entry-form`. Make sure your `@enigmatry/entry-form` version ships this directive before migrating forms that use formatters.

**Custom formatters.** The built-in formatters (`Date`, `Currency`, `Decimal`, `Percent`, `Boolean`) are emitted through a signals-side mirror that escapes the values you configured, so quotes and backslashes in a currency code, digits info or locale are safe. Any other `IPropertyFormatter` — including a **subclass of a built-in one** — keeps its own `ToJsObject()` output, which is emitted verbatim apart from HTML-attribute escaping. That output is TypeScript source you author, so it must already be a valid JS object literal with its own string values correctly single-quoted and escaped.

## i18n

Translation ids are preserved wherever a concept survived the migration — labels, placeholders, hints, tooltips, select option display names, and validation messages keep their ids, so existing translation files keep working. Differences:

- Text is emitted as `i18n` / `i18n-placeholder` / `i18n-matTooltip` attributes on real elements (labels still use `$localize` inside the component), so Angular's standard extraction picks everything up from the generated files.
- New ids introduced by the signals output:
  - array buttons: `<feature>.<component>.<array>.add-item` / `.remove-item`,
  - default async-validator errors: `validators.<kebab-cased-name>`,
  - dynamic "required" errors: `<feature>.<component>.<property>.required`; for array-item children the property segment is prefixed with the array (`...<array>.<child>.required`),
  - readonly-display boolean values: `entry.readonly.boolean.yes` / `entry.readonly.boolean.no`.

## Styling hooks

CSS hooks are preserved: every field carries `entry-<property>-field entry-<control-type>`, groups render as a `div.entry-field-group` with `role="group"` and `aria-labelledby` when labeled (a `CreateUiSection(...)` type is appended as an extra class; a configured group label/hint renders as `label.entry-field-group-label` / `span.entry-field-group-hint`), and `WithClassName(..., ApplyWhen.FormIsReadonly)` becomes a `[class.x]="isReadonly()"` binding. Labels of controls rendered outside a `mat-form-field` (rich-text, custom, fallback) carry generated ids and the elements point back via `aria-labelledby`, so accessible names survive without native `label[for]` support. Arrays are wrapped in a `div.entry-<property>-field.entry-array-field` that also carries the array's configured classes and custom control type name. Buttons render as `mat-button` unless `WithCustomControlType("mat-...")` names another Material button variant (`mat-raised-button`, `mat-flat-button`, ...) — a non-`mat-` type name only lands as a CSS class. New hooks: `entry-array-add-button`, `entry-array-remove-button`, and the readonly-display classes listed above.

## List (table) components

Lists never used Formly, so the migration is mechanical: the signals list component is standalone (imports `EntryTableComponent` from `@enigmatry/entry-components/table`), uses `input()`/`output()` signals and `OnPush`. All input and output names (`data`, `loading`, `pageChange`, `sortChange`, `rowClick`, ...) are unchanged, so parent templates typically migrate without edits — only the module registration changes (import the component directly).

## Known limitations of the signals templates

These are validated at generation time where possible — the tool throws a descriptive error instead of emitting broken TypeScript:

| Limitation | Behavior |
|---|---|
| Nested arrays (an `ArrayFormControl` inside another array's item) | Generation error |
| `AutocompleteFormControl` inside an array item | Generation error (per-row filter state cannot be generated) |
| Custom/rich-text control without `.WithImport(...)` | Generation error |
| A root select property named like an array-item select (`addressesCountry` next to `addresses[].country`) | Generation error (the generated members would collide) |
| `fieldsHideExpressions` / `fieldsLabelExpressions` / `fieldsDisableExpressions` for array-item children | Supported with `'arrayProperty.childProperty'` keys; the expression receives the **row item** as its argument |
| `fieldsPropertyExpressions` / `fieldsRequiredExpressions` for array-item children | Not supported (root-level controls only) |
| Validation rule without a `"<ruleName>: <value>"` template option | No generated validator — logged as a warning; the message markup still renders for runtime-attached validators |
| Overriding fixed select options via an input | Not supported — use dynamic values |
| `PercentPropertyFormatter.WithMultiplier(...)` in the readonly display | Ignored (plain `percent` pipe) |
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
| Generation fails: *"generate colliding member names"* | A root select is named like an array-item select (`addressesCountry` vs `addresses[].country`) — rename one of the properties. |
| Generation warns: *"no Angular validator is generated"* | The rule's template options carry no `"<ruleName>: <value>"` entry — attach the validator at runtime via `.WithValidators(...)` + `ENTRY_ASYNC_VALIDATOR_RESOLVER`, or add the template option. |
| Angular compile error: *"Component X is standalone, and cannot be declared in an NgModule"* | A still-generated feature module declares a migrated component — migrate the whole feature and stop using its generated module. |
| Dynamic select renders no options | The binding is now `[<prop>Callback]` and expects an `Observable<unknown[]>`, not an array. |
| Async validator never runs | `ENTRY_ASYNC_VALIDATOR_RESOLVER` is not provided; the component skips async validators when the token is absent. |
| Custom control ignores readonly mode | The custom component must expose a `readonly` input; the disabled state also propagates through its `ControlValueAccessor`. |
| Formatter directive not applied / unknown attribute `entryFieldFormat` | Update `@enigmatry/entry-form` to a version that ships `EntryFieldFormatDirective`. |
