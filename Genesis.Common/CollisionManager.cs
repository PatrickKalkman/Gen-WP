using System.Collections.Generic;
using System.Linq;

using Genesis.Common.Enemy;
using Genesis.Common.Score;
using Genesis.WP8;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common
{
    public class CollisionManager
    {
        private readonly BulletManager bulletManager;

        private readonly EnemyBulletManager enemyBulletManager;

        private readonly EnemyManager enemyManager;

        private readonly Rectangle screenDimension;

        private readonly ITextureManager textureManager;

        private readonly ContentManager contentManager;

        private readonly Spaceship spaceship;

        private readonly Score.Score score;

        private readonly List<Explosion> explosions = new List<Explosion>();

        private readonly List<ScoreEffect> scoreEffects = new List<ScoreEffect>(); 

        private readonly Explosion spaceshipExplosion;

        private SoundEffect soundEffect;

        private SoundEffect killedSoundEffect;

        public CollisionManager(
            BulletManager bulletManager, 
            EnemyBulletManager enemyBulletManager,
            EnemyManager enemyManager, 
            Rectangle screenDimension, 
            ITextureManager textureManager, 
            ContentManager contentManager, 
            Spaceship spaceship,
            Score.Score score)
        {
            this.bulletManager = bulletManager;
            this.enemyBulletManager = enemyBulletManager;
            this.enemyManager = enemyManager;
            this.screenDimension = screenDimension;
            this.textureManager = textureManager;
            this.contentManager = contentManager;
            this.spaceship = spaceship;
            this.score = score;
            this.spaceshipExplosion = new Explosion(screenDimension, textureManager, 90, 98, 95, 101, new Point(1, 405), SpriteAnimationType.Continous, 7, 4, 8);
            this.spaceshipExplosion.SetLocation(-100, -100);
            this.spaceshipExplosion.Visible = false;
        }

        public void Load()
        {
            soundEffect = contentManager.Load<SoundEffect>("Explosion3");
            killedSoundEffect = contentManager.Load<SoundEffect>("kablooie");
        }

        public void Detect()
        {
            this.DetectEnemyWithBulletCollisions();
            this.DetectSpaceshipWithRocketCollisions();
            this.DetectEnemyWithRocketCollision();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gameTime);
            }

            foreach (ScoreEffect scoreEffect in scoreEffects)
            {
                scoreEffect.Update(gameTime);
            }

            spaceshipExplosion.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Explosion explosion in explosions)
            {
                explosion.Draw(spriteBatch);
            }

            foreach (ScoreEffect scoreEffect in scoreEffects)
            {
                scoreEffect.Draw(spriteBatch);
            }

            spaceshipExplosion.Draw(spriteBatch);
        }

        public void Reset()
        {
            this.spaceshipExplosion.SetLocation(-100, -100);
            this.spaceshipExplosion.Visible = false;

            foreach (Explosion explosion in explosions)
            {
                explosion.Reset();
            }
        }

        private void DetectSpaceshipWithRocketCollisions()
        {
            foreach (Rocket rocket in enemyBulletManager.Rockets.Where(r => r.Status == RocketStatus.Flying))
            {
                if (rocket.GetBoundingBox().Intersects(spaceship.GetBoundingBox()))
                {
                    Explosion smallExplosion = this.GetAvailableExplosion();
                    smallExplosion.SetOptions(new Point(97, 766), 29, 29, 32, 32, 16, 8, 4);
                    smallExplosion.SetLocation(rocket.XPosition + (int)(rocket.Width / 2), rocket.YPosition + rocket.Height);
                    rocket.Status = RocketStatus.Exploded;
                    rocket.Visible = false;
                    rocket.SetLocation(-100, -100);
                    smallExplosion.Visible = true;
                    spaceship.Shot();
                    if (spaceship.IsShot())
                    {
                        try
                        {
                            this.killedSoundEffect.Play(1, 1, 1);
                        }
                        catch (InstancePlayLimitException)
                        {
                            //ignore
                        }

                        spaceshipExplosion.SetLocation(spaceship.XPosition, spaceship.YPosition);
                        spaceshipExplosion.Visible = true;
                        spaceship.Status = SpaceshipStatus.Killed;
                        spaceship.XVelocity = 0;
                        spaceship.YVelocity = 0;
                    }
                }
            }
        }

        private void DetectEnemyWithBulletCollisions()
        {
            foreach (Bullet bullet in this.bulletManager.Bullets())
            {
                foreach (EnemySpaceship enemySpaceship in this.enemyManager.Enemies())
                {
                    if (bullet.Visible && enemySpaceship.Visible)
                    {
                        if (bullet.GetBoundingBox().Intersects(enemySpaceship.GetBoundingBox()))
                        {
                            enemySpaceship.AddShot(1);

                            if (enemySpaceship.IsShot())
                            {
                                Explosion explosion = this.GetAvailableExplosion();
                                ScoreEffect scoreEffect = this.GetAvailableScoreEffect();
                                scoreEffect.SetLocation(enemySpaceship.Center);
                                scoreEffect.SetVelocity(new Vector2(0.6f, -1.3f));
                                scoreEffect.Visible = true;
                                try
                                {
                                    this.soundEffect.Play(1, 1f, 0);
                                }
                                catch (InstancePlayLimitException)
                                {
                                    //ignore
                                }
                                explosion.SetOptions(new Point(0, 202), 94, 100, 95, 101, 16, 8, 4);
                                explosion.SetLocation(enemySpaceship.Center);
                                explosion.Visible = true;
                                score.IncreaseScore(150);
                                enemySpaceship.Killed();
                            }
                            else
                            {
                                Explosion smallExplosion = this.GetAvailableExplosion();
                                smallExplosion.SetOptions(new Point(97, 766), 29, 29, 32, 32, 16, 8, 4);
                                smallExplosion.SetLocation(enemySpaceship.XPosition, enemySpaceship.YPosition);
                                smallExplosion.Visible = true;
                                score.IncreaseScore(20);
                                enemySpaceship.Shot();
                            }

                            bullet.Visible = false;
                            bullet.SetLocation(-100, -100);
                        }
                    }
                }
            }
        }

        private void DetectEnemyWithRocketCollision()
        {
            foreach (HomingMissle homingMissle in bulletManager.Rockets())
            {
                foreach (EnemySpaceship enemySpaceship in this.enemyManager.Enemies())
                {
                    if (enemySpaceship.Visible && homingMissle.Visible)
                    {
                        if (homingMissle.GetBoundingBox().Intersects(enemySpaceship.GetBoundingBox()))
                        {
                            enemySpaceship.AddShot(5);

                            if (enemySpaceship.IsShot())
                            {
                                Explosion explosion = this.GetAvailableExplosion();
                                ScoreEffect scoreEffect = this.GetAvailableScoreEffect();
                                scoreEffect.SetLocation(enemySpaceship.Center);
                                scoreEffect.SetVelocity(new Vector2(0.6f, -1.3f));
                                scoreEffect.Visible = true;
                                try
                                {
                                    this.soundEffect.Play(1, 1f, 0);
                                }
                                catch (InstancePlayLimitException)
                                {
                                    //ignore.
                                }
                                explosion.SetOptions(new Point(1, 203), 93, 99, 95, 101, 16, 8, 4);
                                explosion.SetLocation(enemySpaceship.Center);
                                explosion.Visible = true;
                                score.IncreaseScore(150);
                                enemySpaceship.Killed();
                            }
                            else
                            {
                                Explosion explosion = this.GetAvailableExplosion();
                                explosion.SetOptions(new Point(1, 203), 93, 99, 95, 101, 16, 8, 4);
                                explosion.SetLocation(enemySpaceship.XPosition, enemySpaceship.YPosition);
                                explosion.Visible = true;
                                score.IncreaseScore(20);
                                enemySpaceship.Shot();                                
                            }

                            homingMissle.Visible = false;
                            homingMissle.SetLocation(-100, -100);
                        }
                    }
                }
            }
        }

        private Explosion GetAvailableExplosion()
        {
            Explosion smallExplosion = this.explosions.FirstOrDefault(explosion => !explosion.Visible);
            if (smallExplosion == null)
            {
                smallExplosion = new Explosion(this.screenDimension, this.textureManager, 94, 100, 95, 100, new Point(0, 202), SpriteAnimationType.SingleShot, 8,  4, 8);
                this.explosions.Add(smallExplosion);
            }

            return smallExplosion;
        }

        private ScoreEffect GetAvailableScoreEffect()
        {
            ScoreEffect scoreEffect = this.scoreEffects.FirstOrDefault(se => !se.Visible);
            if (scoreEffect == null)
            {
                scoreEffect = new ScoreEffect(screenDimension, textureManager, 75, 39, 75, 39, 1, 1, 1, 1, new Point(0, 846), SpriteAnimationType.NoAnimation);
                this.scoreEffects.Add(scoreEffect);
            }

            return scoreEffect;
        }
    }
}
