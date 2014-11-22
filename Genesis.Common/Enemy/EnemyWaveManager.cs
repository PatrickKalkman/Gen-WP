using System;
using System.Collections.Generic;

using Genesis.Common.PowerUp;

using Microsoft.Xna.Framework;

namespace Genesis.Common.Enemy
{
    public class EnemyWaveManager
    {
        private readonly EnemyManager enemyManager;

        private readonly CurveGenerator curveGenerator;

        private readonly PowerUpManager powerUpManager;
        private readonly double scaleX;
        private readonly double scaleY;

        private readonly List<EnemyWaves> enemyWavesCollection = new List<EnemyWaves>();

        private int currentWave;

        private Stage stage;

        public EnemyWaveManager(EnemyManager enemyManager, CurveGenerator curveGenerator, PowerUpManager powerUpManager, double scaleX, double scaleY)
        {
            this.enemyManager = enemyManager;
            this.curveGenerator = curveGenerator;
            this.powerUpManager = powerUpManager;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public void SetNextWave()
        {
            EnemyWaves enemyWaves = this.GetNextWave();
            BezierPointsCollectionList bezierPointsCollectionList = CreateBezierPointsCollectionList(enemyWaves);
            curveGenerator.SetBezierPoints(bezierPointsCollectionList);
            enemyManager.SetOptions(enemyWaves);
        }

        private BezierPointsCollectionList CreateBezierPointsCollectionList(EnemyWaves enemyWaves)
        {
            var list = new BezierPointsCollectionList();
            foreach (EnemyWave enemyWave in enemyWaves)
            {
                var collection = new BezierControlPointCollection();
                int numberCounter = 1;
                foreach (Vector2 point in enemyWave.BezierSegmentPoints)
                {
                    collection.Add(new BezierControlPoint {Number = numberCounter++, X = (int)(point.X * scaleX), Y = (int)(point.Y * scaleY)});
                }
                list.Add(collection);
            }

            return list;
        }

        public void Update(GameTime gameTime)
        {
            powerUpManager.Update(gameTime);
        }

        public bool StageIsFinished()
        {
            return currentWave >= stage.EnemyWaves.Count;
        }

        public void IntializeStage(Stage stage)
        {
            currentWave = 0;
            this.SetStage(stage);
        }

        private void SetStage(Stage stage)
        {
            this.stage = stage;
            this.enemyWavesCollection.Clear();
            foreach (EnemyWaveDefinition waveDefinition in stage.EnemyWaves)
            {
                var waves = new EnemyWaves();
                waves.NumberOfMilliSecondsBetweenEnemies = waveDefinition.NumberOfMilliSecondsBetweenEnemies;
                foreach (EnemyDefinition enemyDefinition in waveDefinition.EnemyDefinitions)
                {
                    var enemyWave = new EnemyWave();
                    enemyWave.StartSpriteX = enemyDefinition.StartSpriteX;
                    enemyWave.StartSpriteY = enemyDefinition.StartSpriteY;
                    enemyWave.Width = enemyDefinition.Width;
                    enemyWave.Height = enemyDefinition.Height;
                    enemyWave.FrameWidth = enemyDefinition.FrameWidth;
                    enemyWave.FrameHeight = enemyDefinition.FrameHeight;
                    enemyWave.Strength = GetEnemyStrength(enemyDefinition.Strength);
                    enemyWave.FramesPerSecond = enemyDefinition.FramesPerSecond;
                    enemyWave.FramesPerRow = enemyDefinition.FramesPerRow;
                    enemyWave.NumberOfFrames = enemyDefinition.NumberOfFrames;
                    enemyWave.NumberOfEnemies = enemyDefinition.NumberOfEnemies;
                    enemyWave.MovementSpeed = GetMovementSpeed((int)(enemyDefinition.MovementSpeed * this.scaleY));

                    enemyWave.BezierSegmentPoints = new List<Vector2>();
                    foreach (BezierControlPoint bezierControlPoint in enemyDefinition.BezierPoints)
                    {
                        enemyWave.BezierSegmentPoints.Add(new Vector2(bezierControlPoint.X, bezierControlPoint.Y));
                    }
                    waves.Add(enemyWave);
                }

                if (waveDefinition.BossDefinition == null)
                {
                    this.enemyWavesCollection.Add(waves);
                }

                if (waveDefinition.BossDefinition != null)
                {
                    var bossWaves = new EnemyWaves();
                    var bossWave = new BossWave();
                    bossWave.StartSpriteX = waveDefinition.BossDefinition.StartSpriteX;
                    bossWave.StartSpriteY = waveDefinition.BossDefinition.StartSpriteY;
                    bossWave.Width = waveDefinition.BossDefinition.Width;
                    bossWave.Height = waveDefinition.BossDefinition.Height;
                    bossWave.FrameWidth = waveDefinition.BossDefinition.FrameWidth;
                    bossWave.FrameHeight = waveDefinition.BossDefinition.FrameHeight;
                    bossWave.Strength = GetEnemyStrength(waveDefinition.BossDefinition.Strength);
                    bossWave.FramesPerSecond = waveDefinition.BossDefinition.FramesPerSecond;
                    bossWave.FramesPerRow = waveDefinition.BossDefinition.FramesPerRow;
                    bossWave.NumberOfFrames = waveDefinition.BossDefinition.NumberOfFrames;
                    bossWave.MovementSpeed = GetMovementSpeed((int)(waveDefinition.BossDefinition.MovementSpeed * this.scaleY));

                    bossWave.BezierSegmentPoints = new List<Vector2>();
                    foreach (BezierControlPoint bezierControlPoint in waveDefinition.BossDefinition.BezierPoints)
                    {
                        bossWave.BezierSegmentPoints.Add(new Vector2(bezierControlPoint.X, bezierControlPoint.Y));
                    }
                    bossWaves.Add(bossWave);

                    this.enemyWavesCollection.Add(bossWaves);
                }
            }
        }

        private int GetEnemyStrength(int strength)
        {
            switch (Difficulty)
            {
                case GameDifficulty.Easy:
                    return strength * 1;
                case GameDifficulty.Medium:
                    return (int)(strength * 1.3);
                case GameDifficulty.Hard:
                    return (int)(strength * 2.0);
            }
            throw new InvalidOperationException("No difficulty set");
        }

        private int GetMovementSpeed(int baseSpeed)
        {
            switch (Difficulty)
            {
                case GameDifficulty.Easy:
                    return baseSpeed * 1;
                case GameDifficulty.Medium:
                    return (int)(baseSpeed * 0.9);
                case GameDifficulty.Hard:
                    return (int)(baseSpeed * 0.5);
            }
            throw new InvalidOperationException("No difficulty set");
        }

        public void Reset()
        {
            currentWave = 0;
            SetNextWave();
        }

        private EnemyWaves GetNextWave()
        {
            if (currentWave > this.enemyWavesCollection.Count - 1)
            {
                currentWave = 0;
            }

            EnemyWaves waves = this.enemyWavesCollection[currentWave];
            currentWave++;
            return waves;
        }

        public bool WaveIsFinished()
        {
            return enemyManager.WaveIsFinished();
        }

        public bool WaveIsCompletelyShot()
        {
            return enemyManager.AreAllShot();
        }

        public void ShowPowerUp()
        {
            powerUpManager.AddPowerUp();
        }

        public void SetEnemiesReady()
        {
            enemyManager.SetEnemiesReady();
        }

        public void SetDifficulty(GameDifficulty difficulty)
        {
            this.Difficulty = difficulty;
        }

        public GameDifficulty Difficulty { get; set; }
    }
}
