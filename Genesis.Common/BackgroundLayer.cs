namespace Genesis.WP8
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class BackgroundLayer
    {
        private readonly string textureName;

        private readonly double scrollSpeedY;

        private readonly double scrollSpeedX;

        private readonly int width;
        private readonly int height;

        private readonly ContentManager contentManager;

        private Texture2D texture2D;

        private double scrollY;

        private double scrollX;

        public void Scroll()
        {
            scrollY -= scrollSpeedY;
            scrollX -= scrollSpeedX;
        }

        public BackgroundLayer(ContentManager contentManager, string textureName, double scrollSpeedY, double scrollSpeedX, int width, int height)
        {
            this.contentManager = contentManager;
            this.textureName = textureName;
            this.scrollSpeedY = scrollSpeedY;
            this.scrollSpeedX = scrollSpeedX;
            this.width = width;
            this.height = height;
        }

        public void LoadContent()
        {
            texture2D = contentManager.Load<Texture2D>(textureName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D, Vector2.Zero, new Rectangle((int)scrollX, (int)scrollY, width, height), Color.White);
        }
    }
}
