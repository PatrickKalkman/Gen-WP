using System;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common.Score
{
    public class HighScore
    {
        private readonly ContentManager contentManager;
        private readonly Rectangle screenDimension;
        private readonly HighScorePositionNumberFormatter positionNumberFormatter;
        private HighScoreList highScoreList;
        private readonly double scaleX;
        private readonly double scaleY;
        private SpriteFont scoreListFont;
        private int blinkCounter = 30;

        public HighScore(ContentManager contentManager, Rectangle screenDimension, HighScorePositionNumberFormatter positionNumberFormatter, HighScoreList highScoreList, double scaleX, double scaleY)
        {
            this.contentManager = contentManager;
            this.screenDimension = screenDimension;
            this.positionNumberFormatter = positionNumberFormatter;
            this.highScoreList = highScoreList;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public void Load(string fontName)
        {
            scoreListFont = contentManager.Load<SpriteFont>(fontName);
        }

        bool showTap = true;

        public void Draw(SpriteBatch spriteBatch, string applicationTitle)
        {
            if (highScoreList == null)
                return;

            var tapRectangle = new Rectangle(0, screenDimension.Height - 250, screenDimension.Width, 50);

            blinkCounter--;
            if (blinkCounter > 0 && showTap)
            {
                DrawString(spriteBatch, scoreListFont, "PRESS LEVEL BUTTON TO START", tapRectangle, Alignment.Center, Color.AntiqueWhite);
            }

            if (blinkCounter < 0)
            {
                blinkCounter = 30;
                showTap = !showTap;
            }

            var headerRectangle = new Rectangle(0, 30, screenDimension.Width, 35);
            
            DrawString(spriteBatch, scoreListFont, applicationTitle + " HALL OF FAME", headerRectangle, Alignment.Center, Color.Yellow);

            int rowSize = (int)(35 * scaleY);
            int startPosX = (int)(20 * scaleX);

            var scoreHeaderRectangle = new Rectangle((int)((startPosX + 80) * scaleX), (int)(120 * scaleY), 100, 25);
            var levelHeaderRectangle = new Rectangle((int)((startPosX + 170) * scaleX), (int)(120 * scaleY), 60, 25);
            var nameHeaderRectangle = new Rectangle((int)((startPosX + 305) * scaleX), (int)(120* scaleY), 100, 25);

            DrawString(spriteBatch, scoreListFont, "Score", scoreHeaderRectangle, Alignment.Right, Color.Blue);
            DrawString(spriteBatch, scoreListFont, "Lvl", levelHeaderRectangle, Alignment.Right, Color.Blue);
            DrawString(spriteBatch, scoreListFont, "Name", nameHeaderRectangle, Alignment.Right, Color.Blue);

            var positionRectangle = new Rectangle((int)(startPosX * scaleX), (int)(160 * scaleY), 50, 25);
            var scoreRectangle = new Rectangle((int)((startPosX + 80) * scaleX), (int)(160 * scaleY), 100, 25);
            var levelRectangle = new Rectangle((int)((startPosX + 170) * scaleX), (int)(160 * scaleY), 60, 25);
            var nameRectangle = new Rectangle((int)((startPosX + 305) * scaleX), (int)(160 * scaleY), 100, 25);

            bool isFirstRow = true;

            for (int highScoreIndex = 0; highScoreIndex < highScoreList.Count; highScoreIndex++)
            {
                Color color = Color.Blue;

                if (isFirstRow)
                {
                    color = Color.CornflowerBlue;
                    isFirstRow = false;
                }

                string highScore = string.Format("{0}", positionNumberFormatter.Format(highScoreIndex + 1));
                DrawString(spriteBatch, scoreListFont, highScore, positionRectangle, Alignment.Right, color);
                positionRectangle.Y += rowSize;

                string score = String.Format("{0}", highScoreList[highScoreIndex].Score.ToString(CultureInfo.InvariantCulture));
                DrawString(spriteBatch, scoreListFont, score, scoreRectangle, Alignment.Right, color);
                scoreRectangle.Y += rowSize;

                string level = String.Format("{0}", highScoreList[highScoreIndex].Level.ToString(CultureInfo.InvariantCulture));
                DrawString(spriteBatch, scoreListFont, level, levelRectangle, Alignment.Right, color);
                levelRectangle.Y += rowSize;

                string name = String.Format("{0}", highScoreList[highScoreIndex].Name);
                DrawString(spriteBatch, scoreListFont, name, nameRectangle, Alignment.Right, color);
                nameRectangle.Y += rowSize;
            }
        }

        public void RefreshHighScore(HighScoreList highScoreList)
        {
            this.highScoreList = highScoreList;
        }

        [Flags]
        public enum Alignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle bounds, Alignment align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(Alignment.Left))
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Right))
                origin.X -= bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Top))
                origin.Y += bounds.Height / 2 - size.Y / 2;

            if (align.HasFlag(Alignment.Bottom))
                origin.Y -= bounds.Height / 2 - size.Y / 2;

            spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}
