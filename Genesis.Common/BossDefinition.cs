namespace Genesis.Common
{
    public class BossDefinition
    {
        public BossDefinition()
        {
        }

        public BossDefinition(BossDefinition otherBossDefinition)
        {
            this.StartSpriteX = otherBossDefinition.StartSpriteX;
            this.StartSpriteY = otherBossDefinition.StartSpriteY;
            this.Width = otherBossDefinition.Width;
            this.Height = otherBossDefinition.Height;
            this.FrameWidth = otherBossDefinition.FrameWidth;
            this.FrameHeight = otherBossDefinition.FrameHeight;
            this.NumberOfFrames = otherBossDefinition.NumberOfFrames;
            this.FramesPerRow = otherBossDefinition.FramesPerRow;
            this.Strength = otherBossDefinition.Strength;
            this.MovementSpeed = otherBossDefinition.MovementSpeed;
            this.Order = otherBossDefinition.Order;

            this.BezierPoints = new BezierControlPointCollection();
            foreach (BezierControlPoint otherBezierControlPoint in otherBossDefinition.BezierPoints)
            {
                this.BezierPoints.Add(new BezierControlPoint { Number = otherBezierControlPoint.Number, X = otherBezierControlPoint.X, Y = otherBezierControlPoint.Y });
            }
        }

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