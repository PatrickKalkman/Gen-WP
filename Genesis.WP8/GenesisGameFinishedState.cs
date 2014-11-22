using System;

using Genesis.Common;
using Genesis.Common.GameStates;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.WP8
{
    public partial class GenesisGame
    {
        public class GenesisGameFinishedState : GameState
        {
            private int secondsDelayBeforeStart;
            private readonly string applicationTitle;

            public GenesisGameFinishedState(GenesisGame genesisGame, int secondsDelayBeforeStart, string applicationTitle)
                : base(genesisGame)
            {
                this.secondsDelayBeforeStart = secondsDelayBeforeStart;
                this.applicationTitle = applicationTitle;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                genesisGame.backgroundManager.Draw(spriteBatch);
                genesisGame.congratulations.Draw(spriteBatch);
            }

            public override void Update(GameTime gameTime)
            {
                if (secondsDelayBeforeStart <= 0 && !genesisGame.GamePageController.HighScoreTextBoxIsVisible())
                {
                    genesisGame.GamePageController.ShowPrivacyMenuBar();
                    genesisGame.gameState = new TitleScreenState(genesisGame, 10, applicationTitle);
                }

                genesisGame.backgroundManager.Scroll();
                genesisGame.inputManager.GetTouchInput();

                if (secondsDelayBeforeStart >= 0)
                {
                    secondsDelayBeforeStart--;
                }
            }

            public override void Start(GameDifficulty easy)
            {
                throw new NotImplementedException();
            }
        }
    }
}