namespace Genesis.Common
{
    public interface IGameDefinitionParser
    {
        StageCollection Parse(string gameDefinitionXml);
    }
}