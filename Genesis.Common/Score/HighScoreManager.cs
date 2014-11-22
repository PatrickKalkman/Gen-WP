using System.Linq;

namespace Genesis.Common.Score
{
    public class HighScoreManager
    {
        private readonly ICachingService cachingService;
        private readonly HighScoreListFactory highScoreListFactory;
        private readonly IFlipTileCreator flipTileCreator;
        private HighScoreList highScoreList;
        private const string GenesisHighScoreKey = "GenesisHighScore";
        private const int cachePeriodInMinutes = 60 * 24 * 365 * 2;

        public HighScoreManager(ICachingService cachingService, HighScoreListFactory highScoreListFactory, IFlipTileCreator flipTileCreator)
        {
            this.cachingService = cachingService;
            this.highScoreListFactory = highScoreListFactory;
            this.flipTileCreator = flipTileCreator;
        }

        public void Initialize()
        {
            CacheItem item = cachingService.IsAvailable(GenesisHighScoreKey);
            if (!item.IsAvailable)
            {
                highScoreList = highScoreListFactory.CreateDefaultScoreList();
            }
            else
            {
                highScoreList = cachingService.LoadCachedData<HighScoreList>(GenesisHighScoreKey);
            }
        }

        public bool IsHighScore(ScoreData score)
        {
            int lowestHighScore = GetLowestHighScore();
            if (score.Score > lowestHighScore)
            {
                return true;
            }
            return false;
        }

        public void StoreHighScore(ScoreData score)
        {
            int index = GetIndexScoreToReplace(score.Score);
            var entry = new HighScoreListEntry();
            entry.Score = score.Score;
            entry.Level = score.Level;
            entry.Name = score.Name;
            highScoreList.Insert(index, entry);
            highScoreList.RemoveAt(10);

            string wideContent = "Try to beat your highscore of " + score.Score + ", " + entry.Name + "!";
            string content = "Your highscore (" + score.Score + "), " + entry.Name + "!";
            flipTileCreator.UpdateDefaultTile(content, wideContent);

            cachingService.StoreCachedData(GenesisHighScoreKey, highScoreList);
        }

        private int GetLowestHighScore()
        {
            return highScoreList.Select(highScore => highScore.Score).Concat(new[] { int.MaxValue }).Min();
        }

        private int GetIndexScoreToReplace(int score)
        {
            for (int index = highScoreList.Count - 1; index >=0 ; index--)
            {
                if (highScoreList[index].Score > score)
                {
                    return index + 1;
                }
            }
            return 0;
        }

        public HighScoreList GetHighScoreList()
        {
            return highScoreList;
        }

        public int GetHigestScore()
        {
            if (highScoreList != null)
            {
                return highScoreList[0].Score;
            }
            return 0;
        }

        public void Reset()
        {
            cachingService.Clear(GenesisHighScoreKey);
            highScoreList = highScoreListFactory.CreateDefaultScoreList();
            cachingService.StoreCachedData(GenesisHighScoreKey, highScoreList);
        }
    }
}
