using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Genesis.Common.Enemy
{
    public class EnemyWave
    {
        public List<Vector2> BezierSegmentPoints { get; set; }

        public int StartSpriteX { get; set; }

        public int StartSpriteY { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Strength { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public int NumberOfFrames { get; set; }

        public int FramesPerRow { get; set; }

        public int FramesPerSecond { get; set; }

        public int NumberOfEnemies { get; set; }

        public int MovementSpeed { get; set; }
    }

    public class BossWave : EnemyWave
    {
        
    }
}