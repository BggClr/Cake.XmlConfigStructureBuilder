using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XmlConfigStructureBuilder
{
	public class Builder
	{
		private const string ConfigsFolder = "_configs\\default";
		private readonly Action<string> _logger;
		private readonly IXmlTransformer _transformer;

		public Builder(Action<string> logger = null)
		{
			_logger = logger ?? (message => { });
			_transformer = new XmlTransformer();
		}

		private void CompileXmlConfig(List<string> filenames, string result)
		{
			_logger($"Name = {result}");

			File.Copy(filenames.First(), result, true);
			var restConfigFiles = filenames.Skip(1);

			foreach (var config in restConfigFiles)
			{
				_logger($"Transorm = {result} using {config}");

				_transformer.Transform(result, config, result);
			}
		}

		public void CompileConfig(string name, string outDir, string buildConfig,
			Func<string, string, bool, IEnumerable<string>> fileNameTemplatesFactory = null)
		{
			var needGlobal = new[] {"app", "web"}.Contains(name);
			var currentFileNameTemplatesFactory = fileNameTemplatesFactory ?? GetFileNameTemplates;
			var filenameTemplates = currentFileNameTemplatesFactory(ConfigsFolder, outDir, needGlobal);
			var filenames = filenameTemplates.Select(p => string.Format(p, buildConfig, name)).Where(File.Exists).ToList();
			var outputFileName = Path.Combine(outDir, $"{name}.config");

			if (filenames.Any())
				CompileXmlConfig(filenames, outputFileName);
		}

		public void CompileProjectConfigs(string buildConfig, string projectRoot = ".", Func<string, string, bool, IEnumerable<string>> fileNameTemplatesFactory = null)
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
						CompileConfig(configFile, path, buildConfig, fileNameTemplatesFactory);
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
				Path.Combine(configsFolder, "{1}.Generic.config"),
				Path.Combine(configsFolder, "{1}.{0}.config")
			};

			var localConfigs = new []
			{
				Path.Combine(outDir, "{1}.Generic.config"),
				Path.Combine(outDir, "{1}.{0}.config")
			};

			return needGlobal ? globalConfigs.Union(localConfigs) : localConfigs;
		}
	}
}
