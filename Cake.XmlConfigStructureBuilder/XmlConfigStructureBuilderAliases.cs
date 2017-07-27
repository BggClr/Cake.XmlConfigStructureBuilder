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
		public static void MakeConfigs(this ICakeContext context, string buildConfiguration, string projectRoot)
		{
			Builder.CompileProjectConfigs(buildConfiguration, projectRoot);
		}
	}
}
