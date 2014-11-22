using System;

using Microsoft.Xna.Framework;

namespace Genesis.Common.Enemy
{
    public class EnemySpaceship : AnimatedSprite
    {
        private readonly ICurveGenerator curveGenerator;

        private readonly IAngleCalculator angleCalculator;

        private readonly EnemyBulletManager enemyBulletManager;

        private readonly RandomGenerator randomGenerator;

        private float bezierCurveCounter;

        private int power;

        public EnemySpaceship(
            Rectangle screenDimension, 
            ITextureManager textureManager, 
            ICurveGenerator curveGenerator, 
            IAngleCalculator angleCalculator, 
            EnemyBulletManager enemyBulletManager,
            RandomGenerator randomGenerator)
            : base(screenDimension, textureManager, SpriteAnimationType.Continous)
        {
            this.curveGenerator = curveGenerator;
            this.angleCalculator = angleCalculator;
            this.enemyBulletManager = enemyBulletManager;
            this.randomGenerator = randomGenerator;
        }

        public SpaceshipStatus Status { get; set; }

        public int CurveIndex { get; set; }

        public int MovementSpeed { get; set; }

        public bool Fired { get; set; }

        public long ShotTime { get; set; }

        public bool IsBoss { get; set; }

        public float PreviousXPosition
        {
            get
            {
                return previousLocation.X;
            }
        }

        public float PreviousYposition
        {
            get
            {
                return previousLocation.Y;
            }
        }

        public bool Attached { get; set; }

        public override void UpdateGame(GameTime gameTime)
        {
            if (this.Status == SpaceshipStatus.Flying || this.Status == SpaceshipStatus.ShotButStillFlying)
            {
                Vector2 newLocation = this.curveGenerator.GetPoint(CurveIndex, this.bezierCurveCounter);

                this.Angle = this.angleCalculator.Calculate(newLocation, this.previousLocation) + 55;

                this.SetLocation(newLocation.X, newLocation.Y);

                this.FireRocketIfNeeded();

                this.UpdateCurveCounter();

                this.previousLocation = newLocation;
            }
        }

        private void FireRocketIfNeeded()
        {
            if (!Fired)
            {
                if (IsNear())
                {
                    if (this.randomGenerator.NextDouble() * 1000 > 980)
                    {
                        this.enemyBulletManager.FireRocket(this.location);
                        Fired = true;
                    }
                }
            }
        }

        private bool IsNear()
        {
            if (this.Location.X > 20 && this.location.X < screenDimension.Width - 20
                && this.Location.Y > 20 && this.Location.Y < screenDimension.Height - 20)
            {
                return true;
            }
            return false;
        }

        public void SetOptions(EnemyWave enemyWave, int curveIndex)
        {
            this.IsBoss = false;
            this.TextureSourceStart = new Point(enemyWave.StartSpriteX, enemyWave.StartSpriteY);
            this.Width = enemyWave.Width;
            this.Height = enemyWave.Height;
            this.FrameWidth = enemyWave.FrameWidth;
            this.FrameHeight = enemyWave.FrameHeight;
            this.FramesPerSecond = enemyWave.FramesPerSecond;
            this.NumberOfFrames = enemyWave.NumberOfFrames;
            this.NumberOfFramesPerRow = enemyWave.FramesPerRow;
            this.power = enemyWave.Strength;
            this.RotationOrigin = new Vector2(this.Width / 2, Height / 2);
            this.CurveIndex = curveIndex;
            this.Status = SpaceshipStatus.Ready;
            this.MovementSpeed = enemyWave.MovementSpeed;
            this.scale = 1;
            this.Fired = false;
            if (enemyWave is BossWave)
            {
                this.IsBoss = true;
            }
        }

        public override Rectangle GetBoundingBox()
        {
            return new Rectangle((int)(location.X - (scale * Width * 0.5)), (int)(location.Y - (scale * Height * 0.5)), (int)(Width * scale), (int)(Height * scale));
        }

        public void Shot()
        {
            this.Status = SpaceshipStatus.ShotButStillFlying;
        }

        public void Killed()
        {
            this.bezierCurveCounter = 0;
            this.Visible = false;
            this.Status = SpaceshipStatus.Killed;
            this.ShotTime = DateTime.Now.Ticks;
            this.SetLocation(-100, -100);
        }

        private void UpdateCurveCounter()
        {
            float bezierCurveStep = 1.0f / this.MovementSpeed;

            this.bezierCurveCounter = this.bezierCurveCounter + bezierCurveStep;

            if (this.bezierCurveCounter >= this.curveGenerator.GetMaxTime())
            {
                this.bezierCurveCounter = 0;
                if (!this.IsBoss)
                {
                    this.Visible = false;
                    this.Status = SpaceshipStatus.Finished;
                }
            }
        }

        public void AddShot(int hitPower)
        {
            power -= hitPower;
        }

        public bool IsShot()
        {
            return power <= 0;
        }

        public void Reset()
        {
            this.bezierCurveCounter = 0;
            this.Visible = false;
            this.Status = SpaceshipStatus.Finished;
        }

        public void SetPreviousLocation(int x, int y)
        {
            previousLocation = new Vector2(x, y);
        }
    }
}
