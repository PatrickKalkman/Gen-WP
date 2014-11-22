using Genesis.Common;
using Genesis.Common.GameStates;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.WP8
{
    public partial class GenesisGame
    {
        public class TitleScreenState : GameState
        {
            private int secondsDelayBeforeStart;
            private readonly string applicationTitle;
            private bool shouldStart;

            public TitleScreenState(GenesisGame genesisGame, int secondsDelayBeforeStart, string applicationTitle)
                : base(genesisGame)
            {
                this.secondsDelayBeforeStart = secondsDelayBeforeStart;
                this.applicationTitle = applicationTitle;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                genesisGame.backgroundManager.Draw(spriteBatch);
                genesisGame.highScoreLayer.Draw(spriteBatch, applicationTitle);
            }
            
            public override void Update(GameTime gameTime)
            {
                if (secondsDelayBeforeStart <= 0 && shouldStart)
                {
                    genesisGame.GamePageController.HidePrivacyMenuBar();
                    genesisGame.gameState = new PlayingState(genesisGame, applicationTitle);
                    genesisGame.spaceship.Reset();
                    genesisGame.bulletManager.Reset();
                    genesisGame.enemyBulletManager.Reset();
                    genesisGame.collisionManager.Reset();
                    genesisGame.score.Reset();
                    genesisGame.killedFrameCounter = 300;
                    genesisGame.enemyManager.SetEnemiesReady();
                    genesisGame.enemyManager.Reset();
                    genesisGame.enemyWaveManager.Reset();
                }

                if (secondsDelayBeforeStart >= 0)
                {
                    secondsDelayBeforeStart--; 
                }

                genesisGame.backgroundManager.Scroll();
                genesisGame.inputManager.GetTouchInput();
            }

            public override void Start(GameDifficulty difficulty)
            {
                genesisGame.enemyWaveManager.SetDifficulty(difficulty);
                shouldStart = true;

            }
        }
    }
}