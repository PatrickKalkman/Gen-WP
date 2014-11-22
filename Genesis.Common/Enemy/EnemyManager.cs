using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common.Enemy
{
    public class EnemyManager
    {
        private readonly Rectangle screenDimension;

        private readonly ITextureManager textureManager;

        private readonly ICurveGenerator curveGenerator;

        private readonly IAngleCalculator angleCalculator;

        private readonly EnemyBulletManager enemyBulletManager;

        private readonly RandomGenerator randomGenerator;

        private readonly List<EnemySpaceship> enemies = new List<EnemySpaceship>();

        private int numberOfEnemies;

        private int currentNumberOfEnemies;

        private double elapsedMilliSeconds;

        private double firstTimeMilliSeconds;

        private double numberOfMilisecondsBetweenEnemies;

        private int numberOfWaves;

        public EnemyManager(Rectangle screenDimension, ITextureManager textureManager, ICurveGenerator curveGenerator, IAngleCalculator angleCalculator, EnemyBulletManager enemyBulletManager, RandomGenerator randomGenerator)
        {
            this.screenDimension = screenDimension;
            this.textureManager = textureManager;
            this.curveGenerator = curveGenerator;
            this.angleCalculator = angleCalculator;
            this.enemyBulletManager = enemyBulletManager;
            this.randomGenerator = randomGenerator;
        }

        public bool Enabled { get; set; }

        public void Update(GameTime gameTime)
        {
            if (this.IsTimeForNewEnemy(gameTime) && Enabled)
            {
                if (this.currentNumberOfEnemies < numberOfEnemies)
                {
                    for (int waveCounter = 0; waveCounter < numberOfWaves; waveCounter++)
                    {
                        var enemySpaceship = GetOrCreateEnemySpaceship(waveCounter);
                        enemySpaceship.Status = SpaceshipStatus.Flying;
                        enemySpaceship.Visible = true;
                        currentNumberOfEnemies++;
                    }
                }
            }

            foreach (EnemySpaceship enemySpaceship in enemies)
            {
                enemySpaceship.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemySpaceship in enemies)
            {
                enemySpaceship.Draw(spriteBatch);
            }
        }

        public bool WaveIsFinished()
        {
            return enemies.Count() != 0 && enemies.Count(e => e.Status == SpaceshipStatus.Flying || e.Status == SpaceshipStatus.Ready || e.Status == SpaceshipStatus.ShotButStillFlying) == 0;
        }

        public IEnumerable<EnemySpaceship> Enemies()
        {
            return enemies;
        }

        public void SetEnemiesReady()
        {
            Enabled = false;
            foreach (EnemySpaceship enemySpaceship in enemies)
            {
                enemySpaceship.Status = SpaceshipStatus.Killed;
                enemySpaceship.ShotTime = 0;
            }

            for (int enemyCounter = 0; enemyCounter < numberOfEnemies; enemyCounter++)
            {
                enemies[enemyCounter].Status = SpaceshipStatus.Reserved;
            }

            currentNumberOfEnemies = 0;
        }

        public void SetOptions(EnemyWaves enemyWaves)
        {
            int waveIndex = 0;
            numberOfEnemies = 0;
            this.numberOfMilisecondsBetweenEnemies = enemyWaves.NumberOfMilliSecondsBetweenEnemies;
            foreach (EnemyWave enemyWave in enemyWaves)
            {
                int enemiesInThisWave = enemyWave.NumberOfEnemies;
                if (enemiesInThisWave == 0) enemiesInThisWave = 1;
                for (int enemyCounter = 0; enemyCounter < enemiesInThisWave; enemyCounter++)
                {
                    EnemySpaceship enemySpaceship = GetOrCreateEnemySpaceship();
                    enemySpaceship.SetOptions(enemyWave, waveIndex);
                    enemySpaceship.Status = SpaceshipStatus.Ready;
                }
                numberOfEnemies += enemiesInThisWave;
                waveIndex++;
            }
            numberOfWaves = waveIndex;
            Enabled = true;
        }

        public bool AreAllShot()
        {
            return enemies.Count() != 0 && enemies.Count(e => e.Status != SpaceshipStatus.Killed && e.Status != SpaceshipStatus.Reserved) == 0;
        }

        public void Reset()
        {
            foreach (EnemySpaceship enemySpaceship in enemies)
            {
                enemySpaceship.Reset();
            }
            
            numberOfEnemies = 0;
        }

        private bool IsTimeForNewEnemy(GameTime gameTime)
        {
            elapsedMilliSeconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if ((elapsedMilliSeconds - this.firstTimeMilliSeconds) > numberOfMilisecondsBetweenEnemies)
            {
                this.firstTimeMilliSeconds = elapsedMilliSeconds;
                return true;
            }

            return false;
        }

        private EnemySpaceship GetOrCreateEnemySpaceship(int waveCounter)
        {
            return this.enemies.First(e => e.Status == SpaceshipStatus.Ready && e.CurveIndex == waveCounter);
        }

        private EnemySpaceship GetOrCreateEnemySpaceship()
        {
            EnemySpaceship enemySpaceship = this.enemies.FirstOrDefault(e => e.Status == SpaceshipStatus.Reserved);
            if (enemySpaceship == null)
            {
                enemySpaceship = new EnemySpaceship(screenDimension, textureManager, curveGenerator, angleCalculator, enemyBulletManager, randomGenerator);
                enemies.Add(enemySpaceship);
            }

            return enemySpaceship;
        }

        public EnemySpaceship GetLastEnemyThatWasShot()
        {
            if (enemies.Count == 0)
            {
                return null;
            }

            EnemySpaceship latestShopEnemySpaceship = enemies[0];

            foreach (EnemySpaceship enemySpaceship in enemies)
            {
                if (enemySpaceship.ShotTime > latestShopEnemySpaceship.ShotTime)
                {
                    latestShopEnemySpaceship = enemySpaceship;
                }
            }

            return latestShopEnemySpaceship;
        }
    }
}
