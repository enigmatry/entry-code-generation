# Enigmatry Entry Code Generator (codegen)

## Intro

Short, easy to understand, explanation of the tool ...

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
