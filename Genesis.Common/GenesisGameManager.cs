using System.Linq;

using Genesis.Common.Enemy;

using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public class GenesisGameManager
    {
        private readonly EnemyManager enemyManager;

        private readonly float[] bulletXPositions  = new float[2];

        private readonly float[] bulletYPositions = new float[2];

        public GenesisGameManager(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
            bulletXPositions[0] = 15.0f;
            bulletXPositions[1] = 77.0f;

            bulletYPositions[0] = 39.0f;
            bulletYPositions[1] = 39.0f;
        }

        public Spaceship Spaceship { get; set; }

        public SpaceshipStatus Status
        {
            get
            {
                return Spaceship.Status;
            }
        }

        public bool RocketsEnabled { get; set; }

        public virtual float GetSpaceshipBulletStartPositionX(int index)
        {
            return Spaceship.XPosition + bulletXPositions[index];
        }

        public virtual float GetSpaceshipBulletStartPositionY(int index)
        {
            return Spaceship.YPosition + bulletYPositions[index];
        }

        public float GetSpaceshipRocketStartPositionX(int index)
        {
            return Spaceship.XPosition + bulletXPositions[index];
        }

        public float GetSpaceshipRocketStartPositionY(int index)
        {
            return Spaceship.YPosition + bulletYPositions[index];
        }

        public EnemySpaceship GetNearestEnemy(Vector2 location)
        {
            float smallestDistance = float.MaxValue;
            EnemySpaceship nearestEnemy = null;

            foreach (var enemy in enemyManager.Enemies().Where(enemy => !enemy.Attached && enemy.Visible && (enemy.Status == SpaceshipStatus.Flying || enemy.Status == SpaceshipStatus.ShotButStillFlying)))
            {
                float distance = Vector2.Distance(enemy.Location, location);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null)
            {
                nearestEnemy.Attached = true;
            }

            return nearestEnemy;
        }
    }
}