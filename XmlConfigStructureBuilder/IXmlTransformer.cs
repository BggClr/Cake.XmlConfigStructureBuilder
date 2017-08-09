namespace XmlConfigStructureBuilder
{
	public interface IXmlTransformer
	{
		void Transform(string sourceFile, string transformFile, string targetFile);
	}
}