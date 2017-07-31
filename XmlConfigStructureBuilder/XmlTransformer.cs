using System;
using System.Diagnostics;
using OutcoldSolutions.ConfigTransformationTool;

namespace XmlConfigStructureBuilder
{
	public class XmlTransformer : IXmlTransformer
	{
		public void Transform(string config, string transform, string result)
		{
			var log = OutputLog.FromWriter(Console.Out, Console.Error);
			var task = new TransformationTask(log, config, transform, false)
			{
				Indent = true,
				IndentChars = "\t"
			};

			if (!task.Execute(result))
			{
				throw new Exception("Transformtion is not completed");
			}
		}
	}
}