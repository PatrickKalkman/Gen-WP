using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public interface ICurveGenerator
    {
        void SetBezierPoints(BezierPointsCollectionList bezierPointsCollectionList);

        Vector2 GetPoint(int index, float t);

        float GetMaxTime();
    }
}