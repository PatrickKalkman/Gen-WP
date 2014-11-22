using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public class BezierCurveGenerator
    {
        private readonly int xMove;

        private readonly int yMove;

        private BezierControlPointCollection bezierPoints;

        public bool Filled { get; set; }

        public BezierCurveGenerator() : this(0,0)
        {
        }

        public BezierCurveGenerator(int xMove, int yMove)
        {
            this.xMove = xMove;
            this.yMove = yMove;
        }

        public int XMove
        {
            get
            {
                return xMove;
            }
        }

        public int YMove
        {
            get
            {
                return yMove;
            }
        }

        public Vector2 GetPoint(float t)
        {
            var segment = (int)t;

            int pointListIndex = segment * 3;
            BezierControlPoint p0 = bezierPoints[pointListIndex];
            BezierControlPoint p1 = bezierPoints[pointListIndex + 1];
            BezierControlPoint p2 = bezierPoints[pointListIndex + 2];
            BezierControlPoint p3 = bezierPoints[CalculatePointIndex(pointListIndex + 3)];

            t -= segment;

            float cx = 3 * (p1.X - p0.X);
            float cy = 3 * (p1.Y - p0.Y);

            float bx = 3 * (p2.X - p1.X) - cx;
            float by = 3 * (p2.Y - p1.Y) - cy;

            float ax = p3.X - p0.X - cx - bx;
            float ay = p3.Y - p0.Y - cy - by;

            float Cube = t * t * t;

            float Square = t * t;

            float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
            float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

            return new Vector2(resX + xMove, resY + yMove);
        }

        public Vector2 GetBezierPoint(int index)
        {
            return new Vector2(bezierPoints[index].X + xMove, bezierPoints[index].Y + yMove);
        }

        public int GetNumberOfBezierPoints()
        {
            return bezierPoints.Count;
        }

        private int CalculatePointIndex(int index)
        {
            if (index >= bezierPoints.Count)
            {
                return 0;
            }

            return index;
        }

        public void SetBezierPoints(BezierControlPointCollection bezierPoints)
        {
            this.bezierPoints = bezierPoints;
        }

        public bool IsInControlPoint(int x, int y, out int controlPointIndex)
        {
            for (int index = 0; index < this.bezierPoints.Count; index++)
            {
                BezierControlPoint bezierPoint = this.bezierPoints[index];
                Rectangle rectangle = new Rectangle((int)bezierPoint.X + this.xMove - 8, (int)bezierPoint.Y + this.yMove - 8, 16, 16);
                if (rectangle.Intersects(new Rectangle(x - 1, y - 1, 2, 2)))
                {
                    controlPointIndex = index;
                    return true;
                }
            }
            controlPointIndex = -1;
            return false;
        }
    }
}
