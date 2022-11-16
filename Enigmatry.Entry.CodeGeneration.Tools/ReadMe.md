# About

Generates UI client feature component(s) from configuration(s)

Usage:

```
Enigmatry.Entry.CodeGeneration.Tools [options]
```

Options:

* -sa, --source-assembly (REQUIRED) - Source assembly file from which component(s) configuration(s) will be read
* -dd, --destination-directory (REQUIRED) - Destination directory where angular component(s) will be generated
* -c, --component-name - Single component name to be generated

## Example commands

### Generate all components

```
Enigmatry.Entry.CodeGeneration.Tools --source-assembly D:\Enigmatry\enigmatry-blueprint-reference\Enigmatry.Blueprint.Reference.Api\bin\Debug\netcoreapp3.1\Enigmatry.Blueprint.Reference.Api.dll --destination-directory D:\Enigmatry\enigmatry-blueprint-reference\enigmatry-blueprint-reference-app\src\app\features
```

### Generate single component

```
Enigmatry.Entry.CodeGeneration.Tools --source-assembly D:\Enigmatry\enigmatry-blueprint-reference\Enigmatry.Blueprint.Reference.Api\bin\Debug\netcoreapp3.1\Enigmatry.Blueprint.Reference.Api.dll --destination-directory D:\Enigmatry\enigmatry-blueprint-reference\enigmatry-blueprint-reference-app\src\app\features --component-name ProjectList
```
