namespace Genesis.Common.GameStates
{
    public interface IPlatformSpecificFactory
    {
        IInputManager CreateInputManager();

        IGameDefinitionXmlReader CreateGameDefinitionXmlReader();

        ICachingService CreateCachingService();

        IFlipTileCreator CreateFlipTileCreator();

        string GetGameDefinition();
    }
}