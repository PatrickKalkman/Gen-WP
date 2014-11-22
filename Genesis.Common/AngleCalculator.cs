using System;

using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public class AngleCalculator : IAngleCalculator
    {
        public float Calculate(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Atan2(point1.Y - point2.Y, point1.X - point2.X);
        }
    }
}
