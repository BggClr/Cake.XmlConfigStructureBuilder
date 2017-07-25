using System.Diagnostics;

namespace BggClr.XmlConfigStructureBuilder
{
	public class XmlTransformer : IXmlTransformer
	{
		private const string CttPath = @".\tools\ctt.exe";

		public void Transform(string config, string transform, string result)
		{
			Process.Start(CttPath, $"s:{config} t:{transform} d:{result} i ic:\"\t\"");
		}
	}
}