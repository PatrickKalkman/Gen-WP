using Genesis.Common.Enemy;

using Microsoft.Xna.Framework;

namespace Genesis.Common.PowerUp
{
    public class PowerUpFactory
    {
        private readonly ITextureManager textureManager;

        public PowerUpFactory(ITextureManager textureManager)
        {
            this.textureManager = textureManager;
        }

        public virtual PowerUp Create(EnemySpaceship enemySpaceship, Rectangle screenDimension)
        {
            var powerUp = new PowerUp(screenDimension, textureManager, 27, 27, 31, 30, 2, 3, 3, 5, new Point(572, 817), SpriteAnimationType.Continous);
            powerUp.PowerToAdd = 2;
            powerUp.SetLocation(enemySpaceship);
            powerUp.SetInitialVelocity();
            return powerUp;
        }
    }
}