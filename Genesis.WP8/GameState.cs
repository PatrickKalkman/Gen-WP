using Genesis.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.WP8
{
    public abstract class GameState
    {
        protected readonly GenesisGame genesisGame;

        protected GameState(GenesisGame genesisGame)
        {
            this.genesisGame = genesisGame;
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public abstract void Start(GameDifficulty difficulty);
    }
}