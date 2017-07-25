using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Path = System.IO.Path;

namespace Cake.XmlConfigStructureBuilder
{
	public static class Builder
	{
		private const string ConfigsFolder = "_configs";

		[CakeMethodAlias]
		public static void CompileXmlConfig(this ICakeContext context, IEnumerable<string> filenames, string result)
		{
			var existingFiles = filenames.Where(p => context.FileSystem.Exist(new FilePath(p))).ToArray();
			var firstConfigFile = existingFiles.FirstOrDefault();

			if (firstConfigFile == null)
				return;

			File.Copy(firstConfigFile, result);
			var restConfigFiles = existingFiles.Skip(1);
			var transformer = new XmlTransformer(context);

			foreach (var config in restConfigFiles)
			{
				transformer.Transform(result, config, result);
			}
		}

		public static void CompileConfig(this ICakeContext context, string name, string outDir, string buildConfig)
		{
			var message = $"Name = {outDir}\\{name}.config";
			Console.WriteLine(message);

			var needGlobal = new[] {"app", "web"}.Contains(name);
			var filenameTemplates = GetFileNameTemplates(ConfigsFolder, outDir, needGlobal);
			var filenames = filenameTemplates.Select(p => string.Format(p, buildConfig, name));
			var outputFileName = Path.Combine(outDir, $"{name}.config");
			CompileXmlConfig(context, filenames, outputFileName);
		}

		public static void CompileProjectConfigs(this ICakeContext context, string buildConfig)
		{
			var files = Directory.GetFiles(".", "*.csproj", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				var data = File.ReadAllText(file);
				var path = Path.GetDirectoryName(file);
				var matches = Regex.Matches(data, @"(?<ConfigType>app|web|nlog)\.config", RegexOptions.IgnoreCase)
					.OfType<Match>()
					.Where(p => p.Success)
					.ToArray();

				if (matches.Any())
				{
					foreach (var match in matches)
					{
						CompileConfig(context, match.Groups["ConfigType"].Value, path, buildConfig);
					}
				}
			}
		}

		private static IEnumerable<string> GetFileNameTemplates(string configsFolder, string outDir, bool needGlobal)
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