using System;

using Genesis.Common;
using Genesis.Common.GameStates;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.WP8
{
    public partial class GenesisGame
    {
        public class StageCompletedGameState : GameState
        {
            private int secondsDelayBeforeStart;
            private readonly int stageThatWasCompleted;
            private readonly string applicationTitle;

            public StageCompletedGameState(GenesisGame genesisGame, int secondsDelayBeforeStart, int stageThatWasCompleted, string applicationTitle)
                : base(genesisGame)
            {
                this.secondsDelayBeforeStart = secondsDelayBeforeStart;
                this.stageThatWasCompleted = stageThatWasCompleted;
                this.applicationTitle = applicationTitle;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                genesisGame.backgroundManager.Draw(spriteBatch);
                genesisGame.stageCompletedLayer.Draw(spriteBatch, stageThatWasCompleted);
            }

            public override void Update(GameTime gameTime)
            {
                if (secondsDelayBeforeStart <= 0 && genesisGame.inputManager.ShouldStart())
                {
                    genesisGame.gameState = new PlayingState(genesisGame, applicationTitle);
                    genesisGame.backgroundManager.SetAndLoadLayers(genesisGame.gameDefinition.Stages[genesisGame.currentStage].BackgroundLayers);
                    genesisGame.enemyWaveManager.IntializeStage(genesisGame.gameDefinition.Stages[genesisGame.currentStage]);
                }

                if (secondsDelayBeforeStart >= 0)
                {
                    secondsDelayBeforeStart--; 
                }

                genesisGame.backgroundManager.Scroll();
                genesisGame.inputManager.GetTouchInput();
            }

            public override void Start(GameDifficulty easy)
            {
                throw new NotImplementedException();
            }

        }
    }
}
