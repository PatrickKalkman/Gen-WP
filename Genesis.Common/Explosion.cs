using Genesis.Common;

namespace Genesis.WP8
{
    using Microsoft.Xna.Framework;

    public class Explosion : AnimatedSprite
    {
        private GenesisGameManager gameManager;

        private int bulletIndex;

        public Explosion(
            Rectangle screenDimension, 
            ITextureManager textureManager, 
            int width, 
            int height, 
            int frameWidth, 
            int frameHeight, 
            Point textureStart, 
            SpriteAnimationType typeOfAnimation, 
            int numberOfFrames,
            int framesPerRow,
            int framesPerSecond)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, 1f, numberOfFrames, framesPerRow, framesPerSecond, textureStart, typeOfAnimation)
        {
        }

        public void SetOptions(Point startTexture, int width, int height, int frameWidth, int frameHeight, int framesPerSecond, int numberOfFrames, int numberOfFramesPerRow)
        {
            this.TextureSourceStart = new Point(startTexture.X, startTexture.Y);
            this.Width = width;
            this.Height = height;
            this.FramesPerSecond = framesPerSecond;
            this.NumberOfFramesPerRow = numberOfFramesPerRow;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
        }

        public override void UpdateGame(GameTime gameTime)
        {
            if (this.gameManager != null)
            {
                this.SetLocation(this.gameManager.GetSpaceshipBulletStartPositionX(this.bulletIndex) - 6, this.gameManager.GetSpaceshipBulletStartPositionY(this.bulletIndex) - 22);
            }
        }

        public void Reset()
        {
            this.Visible = false;
            this.SetLocation(-100, -100);
        }

        public void Attach(GenesisGameManager genesisGameManager, int shipBulletIndex)
        {
            this.gameManager = genesisGameManager;
            this.bulletIndex = shipBulletIndex;
        }
    }
}
