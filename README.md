# XmlConfigStructureBuilder

[![NuGet](https://img.shields.io/nuget/v/Cake.XmlConfigStructureBuilder.svg)](https://www.nuget.org/packages/Cake.XmlConfigStructureBuilder) [![Build Status](https://travis-ci.org/graph-uk/Cake.XmlConfigStructureBuilder.svg?branch=master)](https://travis-ci.org/graph-uk/Cake.XmlConfigStructureBuilder) [![Build status](https://ci.appveyor.com/api/projects/status/qjqkajldidiwyjdm?svg=true)](https://ci.appveyor.com/project/jincod/cake-xmlconfigstructurebuilder)

The project compiles multiple xml configs to single one using the [XDT](https://www.nuget.org/packages/Microsoft.Web.Xdt/)

## Usage

### Minimal configuration

```csharp
#addin nuget:?package=Cake.XmlConfigStructureBuilder&version=0.6.1-alpha&prerelease&loaddependencies=true

var configuration = Argument("buildConfiguration", "Debug");

Task("MakeConfigs")
	.Does(() =>
	{
		MakeConfigs(configuration, ".");
	});
```

### Override Filename Templates Factory

You can override fileNameTemplatesFactory 

```csharp
#addin "Cake.XmlConfigStructureBuilder"

var configuration = Argument("buildConfiguration", "Debug");


IEnumerable<string> GetFileNameTemplates(string configsFolder, string outDir, bool needGlobal)
{
	var globalConfigs = new[]
		{
			System.IO.Path.Combine(configsFolder, "Global.config.generic"),
			System.IO.Path.Combine(configsFolder, "Global.config.{0}"),
			System.IO.Path.Combine(configsFolder, "{1}.config.generic"),
			System.IO.Path.Combine(configsFolder, "{1}.config.{0}"),
		};

	var localConfigs = new[]
	{
		System.IO.Path.Combine(outDir, "{1}.config.Generic"),
		System.IO.Path.Combine(outDir, "{1}.config.{0}")
	};
	return needGlobal ? globalConfigs.Union(localConfigs) : localConfigs;
}

Task("MakeConfigs")
	.Does(() =>
	{
		MakeConfigs(configuration, ".", GetFileNameTemplates);
	});
```
