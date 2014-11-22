using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common.Score
{
    public class Score
    {
        private readonly ContentManager contentManager;

        private readonly Rectangle screenDimension;

        private readonly Spaceship spaceship;

        private int playerScore;

        private int highScore;

        private Vector2 position;

        private SpriteFont spriteFont;

        public Score(ContentManager contentManager, Rectangle screenDimension, Spaceship spaceship, int highScore)
        {
            this.contentManager = contentManager;
            this.screenDimension = screenDimension;
            this.spaceship = spaceship;
            this.highScore = highScore;
        }

        public void Load(string fontName)
        {
            spriteFont = contentManager.Load<SpriteFont>(fontName);
        }

        public void IncreaseScore(int scoreToAdd)
        {
            playerScore += scoreToAdd;
            if (playerScore > highScore)
            {
                highScore = playerScore;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string scoreString = string.Format("Score {0}", playerScore);
            position = new Vector2(screenDimension.Width - (spriteFont.MeasureString(scoreString).X + 5), 2);
            spriteBatch.DrawString(spriteFont, scoreString, position, Color.Blue);
            string highscoreString = string.Format("High {0}", highScore);
            position = new Vector2(5, 2);
            spriteBatch.DrawString(spriteFont, highscoreString, position, Color.Blue);
            string powerString = string.Format("Power {0}", spaceship.Power);
            position = new Vector2(5, screenDimension.Height - 80);
            spriteBatch.DrawString(spriteFont, powerString, position, Color.Blue);
        }

        public int PlayerScore
        {
            get { return playerScore; }
        }

        public void Reset()
        {
            playerScore = 0;
        }
    }
}
