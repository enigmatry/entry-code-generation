# Enigmatry Entry Code Generator (codegen)

## Intro

The Entry code generator (`entry-codegen`) is a dotnet tool that generates Angular list and form components from C# configuration. You describe a table or a form once, in a fluent builder next to your domain model; the tool scans that assembly and renders the Angular TypeScript and HTML through Razor templates, as ordinary readable source in your repository. The generated components use [@enigmatry/entry-components and @enigmatry/entry-form](https://github.com/enigmatry/entry-angular-building-blocks). The predictable layers are generated, so engineers and coding agents can spend their time on the part that is specific to the business.

The generator is one of the three parts of Entry, the open-source foundation described at https://www.enigmatry.com/en/entry/; the [Entry blueprint](https://github.com/enigmatry/entry-blueprint) shows it wired into a complete application.

## Index

* [Intro](#intro)
* [Glossary](#glossary)
* [Setup](#setup)
  * [Configure Enigmatry npm feed authentication (temp)](#configure-enigmatry-npm-feed-authentication-temp)
  * [Install codegen tool](#install-codegen-tool)
  * [Update codegen tool](#update-codegen-tool)
  * [Resolve codegen update problems (temp)](#resolve-codegen-update-problems-temp)
  * [Create codegen configurations](#create-codegen-configurations)
  * [Run codegen tool](#run-codegen-tool)
* [Tables](#tables)
  * [List component configuration](#list-component-configuration)
  * [Column configuration](#column-configuration)
  * [Custom cells](#custom-cells)
  * [Custom headers](#custom-headers)
  * [Paging](#paging)
  * [Row context menu](#row-context-menu)
  * [Rows selection](#rows-selection)
* [Forms](#forms)
  * [Form component configuration](#form-component-configuration)
* [FAQ](#faq)
* [Examples](#examples)
* [Migrating from Formly to signals-based generation](MIGRATION.md)

## Glossary

| Name | Description |
|-|-|
| codegen | Enigmatry Entry Code Generator |
| codegen tool | Enigmatry Entry Code Generator dotnet tool used to generate client side components |
| List | Table component that supports: multiple rows, headers, sorting, paging, sow selection, context menu, etc. |
| Form | Form component that supports: TODO |

## Setup

### Configure Enigmatry npm feed authentication (temp)

### Install codegen tool

```dotnet tool install enigmatry.codegeneration.console -g```

### Create codegen configurations

### Update codegen tool

```dotnet tool update enigmatry.codegeneration.console -g```

### Resolve codegen update problems (temp)

### Run codegen tool

## Tables

TODO: explain `IListComponentConfiguration` builder

### List component configuration

List configuration is built via `ListComponentBuilder` api.

| Method | Description |
|-|-|
| HasName() | Define pascal-case component name (e.g. "UserList")  |
| BelongsToFeature() | TODO |
| OrderBy() | TODO |
| IncludeUnconfiguredProperties() | TODO |

### Column configuration

`ListComponentBuilder.Column<TProperty>(Expression<Func<T, TProperty>> propertyExpression)` ...

| Method | Description |
|-|-|
| IsSortable(bool) | When `true` (default value) column is sortable |
| IsVisible(bool) | When `true` (default value) column is visible, otherwise it becomes hidden but still part of the model |
| WithHeaderName() | TODO |
| WithTranslationId() | TODO |
| WithSortId() | TODO |
| WithFormat() | TODO |
| WithCustomComponent() | TODO |
| WithCustomCssClass() | TODO |
| WithCustomProperties() | TODO |

### Custom cells

### Custom headers

### Paging

### Row context menu

### Rows selection

## Forms

### Form component configuration

TODO: explain `IFormComponentConfiguration` builder

## FAQ

## Examples

## About Enigmatry

Entry is the open-source foundation Enigmatry builds client systems on. Enigmatry is a software company from Rotterdam, founded in 2006, with teams in Belgrade and Novi Sad.

* How we use Entry for clients: https://www.enigmatry.com/en/entry/
* How we build, with agents under checks: https://www.enigmatry.com/en/how-we-build/

Apache-2.0 © Enigmatry
