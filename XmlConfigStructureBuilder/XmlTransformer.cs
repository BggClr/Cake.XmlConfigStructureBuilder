using System;
using Microsoft.Web.XmlTransform;

namespace XmlConfigStructureBuilder
{
	public class XmlTransformer : IXmlTransformer
	{
		public void Transform(string sourceFile, string transformFile, string targetFile)
		{
			using (var document = new XmlTransformableDocument {PreserveWhitespace = true})
			{
				using (var transform = new XmlTransformation(transformFile))
				{
					document.Load(sourceFile);

					if (!transform.Apply(document))
						throw new Exception($"Failed to transform \"{sourceFile}\" using \"{transformFile}\" to \"{targetFile}\"");

					document.Save(targetFile);
				}
			}
		}
	}
}
