using System.Diagnostics;

namespace Cake.XmlConfigStructureBuilder
{
	public class XmlTransformer : IXmlTransformer
	{
		private const string CttPath = @".\tools\ctt.exe";

		public void Transform(string config, string transform, string result)
		{
			var startInfo = new ProcessStartInfo
			{
				CreateNoWindow = false,
				UseShellExecute = false,
				FileName = CttPath,
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = $"s:{config} t:{transform} d:{result} i ic:\"\t\""
			};

			using (var exeProcess = Process.Start(startInfo))
			{
				exeProcess.WaitForExit();
			}
		}
	}
}