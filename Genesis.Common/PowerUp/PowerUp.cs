using Genesis.Common.Enemy;

using Microsoft.Xna.Framework;

namespace Genesis.Common.PowerUp
{
    public class PowerUp : AnimatedSprite
    {
        public PowerUp(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale)
        {
        }

        public PowerUp(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale, int numberOfFrames, int numberOfFramesPerRow, int framesPerSecond, Point textureSourceStart, SpriteAnimationType typeOfAnimation)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale, numberOfFrames, numberOfFramesPerRow, framesPerSecond, textureSourceStart, typeOfAnimation)
        {
            this.Angle = 0;
            this.RotationOrigin = new Vector2(width / 2.0f, height / 2.0f);
        }

        public PowerUp(Rectangle screenDimension, ITextureManager textureManager, SpriteAnimationType typeOfAnimation)
            : base(screenDimension, textureManager, typeOfAnimation)
        {
        }

        public void Execute(Spaceship spaceship)
        {
            spaceship.Power += this.PowerToAdd;
        }

        public int PowerToAdd { get; set; }

        public override void UpdateGame(GameTime gameTime)
        {
            this.Angle += 0.05f;
            if (this.YPosition > this.screenDimension.Height + this.Height)
            {
                this.Visible = false;
            }
        }

        public void SetLocation(EnemySpaceship spaceship)
        {
            this.SetLocation(spaceship.PreviousXPosition, spaceship.PreviousYposition);
        }

        public void SetInitialVelocity()
        {
            this.SetVelocity(new Vector2(0, 2.5f));
        }
    }
}