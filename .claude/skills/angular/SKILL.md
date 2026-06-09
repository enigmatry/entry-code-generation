---
name: angular
description: Angular component and service authoring guidelines — signals API, standalone components, DI with inject(), reactive forms, and lifecycle rules. Use this when writing or reviewing Angular components, services, templates, or Razor templates (.cshtml) that generate Angular code.
---

# Angular Development

## Signals API

- Use `input()`, `model()`, `output()`, `signal()`, `computed()`, `effect()`, and `toSignal()` for reactive state.
- Never use `@Input()` / `@Output()` decorators in new code — use signal-based equivalents.
- `effect()` runs inside the constructor; do not call it in lifecycle hooks.
- `toSignal()` converts an Observable to a signal; use it with `{ initialValue: [] }` when the Observable is optional or may emit nothing.

```ts
// ✅ correct
protected readonly model = model<IMyModel>({} as IMyModel);
protected readonly isReadonly = input<boolean>(false);
protected readonly save = output<IMyModel>();
private readonly items = toSignal(this.itemsCallback(), { initialValue: [] });

constructor() {
    effect(() => {
        this.form.patchValue(this.model() as unknown as typeof this.form.value, { emitEvent: false });
    });
}

// ❌ avoid
@Input() model: IMyModel = {} as IMyModel;
@Output() save = new EventEmitter<IMyModel>();
```

## Standalone Components

- All components must be standalone (`imports: [...]` in `@Component`).
- Never use NgModules for new components.
- Explicitly list Angular Material modules and `ReactiveFormsModule` in `imports`.
- Always set `changeDetection: ChangeDetectionStrategy.OnPush`.

## DI and Class Structure

- Use `inject()` for all dependency injection — never constructor injection.
- Use `readonly` arrow-function properties for all component methods except Angular lifecycle hooks, which must be plain methods (the framework calls them by name through the interface).
- `constructor()` is for `effect()` registrations and `inject()` calls only.

```ts
// ✅ correct
private readonly destroyRef = inject(DestroyRef);
protected readonly onSubmit = (): void => { ... };
protected readonly isHidden = (name: string, visible: boolean): boolean => { ... };

// Only lifecycle hooks are plain methods:
ngOnInit(): void { ... }

// ❌ avoid
constructor(private destroyRef: DestroyRef) {}
protected onSubmit(): void { ... }
```

## Async / Await over RxJS

- Use `async/await` with `firstValueFrom()` for Observables that emit once and complete (HTTP calls).
- Reserve `.subscribe()` for true streams (Observables that emit multiple values over time).
- Use `resource({ loader })` for component-lifecycle-scoped async work; it owns the Promise and exposes `value()` / `status()` / `error()` signals.
- Never make lifecycle hooks `async`. Never fire-and-forget a Promise.

## Reactive Forms (Angular Material)

- Use `FormGroup` / `FormControl` / `FormArray` from `@angular/forms` with `ReactiveFormsModule`.
- Bind form controls with `formControlName`, arrays with `formArrayName`, and nested groups with `formGroupName`.
- Typed `FormControl<T>` reduces `as any` casts; initialize with `{ value: x, disabled: true }` for static readonly controls.
- Use Angular validators: `Validators.required`, `Validators.maxLength(n)`, `Validators.pattern(/regex/)`, etc.
- Validation error display: `@if (form.get('field')?.hasError('required')) { <mat-error>...</mat-error> }`.

## i18n

- Use `$localize :@@translation-key:default text`` for i18n strings in generated Angular code.
- Translation keys: kebab-case, scoped to feature/component. No abbreviations.
- Reuse `shared.*` keys for generic labels (Save, Cancel, OK, etc.).

## What NOT to do

- Do not use `@Input()` / `@Output()` decorators — use signal inputs/outputs.
- Do not use NgModules for new components.
- Do not use constructor injection — use `inject()`.
- Do not make lifecycle hooks `async`.
- Do not use regular method syntax for non-lifecycle methods — use readonly arrow properties.
- Do not use `ChangeDetectionStrategy.Default` — always use `OnPush`.
