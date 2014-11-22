namespace Genesis.Common
{
    public interface IFlipTileCreator
    {
        void CreateTile(string content, string wideContent);

        void UpdateDefaultTile(string content, string wideContent);

        void UpdateTile(string content, string wideContent);
    }
}