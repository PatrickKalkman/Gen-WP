using System;

using Genesis.Common.Enemy;
using Genesis.WP8;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common
{
    public class HomingMissle : AnimatedSprite
    {
        private readonly IAngleCalculator angleCalculator;

        private readonly GenesisGameManager genesisGameManager;

        private readonly int index;

        private EnemySpaceship enemySpaceship;

        private readonly Random randomizer = new Random();

        private readonly FixedSizedQueue<MissileSmoke> missileSmokeQueue = new FixedSizedQueue<MissileSmoke>(30);

        public HomingMissle(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale)
        {
        }

        public HomingMissle(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale, int numberOfFrames, int numberOfFramesPerRow, int framesPerSecond, Point textureSourceStart, SpriteAnimationType typeOfAnimation, IAngleCalculator angleCalculator, GenesisGameManager genesisGameManager, int index)
            : base(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale, numberOfFrames, numberOfFramesPerRow, framesPerSecond, textureSourceStart, typeOfAnimation)
        {
            this.angleCalculator = angleCalculator;
            this.genesisGameManager = genesisGameManager;
            this.index = index;
        }

        public HomingMissle(Rectangle screenDimension, ITextureManager textureManager, SpriteAnimationType typeOfAnimation)
            : base(screenDimension, textureManager, typeOfAnimation)
        {
        }

        public override void UpdateGame(GameTime gameTime)
        {
            this.YVelocity = -5f;

            if (enemySpaceship != null && enemySpaceship.Visible && enemySpaceship.Status != SpaceshipStatus.Killed) 
            {
                this.Angle = angleCalculator.Calculate(this.PreviousLocation, this.Location) + 55;
                float xVelocity = ((enemySpaceship.Location.X - this.Location.X) / 40);
                this.XVelocity = xVelocity;
            }

            if (enemySpaceship != null && !enemySpaceship.Visible)
            {
                this.enemySpaceship.Attached = false;
            }

            if (enemySpaceship == null)
            {
                this.enemySpaceship = genesisGameManager.GetNearestEnemy(this.Location);
                missileSmokeQueue.Clear();
            }

            var smoke = new MissileSmoke(screenDimension, textureManager, 83, 75, 83, 76, 0.5f, 1, 1, 1, new Point(82, 845), SpriteAnimationType.None);
            smoke.SetLocation(this.Location.X + randomizer.Next(10) - 5 - Width / 2, this.Location.Y + this.Height - 10 + randomizer.Next(10));
            smoke.Angle = this.Angle;
            missileSmokeQueue.Enqueue(smoke);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            DrawSmoke(spriteBatch);
        }

        private void DrawSmoke(SpriteBatch spriteBatch)
        {
            foreach (MissileSmoke missleSmoke in missileSmokeQueue.Queue)
            {
                missleSmoke.Draw(spriteBatch);
            }
        }

        public void Attach(EnemySpaceship enemy)
        {
            this.enemySpaceship = enemy;
        }

        public void SetStartRocketPosition()
        {
            this.SetLocation(
                this.genesisGameManager.GetSpaceshipRocketStartPositionX(index) - (int)(this.Width / 2),
                this.genesisGameManager.GetSpaceshipRocketStartPositionY(index) - (int)(this.Height / 2));
        }
    }
}
