using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public interface IAngleCalculator
    {
        float Calculate(Vector2 point1, Vector2 point2);
    }
}