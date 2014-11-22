using Microsoft.Xna.Framework;

namespace Genesis.Common.Score
{
    public class ScoreEffect : AnimatedSprite
    {
        private int frameCounter;

        public ScoreEffect(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale)
        {
        }

        public ScoreEffect(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale, int numberOfFrames, int numberOfFramesPerRow, int framesPerSecond, Point textureSourceStart, SpriteAnimationType typeOfAnimation)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale, numberOfFrames, numberOfFramesPerRow, framesPerSecond, textureSourceStart, typeOfAnimation)
        {
        }

        public override void UpdateGame(GameTime gameTime)
        {
            this.frameCounter++;
            if (this.frameCounter > 20)
            {
                int alpha = 255 - (int)((this.frameCounter - 20) / 20.0 * 255.0);
                this.color = new Color(255, 255, 255, alpha);
                this.scale += 0.01f;
                if (this.frameCounter > 40)
                {
                    this.Visible = false;
                    this.frameCounter = 0;
                    this.color = Color.White;
                    this.scale = 1;
                }
            }
        }
    }
}