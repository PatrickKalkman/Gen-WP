using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public class Rocket : AnimatedSprite
    {
        private const float RocketYVelocity = -3.4f;

        public Rocket(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, 1, 4, 1, 8, new Point(877, 2), SpriteAnimationType.Continous)
        {
            this.SetVelocityY(-RocketYVelocity);
            this.Visible = true;
        }

        public RocketStatus Status { get; set; }

        public override void UpdateGame(GameTime gameTime)
        {
            if (location.Y < 0)
            {
                this.Status = RocketStatus.Available;
                this.location = new Vector2(-100, -100);
                this.Visible = false;
            }
        }

        public void Reset()
        {
            this.Status = RocketStatus.Available;
            this.location = new Vector2(-100, -100);
            this.Visible = false;
        }
    }
}