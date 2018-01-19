using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using XmlConfigStructureBuilder;

namespace Cake.XmlConfigStructureBuilder
{
	/// <summary>
	/// The project compiles multiple xml configs to single one using the XDT transformation
	/// </summary>
	[CakeAliasCategory("Configuration")]
	[CakeNamespaceImport("XmlConfigStructureBuilder")]
	public static class XmlConfigStructureBuilderAliases
	{
		/// <summary>
        /// Make configs inside project root
        /// </summary>
        /// <example>
        /// <code>
        /// Task("MakeConfigs")
        ///   .Does(() => {
        ///     MakeConfigs("Release", ".");
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="buildConfiguration">Build configuration. "Release" or "Debug" for example.</param>
        /// <param name="projectRoot">Project root. "." for example.</param>
        /// <param name="fileNameTemplatesFactory"></param>
		[CakeMethodAlias]
		public static void MakeConfigs(this ICakeContext context, string buildConfiguration, string projectRoot,
			Func<string, string, bool, IEnumerable<string>> fileNameTemplatesFactory = null)
		{
			Builder.CompileProjectConfigs(buildConfiguration, projectRoot, fileNameTemplatesFactory);
		}

		/// <summary>
        /// Make single config file
        /// </summary>
        /// <example>
        /// <code>
        /// Task("MakeConfigs")
        ///   .Does(() => {
        ///     MakeSingleConfig("Release", "ConnectionStrings", ".\\Web\\App_Config\\");
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="buildConfiguration">Build configuration. "Release" or "Debug" for example.</param>
        /// <param name="configName"></param>
        /// <param name="configDir">Project root. "." for example.</param>
        /// <param name="fileNameTemplatesFactory"></param>
		[CakeMethodAlias]
		public static void MakeSingleConfig(this ICakeContext context, string buildConfiguration, string configName,
			string configDir, Func<string, string, bool, IEnumerable<string>> fileNameTemplatesFactory = null)
		{
			Builder.CompileConfig(configName, configDir, buildConfiguration, fileNameTemplatesFactory);
		}
	}
}
