using Genesis.Common.Enemy;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common.PowerUp
{
    public class PowerUpManager
    {
        private readonly Rectangle screenDimension;

        private readonly PowerUpFactory powerUpFactory;

        private readonly PowerUpStorage powerUpStorage;

        private readonly EnemyManager enemyManager;

        public PowerUpManager(Rectangle screenDimension, PowerUpFactory powerUpFactory, PowerUpStorage powerUpStorage, EnemyManager enemyManager)
        {
            this.screenDimension = screenDimension;
            this.powerUpFactory = powerUpFactory;
            this.powerUpStorage = powerUpStorage;
            this.enemyManager = enemyManager;
        }

        public void AddPowerUp()
        {
            EnemySpaceship enemySpaceship = enemyManager.GetLastEnemyThatWasShot();
            this.powerUpStorage.Clean();
            this.powerUpStorage.Add(this.powerUpFactory.Create(enemySpaceship,screenDimension));
        }

        public void Update(GameTime gameTime)
        {
            this.powerUpStorage.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.powerUpStorage.Draw(spriteBatch);
        }

        public void Load()
        {
            this.powerUpStorage.Load();
        }
    }
}