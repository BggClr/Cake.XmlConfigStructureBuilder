using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Path = System.IO.Path;

namespace XmlConfigStructureBuilder
{
	public static class Builder
	{
		private const string ConfigsFolder = "_configs";
		
		public static void CompileXmlConfig(IEnumerable<string> filenames, string result)
		{
			var existingFiles = filenames.Where(File.Exists).ToArray();
			var firstConfigFile = existingFiles.FirstOrDefault();

			if (firstConfigFile == null)
				return;

			Console.WriteLine($"Name = {result}");

			File.Copy(firstConfigFile, result, true);
			var restConfigFiles = existingFiles.Skip(1);
			var transformer = new XmlTransformer();

			foreach (var config in restConfigFiles)
			{
				transformer.Transform(result, config, result);
			}
		}

		public static void CompileConfig(string name, string outDir, string buildConfig)
		{
			var needGlobal = new[] {"app", "web"}.Contains(name);
			var filenameTemplates = GetFileNameTemplates(ConfigsFolder, outDir, needGlobal);
			var filenames = filenameTemplates.Select(p => string.Format(p, buildConfig, name));
			var outputFileName = Path.Combine(outDir, $"{name}.config");

			CompileXmlConfig(filenames, outputFileName);
		}

		public static void CompileProjectConfigs(string buildConfig, string projectRoot = ".")
		{
			var files = Directory.GetFiles(projectRoot, "*.csproj", SearchOption.AllDirectories);

			foreach (var file in files)
			{
				var data = File.ReadAllText(file);
				var path = Path.GetDirectoryName(file);
				var configFiles = Regex.Matches(data, @"(?<ConfigType>app|web|nlog)\.config", RegexOptions.IgnoreCase)
					.OfType<Match>()
					.Where(p => p.Success)
					.Select(x => x.Groups["ConfigType"].Value.ToLower())
					.Distinct()
					.ToArray();

				if (configFiles.Any())
				{
					foreach (var configFile in configFiles)
					{
						CompileConfig(configFile, path, buildConfig);
					}
				}
			}
		}

		private static IEnumerable<string> GetFileNameTemplates(string configsFolder, string outDir, bool needGlobal)
		{
			var globalConfigs = new[]
				{
					Path.Combine(configsFolder, "Global.Generic.config"),
					Path.Combine(configsFolder, "Global.{0}.config"),
					Path.Combine(configsFolder, "Global.{0}.config"),
					Path.Combine(configsFolder, "{1}.{0}.config"),
				};

			var localConfigs = new[]
			{
				Path.Combine(outDir, "{1}.Generic.config"),
				Path.Combine(outDir, "{1}.{0}.config")
			};
			return needGlobal ? globalConfigs.Union(localConfigs) : localConfigs;
		}
	}
}