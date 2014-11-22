using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public class MissileSmoke : AnimatedSprite
    {
        public MissileSmoke(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale)
        {
        }

        public MissileSmoke(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale, int numberOfFrames, int numberOfFramesPerRow, int framesPerSecond, Point textureSourceStart, SpriteAnimationType typeOfAnimation)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale, numberOfFrames, numberOfFramesPerRow, framesPerSecond, textureSourceStart, typeOfAnimation)
        {
            this.color = new Color(255, 255, 255, 30);
        }

        public MissileSmoke(Rectangle screenDimension, ITextureManager textureManager, SpriteAnimationType typeOfAnimation)
            : base(screenDimension, textureManager, typeOfAnimation)
        {
        }

        public override void UpdateGame(GameTime gameTime)
        {

        }
    }
}
