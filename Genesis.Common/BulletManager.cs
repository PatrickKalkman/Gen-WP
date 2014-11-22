using Genesis.Common;
using Genesis.Common.Enemy;

namespace Genesis.WP8
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class BulletManager
    {
        private const double NumberOfMilisecondsBetweenBullets = 260;

        private const double NumberOfMilisecondsBetweenRockets = 1200;

        private readonly List<Bullet> bulletList = new List<Bullet>();

        private readonly List<HomingMissle> homingRockets = new List<HomingMissle>(); 

        private readonly List<Explosion> explosions = new List<Explosion>(); 

        private readonly Rectangle clientBounds;

        private readonly ITextureManager textureManager;

        private readonly GenesisGameManager gameManager;

        private readonly int numberOfBullets;

        private readonly ContentManager contentManager;

        private readonly IInputManager inputManager;

        private readonly IAngleCalculator angleCalculator;

        private int currentNumberOfBullets;

        private int currentNumberOfRockets;

        private double firstTimeMilliSecondsRocket;

        private double elapsedMilliSecondsRocket;
        
        private double firstTimeMilliSeconds;

        private double elapsedMilliSeconds;

        private SoundEffect soundEffect;

        private SoundEffect missleSoundEffect;

        private bool drawBullets = true;
        
        public BulletManager(
            Rectangle clientBounds, 
            ITextureManager textureManager, 
            GenesisGameManager gameManager, 
            int numberOfBullets, 
            ContentManager contentManager, 
            IInputManager inputManager, 
            IAngleCalculator angleCalculator)
        {
            this.clientBounds = clientBounds;
            this.textureManager = textureManager;
            this.gameManager = gameManager;
            this.numberOfBullets = numberOfBullets;
            this.contentManager = contentManager;
            this.inputManager = inputManager;
            this.angleCalculator = angleCalculator;
        }

        public void Load()
        {
            soundEffect = contentManager.Load<SoundEffect>("missile3");
            missleSoundEffect = contentManager.Load<SoundEffect>("rocket_launcher");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.drawBullets)
            {
                foreach (var bullet in this.bulletList)
                {
                    bullet.Draw(spriteBatch);
                }

                foreach (Explosion explosion in explosions)
                {
                    explosion.Draw(spriteBatch);
                }

                foreach (HomingMissle homingRocket in homingRockets)
                {
                    homingRocket.Draw(spriteBatch);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (gameManager.Status == SpaceshipStatus.Killed)
            {
                return;
            }

            if (inputManager.HasBulletToggleButtonBeenPress())
            {
                drawBullets = !drawBullets;
                ResetAllAvailableBullets();
            }

            if (drawBullets)
            {
                if (this.IsTimeForNewBullets(gameTime))
                {
                    if (this.currentNumberOfBullets < this.numberOfBullets)
                    {
                        this.currentNumberOfBullets += 2;
                        var leftBullet = new Bullet(this.clientBounds, this.textureManager, this.gameManager, 0, 10, 30);
                        var rightBullet = new Bullet(this.clientBounds, this.textureManager, this.gameManager, 1, 10, 30);

                        this.bulletList.Add(leftBullet);
                        this.bulletList.Add(rightBullet);

                        SoundEffectInstance instance = soundEffect.CreateInstance();
                        instance.Volume = 0.25f;
                        instance.Pitch = 1.0f;
                        try
                        {
                            instance.Play();
                        }
                        catch (InstancePlayLimitException)
                        {
                            //ignore
                        }
                    }
                    else
                    {
                        Bullet bullet1 = this.ResetAvailableBullet();
                        Bullet bullet2 = this.ResetAvailableBullet();
                        if (bullet1 != null || bullet2 != null)
                        {
                            SoundEffectInstance instance = soundEffect.CreateInstance();
                            instance.Volume = 0.25f;
                            instance.Pitch = 1.0f;
                            try
                            {
                                instance.Play();
                            }
                            catch (InstancePlayLimitException)
                            {
                                //ignore
                            }
                        }
                    }


                    Explosion explosion1 = GetOrCreateExplosion();
                    Explosion explosion2 = GetOrCreateExplosion();
                    explosion1.SetLocation(this.gameManager.GetSpaceshipBulletStartPositionX(0) - 7, this.gameManager.GetSpaceshipBulletStartPositionY(0) - 22);
                    explosion1.Attach(gameManager, 0);
                    explosion2.SetLocation(this.gameManager.GetSpaceshipBulletStartPositionX(1) - 7, this.gameManager.GetSpaceshipBulletStartPositionY(1) - 22);
                    explosion2.Attach(gameManager, 1);
                }

                foreach (Bullet bullet in this.bulletList)
                {
                    bullet.Update(gameTime);
                }

                foreach (Explosion explosion in explosions)
                {
                    explosion.Update(gameTime);
                }

                if (gameManager.RocketsEnabled)
                {
                    this.CreateRocket(gameTime);
                }

                foreach (HomingMissle homingRocket in homingRockets)
                {
                    homingRocket.Update(gameTime);
                }

            }
        }

        private void CreateRocket(GameTime gameTime)
        {
            if (this.IsTimeForNewRocket(gameTime))
            {
                if (this.currentNumberOfRockets < 2)
                {
                    this.currentNumberOfRockets += 2;
                    var leftRocket = new HomingMissle(this.clientBounds,this.textureManager,18,45,19,49,1,4,1,15,new Point(856, 233),SpriteAnimationType.Continous,this.angleCalculator,this.gameManager,0);
                    leftRocket.SetStartRocketPosition();
                    var rightRocket = new HomingMissle(this.clientBounds,this.textureManager,18,45,19,49,1,4,1,15,new Point(856, 233),SpriteAnimationType.Continous,this.angleCalculator,this.gameManager,1);
                    rightRocket.SetStartRocketPosition();

                    EnemySpaceship leftEnemySpaceship = this.gameManager.GetNearestEnemy(leftRocket.Location);
                    leftRocket.Attach(leftEnemySpaceship);

                    EnemySpaceship rightEnemySpaceship = this.gameManager.GetNearestEnemy(rightRocket.Location);
                    rightRocket.Attach(rightEnemySpaceship);

                    this.homingRockets.Add(leftRocket);
                    this.homingRockets.Add(rightRocket);

                    SoundEffectInstance instance = this.missleSoundEffect.CreateInstance();
                    try
                    {
                        instance.Play();
                    }
                    catch (InstancePlayLimitException)
                    {
                        //ignore
                    }
                }
                else
                {
                    HomingMissle rocket1 = this.ResetAvailableRocket();
                    HomingMissle rocket2 = this.ResetAvailableRocket();

                    if (rocket1 != null)
                    {
                        EnemySpaceship leftEnemySpaceship = this.gameManager.GetNearestEnemy(rocket1.Location);
                        rocket1.Attach(leftEnemySpaceship);
                    }

                    if (rocket2 != null)
                    {
                        EnemySpaceship rightEnemySpaceship = this.gameManager.GetNearestEnemy(rocket2.Location);
                        rocket2.Attach(rightEnemySpaceship);
                    }

                    if (rocket1 != null || rocket2 != null)
                    {
                        SoundEffectInstance instance = this.missleSoundEffect.CreateInstance();
                        try
                        {
                            instance.Play();
                        }
                        catch (InstancePlayLimitException)
                        {
                            //ignore
                        }
                    }
                }
            }
        }

        private bool IsTimeForNewRocket(GameTime gameTime)
        {
            elapsedMilliSecondsRocket += gameTime.ElapsedGameTime.TotalMilliseconds;

            if ((elapsedMilliSecondsRocket - this.firstTimeMilliSecondsRocket) > NumberOfMilisecondsBetweenRockets)
            {
                this.firstTimeMilliSecondsRocket = elapsedMilliSecondsRocket;
                return true;
            }

            return false;
        }

        public IEnumerable<Bullet> Bullets()
        {
            return this.bulletList;
        }

        public IEnumerable<HomingMissle> Rockets()
        {
            return this.homingRockets;
        }

        public void Reset()
        {
            ResetAllAvailableBullets();
        }

        private Explosion GetOrCreateExplosion()
        {
            foreach (Explosion explosion in explosions)
            {
                if (!explosion.Visible)
                {
                    explosion.Visible = true;
                    return explosion;
                }
            }

            var bulletLaunchExplosion = new Explosion(clientBounds, textureManager, 14, 30, 17, 32, new Point(298, 766), SpriteAnimationType.SingleShot, 4, 4, 20);
            explosions.Add(bulletLaunchExplosion);
            return bulletLaunchExplosion;
        }

        private void ResetAllAvailableBullets()
        {
            foreach (Bullet bullet in bulletList)
            {
                bullet.SetLocation(-100, -100);
                bullet.Visible = true;
            }
        }

        private Bullet ResetAvailableBullet()
        {
            Bullet bullet = this.FindAvailableBullet();
            if (bullet != null)
            {
                bullet.SetStartBulletPosition();
                bullet.Visible = true;
            }

            return bullet;
        }

        private HomingMissle ResetAvailableRocket()
        {
            HomingMissle rocket = this.FindAvailableRocket();
            if (rocket != null)
            {
                rocket.SetStartRocketPosition();
                rocket.Visible = true;
            }

            return rocket;
        }


        private Bullet FindAvailableBullet()
        {
            return this.bulletList.FirstOrDefault(bullet => bullet.YPosition + 33 < 0);
        }

        private HomingMissle FindAvailableRocket()
        {
            return this.homingRockets.FirstOrDefault(rocket => rocket.YPosition + 33 < 0);
        }


        private bool IsTimeForNewBullets(GameTime gameTime)
        {
            elapsedMilliSeconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if ((elapsedMilliSeconds - this.firstTimeMilliSeconds) > NumberOfMilisecondsBetweenBullets)
            {
                this.firstTimeMilliSeconds = elapsedMilliSeconds;
                return true;
            }

            return false;
        }
    }
}