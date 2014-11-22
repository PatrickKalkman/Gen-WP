using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Genesis.Common
{
    public class CurveGenerator : ICurveGenerator
    {
        private readonly List<BezierCurveGenerator> curveGenerators = new List<BezierCurveGenerator>();

        public void SetBezierPoints(BezierPointsCollectionList bezierPointsCollectionList)
        {
            foreach (BezierCurveGenerator curveGenerator in curveGenerators)
            {
                curveGenerator.Filled = false;
            }

            foreach (BezierControlPointCollection bezierControlPointCollection in bezierPointsCollectionList)
            {
                BezierCurveGenerator curveGenerator = GetOrCreateCurveGenerator();
                curveGenerator.SetBezierPoints(bezierControlPointCollection);
            }
        }

        public Vector2 GetPoint(int index, float t)
        {
            return curveGenerators[index].GetPoint(t);
        }

        public float GetMaxTime()
        {
            return this.curveGenerators[0].GetNumberOfBezierPoints() / 3.0f;
        }

        private BezierCurveGenerator GetOrCreateCurveGenerator()
        {
            BezierCurveGenerator curveGenerator = this.curveGenerators.FirstOrDefault(c => !c.Filled);
            if (curveGenerator == null)
            {
                curveGenerator = new BezierCurveGenerator();
                curveGenerators.Add(curveGenerator);
            }
            curveGenerator.Filled = true;
            return curveGenerator;
        }
    }
}
