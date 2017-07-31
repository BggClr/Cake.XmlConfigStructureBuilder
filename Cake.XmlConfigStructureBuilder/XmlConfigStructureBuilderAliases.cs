using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using XmlConfigStructureBuilder;

namespace Cake.XmlConfigStructureBuilder
{
	[CakeAliasCategory("Configuration")]
	[CakeNamespaceImport("XmlConfigStructureBuilder")]
	public static class XmlConfigStructureBuilderAliases
	{
		[CakeMethodAlias]
		public static void MakeConfigs(this ICakeContext context, string buildConfiguration, string projectRoot, Func<string, string, bool, IEnumerable<string>> fileNameTemplatesFactory = null)
		{
			Builder.CompileProjectConfigs(buildConfiguration, projectRoot, fileNameTemplatesFactory);
		}
	}
}
