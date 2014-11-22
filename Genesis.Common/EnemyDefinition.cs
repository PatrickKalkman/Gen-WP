namespace Genesis.Common
{
    public class EnemyDefinition
    {
        public EnemyDefinition()
        {
        }

        public EnemyDefinition(EnemyDefinition otherEnemyDefinition)
        {
            this.NumberOfEnemies = otherEnemyDefinition.NumberOfEnemies;
            this.StartSpriteX = otherEnemyDefinition.StartSpriteX;
            this.StartSpriteY = otherEnemyDefinition.StartSpriteY;
            this.Width = otherEnemyDefinition.Width;
            this.Height = otherEnemyDefinition.Height;
            this.FrameWidth = otherEnemyDefinition.FrameWidth;
            this.FrameHeight = otherEnemyDefinition.FrameHeight;
            this.NumberOfFrames = otherEnemyDefinition.NumberOfFrames;
            this.FramesPerRow = otherEnemyDefinition.FramesPerRow;
            this.Strength = otherEnemyDefinition.Strength;
            this.MovementSpeed = otherEnemyDefinition.MovementSpeed;
            this.Order = otherEnemyDefinition.Order;

            this.BezierPoints = new BezierControlPointCollection();
            foreach (BezierControlPoint otherBezierControlPoint in otherEnemyDefinition.BezierPoints)
            {
                this.BezierPoints.Add(new BezierControlPoint { Number = otherBezierControlPoint.Number, X = otherBezierControlPoint.X, Y = otherBezierControlPoint.Y });
            }
        }

        public int NumberOfEnemies { get; set; }

        public int StartSpriteX { get; set; }

        public int StartSpriteY { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public int NumberOfFrames { get; set; }

        public int FramesPerRow { get; set; }

        public int FramesPerSecond { get; set; }

        public int Strength { get; set; }

        public int MovementSpeed { get; set; }

        public BezierControlPointCollection BezierPoints { get; set; }

        public int NumberOfBezierPoints
        {
            get
            {
                return BezierPoints.Count;
            }
        }

        public int Order { get; set; }

        //public void SetValue(string propertyName, int value)
        //{
        //    Type type = this.GetType();
        //    PropertyInfo propertyInfo = type.GetProperty(propertyName);
        //    propertyInfo.SetValue(this, value, null);
        //}
    }
}