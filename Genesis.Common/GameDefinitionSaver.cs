using System.Xml.Linq;

using Genesis.Management.WP8;

namespace Genesis.Common
{
    public class GameDefinitionSaver
    {
        public void Save(string stageFilename, GameDefinition gameDefinition)
        {
            XElement definitionElement = this.SaveStages(gameDefinition.Stages);
            //definitionElement.Save(stageFilename);
        }

        public XElement SaveStages(StageCollection stageCollection)
        {
            var xElement = new XElement("Stages");
            foreach (Stage stage in stageCollection)
            {
                xElement.Add(this.SaveStage(stage));
            }

            return xElement;
        }

        public XElement SaveBackgroundLayer(BackgroundLayerDefinition definition)
        {
            var xElement = new XElement("Layer");
            xElement.SetAttributeValue("Order", definition.Order);
            xElement.SetAttributeValue("Image", definition.Image);
            xElement.SetAttributeValue("XSpeed", definition.XSpeed);
            xElement.SetAttributeValue("YSpeed", definition.YSpeed);
            return xElement;
        }

        public XElement SaveBezierControlPoint(BezierControlPoint point)
        {
            var xElement = new XElement("Point");
            xElement.SetAttributeValue("Nr", point.Number);
            xElement.SetAttributeValue("X", point.X);
            xElement.SetAttributeValue("Y", point.Y);
            return xElement;
        }

        public XElement SaveBackgroundLayers(BackgroundLayerCollection layers)
        {
            var xElement = new XElement("BackgroundLayers");
            foreach (BackgroundLayerDefinition backgroundLayerDefinition in layers)
            {
                xElement.Add(this.SaveBackgroundLayer(backgroundLayerDefinition));
            }

            return xElement;
        }

        public XElement SaveBezierControlPoints(BezierControlPointCollection bezierControlPointCollection)
        {
            var xElement = new XElement("BezierPoints");
            foreach (BezierControlPoint bezierControlPoint in bezierControlPointCollection)
            {
                xElement.Add(this.SaveBezierControlPoint(bezierControlPoint));
            }

            return xElement;
        }

        public XElement SaveEnemyDefinition(EnemyDefinition enemyDefinition)
        {
            var xElement = new XElement("EnemyDefinition");

            xElement.SetAttributeValue("FrameHeight", enemyDefinition.FrameHeight);
            xElement.SetAttributeValue("FrameWidth", enemyDefinition.FrameWidth);
            xElement.SetAttributeValue("FramesPerRow", enemyDefinition.FramesPerRow);
            xElement.SetAttributeValue("FramesPerSecond", enemyDefinition.FramesPerSecond);
            xElement.SetAttributeValue("Width", enemyDefinition.Width);
            xElement.SetAttributeValue("Height", enemyDefinition.Height);
            xElement.SetAttributeValue("Order", enemyDefinition.Order);
            xElement.SetAttributeValue("NumberOfFrames", enemyDefinition.NumberOfFrames);
            xElement.SetAttributeValue("StartSpriteX", enemyDefinition.StartSpriteX);
            xElement.SetAttributeValue("StartSpriteY", enemyDefinition.StartSpriteY);
            xElement.SetAttributeValue("Strength", enemyDefinition.Strength);
            xElement.SetAttributeValue("NumberOfEnemies", enemyDefinition.NumberOfEnemies);
            xElement.SetAttributeValue("MovementSpeed", enemyDefinition.MovementSpeed);

            var bezierPointsElement = new XElement("BezierPoints");
            if (enemyDefinition.BezierPoints != null)
            {
                foreach (BezierControlPoint controlPoint in enemyDefinition.BezierPoints)
                {
                    bezierPointsElement.Add(this.SaveBezierControlPoint(controlPoint));
                }
            }

            xElement.Add(bezierPointsElement);
            return xElement;
        }

        public XElement SaveEnemyBossDefinition(BossDefinition bossDefinition)
        {
            var xElement = new XElement("BossDefinition");

            xElement.SetAttributeValue("FrameHeight", bossDefinition.FrameHeight);
            xElement.SetAttributeValue("FrameWidth", bossDefinition.FrameWidth);
            xElement.SetAttributeValue("FramesPerRow", bossDefinition.FramesPerRow);
            xElement.SetAttributeValue("FramesPerSecond", bossDefinition.FramesPerSecond);
            xElement.SetAttributeValue("Width", bossDefinition.Width);
            xElement.SetAttributeValue("Height", bossDefinition.Height);
            xElement.SetAttributeValue("Order", bossDefinition.Order);
            xElement.SetAttributeValue("NumberOfFrames", bossDefinition.NumberOfFrames);
            xElement.SetAttributeValue("StartSpriteX", bossDefinition.StartSpriteX);
            xElement.SetAttributeValue("StartSpriteY", bossDefinition.StartSpriteY);
            xElement.SetAttributeValue("Strength", bossDefinition.Strength);
            xElement.SetAttributeValue("MovementSpeed", bossDefinition.MovementSpeed);

            var bezierPointsElement = new XElement("BezierPoints");
            if (bossDefinition.BezierPoints != null)
            {
                foreach (BezierControlPoint controlPoint in bossDefinition.BezierPoints)
                {
                    bezierPointsElement.Add(this.SaveBezierControlPoint(controlPoint));
                }
            }

            xElement.Add(bezierPointsElement);
            return xElement;
        }

        public XElement SaveWave(EnemyWaveDefinition enemyWaveDefinition)
        {
            var xElement = new XElement("Wave");
            xElement.SetAttributeValue("Order", enemyWaveDefinition.Order);
            xElement.SetAttributeValue("NumberOfMilliSecondsBetweenEnemies", enemyWaveDefinition.NumberOfMilliSecondsBetweenEnemies);

            if (enemyWaveDefinition.EnemyDefinitions != null)
            {
                foreach (EnemyDefinition enemyDefinition in enemyWaveDefinition.EnemyDefinitions)
                {
                    xElement.Add(this.SaveEnemyDefinition(enemyDefinition));
                }

                if (enemyWaveDefinition.BossDefinition != null)
                {
                    xElement.Add(this.SaveEnemyBossDefinition(enemyWaveDefinition.BossDefinition));
                }
            }

            return xElement;
        }

        public XElement SaveWaves(EnemyWaveCollection enemyWaveCollection)
        {
            var xElement = new XElement("EnemeyWaves");
            foreach (EnemyWaveDefinition enemyWaveDefinition in enemyWaveCollection)
            {
                xElement.Add(this.SaveWave(enemyWaveDefinition));
            }

            return xElement;
        }

        public XElement SaveStage(Stage stage)
        {
            var xElement = new XElement("Stage");
            xElement.SetAttributeValue("Order", stage.Order);
            if (stage.BackgroundLayers != null)
            {
                xElement.Add(this.SaveBackgroundLayers(stage.BackgroundLayers));
            }

            if (stage.EnemyWaves != null)
            {
                xElement.Add(this.SaveWaves(stage.EnemyWaves));
            }

            return xElement;
        }
    }
}
