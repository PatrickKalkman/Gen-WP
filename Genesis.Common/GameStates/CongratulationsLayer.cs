using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common.GameStates
{
    public class CongratulationsLayer
    {
        private readonly ContentManager contentManager;
        private readonly Rectangle screenDimension;
        private readonly double scaleX;
        private readonly double scaleY;
        private SpriteFont scoreListFont;

        public CongratulationsLayer(ContentManager contentManager, Rectangle screenDimension, double scaleX, double scaleY)
        {
            this.contentManager = contentManager;
            this.screenDimension = screenDimension;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public void Load(string fontName)
        {
            scoreListFont = contentManager.Load<SpriteFont>(fontName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var headerRectangle = new Rectangle(0, 0, screenDimension.Width, screenDimension.Height);

            DrawString(spriteBatch, scoreListFont, "A high score!", headerRectangle, Alignment.Center, Color.Beige);
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