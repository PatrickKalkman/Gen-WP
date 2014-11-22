using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using Genesis.Management.WP8;

namespace Genesis.Common
{
    public class GameDefinitionParser : IGameDefinitionParser
    {
        public StageCollection Parse(string stagesXml)
        {
            if (string.IsNullOrEmpty(stagesXml))
            {
                throw new ArgumentException("Cannot parse a empty or null string");
            }

            var stageCollection = new StageCollection();
            XElement stagesXmlElement = XElement.Parse(stagesXml);
            IEnumerable<XElement> stageElements = stagesXmlElement.Descendants("Stage");
            foreach (XElement stageElement in stageElements)
            {
                int order = int.Parse(stageElement.Attribute("Order").Value);
                Stage stage = new Stage();
                stage.Order = order;
                stage.BackgroundLayers = this.ParseBackgroundLayers(stageElement.Descendants("BackgroundLayers").Single());
                stage.EnemyWaves = this.ParseEnemyWaves(stageElement.Descendants("EnemeyWaves").Single());
                stageCollection.Add(stage);
            }
            return stageCollection;
        }

        public BezierControlPoint ParsePoint(XElement pointElement)
        {
            ValidateArgument(pointElement);
            int nr = int.Parse(pointElement.Attribute("Nr").Value);
            int x = int.Parse(pointElement.Attribute("X").Value);
            int y = int.Parse(pointElement.Attribute("Y").Value);
            return new BezierControlPoint { Number = nr, X = x, Y = y };
        }

        public BezierControlPointCollection ParsePoints(XElement bezierpoints)
        {
            ValidateArgument(bezierpoints);
            var collection = new BezierControlPointCollection();
            IEnumerable<XElement> pointElements  = bezierpoints.Descendants("Point");
            collection.AddRange(pointElements.Select(this.ParsePoint));
            return collection;
        }

        public EnemyDefinition ParseEnemyDefinition(XElement enemyDefinitionElement)
        {
            int order = int.Parse(enemyDefinitionElement.Attribute("Order").Value);
            int startSpriteX = int.Parse(enemyDefinitionElement.Attribute("StartSpriteX").Value);
            int startSpriteY = int.Parse(enemyDefinitionElement.Attribute("StartSpriteY").Value);
            int width = int.Parse(enemyDefinitionElement.Attribute("Width").Value);
            int height = int.Parse(enemyDefinitionElement.Attribute("Height").Value);
            int frameWidth = int.Parse(enemyDefinitionElement.Attribute("FrameWidth").Value);
            int frameHeight = int.Parse(enemyDefinitionElement.Attribute("FrameHeight").Value);
            int numberOfFrames = int.Parse(enemyDefinitionElement.Attribute("NumberOfFrames").Value);
            int framesPerRow = int.Parse(enemyDefinitionElement.Attribute("FramesPerRow").Value);
            int framesPerSecond = int.Parse(enemyDefinitionElement.Attribute("FramesPerSecond").Value);
            int strength = int.Parse(enemyDefinitionElement.Attribute("Strength").Value);
            int numberOfEnemies = int.Parse(enemyDefinitionElement.Attribute("NumberOfEnemies").Value);
            int movementSpeed = int.Parse(enemyDefinitionElement.Attribute("MovementSpeed").Value);

            EnemyDefinition enemyDefinition = new EnemyDefinition 
                {
                    Order = order,
                    NumberOfEnemies = numberOfEnemies,
                    StartSpriteX = startSpriteX,
                    StartSpriteY = startSpriteY,
                    FrameWidth = frameWidth,
                    FrameHeight = frameHeight,
                    Width = width,
                    Height = height,
                    NumberOfFrames = numberOfFrames,
                    FramesPerRow = framesPerRow,
                    FramesPerSecond = framesPerSecond,
                    Strength = strength,
                    MovementSpeed = movementSpeed
                };

            enemyDefinition.BezierPoints = this.ParsePoints(enemyDefinitionElement.Element("BezierPoints"));

            return enemyDefinition;
        }

