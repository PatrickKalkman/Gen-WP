namespace Genesis.Common.Score
{
    public class HighScoreListFactory
    {
        public HighScoreList CreateDefaultScoreList()
        {
            return new HighScoreList()
            {
                new HighScoreListEntry() { Id = 1, Level = 1, Name = "Patrick", Position = 1, Score = 10 },
                new HighScoreListEntry() { Id = 2, Level = 1, Name = "Patrick", Position = 2, Score =  10 },
                new HighScoreListEntry() { Id = 3, Level = 1, Name = "Patrick", Position = 3, Score =  10 },
                new HighScoreListEntry() { Id = 4, Level = 1, Name = "Patrick", Position = 4, Score =   10 },
                new HighScoreListEntry() { Id = 5, Level = 1, Name = "Patrick", Position = 5, Score =   10 },
                new HighScoreListEntry() { Id = 6, Level = 1, Name = "Patrick", Position = 6, Score =   10 },
                new HighScoreListEntry() { Id = 7, Level = 1, Name = "Patrick", Position = 7, Score =   10 },
                new HighScoreListEntry() { Id = 8, Level = 1, Name = "Patrick", Position = 8, Score =   10 },
                new HighScoreListEntry() { Id = 9, Level = 1, Name = "Patrick", Position = 9, Score =   10 },
                new HighScoreListEntry() { Id = 10, Level = 1, Name = "Patrick", Position = 10, Score =  10 },
            };
        }
    }
}
