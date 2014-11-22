namespace Genesis.Common
{
    public interface IGameDefinitionXmlReader
    {
        bool ReadAll(string xmlFilename, out string contents);
    }
}