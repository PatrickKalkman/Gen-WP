using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common.PowerUp
{
    public class PowerUpStorage : List<PowerUp>
    {
        private readonly Spaceship spaceship;

        private readonly ContentManager contentManager;

        private readonly GenesisGameManager genesisGameManager;

        private SoundEffect soundEffect;

        public PowerUpStorage(Spaceship spaceship, ContentManager contentManager, GenesisGameManager genesisGameManager)
        {
            this.spaceship = spaceship;
            this.contentManager = contentManager;
            this.genesisGameManager = genesisGameManager;
        }

        public void Load()
        {
            soundEffect = contentManager.Load<SoundEffect>("creature_roar2");
        }

        public virtual new void Add(PowerUp powerUp)
        {
            base.Add(powerUp);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (PowerUp powerUp in this)
            {
                powerUp.Update(gameTime);
                if (this.spaceship.GetBoundingBox().Intersects(powerUp.GetBoundingBox()))
                {
                    try
                    {
                        soundEffect.Play(0.4f, 0.5f, 0);
                    }
                    catch (InstancePlayLimitException)
                    {
                        //ignore.
                    }
                    powerUp.Execute(spaceship);
                    genesisGameManager.RocketsEnabled = true;
                    powerUp.Visible = false;
                }
            }
        }

        public void Clean()
        {
            List<PowerUp> toRemove = this.Where(p => !p.Visible).ToList();
            foreach (var powerUp in toRemove)
            {
                this.Remove(powerUp);
            }
        }

        public bool PowerUpsAreAvailable
        {
            get
            {
                return this.Count > 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PowerUp powerUp in this)
            {
                powerUp.Draw(spriteBatch);
            }
        }
    }
}