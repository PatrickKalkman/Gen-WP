using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    /// <summary>
    /// This class represents a spaceship, the object the user controls in the game.
    /// </summary>
    public class Spaceship : AnimatedSprite
    {
        private const float SpaceshipYVelocity = 2.0f;

        private const float SpaceshipXVelocity = 2.0f;

        private const int MaximumPower = 4;

        private readonly IInputManager inputManager;

        public Spaceship(Rectangle screenDimension, IInputManager inputManager, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight) :
            base(screenDimension, textureManager, width, height, frameWidth, frameHeight, 1, 4, 4, 10, new Point(1, 1), SpriteAnimationType.Continous)
        {
            this.inputManager = inputManager;
            this.SetLocation(screenDimension.Width / 2 - this.Width / 2, screenDimension.Height - this.Height);
            this.SetVelocity(Vector2.Zero);
            this.Power = 3;
        }

        public SpaceshipStatus Status { get; set; }

        public override void UpdateGame(GameTime gameTime)
        {
            if (this.Status != SpaceshipStatus.Killed)
            {
                this.HandleInputManagement();
            }
        }

        private int power;

        public int Power
        {
            get
            {
                return power;
            }
            set
            {
                power = value;
                if (value > MaximumPower)
                {
                    power = MaximumPower;
                }
            }
        }

        public void Shot()
        {
            this.Power--;
        }

        public bool IsShot()
        {
            return this.Power <= 0;
        }

        public void Reset()
        {
            this.Power = 3;
            this.Status = SpaceshipStatus.Flying;
            this.SetLocation(screenDimension.Width / 2 - this.Width / 2, screenDimension.Height - this.Height);
            this.SetVelocity(Vector2.Zero);
        }

        private void HandleInputManagement()
        {
            if (this.inputManager.ShouldMoveUp())
            {
                this.SetVelocityY(-SpaceshipYVelocity);
            }

            if (this.inputManager.ShouldMoveDown())
            {
                this.SetVelocityY(SpaceshipYVelocity);
            }

            if (this.inputManager.ShouldMoveLeft())
            {
                this.SetVelocityX(-SpaceshipXVelocity);
            }

            if (this.inputManager.ShouldMoveRight())
            {
                this.SetVelocityX(SpaceshipXVelocity);
            }

            if (this.inputManager.NoInput())
            {
                this.SetVelocity(Vector2.Zero);
            }

            if (this.inputManager.DragAvailable)
            {
                this.SetLocation(location.X + (this.inputManager.Delta.X * 3), location.Y + (this.inputManager.Delta.Y * 3));
            }
        }
    }
}