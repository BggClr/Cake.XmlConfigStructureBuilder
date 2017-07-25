using System.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;

namespace Cake.XmlConfigStructureBuilder
{
	public class XmlTransformer : IXmlTransformer
	{
		private readonly ICakeContext _context;

		public XmlTransformer(ICakeContext context)
		{
			_context = context;
		}

		private const string CttPath = @".\tools\ctt.exe";

		public void Transform(string config, string transform, string result)
		{
			var arguments = new ProcessArgumentBuilder();

			arguments.Append(new TextArgument($"s:{config}"));
			arguments.Append(new TextArgument($"t:{transform}"));
			arguments.Append(new TextArgument($"d:{result}"));
			arguments.Append(new TextArgument("i"));
			arguments.Append(new TextArgument("ic:\"\t\""));

			_context.ProcessRunner.Start(CttPath, new ProcessSettings
			{
				Arguments = arguments
			});
		}
	}
}