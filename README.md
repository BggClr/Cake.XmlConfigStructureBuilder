# XmlConfigStructureBuilder

[![NuGet](https://img.shields.io/nuget/v/Cake.XmlConfigStructureBuilder.svg)](https://www.nuget.org/packages/Cake.XmlConfigStructureBuilder)

The project compiles multiple xml configs to single one using the [CTT](https://ctt.codeplex.com/)

## Usage

### Cake integrations

```csharp
#addin "Cake.XmlConfigStructureBuilder"

var configuration = Argument("buildConfiguration", "Debug");

Task("MakeConfigs")
	.Does(() =>
	{
		MakeConfigs(configuration, ".");
	});
```
