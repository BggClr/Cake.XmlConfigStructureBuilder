namespace BggClr.XmlConfigStructureBuilder
{
	public interface IXmlTransformer
	{
		void Transform(string config, string transform, string result);
	}
}