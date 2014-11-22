using Genesis.Common;
using Genesis.Common.GameStates;
using Genesis.Management.WP8;
using Genesis.WP8.Common;
using Genesis.WP8.Resources;

namespace Genesis.WP8
{
    public class WindowsPhoneFactory : IPlatformSpecificFactory
    {
        public IInputManager CreateInputManager()
        {
            return new InputManager();
        }

        public IGameDefinitionXmlReader CreateGameDefinitionXmlReader()
        {
            return new GameDefinitionXmlReader();
        }

        public ICachingService CreateCachingService()
        {
            return new CachingService();
        }

        public IFlipTileCreator CreateFlipTileCreator()
        {
            return new FlipTileCreator();
        }

        public string GetGameDefinition()
        {
            return AppResources.GameDefinition;
        }
    }
}