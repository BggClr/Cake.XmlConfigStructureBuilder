using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cake.XmlConfigStructureBuilder
{
	public class Builder
	{
		private readonly ISettings _settings;
		private readonly IXmlTransformer _transformer;

		public Builder(IXmlTransformer transformer, ISettings settings)
		{
			_transformer = transformer;
			_settings = settings;
		}

		public void CompileXmlConfig(IEnumerable<string> filenames, string result)
		{
			var existingFiles = filenames.Where(File.Exists).ToArray();
			var firstConfigFile = existingFiles.FirstOrDefault();

			if (firstConfigFile == null)
				return;

			File.Copy(firstConfigFile, result);
			var restConfigFiles = existingFiles.Skip(1);

			foreach (var config in restConfigFiles) _transformer.Transform(result, config, result);
		}

		public void CompileConfig(string name, string outDir, string buildConfig)
		{
			var message = $"Name = {outDir}\\{name}.config";
			Console.WriteLine(message);

			var needGlobal = new[] {"app", "web"}.Contains(name);
			var filenameTemplates = GetFileNameTemplates(_settings.ConfigsFolder, outDir, needGlobal);
			var filenames = filenameTemplates.Select(p => string.Format(p, buildConfig, name));
			var outputFileName = Path.Combine(outDir, $"{name}.config");
			CompileXmlConfig(filenames, outputFileName);
		}

		public void CompileProjectConfigs(string buildConfig)
		{
			var files = Directory.GetFiles(_settings.ProjectDir, "*.csproj", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				var data = File.ReadAllText(file);
				var path = Path.GetDirectoryName(file);
				var matches = Regex.Matches(data, @"(?<ConfigType>app|web|nlog)\.config", RegexOptions.IgnoreCase)
					.OfType<Match>()
					.Where(p => p.Success)
					.ToArray();

				if (matches.Any())
					foreach (var match in matches)
						CompileConfig(match.Groups["ConfigType"].Value, path, buildConfig);
			}
		}

		private IEnumerable<string> GetFileNameTemplates(string configsFolder, string outDir, bool needGlobal)
		{
			var globals = new string[] {};
			if (needGlobal)
				globals = new[]
				{
					Path.Combine(configsFolder, "Global.Generic.config"),
					Path.Combine(configsFolder, "Global.{0}.config")
				};

			return globals.Union(new[]
			{
				Path.Combine(configsFolder, "Global.{0}.config"),
				Path.Combine(configsFolder, "{1}.{0}.config"),
				Path.Combine(outDir, "{1}.Generic.config"),
				Path.Combine(outDir, "{1}.{0}.config")
			});
		}
	}
}