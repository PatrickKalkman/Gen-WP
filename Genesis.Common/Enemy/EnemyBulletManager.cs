using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common.Enemy
{
    public class EnemyBulletManager
    {
        private readonly List<Rocket> rocketList = new List<Rocket>();

        private readonly Rectangle clientBounds;

        private readonly ITextureManager textureManager;

        private readonly ContentManager contentManager;

        private readonly RandomGenerator randomGenerator;
         
        private SoundEffect soundEffect;

        public EnemyBulletManager(Rectangle clientBounds, ITextureManager textureManager, ContentManager contentManager, RandomGenerator randomGenerator)
        {
            this.clientBounds = clientBounds;
            this.textureManager = textureManager;
            this.contentManager = contentManager;
            this.randomGenerator = randomGenerator;
        }

        public IEnumerable<Rocket> Rockets
        {
            get
            {
                return this.rocketList;
            }
        }

        public void Load()
        {
            soundEffect = contentManager.Load<SoundEffect>("shot2");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Rocket rocket in this.rocketList)
            {
                rocket.Draw(spriteBatch);
            }
        }

        public void FireRocket(Vector2 enemySpaceshipLocation)
        {
            Rocket rocket = GetRocket();
            rocket.SetLocation(enemySpaceshipLocation.X, enemySpaceshipLocation.Y);
            rocket.SetVelocityY((float)randomGenerator.NextDouble() + 0.65f * 4);
            rocket.Status = RocketStatus.Flying;
            rocket.Visible = true;
            try
            {
                soundEffect.Play(0.25f, 1.0f, 1);
            }
            catch (InstancePlayLimitException)
            {
                //ignore
            }
        }

        private Rocket GetRocket()
        {
            foreach (Rocket rocket in rocketList)
            {
                if (rocket.Status == RocketStatus.Available)
                {
                    return rocket;
                }
            }

            var newRocket = new Rocket(this.clientBounds, this.textureManager, 18, 48, 20, 50);
            this.rocketList.Add(newRocket);
            return newRocket;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Rocket rocket in this.rocketList)
            {
                rocket.Update(gameTime);
            }
        }

        public void Reset()
        {
            foreach (Rocket rocket in Rockets)
            {
                rocket.Reset();
            }
        }
    }
}