using System;

using Genesis.Common;
using Genesis.Common.GameStates;
using Genesis.Common.Score;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.WP8
{
    public partial class GenesisGame
    {
        public class PlayingState : GameState
        {
            private readonly string applicationTitle;

            public PlayingState(GenesisGame genesisGame, string applicationTitle)
                : base(genesisGame)
            {
                this.applicationTitle = applicationTitle;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                genesisGame.backgroundManager.Draw(spriteBatch);
                genesisGame.score.Draw(spriteBatch);
                genesisGame.spaceship.Draw(spriteBatch);
                genesisGame.enemyBulletManager.Draw(spriteBatch);
                genesisGame.enemyManager.Draw(spriteBatch);
                genesisGame.bulletManager.Draw(spriteBatch);
                genesisGame.collisionManager.Draw(spriteBatch);
                genesisGame.powerUpManager.Draw(spriteBatch);
            }

            public override void Update(GameTime gameTime)
            {
                if (genesisGame.spaceship.Status == SpaceshipStatus.Killed)
                {
                    genesisGame.killedFrameCounter--;
                    if (genesisGame.killedFrameCounter <= 0)
                    {
                        ResetGame();
                        EnterHighScore();
                        genesisGame.gameState = new GenesisGameFinishedState(genesisGame, 500, applicationTitle);
                    }
                    else
                    {
                        UpdateGame(gameTime);
                    }
                }
                else
                {
                    UpdateGame(gameTime);
                }
            }

            private void UpdateGame(GameTime gameTime)
            {
                genesisGame.backgroundManager.Scroll();
                genesisGame.spaceship.Update(gameTime);
                genesisGame.bulletManager.Update(gameTime);
                UpdateWaveAndStage();
                genesisGame.enemyWaveManager.Update(gameTime);
                genesisGame.enemyBulletManager.Update(gameTime);
                genesisGame.enemyManager.Update(gameTime);
                genesisGame.collisionManager.Detect();
                genesisGame.collisionManager.Update(gameTime);
                genesisGame.inputManager.GetTouchInput();
            }

            private void UpdateWaveAndStage()
            {
                if (!genesisGame.enemyWaveManager.WaveIsFinished())
                {
                    return;
                }

                if (!genesisGame.enemyWaveManager.StageIsFinished())
                {
                    if (genesisGame.enemyWaveManager.WaveIsCompletelyShot())
                    {
                        genesisGame.enemyWaveManager.ShowPowerUp();
                    }

                    genesisGame.enemyWaveManager.SetEnemiesReady();
                    genesisGame.enemyWaveManager.SetNextWave();
                }
                else
                {
                    genesisGame.currentStage++;
                    if (genesisGame.IsFinished())
                    {
                        ResetGame();
                        EnterHighScore();
                        genesisGame.gameState = new GenesisGameFinishedState(genesisGame, 500, applicationTitle);
                    }
                    else
                    {
                        genesisGame.gameState = new StageCompletedGameState(genesisGame, 50, genesisGame.currentStage, applicationTitle);
                    }
                }
            }

            private void EnterHighScore()
            {
                var data = new ScoreData();
                data.Score = genesisGame.score.PlayerScore;
                data.Level = genesisGame.currentStage;
                if (genesisGame.highScoreManager.IsHighScore(data))
                {
                    genesisGame.GamePageController.ShowHighscoreTextBox();
                }
            }

            private void ResetGame()
            {
                genesisGame.currentStage = 0;
                genesisGame.backgroundManager.SetAndLoadLayers(genesisGame.gameDefinition.Stages[genesisGame.currentStage].BackgroundLayers);
                genesisGame.enemyWaveManager.IntializeStage(genesisGame.gameDefinition.Stages[genesisGame.currentStage]);
                genesisGame.gameManager.RocketsEnabled = false;
            }

            public override void Start(GameDifficulty easy)
            {
                throw new NotImplementedException();
            }
        }
    }
}