        public BossDefinition ParseBossDefinition(XElement bossDefinitionElement)
        {
            int order = int.Parse(bossDefinitionElement.Attribute("Order").Value);
            int startSpriteX = int.Parse(bossDefinitionElement.Attribute("StartSpriteX").Value);
            int startSpriteY = int.Parse(bossDefinitionElement.Attribute("StartSpriteY").Value);
            int width = int.Parse(bossDefinitionElement.Attribute("Width").Value);
            int height = int.Parse(bossDefinitionElement.Attribute("Height").Value);
            int frameWidth = int.Parse(bossDefinitionElement.Attribute("FrameWidth").Value);
            int frameHeight = int.Parse(bossDefinitionElement.Attribute("FrameHeight").Value);
            int numberOfFrames = int.Parse(bossDefinitionElement.Attribute("NumberOfFrames").Value);
            int framesPerRow = int.Parse(bossDefinitionElement.Attribute("FramesPerRow").Value);
            int framesPerSecond = int.Parse(bossDefinitionElement.Attribute("FramesPerSecond").Value);
            int strength = int.Parse(bossDefinitionElement.Attribute("Strength").Value);
            int movementSpeed = int.Parse(bossDefinitionElement.Attribute("MovementSpeed").Value);

            BossDefinition bossDefinition = new BossDefinition
            {
                Order = order,
                StartSpriteX = startSpriteX,
                StartSpriteY = startSpriteY,
                FrameWidth = frameWidth,
                FrameHeight = frameHeight,
                Width = width,
                Height = height,
                NumberOfFrames = numberOfFrames,
                FramesPerRow = framesPerRow,
                FramesPerSecond = framesPerSecond,
                Strength = strength,
                MovementSpeed = movementSpeed
            };

            bossDefinition.BezierPoints = this.ParsePoints(bossDefinitionElement.Element("BezierPoints"));

            return bossDefinition;
        }

        public EnemyWaveCollection ParseEnemyWaves(XElement validEnemyWavesElement)
        {
            ValidateArgument(validEnemyWavesElement);
            var enemyWaves = new EnemyWaveCollection();
            enemyWaves.AddRange(validEnemyWavesElement.Descendants("Wave").Select(this.ParseEnemyWave));
            return enemyWaves;
        }


        public BackgroundLayerCollection ParseBackgroundLayers(XElement validEnemyBackgroundLayersElement)
        {
            var layers = new BackgroundLayerCollection();
            var layerElements = validEnemyBackgroundLayersElement.Descendants("Layer");
            layers.AddRange(layerElements.Select(this.ParseLayer));
            return layers;
        }

        public BackgroundLayerDefinition ParseLayer(XElement layerElement)
        {
            ValidateArgument(layerElement);
            int order = int.Parse(layerElement.Attribute("Order").Value);
            double xSpeed = double.Parse(layerElement.Attribute("XSpeed").Value, CultureInfo.InvariantCulture);
            double ySpeed = double.Parse(layerElement.Attribute("YSpeed").Value, CultureInfo.InvariantCulture);
            string image = layerElement.Attribute("Image").Value;

            return new BackgroundLayerDefinition { Image = image, Order = order, XSpeed = xSpeed, YSpeed = ySpeed };
        }

        private EnemyWaveDefinition ParseEnemyWave(XElement enemyWaveElement)
        {
            ValidateArgument(enemyWaveElement);
            int waveOrder = int.Parse(enemyWaveElement.Attribute("Order").Value);
            int numberOfMilliSecondsBetweenEnemies = int.Parse(enemyWaveElement.Attribute("NumberOfMilliSecondsBetweenEnemies").Value);
            var enemyWaveDefinition = new EnemyWaveDefinition { Order = waveOrder, NumberOfMilliSecondsBetweenEnemies = numberOfMilliSecondsBetweenEnemies };
            
            enemyWaveDefinition.EnemyDefinitions = this.ParseEnemyDefinitions(enemyWaveElement);
            enemyWaveDefinition.BossDefinition = this.ParseBossDefinitions(enemyWaveElement);
            return enemyWaveDefinition;
        }

        private BossDefinition ParseBossDefinitions(XElement enemyWaveElement)
        {
            ValidateArgument(enemyWaveElement);
            var enemyDefinitionElements = enemyWaveElement.Descendants("BossDefinition");
            BossDefinition bossDefinition = null;
            foreach (var enemyDefinitionElement in enemyDefinitionElements)
            {
                bossDefinition = this.ParseBossDefinition(enemyDefinitionElement);
            }
            return bossDefinition;
        }

        private EnemyDefinitionCollection ParseEnemyDefinitions(XElement enemyWaveElement)
        {
            ValidateArgument(enemyWaveElement);
            var enemyDefinitionElements = enemyWaveElement.Descendants("EnemyDefinition");
            EnemyDefinitionCollection enemyDefinitions = new EnemyDefinitionCollection();
            enemyDefinitions.AddRange(enemyDefinitionElements.Select(this.ParseEnemyDefinition));
            return enemyDefinitions;
        }

        private void ValidateArgument(XElement pointElement)
        {
            if (pointElement == null)
            {
                throw new ArgumentException("Cannot parse an empty element.");
            }
        }
    }
}